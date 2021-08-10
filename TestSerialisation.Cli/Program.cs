using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MessagePack;
using ZstdNet;

namespace TestSerialisation.Cli
{
	internal static class Program
	{
		private static ExampleDataClass  _class;
		private static ExampleDataStruct _struct;

		private static string JsonStr;
		private static byte[] JsonUtf1;
		private static byte[] JsonUtf2;
		private static byte[] MsgPack;
		private static byte[] MsgPackZstd;

		private static void Main(string[] args)
		{
			Console.WriteLine(@"Testing the following:
 - System.Text.Json to string (dotnet builtin)
 - System.Text.Json to utf8 bytes (dotnet builtin)
 - Utf8Json (Utf8Json nuget pkg)
 - MessagePack (MessagePack nuget pkg)
 - MessagePack, Zstandard compressed (ZstdNet nuget pkg)
");
			ReadTestData();
			var ((scss, scsu, scu, scmp, scmz), (ssss, sssu, ssu, ssmp, smz)) = TestSerialise();
			var ((dcss, dcsu, dcu, dcmp, dcmz), (dsss, dssu, dsu, dsmp, dmz)) = TestDeserialise();
			ForTheLoveOfGodStopUsingSoMuchRam();

			Console.WriteLine(Formatter.Table(scss,
											  scsu,
											  scu,
											  scmp,
											  scmz,
											  ssss,
											  sssu,
											  ssu,
											  ssmp,
											  smz,
											  dcss,
											  dcsu,
											  dcu,
											  dcmp,
											  dcmz,
											  dsss,
											  dssu,
											  dsu,
											  dsmp,
											  dmz));
		}

		private static void ForTheLoveOfGodStopUsingSoMuchRam()
		{
			// set anything possible to null (basically means "the absence of a value")
			_class   = null;
			JsonStr  = null;
			// this stuff well just set to default and empty instead because they arent nullable
			_struct  = default;
			JsonUtf1 = JsonUtf2 = MsgPack = MsgPackZstd = Array.Empty<byte>();
			// finally force the garbage collector to actually do its job
			GC.Collect();
		}

		private static void ReadTestData()
		{
			Console.WriteLine("Loading test data");
			// find data
			var data = new DirectoryInfo(Environment.CurrentDirectory).EnumerateFiles()
																	  .FirstOrDefault(f => f.Name.Contains("testdata"));

			if (data == null)
			{
				Console.WriteLine("Could not locate test data");
				Environment.Exit(1);
			}

			var sw = Stopwatch.StartNew();
			_class  = MessagePackSerializer.Deserialize<ExampleDataClass>(new DecompressionStream(data.OpenRead()));
			_struct = MessagePackSerializer.Deserialize<ExampleDataStruct>(new DecompressionStream(data.OpenRead()));
			sw.Stop();
			
			Console.WriteLine($"Loaded test data from disk in {sw.Elapsed.TotalSeconds} seconds\n");
		}

		private static TimeSpan MeasurePerformance(Action codeToTest, int iterations = 1)
		{
			long? averageTicks = null;
			for (var i = 0; i < iterations; i++)
			{
				var sw = Stopwatch.StartNew();
				codeToTest();
				sw.Stop();
				if (!averageTicks.HasValue)
					averageTicks  = sw.ElapsedTicks;
				else averageTicks = (sw.ElapsedTicks + (i * averageTicks)) / i + 1;
			}

			if (!averageTicks.HasValue) throw new Exception($"{nameof(averageTicks)} is null");
			
			return new TimeSpan(averageTicks.Value);
		}

		private static ((double, double, double, double, double) classResults, (double, double, double, double, double) structResults) TestSerialise()
		{
			var classResults  = TestSerialise(false);
			var structResults = TestSerialise(true);
			return (classResults, structResults);
		}
		
		private static (double, double, double, double, double) TestSerialise(bool @struct)
		{
			Console.WriteLine($"Testing serialise {(@struct ? "struct" : "class")} performance with three iterations");
			
			// im aware this is bad practice but aaaaaaaaaa
			dynamic obj = @struct ? _struct : _class;
			
			var sysTxtJson     = MeasurePerformance(() => JsonStr = SerialiseFuncs.SystemTextJson(obj),   3);
			var sysTxtJsonUtf8 = MeasurePerformance(() => JsonUtf1 = SerialiseFuncs.SysTextJsonUtf8(obj), 3);
			var utf8Json       = MeasurePerformance(() => JsonUtf2 = SerialiseFuncs.Utf8Json(obj),        3);
			var msgpack        = MeasurePerformance(() => MsgPack = SerialiseFuncs.MsgPack(obj),          3);
			var msgpackZstd    = MeasurePerformance(() => MsgPackZstd = SerialiseFuncs.MsgPackZstd(obj),  3);
			
			return (sysTxtJson.TotalSeconds, sysTxtJsonUtf8.TotalSeconds, utf8Json.TotalSeconds, msgpack.TotalSeconds, msgpackZstd.TotalSeconds);
		}
		
		private static ((double, double, double, double, double) classResults, (double, double, double, double, double) structResults)  TestDeserialise()
		{
			var classResults  = TestDeserialise(false);
			var structResults = TestDeserialise(true);
			return (classResults, structResults);
		}
		
		private static (double, double, double, double, double) TestDeserialise(bool @struct)
		{
			Console.WriteLine($"Testing deserialise {(@struct ? "struct" : "class")} performance with five iterations");
			
			TimeSpan sysTxtJson;
			TimeSpan sysTxtJsonUtf8;
			TimeSpan utf8Json;
			TimeSpan msgpack;
			TimeSpan msgpackZstd;
			
			if (@struct)
			{
				sysTxtJson     = MeasurePerformance(() => DeserialiseFuncs.SystemTextJson(JsonStr, ref _struct),   5);
				sysTxtJsonUtf8 = MeasurePerformance(() => DeserialiseFuncs.SysTextJsonUtf8(JsonUtf1, ref _struct), 5);
				utf8Json       = MeasurePerformance(() => DeserialiseFuncs.Utf8Json(JsonUtf2, ref _struct),        5);
				msgpack        = MeasurePerformance(() => DeserialiseFuncs.MsgPack(MsgPack, ref _struct),          5);
				msgpackZstd    = MeasurePerformance(() => DeserialiseFuncs.MsgPackZstd(MsgPackZstd, ref _struct),  5);
			}
			else
			{
				sysTxtJson     = MeasurePerformance(() => DeserialiseFuncs.SystemTextJson(JsonStr, ref _class),   5);
				sysTxtJsonUtf8 = MeasurePerformance(() => DeserialiseFuncs.SysTextJsonUtf8(JsonUtf1, ref _class), 5);
				utf8Json       = MeasurePerformance(() => DeserialiseFuncs.Utf8Json(JsonUtf2, ref _class),        5);
				msgpack        = MeasurePerformance(() => DeserialiseFuncs.MsgPack(MsgPack, ref _class),          5);
				msgpackZstd    = MeasurePerformance(() => DeserialiseFuncs.MsgPackZstd(MsgPackZstd, ref _class),  5);
			}

			return (sysTxtJson.TotalSeconds, sysTxtJsonUtf8.TotalSeconds, utf8Json.TotalSeconds, msgpack.TotalSeconds, msgpackZstd.TotalSeconds);
		}
	}
}
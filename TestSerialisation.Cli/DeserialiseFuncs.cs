using MessagePack;
using ZstdNet;
using stj = System.Text.Json;
using u8j = Utf8Json;

namespace TestSerialisation.Cli
{
	public static class DeserialiseFuncs
	{
		public static void SystemTextJson(string inputData, ref ExampleDataClass  output)  => output = stj::JsonSerializer.Deserialize<ExampleDataClass>(inputData);
		public static void SystemTextJson(string inputData, ref ExampleDataStruct output) => output = stj::JsonSerializer.Deserialize<ExampleDataStruct>(inputData);
		
		public static void SysTextJsonUtf8(byte[] inputData, ref ExampleDataClass  output)  => output = stj::JsonSerializer.Deserialize<ExampleDataClass>(inputData);
		public static void SysTextJsonUtf8(byte[] inputData, ref ExampleDataStruct output) => output = stj::JsonSerializer.Deserialize<ExampleDataStruct>(inputData);
		
		public static void Utf8Json(byte[] inputData, ref ExampleDataClass  output)  => output = u8j::JsonSerializer.Deserialize<ExampleDataClass>(inputData);
		public static void Utf8Json(byte[] inputData, ref ExampleDataStruct output) => output = u8j::JsonSerializer.Deserialize<ExampleDataStruct>(inputData);

		public static void MsgPack(byte[] inputData, ref ExampleDataClass output) => output = MessagePackSerializer.Deserialize<ExampleDataClass>(inputData);
		public static void MsgPack(byte[] inputData, ref ExampleDataStruct output) => output = MessagePackSerializer.Deserialize<ExampleDataStruct>(inputData);
		
		public static void MsgPackZstd(byte[] inputData, ref ExampleDataClass  output)  => output = MessagePackSerializer.Deserialize<ExampleDataClass>(new Decompressor().Unwrap(inputData));
		public static void MsgPackZstd(byte[] inputData, ref ExampleDataStruct output) => output = MessagePackSerializer.Deserialize<ExampleDataStruct>(new Decompressor().Unwrap(inputData));
	}
}
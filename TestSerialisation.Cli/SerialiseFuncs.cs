using MessagePack;
using ZstdNet;
using stj = System.Text.Json;
using u8j = Utf8Json;

namespace TestSerialisation.Cli
{
	public static class SerialiseFuncs
	{
		public static string SystemTextJson(ExampleDataClass  @class)  => stj::JsonSerializer.Serialize(@class);
		public static string SystemTextJson(ExampleDataStruct @struct) => stj::JsonSerializer.Serialize(@struct);
		
		public static byte[] SysTextJsonUtf8(ExampleDataClass  @class)  => stj::JsonSerializer.SerializeToUtf8Bytes(@class);
		public static byte[] SysTextJsonUtf8(ExampleDataStruct @struct) => stj::JsonSerializer.SerializeToUtf8Bytes(@struct);
		
		public static byte[] Utf8Json(ExampleDataClass  @class)  => u8j::JsonSerializer.Serialize(@class);
		public static byte[] Utf8Json(ExampleDataStruct @struct) => u8j::JsonSerializer.Serialize(@struct);

		public static byte[] MsgPack(ExampleDataClass @class) => MessagePackSerializer.Serialize(@class);
		public static byte[] MsgPack(ExampleDataStruct @struct) => MessagePackSerializer.Serialize(@struct);
		
		public static byte[] MsgPackZstd(ExampleDataClass  @class)  => new Compressor().Wrap(MessagePackSerializer.Serialize(@class));
		public static byte[] MsgPackZstd(ExampleDataStruct @struct) => new Compressor().Wrap(MessagePackSerializer.Serialize(@struct));
	}
}
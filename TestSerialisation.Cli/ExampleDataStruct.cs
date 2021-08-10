using MessagePack;

namespace TestSerialisation.Cli
{
	[MessagePackObject]
	public struct ExampleDataStruct
	{
		// CVID format - see https://github.com/yellowsink/ConsoleVideoPlayer/blob/master/ConsoleVideoPlayer.Player/Cvid.cs

		[Key(0)] public string[] CvidFrameData { get; set; }
		[Key(1)] public double   CvidRate      { get; set; }
		[Key(2)] public byte[]   CvidAudioData { get; set; }
	}
}
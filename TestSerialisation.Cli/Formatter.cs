using System;
using System.Globalization;

namespace TestSerialisation.Cli
{
	public static class Formatter
	{
		public static string F(double val)
		{
			// if val > 99999.99 this will return a bigger string so be careful abt the size of ur nums
			var split = Math.Round(val, 2).ToString(CultureInfo.CurrentCulture).Split('.');
			
			var topPart    = split[0];
			var bottomPart = split.Length == 2 ? split[1] : "";
			
			return topPart.PadLeft(5) + '.' + bottomPart.PadRight(2, '0');
		}

		public static string Table(double scss, double scsu, double scu,  double scmp, double scmz, double ssss,
								   double sssu, double ssu,  double ssmp, double smz,  double dcss, double dcsu,
								   double dcu,  double dcmp, double dcmz, double dsss, double dssu, double dsu,
								   double dsmp, double dmz)
		{
			return @$"
Results from test runs (in seconds per single iteration):
|                         | Class serialise | Struct serialise | Class deserialise | Struct deserialise |
|-------------------------|-----------------|------------------|-------------------|--------------------|
| System.Text.Json string | {F(scss)}        | {F(ssss)}         | {F(dcss)}          | {F(dsss)}           |
| System.Text.Json bytes  | {F(scsu)}        | {F(sssu)}         | {F(dcsu)}          | {F(dssu)}           |
| Utf8Json                | {F(scu) }        | {F(ssu) }         | {F(dcu) }          | {F(dsu) }           |
| MessagePack             | {F(scmp)}        | {F(ssmp)}         | {F(dcmp)}          | {F(dsmp)}           |
| MsgPack + Zstandard     | {F(scmz)}        | {F(smz) }         | {F(dcmz)}          | {F(dmz) }           |";
		}
	}
}
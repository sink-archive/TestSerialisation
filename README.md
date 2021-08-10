# TestSerialisation

Testing the speed of data de/serialisation libs

An example run I did - when uncompressed that file was ~800mb
```
❯ ~/source/ConsoleVideoPlayer/ConsoleVideoPlayer.Player/bin/Debug/net5/ConsoleVideoPlayer.Player -v ~/M
Timer frequency: 1GHz (1000MHz), High precision: Yes
Reading metadata             Done in 546ms
Preparing to pre-process     Done in 6ms
Extracting Audio             Done in 2.56s
Splitting into images        Done in 138.7s
Creating ASCII art           Done in 12m 10s
Saving to CVID file...       Done in 9.13s

Saved the converted video to testdata.cvid.
❯ ls -l
.rwxr--r-- 9.2k cain 29 Jul 04:14 MessagePack.Annotations.dll
.rwxr--r-- 303k cain 29 Jul 04:15 MessagePack.dll
drwxr-xr-x    - cain 10 Aug 21:51 ref
.rw-r--r-- 157M cain 10 Aug 22:19 testdata.cvid
.rwxr-xr-x  85k cain 10 Aug 21:47 TestSerialisation.Cli
.rw-r--r-- 9.5k cain 10 Aug 21:51 TestSerialisation.Cli.deps.json
.rw-r--r--  17k cain 10 Aug 21:47 TestSerialisation.Cli.dll
.rw-r--r--  13k cain 10 Aug 21:47 TestSerialisation.Cli.pdb
.rw-r--r--  149 cain 10 Aug 21:51 TestSerialisation.Cli.runtimeconfig.dev.json
.rw-r--r--  139 cain 10 Aug 21:51 TestSerialisation.Cli.runtimeconfig.json
.rwxr--r-- 238k cain 30 Jan  2018 Utf8Json.dll
drwxr-xr-x    - cain 10 Aug 21:51 x64
drwxr-xr-x    - cain 10 Aug 21:51 x86
.rwxr--r--  30k cain 29 Nov  2020 ZstdNet.dll
❯ time ./TestSerialisation.Cli
Testing the following:
 - System.Text.Json to string (dotnet builtin)
 - System.Text.Json to utf8 bytes (dotnet builtin)
 - Utf8Json (Utf8Json nuget pkg)
 - MessagePack (MessagePack nuget pkg)
 - MessagePack, Zstandard compressed (ZstdNet nuget pkg)

Loading test data
Loaded test data from disk in 5.3617443 seconds

Testing serialise class performance with three iterations
Testing serialise struct performance with three iterations
Testing deserialise class performance with five iterations
Testing deserialise struct performance with five iterations

Results from test runs (in seconds per single iteration):
|                         | Class serialise | Struct serialise | Class deserialise | Struct deserialise |
|-------------------------|-----------------|------------------|-------------------|--------------------|
| System.Text.Json string |  4511.58        | 10989.64         | 11577.90          | 10666.33           |
| System.Text.Json bytes  |  3009.60        |  7651.67         |  5392.68          |  5911.35           |
| Utf8Json                |  3286.07        |  3749.98         |  7917.76          |  8286.83           |
| MessagePack             |  2928.20        |  3169.67         |  6484.90          |  6632.69           |
| MsgPack + Zstandard     |  3499.74        |  4623.93         |  4752.40          |  6566.74           |
./TestSerialisation.Cli  294.06s user 197.28s system 27% cpu 29:17.57 total
```

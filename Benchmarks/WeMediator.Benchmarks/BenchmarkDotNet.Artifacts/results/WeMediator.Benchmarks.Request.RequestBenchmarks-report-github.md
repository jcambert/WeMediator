``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
Intel Xeon W-2125 CPU 4.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2


```
|                                             Method |      Mean |    Error |   StdDev | Rank |   Gen0 | Allocated |
|--------------------------------------------------- |----------:|---------:|---------:|-----:|-------:|----------:|
|                  SendRequest_SomeRequest_IMediator |  24.84 ns | 0.192 ns | 0.180 ns |    1 |      - |         - |
| SendRequest_PingRequest_WithoutValidator_IMediator | 282.17 ns | 1.380 ns | 1.152 ns |    2 | 0.1464 |     632 B |
|    SendRequest_PingRequest_WithValidator_IMediator | 284.10 ns | 2.648 ns | 2.477 ns |    2 | 0.1464 |     632 B |

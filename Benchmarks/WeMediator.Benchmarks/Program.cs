// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using WeMediator.Benchmarks.Request;

Console.WriteLine("Hello, World!");
BenchmarkRunner.Run<RequestBenchmarks>();
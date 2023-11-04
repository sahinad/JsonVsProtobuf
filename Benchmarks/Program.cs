using BenchmarkDotNet.Running;
using Benchmarks;

var _ = BenchmarkRunner.Run<JsonVsProtobufSerializationBenchmark>();
//var benchmark = new JsonVsProtobufSerializationBenchmark();
//benchmark.Setup();
//benchmark.SerializeWithProtobuf();
//benchmark.SerializeWithJson();

Console.WriteLine("Hello, World!");

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using ProtoBuf;
using System.Text.Json;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[HideColumns(Column.RatioSD)]
public class JsonVsProtobufSerializationBenchmark
{
    private Trade[] _trades = Array.Empty<Trade>();

    [Params(100_000, 500_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _trades = Enumerable.Range(1, Count).Select(index =>
            new Trade
            {
                TickerSymbol = $"SYM{index % 1000}",
                TradeTime = DateTime.Now.AddMilliseconds(index),
                Price = 100 + (index % 50 * 0.1),
                Volume = index % 10000
            }).ToArray();
    }

    [Benchmark(Baseline = true)]
    public byte[] SerializeWithJson()
    {
        return JsonSerializer.SerializeToUtf8Bytes(_trades);
    }

    [Benchmark]
    public byte[] SerializeWithProtobuf()
    {
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, _trades);
        return stream.ToArray();
    }
}

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using ProtoBuf;
using System.Text.Json;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[HideColumns(Column.RatioSD)]
public class JsonVsProtobufDeserializationBenchmark
{
    private byte[] _protobufData = Array.Empty<byte>();
    private byte[] _jsonData = Array.Empty<byte>();

    [Params(100_000, 500_000, 1_000_000, 5_000_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var trades = Enumerable.Range(1, Count).Select(index =>
            new Trade
            {
                TickerSymbol = $"SYM{index % 1000}",
                TradeTime = DateTime.Now.AddMilliseconds(index),
                Price = 100 + (index % 50 * 0.1),
                Volume = index % 10000
            }).ToArray();

        using (var stream = new MemoryStream())
        {
            Serializer.Serialize(stream, trades);
            _protobufData = stream.ToArray();
        }

        _jsonData = 
            JsonSerializer.SerializeToUtf8Bytes(trades, trades.GetType());
    }

    [Benchmark(Baseline = true)]
    public Trade[] DeserializeWithJson()
    {
        return JsonSerializer.Deserialize<Trade[]>(_jsonData);
    }

    [Benchmark]
    public Trade[] DeserializeWithProtobuf()
    {
        using var stream = new MemoryStream(_protobufData);
        return Serializer.Deserialize<Trade[]>(stream);
    }
}

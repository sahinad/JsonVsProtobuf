using ProtoBuf;

namespace Benchmarks;

[ProtoContract]
public class Trade
{
    [ProtoMember(1)]
    public required string TickerSymbol { get; set; }

    [ProtoMember(2)]
    public DateTime TradeTime { get; set; }

    [ProtoMember(3)]
    public double Price { get; set; }

    [ProtoMember(4)]
    public int Volume { get; set; }
}

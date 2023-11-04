using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace Benchmarks;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        SummaryStyle =
            SummaryStyle.Default.WithRatioStyle(RatioStyle.Percentage);
    }
}

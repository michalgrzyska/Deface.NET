namespace Deface.NET.PerformanceTestRunner.Config;

public static class TimeSpanExtensions
{
    public static string AsPercentageComparingTo(this TimeSpan t2, TimeSpan t1)
    {
        var diff = t1.TotalMilliseconds - t2.TotalMilliseconds;
        var percentage = (diff / t1.TotalMilliseconds) * 100;

        return percentage > 0
            ? $"+{(int)percentage} %"
            : $"{(int)percentage} %";
    }
}

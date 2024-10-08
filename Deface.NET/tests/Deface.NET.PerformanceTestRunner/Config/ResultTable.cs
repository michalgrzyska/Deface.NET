using ConsoleTables;

namespace Deface.NET.PerformanceTestRunner.Config;

public class ResultTable
{
    public static void Display(IEnumerable<TestResult> data)
    {
        var first = data.First();

        Dictionary<string, Func<TestResult, object>> definition = new()
        {
            { "File", x => x.Result.InputFile },
            { "Test Name", x => x.TestName },
            { "Processing Time", x => x.Result.ProcessingTime },
            { "%", x => x.Result.ProcessingTime.AsPercentageComparingTo(first.Result.ProcessingTime) },
        };

        var table = new ConsoleTable([.. definition.Keys]);

        foreach (var d in data)
        {
            var values = definition.Select(x => x.Value(d));
            table.AddRow(values.ToArray());
        }

        table.Write();
    }
}

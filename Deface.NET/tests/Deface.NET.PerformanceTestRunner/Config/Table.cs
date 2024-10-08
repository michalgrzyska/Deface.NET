using ConsoleTables;

namespace Deface.NET.PerformanceTestRunner.Config;

public class Table
{
    public static void Display<T>(Dictionary<string, Func<T, object>> definition, IEnumerable<T> data)
    {
        var table = new ConsoleTable([.. definition.Keys]);

        foreach (var d in data)
        {
            var values = definition.Select(x => x.Value(d));
            table.AddRow(values.ToArray());
        }

        table.Write();
    }
}

using Deface.NET.PerformanceTestRunner.Config;
using Deface.NET.PerformanceTestRunner.TestScenarios;

var data = new PerformanceTestRunner()
    .AddScenariosFrom<RescaleScenario>()
    .Run();

var first = data.First();

Table.Display(new()
{
    { "File", x => x.Result.InputFile },
    { "Test Name", x => x.TestName },
    { "Processing Time", x => x.Result.ProcessingTime },
    { "%", x => x.Result.ProcessingTime.AsPercentageComparingTo(first.Result.ProcessingTime) },
}, data);
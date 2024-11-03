using Deface.NET.PerformanceTestRunner.Config;

namespace Deface.NET.PerformanceTestRunner.TestScenarios;


internal class RescaleScenario : VideoTestScenarioBase
{
    public RescaleScenario() : base(TestResources.TestResources.Video_Short_HD_1280_720_24fps)
    { }

    [Scenario("Default")]
    public async Task<ProcessingResult> Default()
    {
        return await Run();
    }
}

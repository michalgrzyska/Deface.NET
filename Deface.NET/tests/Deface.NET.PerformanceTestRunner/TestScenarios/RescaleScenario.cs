using Deface.NET.PerformanceTestRunner.Config;
using Deface.NET.TestResources;

namespace Deface.NET.PerformanceTestRunner.TestScenarios;


internal class RescaleScenario : VideoTestScenarioBase
{
    public RescaleScenario() : base(TestResources.TestResources.Video_Short_HD_1280_720_24fps)
    { }

    [Scenario("Default")]
    public ProcessingResult Default()
    {
        return Run();
    }

    [Scenario("480p")]
    public ProcessingResult Scale480p()
    {
        return Run(x =>
        {
            x.RescaleVideoWithShorterSideEqualsTo = 480;
        });
    }

    [Scenario("360p")]
    public ProcessingResult Scale360p()
    {
        return Run(x =>
        {
            x.RescaleVideoWithShorterSideEqualsTo = 360;
        });
    }
}

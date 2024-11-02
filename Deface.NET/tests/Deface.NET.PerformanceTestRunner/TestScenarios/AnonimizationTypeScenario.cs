using Deface.NET.PerformanceTestRunner.Config;

namespace Deface.NET.PerformanceTestRunner.TestScenarios;

public class AnonimizationTypeScenario : VideoTestScenarioBase
{
    public AnonimizationTypeScenario() : base(TestResources.TestResources.Video_Short_HD_1280_720_24fps)
    { }
    
    [Scenario("Ellipse GaussianBlur")]
    public async Task<ProcessingResult> Ellipse_GaussianBlur()
    {
        return await Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
            x.AnonimizationShape = AnonimizationShape.Ellipse;
        });
    }

    [Scenario("Ellipse Color")]
    public async Task<ProcessingResult> Ellipse_Color()
    {
        return await Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
            x.AnonimizationShape = AnonimizationShape.Ellipse;
        });
    }

    [Scenario("Rectangle GaussianBlur")]
    public async Task<ProcessingResult> Rectangle_GaussianBlur()
    {
        return await Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
            x.AnonimizationShape = AnonimizationShape.Rectangle;
        });
    }

    [Scenario("Rectangle Color")]
    public async Task<ProcessingResult> Rectangle_Color()
    {
        return await Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
            x.AnonimizationShape = AnonimizationShape.Rectangle;
        });
    }
}

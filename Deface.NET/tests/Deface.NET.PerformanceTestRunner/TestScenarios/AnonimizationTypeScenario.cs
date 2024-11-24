using Deface.NET.PerformanceTestRunner.Config;

namespace Deface.NET.PerformanceTestRunner.TestScenarios;

public class AnonimizationTypeScenario : VideoTestScenarioBase
{
    public AnonimizationTypeScenario() : base(TestResources.TestResources.Video_Short_640_360_24fps)
    { }

    [Scenario("Ellipse GaussianBlur")]
    public ProcessingResult Ellipse_GaussianBlur()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
            x.AnonimizationShape = AnonimizationShape.Ellipse;
        });
    }

    [Scenario("Ellipse Color")]
    public ProcessingResult Ellipse_Color()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
            x.AnonimizationShape = AnonimizationShape.Ellipse;
        });
    }

    [Scenario("Rectangle GaussianBlur")]
    public ProcessingResult Rectangle_GaussianBlur()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
            x.AnonimizationShape = AnonimizationShape.Rectangle;
        });
    }

    [Scenario("Rectangle Color")]
    public ProcessingResult Rectangle_Color()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
            x.AnonimizationShape = AnonimizationShape.Rectangle;
        });
    }

    [Scenario("Rectangle Mosaic")]
    public ProcessingResult Rectangle_Mosaic()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Mosaic;
            x.AnonimizationShape = AnonimizationShape.Rectangle;
        });
    }

    [Scenario("Ellipse Mosaic")]
    public ProcessingResult Ellipse_Mosaic()
    {
        return Run(x =>
        {
            x.AnonimizationMethod = AnonimizationMethod.Mosaic;
            x.AnonimizationShape = AnonimizationShape.Ellipse;
        });
    }
}

namespace Deface.NET.PerformanceTestRunner.Config;

[AttributeUsage(AttributeTargets.Method)]
public class ScenarioAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}

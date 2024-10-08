using System.Reflection;

namespace Deface.NET.PerformanceTestRunner.Config;

public class PerformanceTestRunner
{
    private List<ScenarioMethodToRun> scenarioMethodToRun = [];

    public PerformanceTestRunner AddScenariosFrom<T>() where T : class, new()
    {
        var methods = typeof(T)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.GetCustomAttribute<ScenarioAttribute>() != null)
            .ToArray();

        var testObj = new T();

        var preparedMethods = methods.Select(x =>
        {
            var description = x.GetCustomAttribute<ScenarioAttribute>()!.Description;
            return new ScenarioMethodToRun(x, testObj);
        });

        scenarioMethodToRun.AddRange(preparedMethods);
        return this;
    }

    public List<TestResult> Run()
    {
        List<TestResult> results = [];

        foreach (var s in scenarioMethodToRun)
        {
            Console.WriteLine($"Running {s.TestObj.GetType().Name}: {s.Method.Name}");
            ProcessingResult result = (ProcessingResult)s.Method.Invoke(s.TestObj, null)!;
            results.Add(new(s.Method.Name, result));
        }

        return results;
    }
}

public record ScenarioMethodToRun(MethodInfo Method, object TestObj);
public record TestResult(string TestName, ProcessingResult Result);
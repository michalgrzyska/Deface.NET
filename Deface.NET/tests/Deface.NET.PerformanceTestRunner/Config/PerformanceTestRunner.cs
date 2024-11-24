using System.Reflection;

namespace Deface.NET.PerformanceTestRunner.Config;

public class PerformanceTestRunner(string ffMpegPath, string ffProbePath)
{
    private readonly string _ffMpegPath = ffMpegPath;
    private readonly string _ffProbePath = ffProbePath;

    private readonly List<ScenarioMethodToRun> scenarioMethodToRun = [];

    public PerformanceTestRunner AddScenariosFrom<T>() where T : VideoTestScenarioBase, new()
    {
        var methods = typeof(T)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.GetCustomAttribute<ScenarioAttribute>() != null)
            .ToArray();

        T testObj = new();
        testObj.SetFilesPaths(_ffMpegPath, _ffProbePath);

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
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Deface.NET.Logging;

internal class DProgressLogger<T>(ILogger<T> logger, LoggingLevel loggingLevel)
{
    private readonly ILogger<T> _logger = logger;
    private readonly LoggingLevel _loggingLevel = loggingLevel;
    private readonly Stopwatch _stopwatch = new();

    private const int MinLoggingInterval = 1_000;

    private long _lastLogTime = 0;

    public void Start()
    {
        _stopwatch.Start();
    }

    public TimeSpan Stop()
    {
        _stopwatch.Stop();
        return _stopwatch.Elapsed;
    }

    public void Log(int currentStep, string message, int totalSteps)
    {
        if (totalSteps == 0)
        {
            return;
        }

        if (_loggingLevel < LoggingLevel.Detailed)
        {
            return;
        }

        if (_stopwatch.ElapsedMilliseconds - _lastLogTime < MinLoggingInterval)
        {
            return;
        }

        _lastLogTime = _stopwatch.ElapsedMilliseconds;

        var stepInfo = $"{currentStep}/{totalSteps}";
        var paddedStepInfo = stepInfo.PadLeft(stepInfo.Length + totalSteps.ToString().Length - currentStep.ToString().Length);

        var percentage = (currentStep * 100 / totalSteps).ToString();
        var paddedPercentInfo = $"{percentage}%".PadLeft(4);

        var time = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");

        var resultMessage = message + " | {StepInfo} | {PercentInfo} | {Time}";

        _logger.LogInformation(resultMessage, paddedStepInfo, paddedPercentInfo, time);
    }
}
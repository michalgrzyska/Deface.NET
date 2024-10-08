using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Deface.NET.Logging;

internal class DProgressLogger<T>(ILogger<T> logger, DefaceLoggingLevel loggingLevel, int totalSteps)
{
    private readonly ILogger<T> _logger = logger;
    private readonly DefaceLoggingLevel _loggingLevel = loggingLevel;
    private readonly int _totalSteps = totalSteps;
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

    public void LogProgress(int currentStep, string message, object[]? args = default)
    {
        if (_loggingLevel < DefaceLoggingLevel.Detailed)
        {
            return;
        }

        if (_stopwatch.ElapsedMilliseconds - _lastLogTime < MinLoggingInterval)
        {
            return;
        }

        _lastLogTime = _stopwatch.ElapsedMilliseconds;

        string stepInfo = $"{currentStep}/{_totalSteps}";
        string paddedStepInfo = stepInfo.PadLeft(stepInfo.Length + _totalSteps.ToString().Length - currentStep.ToString().Length);

        string percentage = (currentStep * 100 / _totalSteps).ToString();
        string paddedPercentInfo = $"{percentage}%".PadLeft(4);

        string time = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");

        string resultMessage = message + " | {StepInfo} | {PercentInfo} | {Time}";

        _logger.LogInformation(resultMessage, args, paddedStepInfo, paddedPercentInfo, time);
    }
}
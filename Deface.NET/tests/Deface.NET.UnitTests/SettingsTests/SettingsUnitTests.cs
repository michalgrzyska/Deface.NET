using FluentAssertions;

namespace Deface.NET.UnitTests.SettingsTests;

public class SettingsUnitTests : IDisposable
{
    private readonly TestSettingsProvider _settingsProvider;

    public SettingsUnitTests()
    {
        _settingsProvider = new();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(100.1)]
    [InlineData(101)]
    public void Threshold_IncorrectValues_ThrowsArgumentOutOfRangeException(float threshold)
    {
        var settings = _settingsProvider.GetSettings();

        Action<Settings> settingsAction = settings =>
        {
            settings.Threshold = threshold;
        };

        var testAction = () => settings.ApplyAction(settingsAction);

        testAction.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.25)]
    [InlineData(0.550)]
    [InlineData(0.7500001)]
    [InlineData(1)]
    public void Threshold_CorrectValues_NoExceptionThrown(float threshold)
    {
        var settings = _settingsProvider.GetSettings();

        Action<Settings> settingsAction = settings =>
        {
            settings.Threshold = threshold;
        };

        settings.ApplyAction(settingsAction);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void RunDetectionEachNFrames_IncorrectData_ThrowsArgumentOutOfRangeException(int runDetectionEachNFrames)
    {
        var settings = _settingsProvider.GetSettings();

        Action<Settings> settingsAction = settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        };

        var action = () => settings.ApplyAction(settingsAction);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(8)]
    [InlineData(27)]
    [InlineData(1000)]
    public void RunDetectionEachNFrames_CorrectData_NoExceptionThrown(int runDetectionEachNFrames)
    {
        var settings = _settingsProvider.GetSettings();

        Action<Settings> settingsAction = settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        };

        settings.ApplyAction(settingsAction);
    }

    public void Dispose()
    {
        _settingsProvider.Dispose();
    }
}

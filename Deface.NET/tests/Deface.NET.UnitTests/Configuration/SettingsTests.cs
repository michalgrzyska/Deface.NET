﻿using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.Configuration;

[Collection(nameof(SettingsCollection))]
public class SettingsTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(100.1)]
    [InlineData(101)]
    public void Threshold_IncorrectValues_ThrowsArgumentOutOfRangeException(float threshold)
    {
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.Threshold = threshold;
        };

        var testAction = () => settings.ApplyAction(settingsAction);

        testAction.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.25)]
    [InlineData(0.550)]
    [InlineData(0.7500001)]
    [InlineData(1)]
    public void Threshold_CorrectValues_NoExceptionThrown(float threshold)
    {
        var settings = _settingsFixture.Settings;

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
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        };

        var action = () => settings.ApplyAction(settingsAction);

        action.ShouldThrow<ArgumentOutOfRangeException>();
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
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.RunDetectionEachNFrames = runDetectionEachNFrames;
        };

        settings.ApplyAction(settingsAction);
    }

    [Theory]
    [InlineData(0.99999)]
    [InlineData(0)]
    [InlineData(0.9)]
    [InlineData(0.1)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void MaskScale_IncorrectData_ThrowsArgumentOutOfRangeException(float maskScale)
    {
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.MaskScale = maskScale;
        };

        var action = () => settings.ApplyAction(settingsAction);

        action.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1.0f)]
    [InlineData(1.1f)]
    [InlineData(2f)]
    [InlineData(500f)]
    public void MaskScale_CorrectData_NoExceptionThrown(float maskScale)
    {
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.MaskScale = maskScale;
        };

        settings.ApplyAction(settingsAction);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FFMpegPath_NullOrWhitespace_ArgumentNullExceptionThrown(string? value)
    {
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.FFMpegPath = value!;
        };

        var action = () => settings.ApplyAction(settingsAction);
        action.ShouldThrow<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void FFProbePath_NullOrWhitespace_ArgumentNullExceptionThrown(string? value)
    {
        var settings = _settingsFixture.Settings;

        Action<Settings> settingsAction = settings =>
        {
            settings.FFProbePath = value!;
        };

        var action = () => settings.ApplyAction(settingsAction);
        action.ShouldThrow<ArgumentNullException>();
    }
}

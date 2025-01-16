using Deface.NET.System.ExternalProcessing;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Helpers;
using Deface.NET.VideoIO.Models;
using NSubstitute;
using System.Text.Json;

namespace Deface.NET.UnitTests.VideoIO;

[Collection(nameof(SettingsCollection))]
public class VideoInfoProviderTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void GetInfo_OutputMatchesTestData()
    {
        // Arrange

        var testOutput = GetTestVideoInfoOutput();
        var settingsProvider = _settingsFixture.GetScopedSettingsProvider();
        var processFactory = GetExternalProcessFactory(testOutput);
        var path = "test";

        VideoInfoProvider videoInfoProvider = new(settingsProvider, processFactory);

        // Act

        var result = videoInfoProvider.GetInfo(path);

        // Assert

        var testStreamOutput = testOutput.Streams[0];

        result.ShouldBe(testStreamOutput.Width, testStreamOutput.Height);
        result.TotalFrames.ShouldBe(int.Parse(testStreamOutput.Frames));
        result.AverageFps.ShouldBe(VideoInfoHelper.ParseFrameRateString(testStreamOutput.AverageFrameRate));
        result.TargetFps.ShouldBe(VideoInfoHelper.ParseFrameRateString(testStreamOutput.TargetFrameRate));
        result.Path.ShouldBe(path);
    }

    private IExternalProcessFactory GetExternalProcessFactory(VideoInfoOutput testOutput)
    {
        var factory = Substitute.For<IExternalProcessFactory>();
        var process = Substitute.For<IExternalProcess>();

        var outputJson = JsonSerializer.Serialize(testOutput);

        process.ExecuteWithOutput().Returns(outputJson);

        factory
            .CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
            .Returns(process);

        return factory;
    }

    private static VideoInfoOutput GetTestVideoInfoOutput()
    {
        VideoInfoStreamOutput streamOutput = new()
        {
            Width = 100,
            Height = 200,
            Frames = "20",
            TargetFrameRate = "10/2",
            AverageFrameRate = "20/2"
        };

        return new() { Streams = [streamOutput] };
    }
}

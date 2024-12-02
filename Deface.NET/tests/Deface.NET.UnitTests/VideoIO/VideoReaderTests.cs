using Deface.NET.Configuration.Provider;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;
using NSubstitute;

namespace Deface.NET.UnitTests.VideoIO;

[Collection(nameof(SettingsCollection))]
public class VideoReaderTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void ReadVideo_DependenciesInvokedCorrectly()
    {
        // Arrange

        var settingsProvider = _settingsFixture.GetScopedSettingsProvider();
        var videoInfoProvider = GetVideoInfoProvider();
        var externalProcessFactory = GetExternalProcessFactory();

        VideoReader videoReader = new(settingsProvider, videoInfoProvider, externalProcessFactory);

        // Act

        Action<FrameInfo> action = _ => { };
        videoReader.ReadVideo(action, "whateva");

        // Assert

        externalProcessFactory.Received(1).CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>());
        videoInfoProvider.Received(1).GetInfo(Arg.Any<string>());
    }

    private IVideoInfoProvider GetVideoInfoProvider()
    {
        var videoInfoProvider = Substitute.For<IVideoInfoProvider>();
        VideoInfo videoInfo = new(0, 0, 0, 0, 0, "path");

        videoInfoProvider.GetInfo(Arg.Any<string>()).Returns(videoInfo);

        return videoInfoProvider;
    }

    private static IExternalProcessFactory GetExternalProcessFactory()
    {
        var externalProcessFactory = Substitute.For<IExternalProcessFactory>();

        var externalProcess = Substitute.For<IExternalProcess>();
        externalProcess.OutputStream.Returns(new MemoryStream([]));

        externalProcessFactory
            .CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
            .Returns(externalProcess);

        return externalProcessFactory;
    }
}

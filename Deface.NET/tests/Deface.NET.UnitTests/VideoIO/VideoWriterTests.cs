using Deface.NET.Configuration.Provider;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Models;
using NSubstitute;

namespace Deface.NET.UnitTests.VideoIO;

[Collection(nameof(SettingsCollection))]
public class VideoWriterTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public void WriteVideo_NFrames_DependenciesCalledCorrectly(int frameCount)
    {
        // Arrange

        var settingsProvider = _settingsFixture.GetScopedSettingsProvider();

        TestStream stream = new();
        var externalProcessFactory = GetExternalProcessFactory(stream);

        var frames = Enumerable.Range(0, frameCount).Select(x => TestFrameHelper.GetTestFrame()).ToList();

        VideoWriter writer = new(settingsProvider, externalProcessFactory);

        // Act

        var firstFrame = frames[0];
        VideoInfo videoInfo = new(firstFrame.Width, firstFrame.Height, frames.Count, 1, 1, "path");

        writer.WriteVideo(frames, videoInfo, "");

        // Assert

        externalProcessFactory.Received(1).CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>());
        stream.WriteCount.Should().Be(frames.Count);
    }

    private static IExternalProcessFactory GetExternalProcessFactory(Stream stream)
    {
        var externalProcessFactory = Substitute.For<IExternalProcessFactory>();

        var externalProcess = Substitute.For<IExternalProcess>();
        externalProcess.InputStream.Returns(stream);

        externalProcessFactory
            .CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
            .Returns(externalProcess);

        return externalProcessFactory;
    }
}

file class TestStream : Stream
{
    public int WriteCount { get; private set; }

    public override void Write(byte[] buffer, int offset, int count)
    {
        WriteCount++;
    }

    public override bool CanRead => default;
    public override bool CanSeek => default;
    public override bool CanWrite => default;
    public override long Length => default;
    public override long Position { get => default; set { } }
    public override void Flush() { }
    public override int Read(byte[] buffer, int offset, int count) => default;
    public override long Seek(long offset, SeekOrigin origin) => default;
    public override void SetLength(long value) { }
}
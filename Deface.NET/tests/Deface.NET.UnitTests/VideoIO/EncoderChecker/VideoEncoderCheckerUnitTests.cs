using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.VideoIO;
using NSubstitute;

namespace Deface.NET.UnitTests.VideoIO.EncoderChecker;

[Collection(nameof(SettingsCollection))]
public class VideoEncoderCheckerUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void CheckFfmpegCodecs_ValidString_ShouldNotThrow()
    {
        var checker = GetVideoEncoderChecker(EncodingCodec.H264, VideoCheckerUnitTestsContants.ValidOutput);

        var action = checker.CheckFfmpegCodecs;

        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(VideoCheckerUnitTestsContants.NoVP9, EncodingCodec.VP9)]
    [InlineData(VideoCheckerUnitTestsContants.NoH264, EncodingCodec.H264)]
    [InlineData(VideoCheckerUnitTestsContants.NoBoth, EncodingCodec.VP9)]
    [InlineData(VideoCheckerUnitTestsContants.NoBoth, EncodingCodec.H264)]
    public void CheckFfmpegCodecs_NoCodecAvailable_ShouldThrowDefaceException(string output, EncodingCodec encodingCodec)
    {
        var checker = GetVideoEncoderChecker(encodingCodec, output);

        var action = checker.CheckFfmpegCodecs;

        action.ShouldThrow<DefaceException>();
    }

    [Theory]
    [InlineData(VideoCheckerUnitTestsContants.NoEncodersPrepended, EncodingCodec.H264)]
    [InlineData(VideoCheckerUnitTestsContants.NoEncodersPrepended, EncodingCodec.VP9)]
    [InlineData("", EncodingCodec.VP9)]
    [InlineData("", EncodingCodec.H264)]
    [InlineData("whatever", EncodingCodec.VP9)]
    [InlineData("whatever", EncodingCodec.H264)]
    [InlineData(" ", EncodingCodec.VP9)]
    [InlineData(" ", EncodingCodec.H264)]
    public void CheckFfmpegCodecs_InvalidOutput_ShouldThrowInvalidOperationException(string output, EncodingCodec encodingCodec)
    {
        var checker = GetVideoEncoderChecker(encodingCodec, output);

        var action = checker.CheckFfmpegCodecs;

        action.ShouldThrow<InvalidOperationException>();
    }

    private VideoEncoderChecker GetVideoEncoderChecker(EncodingCodec codec, string output)
    {
        var settingsProvider = GetSettings(codec);
        var externalProcessFactory = GetExternalProcessFactory(output);

        return new(settingsProvider, externalProcessFactory);
    }

    private IScopedSettingsProvider GetSettings(EncodingCodec codec)
    {
        return _settingsFixture.GetScopedSettingsProvider(x => x.EncodingCodec = codec);
    }

    private IExternalProcessFactory GetExternalProcessFactory(string output)
    {
        var externalProcess = Substitute.For<IExternalProcess>();
        externalProcess.ExecuteWithOutput().Returns(output);

        var externalProcessFactory = Substitute.For<IExternalProcessFactory>();

        externalProcessFactory
            .CreateExternalProcess(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
            .Returns(externalProcess);

        return externalProcessFactory;
    }
}

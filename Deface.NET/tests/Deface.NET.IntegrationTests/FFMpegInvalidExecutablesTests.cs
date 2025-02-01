using Deface.NET.IntegrationTests.Resources;
using Shouldly;

namespace Deface.NET.IntegrationTests;

public class FFMpegInvalidExecutablesTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("whatever")]
    public void InvalidFFMpegPath_ShouldThrowDefaceException(string path)
    {
        var action = () => DefaceProvider.GetDefaceService(x =>
        {
            x.FFMpegPath = path;
        });

        action.ShouldThrow<DefaceException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("whatever")]
    public void InvalidFFProbePath_ShouldThrowDefaceException(string path)
    {
        var action = () => DefaceProvider.GetDefaceService(x =>
        {
            x.FFProbePath = path;
        });

        action.ShouldThrow<DefaceException>();
    }
}

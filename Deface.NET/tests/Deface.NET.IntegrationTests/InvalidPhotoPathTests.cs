using Deface.NET.Common;
using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class InvalidPhotoPathTests : BaseIntegrationTest
{
    [Fact]
    public void ProcessImage_InvalidOutputPath_ThrowsProperException()
    {
        var location = "Y://fakeLocation";

        var action = () => DefaceService().ProcessImage(TestResources.TestResources.Photo1, location);

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<InvalidOperationException>(string.Format(ExceptionMessages.InvalidOutputPath, location));
    }

    [Fact]
    public void ProcessImages_InvalidOutputPath_ThrowsProperException()
    {
        var location = "Y://fakeLocation";

        var action = () => DefaceService().ProcessImages(TestResources.TestResources.PhotosDir, location);

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<DirectoryNotFoundException>(string.Format(ExceptionMessages.DirectoryMustExist, "outputDirectory"));
    }
}

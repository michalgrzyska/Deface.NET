using Deface.NET.Graphics;
using Deface.NET.Graphics.Models;
using Deface.NET.System;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics;

public class FrameCreatorTests
{
    private readonly FrameCreator _frameCreator;

    public FrameCreatorTests()
    {
        FileSystem fileSystem = new();
        _frameCreator = new FrameCreator(fileSystem);
    }

    [Fact]
    public void FromFile_ProperImagePath_CreatesFrame()
    {
        var path = TestResources.TestResources.Photo1;

        var frame = _frameCreator.FromFile(path);

        frame.ShouldBe(1280, 946);
    }

    [Fact]
    public void FromFile_InvalidPath_ThrowsDefaceException()
    {
        var path = Guid.NewGuid().ToString();

        var action = () => _frameCreator.FromFile(path);

        action.Should().Throw<DefaceException>();
    }

    [Fact]
    public void FromBgrArray_RedImage_FrameIsCorrectAndBlue()
    {
        // Arrange

        var width = 1000;
        var height = 1000;

        var bgraArray = CreateRedBgraImage(1000, 1000);

        // Act

        var result = _frameCreator.FromBgraArray(bgraArray, width, height);

        // Assert

        result.ShouldBe(width, height);

        ShapeTestHelper.ValidateWholeFrame(result, pixel =>
        {
            pixel.ShouldBe(255, 0, 0);
        });
    }

    private static byte[] CreateRedBgraImage(int width, int height)
    {
        int bytesPerPixel = Frame.ChannelsCount;
        int imageSize = width * height * bytesPerPixel;

        byte[] imageData = new byte[imageSize];

        for (int i = 0; i < imageSize; i += bytesPerPixel)
        {
            imageData[i] = 0;
            imageData[i + 1] = 0;
            imageData[i + 2] = 255;
            imageData[i + 3] = 255;
        }

        return imageData;
    }
}

using Deface.NET.Graphics;
using Deface.NET.Graphics.Models;
using Deface.NET.System;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class FrameCreatorTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void FromFile_ProperImagePath_CreatesFrame()
    {
        var path = TestResources.TestResources.Photo1;

        var frame = GetFrameCreator().FromFile(path);

        frame.ShouldBe(1280, 946);
    }

    [Fact]
    public void FromFile_InvalidPath_ThrowsFileNotFoundException()
    {
        var path = Guid.NewGuid().ToString();

        var action = () => GetFrameCreator().FromFile(path);

        action.ShouldThrow<FileNotFoundException>();
    }

    [Fact]
    public void FromBgrArray_RedImage_FrameIsCorrectAndBlue()
    {
        // Arrange

        var width = 1000;
        var height = 1000;

        var bgraArray = CreateRedBgraImage(1000, 1000);

        // Act

        var result = GetFrameCreator().FromBgraArray(bgraArray, width, height);

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

    private FrameCreator GetFrameCreator(Action<Settings>? action = null)
    {
        var settingsProvider = _settingsFixture.GetSettingsProvider(action);
        FileSystem fileSystem = new(settingsProvider);

        return new FrameCreator(fileSystem);
    }
}

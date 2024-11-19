using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection.UltraFace;

namespace Deface.NET.UnitTests.ObjectDetection;

public class UltraFaceDetectorUnitTests : IDisposable
{
    private readonly UltraFaceDetector _detector = new();

    [Theory]
    [InlineData(TestResources.TestResources.Photo1, 1)]
    [InlineData(TestResources.TestResources.Photo2, 1)]
    [InlineData(TestResources.TestResources.Photo3, 2)]
    [InlineData(TestResources.TestResources.Photo4, 7)]
    [InlineData(TestResources.TestResources.Photo5, 6)]
    [InlineData(TestResources.TestResources.Photo6, 0)]
    public void Detect_ProperData_DetectsProperAmountOfFaces(string imagePath, int expectedFaces)
    {
        using FileStream stream = new(imagePath, FileMode.Open);
        Frame frame = new(stream);

        var result = _detector.Detect(frame, 0.6f);

        result.Count.Should().Be(expectedFaces);
    }

    public void Dispose() => _detector.Dispose();
}

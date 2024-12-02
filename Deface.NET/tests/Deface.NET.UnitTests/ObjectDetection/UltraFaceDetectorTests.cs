using Deface.NET.ObjectDetection.UltraFace;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.ObjectDetection;

public class UltraFaceDetectorTests : IDisposable
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
        var frame = TestFrameHelper.GetTestFrame(imagePath);

        var result = _detector.Detect(frame, 0.6f);

        result.Count.Should().Be(expectedFaces);
    }

    public void Dispose() => _detector.Dispose();
}

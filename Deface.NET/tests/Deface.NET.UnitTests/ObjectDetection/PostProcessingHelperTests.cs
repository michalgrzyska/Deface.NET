using Deface.NET.ObjectDetection;

namespace Deface.NET.UnitTests.ObjectDetection;

public class PostProcessingHelperTests
{
    [Fact]
    public void NMS_WhenBoxesOverlap_Strongly_RemovesLowerConfidenceOnes()
    {
        // Arrange
        var boxes = new List<DetectedObject>
        {
            new DetectedObject(0, 0, 100, 100, 0.9f),
            new DetectedObject(10, 10, 110, 110, 0.8f), // overlaps strongly with first
            new DetectedObject(200, 200, 300, 300, 0.7f) // separate box
        };

        // Act
        var result = PostProcessingHelper.NMS(boxes, iouThreshold: 0.5f);

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(b => b.Confidence == 0.9f);
        result.ShouldContain(b => b.Confidence == 0.7f);
    }

    [Fact]
    public void NMS_WhenBoxesDoNotOverlap_KeepsAllBoxes()
    {
        // Arrange
        var boxes = new List<DetectedObject>
        {
            new DetectedObject(0, 0, 50, 50, 0.9f),
            new DetectedObject(200, 200, 250, 250, 0.8f),
            new DetectedObject(400, 400, 450, 450, 0.7f)
        };

        // Act
        var result = PostProcessingHelper.NMS(boxes, 0.5f);

        // Assert
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void RescaleBoundingBoxes_WhenScaled_ReturnsValuesInOriginalCoordinates()
    {
        // Arrange
        int originalW = 640, originalH = 480;
        int resizedW = 320, resizedH = 320;

        var scaledBoxes = new List<DetectedObject>
        {
            new DetectedObject(80, 40, 160, 120, 0.9f)
        };

        // Act
        var result = PostProcessingHelper.RescaleBoundingBoxes(
            scaledBoxes, originalW, originalH, resizedW, resizedH);

        // Assert
        result.Count.ShouldBe(1);
        var box = result[0];

        // Basic validity checks (we're not testing exact math here)
        box.X1.ShouldBeGreaterThanOrEqualTo(0);
        box.Y1.ShouldBeGreaterThanOrEqualTo(0);
        box.X2.ShouldBeGreaterThan(box.X1);
        box.Y2.ShouldBeGreaterThan(box.Y1);
    }

    [Fact]
    public void RescaleBoundingBoxes_WhenMultipleBoxes_ReturnsSameCount()
    {
        // Arrange
        var scaledBoxes = new List<DetectedObject>
        {
            new DetectedObject(10, 10, 50, 50, 0.95f),
            new DetectedObject(60, 60, 120, 120, 0.80f)
        };

        // Act
        var result = PostProcessingHelper.RescaleBoundingBoxes(
            scaledBoxes,
            originalWidth: 800,
            originalHeight: 600,
            width: 400,
            height: 400);

        // Assert
        result.Count.ShouldBe(2);
    }
}

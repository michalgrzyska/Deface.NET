using Deface.NET.ObjectDetection;

namespace Deface.NET.UnitTests.ObjectDetection;

public class DetectedObjectTests
{
    [Fact]
    public void GetResized_WithScaleFactorOfOne_ShouldReturnSameObject()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 20, 20, 0.9f);

        // Act
        var result = detectedObject.GetResized(1.0f);

        // Assert
        result.ShouldBeSameAs(detectedObject);
    }

    [Fact]
    public void GetResized_WhenAlreadyResized_ShouldReturnSameObject()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 20, 20, 0.9f, IsResized: true);

        // Act
        var result = detectedObject.GetResized(2.0f);

        // Assert
        result.ShouldBeSameAs(detectedObject);
    }

    [Fact]
    public void GetResized_WithLargerScaleFactor_ShouldIncreaseBoundingBoxSize()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 20, 20, 0.9f);
        var scaleFactor = 2.0f;

        // Act
        var result = detectedObject.GetResized(scaleFactor);

        // Assert
        result.X1.ShouldBe(5);
        result.Y1.ShouldBe(5);
        result.X2.ShouldBe(25);
        result.Y2.ShouldBe(25);
        result.IsResized.ShouldBeTrue();
    }

    [Fact]
    public void GetResized_WithSmallerScaleFactor_ShouldDecreaseBoundingBoxSize()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 30, 30, 0.9f);
        var scaleFactor = 0.5f;

        // Act
        var result = detectedObject.GetResized(scaleFactor);

        // Assert
        result.X1.ShouldBe(15);
        result.Y1.ShouldBe(15);
        result.X2.ShouldBe(25);
        result.Y2.ShouldBe(25);
        result.IsResized.ShouldBeTrue();
    }

    [Fact]
    public void GetResized_ShouldMaintainCenterPosition()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 30, 30, 0.9f);
        var scaleFactor = 2.0f;
        var expectedCenterX = (detectedObject.X1 + detectedObject.X2) / 2;
        var expectedCenterY = (detectedObject.Y1 + detectedObject.Y2) / 2;

        // Act
        var result = detectedObject.GetResized(scaleFactor);
        var resultCenterX = (result.X1 + result.X2) / 2;
        var resultCenterY = (result.Y1 + result.Y2) / 2;

        // Assert
        resultCenterX.ShouldBe(expectedCenterX);
        resultCenterY.ShouldBe(expectedCenterY);
    }

    [Fact]
    public void GetResized_SinglePointBoundingBox_ShouldRemainSinglePoint()
    {
        // Arrange
        var detectedObject = new DetectedObject(10, 10, 10, 10, 0.9f);

        // Act
        var result = detectedObject.GetResized(2.0f);

        // Assert
        result.X1.ShouldBe(10);
        result.Y1.ShouldBe(10);
        result.X2.ShouldBe(10);
        result.Y2.ShouldBe(10);
        result.IsResized.ShouldBeTrue();
    }
}
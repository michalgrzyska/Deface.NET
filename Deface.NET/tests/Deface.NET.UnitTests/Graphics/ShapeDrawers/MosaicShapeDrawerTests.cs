using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.ObjectDetection;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics.ShapeDrawers;

[Collection(nameof(SettingsCollection))]
public class MosaicShapeDrawerTests(SettingsFixture settingsFixture) : ShapeDrawerTestsBase(settingsFixture)
{
    protected override AnonimizationMethod AnonimizationMethod => AnonimizationMethod.Mosaic;

    [Fact]
    public override void DrawObject_NoObjects_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            var originalFrame = GetTestFrame();
            var result = drawer.Draw(frame, []);

            ShapeTestHelper.ValidateWholeFrame(frame, pixel =>
            {
                var originalPixel = originalFrame.GetPixel(pixel.X, pixel.Y);
                pixel.ShouldBe(originalPixel);
            });
        });
    }

    [Fact]
    public override void DrawObject_SingleRectangle_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            var originalFrame = GetTestFrame();
            var result = drawer.Draw(frame, [Object1]);

            ValidateFrameForObject(originalFrame, frame, Object1, settings);
        });
    }

    [Fact]
    public override void DrawObject_SingleEllipse_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Ellipse, (settings, frame, drawer) =>
        {
            var originalFrame = GetTestFrame();
            var result = drawer.Draw(frame, [Object1]);

            ValidateFrameForObject(originalFrame, frame, Object1, settings);
        });
    }

    [Fact]
    public override void DrawObject_MultipleEllipses_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Ellipse, (settings, frame, drawer) =>
        {
            List<DetectedObject> detectedObjects = [Object1, Object2, Object3];

            var originalFrame = GetTestFrame();
            var result = drawer.Draw(frame, detectedObjects);

            foreach (var detectedObject in detectedObjects)
            {
                ValidateFrameForObject(originalFrame, frame, detectedObject, settings);
            }
        });
    }

    [Fact]
    public override void DrawObject_MultipleRectangles_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            List<DetectedObject> detectedObjects = [Object1, Object2, Object3];

            var originalFrame = GetTestFrame();
            var result = drawer.Draw(frame, detectedObjects);

            foreach (var detectedObject in detectedObjects)
            {
                ValidateFrameForObject(originalFrame, frame, detectedObject, settings);
            }
        });
    }

    private static void ValidateFrameForObject(Frame originalFrame, Frame resultFrame, DetectedObject obj, Settings settings)
    {
        List<PixelData> actualPixels = [];
        List<Pixel> originalPixels = [];

        Action<Frame, DetectedObject, Action<PixelData>> validateFn = settings.AnonimizationShape switch
        {
            AnonimizationShape.Ellipse => ShapeTestHelper.ValidateEllipse,
            AnonimizationShape.Rectangle => ShapeTestHelper.ValidateRectangle,
            _ => throw new NotImplementedException(),
        };

        validateFn(resultFrame, obj, actualPixel =>
        {
            var originalPixel = originalFrame.GetPixel(actualPixel.X, actualPixel.Y);

            actualPixels.Add(actualPixel);
            originalPixels.Add(originalPixel);
        });

        var originalUniqueColors = originalPixels.GroupBy(x => new { x.R, x.G, x.B }).Count();
        var actualUniqueColors = actualPixels.GroupBy(x => new { x.R, x.G, x.B }).Count();

        actualUniqueColors.Should().BeLessThan(originalUniqueColors);
    }

    internal override IShapeDrawer GetShapeDrawer(Settings settings)
    {
        return new MosaicShapeDrawer(settings);
    }

    internal override Frame GetTestFrame()
    {
        return TestFrameHelper.GetTestFrame(TestResources.TestResources.Photo2);
    }

    internal override void ValidatePixel(PixelData pixel, Settings settings) { }
    internal override void ValidateWholeFramePixel(PixelData pixel) { }
}
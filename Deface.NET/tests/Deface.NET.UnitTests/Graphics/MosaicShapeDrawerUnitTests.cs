//using Deface.NET.Graphics.Drawers;
//using Deface.NET.Graphics;
//using Deface.NET.ObjectDetection;
//using Deface.NET.UnitTests._TestsConfig;
//using Deface.NET.UnitTests.Graphics.Helpers;
//using SkiaSharp;

//namespace Deface.NET.UnitTests.Graphics;

//[Collection(nameof(SettingsCollection))]
//public class MosaicShapeDrawerUnitTests(SettingsFixture settingsFixture)
//{
//    private readonly SettingsFixture _settingsFixture = settingsFixture;

//    private readonly DetectedObject Object1 = new(10, 10, 100, 100, 1, IsEnlarged: true);
//    private readonly DetectedObject Object2 = new(110, 110, 200, 200, 1, IsEnlarged: true);
//    private readonly DetectedObject Object3 = new(210, 210, 400, 400, 1, IsEnlarged: true);

//    [Fact]
//    public void X()
//    {
//        Settings settings = GetSettings(AnonimizationShape.Rectangle);
//        Frame frame = TestFrameHelper.GetTestFrameWithMesh();

//        MosaicShapeDrawer drawer = new(settings);
//        Frame result = drawer.Draw(frame, [Object1]);

//        ValidateMosaic(frame, Object1);
//    }

//    private Settings GetSettings(AnonimizationShape shape)
//    {
//        return _settingsFixture.WithAction(x =>
//        {
//            x.AnonimizationShape = shape;
//            x.AnonimizationMethod = AnonimizationMethod.Mosaic;
//        });
//    }

//    private void ValidateMosaic(Frame frame, DetectedObject detectedObject)
//    {
//        var (mosaicW, mosaicH) = MosaicShapeDrawer.GetMosaicSize(frame);
//        var nativeElement = frame.GetNativeElement();

//        for (int y = detectedObject.Y1; y < detectedObject.Y2; y += mosaicH)
//        {
//            for (int x = detectedObject.X1; x < detectedObject.X2; x += mosaicW)
//            {
//                if (x + mosaicW > detectedObject.X2 || y + mosaicH > detectedObject.Y2)
//                {
//                    continue;
//                }

//                ValidateMosaicTile(nativeElement, x, y, mosaicW, mosaicH);
//            }
//        }
//    }

//    private void ValidateMosaicTile(SKBitmap nativeElement, int firstPixelX, int firstPixelY, int mosaicW, int mosaicH)
//    {
//        var firstPixel = nativeElement.GetPixel(firstPixelX, firstPixelY);

//        for (int y = firstPixelY; y < firstPixelY + mosaicH; y++)
//        {
//            for (int x = firstPixelX; x < firstPixelX + mosaicW; x++)
//            {
//                var pixelUnderTest = nativeElement.GetPixel(x, y);

//                pixelUnderTest.Red.Should().Be(firstPixel.Red);
//                pixelUnderTest.Green.Should().Be(firstPixel.Green);
//                pixelUnderTest.Blue.Should().Be(firstPixel.Blue);
//            }
//        }
//    }
//}

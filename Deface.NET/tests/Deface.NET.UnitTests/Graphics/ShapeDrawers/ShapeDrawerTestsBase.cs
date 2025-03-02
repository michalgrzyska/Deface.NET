using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using Deface.NET.Tests.Shared.Helpers;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics.ShapeDrawers;

/// <summary>
/// Some of members are internal due to C# accesibility rules.
/// </summary>
public abstract class ShapeDrawerTestsBase(SettingsFixture settingsFixture)
{
    protected readonly SettingsFixture _settingsFixture = settingsFixture;

    internal readonly DetectedObject Object1 = new(10, 10, 100, 100, 1, IsResized: true);
    internal readonly DetectedObject Object2 = new(110, 110, 200, 200, 1, IsResized: true);
    internal readonly DetectedObject Object3 = new(210, 210, 400, 400, 1, IsResized: true);

    protected abstract AnonimizationMethod AnonimizationMethod { get; }

    [Fact]
    public virtual void DrawObject_NoObjects_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            var result = drawer.Draw(frame, []);
            ShapeTestHelper.ValidateWholeFrame(frame, ValidateWholeFramePixel);
        });
    }

    [Fact]
    public virtual void DrawObject_SingleRectangle_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            var result = drawer.Draw(frame, [Object1]);
            ShapeValidationHelper.ValidateRectangle(result, Object1, p => ValidatePixel(p, settings));
        });
    }

    [Fact]
    public virtual void DrawObject_SingleEllipse_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Ellipse, (settings, frame, drawer) =>
        {
            var result = drawer.Draw(frame, [Object1]);
            ShapeTestHelper.ValidateEllipse(result, Object1, p => ValidatePixel(p, settings));
        });
    }

    [Fact]
    public virtual void DrawObject_MultipleEllipses_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Ellipse, (settings, frame, drawer) =>
        {
            List<DetectedObject> objects = [Object1, Object2, Object3];
            var result = drawer.Draw(frame, objects);

            foreach (var obj in objects)
            {
                ShapeTestHelper.ValidateEllipse(result, obj, p => ValidatePixel(p, settings));
            }
        });
    }

    [Fact]
    public virtual void DrawObject_MultipleRectangles_DrawnCorrectly()
    {
        WithTestData(AnonimizationShape.Rectangle, (settings, frame, drawer) =>
        {
            List<DetectedObject> objects = [Object1, Object2, Object3];
            var result = drawer.Draw(frame, objects);

            foreach (var obj in objects)
            {
                ShapeTestHelper.ValidateEllipse(result, obj, p => ValidatePixel(p, settings));
            }
        });
    }

    internal void WithTestData(AnonimizationShape shape, Action<Settings, Frame, IShapeDrawer> action)
    {
        var settings = GetSettings(shape);
        var frame = GetTestFrame();
        var drawer = GetShapeDrawer(settings);

        action(settings, frame, drawer);
    }

    protected Settings GetSettings(AnonimizationShape shape)
    {
        return _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = shape;
            x.AnonimizationMethod = AnonimizationMethod;
        });
    }

    internal abstract IShapeDrawer GetShapeDrawer(Settings settings);
    internal abstract Frame GetTestFrame();
    internal abstract void ValidateWholeFramePixel(PixelData pixel);
    internal abstract void ValidatePixel(PixelData pixel, Settings settings);
}

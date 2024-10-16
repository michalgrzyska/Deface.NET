using OpenCvSharp;

namespace Deface.NET.CenterFace;

internal sealed class FaceInfo
{
    private bool _isEnlarged = false;

    public float X1 { get; internal set; }
    public float Y1 { get; internal set; }
    public float X2 { get; internal set; }
    public float Y2 { get; internal set; }
    public float Score { get; internal set; }
    public float Area { get; internal set; }
    public float[] Landmarks { get; } = new float[10];

    public (Point Center, Size Axes) ToEllipse()
    {
        var centerX = (X1 + X2) / 2;
        var centerY = (Y1 + Y2) / 2;

        var axisX = (X2 - X1) / 2;
        var axisY = (Y2 - Y1) / 2;

        Point center = new(centerX, centerY);
        Size axes = new(axisX, axisY);

        return new(center, axes);
    }

    public Rect ToRect()
    {
        return new((int)X1, (int)Y1, (int)X2 - (int)X1, (int)Y2 - (int)Y1);
    }

    public void Enlarge(double factor)
    {
        if (_isEnlarged)
        {
            return;
        }

        var width = X2 - X1;
        var height = Y2 - Y1;

        var newWidth = width * factor;
        var newHeight = height * factor;

        var centerX = (X1 + X2) / 2;
        var centerY = (Y1 + Y2) / 2;

        X1 = (float)(centerX - newWidth / 2);
        X2 = (float)(centerX + newWidth / 2);
        Y1 = (float)(centerY - newHeight / 2);
        Y2 = (float)(centerY + newHeight / 2);

        _isEnlarged = true;
    }
}
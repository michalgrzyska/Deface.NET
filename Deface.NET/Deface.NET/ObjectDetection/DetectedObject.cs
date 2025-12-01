namespace Deface.NET.ObjectDetection;

internal record DetectedObject(int X1, int Y1, int X2, int Y2, float Confidence, bool IsResized = false)
{
    public DetectedObject(float X1, float Y1, float X2, float Y2, float Confidence) 
        : this((int)X1, (int)Y1, (int)X2, (int)Y2, Confidence)
    { }

    public DetectedObject GetResized(float scaleFactor = 1.0f)
    {
        if (IsResized || scaleFactor == 1.0f)
        {
            return this;
        }

        var width = X2 - X1;
        var height = Y2 - Y1;

        var newWidth = width * scaleFactor;
        var newHeight = height * scaleFactor;

        var centerX = (X1 + X2) / 2;
        var centerY = (Y1 + Y2) / 2;

        var x1 = (int)(centerX - newWidth / 2);
        var x2 = (int)(centerX + newWidth / 2);
        var y1 = (int)(centerY - newHeight / 2);
        var y2 = (int)(centerY + newHeight / 2);

        return new(x1, y1, x2, y2, Confidence, IsResized: true);
    }
}

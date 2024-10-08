using OpenCvSharp;

namespace Deface.NET.CenterFace;

internal class CenterFaceParams
{
    public int DH { get; private init; }
    public int DW { get; private init; }

    public float DScaleH { get; private init; }
    public float DScaleW { get; private init; }

    public float ScaleH { get; private init; }
    public float ScaleW { get; private init; }

    public int ImageH { get; private init; }
    public int ImageW { get; private init; }

    public float ScoreThreshold { get; private init; }
    public float NmsThreshold { get; private init; }

    public CenterFaceParams(Mat image, int resizedW, int resizedH, float scoreThreshold, float nmsThreshold)
    {
        ImageH = image.Height;
        ImageW = image.Width;

        ScaleW = (float)ImageW / resizedW;
        ScaleH = (float)ImageH / resizedH;

        DH = (int)(Math.Ceiling((float)resizedH / 32) * 32);
        DW = (int)(Math.Ceiling((float)resizedW / 32) * 32);

        DScaleH = (float)resizedH / DH;
        DScaleW = (float)resizedW / DW;

        ScoreThreshold = scoreThreshold;
        NmsThreshold = nmsThreshold;
    }
}

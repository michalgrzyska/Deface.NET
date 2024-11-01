using Microsoft.ML.Data;

namespace Deface.NET.ObjectDetection.UltraFace;

internal class Input
{
    [VectorType(1, 3, 480, 640)]
    [ColumnName("input")]
    public float[] Image { get; set; }

    public Input(float[] image)
    {
        Image = image;
    }
}
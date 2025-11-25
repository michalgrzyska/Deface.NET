using Microsoft.ML.Data;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

internal class Input(float[] image)
{
    [ColumnName("input")]
    [VectorType(1, 3, 640, 640)]
    public float[] Image { get; set; } = image;
}
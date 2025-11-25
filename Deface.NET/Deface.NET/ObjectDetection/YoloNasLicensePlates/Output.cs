using Microsoft.ML.Data;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

internal class Output
{
    [ColumnName("output")]
    public float[] Boxes { get; set; } = [];

    // 1711: shape [1, N, D]
    [ColumnName("1711")]
    public float[] Scores { get; set; } = [];
}
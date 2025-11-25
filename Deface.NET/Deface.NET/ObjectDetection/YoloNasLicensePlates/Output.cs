using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

[ExcludeFromCodeCoverage]
internal class Output
{
    [ColumnName("output")]
    public float[] Boxes { get; set; } = [];

    // 1711: shape [1, N, D]
    [ColumnName("1711")]
    public float[] Scores { get; set; } = [];
}
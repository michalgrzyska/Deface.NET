using Deface.NET.ObjectDetection.UltraFace;
using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.LicensePlates;

[ExcludeFromCodeCoverage]
internal class Output
{
    [VectorType(8400, 4)]
    [ColumnName("boxes")]
    public float[] Boxes { get; set; } = [];

    [VectorType(8400, 1)]
    [ColumnName("scores")]
    public float[] Scores { get; set; } = [];
}
using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

[ExcludeFromCodeCoverage]
internal class Output
{
    [VectorType(1, 17640, 2)]
    [ColumnName("scores")]
    public float[] Scores { get; set; } = [];

    [VectorType(1, 17640, 4)]
    [ColumnName("boxes")]
    public float[] Boxes { get; set; } = [];
}

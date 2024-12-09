using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

[ExcludeFromCodeCoverage]
internal class Output
{
    [VectorType(1, 17640, 2)]
    [ColumnName(UltraFaceConstants.Scores)]
    public float[] Scores { get; set; } = [];

    [VectorType(1, 17640, 4)]
    [ColumnName(UltraFaceConstants.Boxes)]
    public float[] Boxes { get; set; } = [];
}

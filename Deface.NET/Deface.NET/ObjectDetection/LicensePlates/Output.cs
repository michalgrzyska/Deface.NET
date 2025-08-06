using Deface.NET.ObjectDetection.UltraFace;
using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.LicensePlates;

[ExcludeFromCodeCoverage]
internal class Output
{
    [VectorType(1, 25200, 6)]
    [ColumnName("output0")]
    public float[] Scores { get; set; } = [];
}

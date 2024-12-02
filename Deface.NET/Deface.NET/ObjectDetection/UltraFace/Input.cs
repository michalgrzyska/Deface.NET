using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

[ExcludeFromCodeCoverage]
internal class Input(float[] image)
{
    [VectorType(1, 3, 480, 640)]
    [ColumnName("input")]
    public float[] Image { get; set; } = image;
}
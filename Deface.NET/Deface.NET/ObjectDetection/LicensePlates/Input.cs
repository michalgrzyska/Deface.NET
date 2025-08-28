using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Input(float[] image)
{
    [VectorType(1, 3, 640, 640)]
    [ColumnName("input")]
    public float[] Image { get; set; } = image;
}
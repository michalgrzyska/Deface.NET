using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.LicensePlates;

[ExcludeFromCodeCoverage]
internal class Input(float[] image)
{
    [VectorType(1, 3, 640, 640)]
    [ColumnName("images")]
    public float[] Image { get; set; } = image;
}
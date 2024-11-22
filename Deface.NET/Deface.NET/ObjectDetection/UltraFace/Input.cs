using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

[ExcludeFromCodeCoverage]
internal class Input
{
    [VectorType(1, 3, 480, 640)]
    [ColumnName("input")]
    public float[] Image { get; set; }

    public Input(float[] image)
    {
        Image = image;
    }
}
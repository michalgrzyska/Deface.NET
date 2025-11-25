using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

//[ExcludeFromCodeCoverage]
//internal class Input(float[] image)
//{
//    [VectorType(1, 3, 480, 640)]
//    [ColumnName(UltraFaceConstants.Input)]
//    public float[] Image { get; set; } = image;
//}

internal class Input
{
    [ColumnName("input")]
    [VectorType(1, 3, 640, 640)]
    public float[] Image { get; set; }

    public Input(float[] image)
    {
        Image = image;
    }
}
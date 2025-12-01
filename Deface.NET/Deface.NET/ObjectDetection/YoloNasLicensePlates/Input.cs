using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

[ExcludeFromCodeCoverage]
internal class Input(float[] image)
{
    [ColumnName(LicensePlatesContants.Input)]
    [VectorType(1, 3, 640, 640)]
    public float[] Image { get; set; } = image;
}
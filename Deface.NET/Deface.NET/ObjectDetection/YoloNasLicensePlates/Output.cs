using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.YoloNasLicensePlates;

[ExcludeFromCodeCoverage]
internal class Output
{
    [ColumnName(LicensePlatesContants.Boxes)]
    public float[] Boxes { get; set; } = [];

    // shape [1, N, D]
    [ColumnName(LicensePlatesContants.Scores)]
    public float[] Scores { get; set; } = [];
}
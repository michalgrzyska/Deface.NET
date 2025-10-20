using Microsoft.ML.Data;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.ObjectDetection.UltraFace;

//[ExcludeFromCodeCoverage]
//internal class Output
//{
//    [VectorType(1, 17640, 2)]
//    [ColumnName(UltraFaceConstants.Scores)]
//    public float[] Scores { get; set; } = [];

//    [VectorType(1, 17640, 4)]
//    [ColumnName(UltraFaceConstants.Boxes)]
//    public float[] Boxes { get; set; } = [];
//}

internal class Output
{
    [ColumnName("graph2_num_predictions")]
    public long[] NumPredictions { get; set; }

    [ColumnName("graph2_pred_boxes")]
    public float[] Boxes { get; set; }

    [ColumnName("graph2_pred_scores")]
    public float[] Scores { get; set; }

    [ColumnName("graph2_pred_classes")]
    public long[] Classes { get; set; }
}
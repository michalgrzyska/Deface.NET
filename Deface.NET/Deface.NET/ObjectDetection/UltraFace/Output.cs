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
    // Number of predictions: [1,1]
    [ColumnName("graph2_num_predictions")]
    public long[] NumPredictions { get; set; }

    // Boxes: [1, N, 4]
    [ColumnName("graph2_pred_boxes")]
    public float[] Boxes { get; set; }

    // Confidence scores: [1, N]
    [ColumnName("graph2_pred_scores")]
    public float[] Scores { get; set; }

    // Class IDs: [1, N]
    [ColumnName("graph2_pred_classes")]
    public long[] Classes { get; set; }
}
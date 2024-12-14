using Microsoft.ML.Data;

namespace Deface.NET.UnitTests.ObjectDetection.ONNX;

public class TestOutput
{
    [ColumnName("output1")]
    public string TestProp1 { get; set; } = "";

    [ColumnName("output2")]
    public string TestProp2 { get; set; } = "";
}

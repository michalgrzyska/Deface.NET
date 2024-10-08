using Microsoft.ML.Data;

namespace Deface.NET.UnitTests.ObjectDetection.ONNX;

public class TestInput
{
    [ColumnName("input1")]
    public string TestProp1 { get; set; } = "";

    [ColumnName("input2")]
    public string TestProp2 { get; set; } = "";
}

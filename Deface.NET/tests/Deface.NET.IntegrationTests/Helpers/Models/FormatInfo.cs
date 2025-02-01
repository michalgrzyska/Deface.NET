using System.Text.Json.Serialization;

namespace Deface.NET.IntegrationTests.Helpers.Models;

public class FormatInfo
{
    [JsonPropertyName("size")]
    public long Size { get; set; }
}

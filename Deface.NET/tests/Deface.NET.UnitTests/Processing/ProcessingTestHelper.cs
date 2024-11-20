using Deface.NET.Graphics.Models;

namespace Deface.NET.UnitTests.Processing;

internal static class ProcessingTestHelper
{
    public static Frame GetTestFrame()
    {
        using FileStream fs = File.OpenRead(TestResources.TestResources.Photo1);
        return new(fs);
    }
}

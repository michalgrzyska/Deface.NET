namespace Deface.NET.IntegrationTests.Helpers;

public abstract class IOTest
{
    public static void Cleanup(params string[] paths)
    {
        foreach (var path in paths)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

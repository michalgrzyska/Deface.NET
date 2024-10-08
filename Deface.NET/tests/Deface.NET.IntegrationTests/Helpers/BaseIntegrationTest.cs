using Deface.NET.IntegrationTests.Config;

namespace Deface.NET.IntegrationTests.Helpers;

public abstract class BaseIntegrationTest
{
    public static IDefaceService DefaceService(Action<Settings> settingsFn = default)
    {
        var ffmpegAction = (Settings settings) =>
        {
            settings.FFMpegPath = ExecutablePath.FFMpeg;
            settings.FFProbePath = ExecutablePath.FFProbe;
        };

        var settingsFinalFn = settingsFn switch
        {
            not null => settingsFn + ffmpegAction,
            _ => ffmpegAction
        };

        return DefaceProvider.GetDefaceService(settingsFinalFn);
    }

    public static void CleanupFiles(params string[] paths)
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

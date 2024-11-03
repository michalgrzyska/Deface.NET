using System.Runtime.InteropServices;

namespace Deface.NET.Utils;

internal static class PlatformChecker
{
    public static Platform GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Platform.Windows;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Platform.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Platform.MacOS;
        }

        throw new NotImplementedException("System not supported.");
    }
}
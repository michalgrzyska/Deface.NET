using Deface.NET.System;

namespace Deface.NET.Common;

internal class AppFiles(IFileSystem fileSystem) : IAppFiles
{
    private const string FilesDir = "Resources";

    private readonly IFileSystem _fileSystem = fileSystem;

    public string UltraFaceONNX => TryGet("ultraface.onnx");

    private string TryGet(string filename)
    {
        var fullPath = Path.Combine(_fileSystem.BaseDirectory, FilesDir, filename);

        if (!_fileSystem.Exists(fullPath))
        {
            throw new FileNotFoundException($"Could not find the required file: {filename}. This error may occur because the parent application uses a different base directory than the actual application's folder. Refer to https://github.com/michalgrzyska/Deface.NET?tab=readme-ov-file#3-custombasedirectory to learn how to use the {nameof(Settings)}.{nameof(Settings.CustomBaseDirectory)} property to resolve this issue.");
        }

        return fullPath;
    }
}
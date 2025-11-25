using Deface.NET.System;

namespace Deface.NET.Common;

internal class AppFiles(IFileSystem fileSystem) : IAppFiles
{
    private const string FilesDir = "Resources";

    private readonly IFileSystem _fileSystem = fileSystem;

    public string LicensePlatesONNX => TryGet("license-plate-yolo-nas.onnx");

    private string TryGet(string filename)
    {
        var fullPath = Path.Combine(_fileSystem.BaseDirectory, FilesDir, filename);

        if (!_fileSystem.Exists(fullPath))
        {
            throw new FileNotFoundException(string.Format(ExceptionMessages.AppFileNotFound, filename));
        }

        return fullPath;
    }
}
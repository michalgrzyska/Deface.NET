﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.System.ExternalProcessing;

[ExcludeFromCodeCoverage]
internal class ExternalProcess : IExternalProcess
{
    private readonly Process _process;

    public Stream InputStream => _process.StandardInput.BaseStream;
    public Stream OutputStream => _process.StandardOutput.BaseStream;

    public ExternalProcess(string path, string arguments, bool redirectStandardInput = false)
    {
        ProcessStartInfo info = new()
        {
            FileName = path,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = redirectStandardInput,
            RedirectStandardError = true
        };

        _process = new()
        {
            StartInfo = info
        };
    }

    public string ExecuteWithOutput()
    {
        Start();

        var output = _process.StandardOutput.ReadToEnd();

        WaitForExit();

        return output;
    }

    public void Start() => _process.Start();
    public void WaitForExit() => _process.WaitForExit();
    public void Dispose() => _process?.Dispose();
}

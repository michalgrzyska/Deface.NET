﻿namespace Deface.NET.UnitTests._TestsConfig;

public sealed class SettingsFixture : IDisposable
{
    private readonly string _ffMpegPath;
    private readonly string _ffProbePath;
    private readonly Action<Settings> _action;

    public Settings Settings => new(_action);

    public SettingsFixture()
    {
        _ffMpegPath = Path.GetTempFileName();
        _ffProbePath = Path.GetTempFileName();

        _action = settings =>
        {
            settings.FFMpegPath = Path.GetTempFileName();
            settings.FFProbePath = Path.GetTempFileName();
        };
    }

    public void Dispose()
    {
        File.Delete(_ffMpegPath);
        File.Delete(_ffProbePath);
    }
}

[CollectionDefinition(nameof(SettingsCollection))]
public class SettingsCollection : ICollectionFixture<SettingsFixture>;
﻿using Deface.NET.Graphics.Interfaces;

namespace Deface.NET.VideoIO.Models;

internal record VideoInfo
(
    int Width,
    int Height,
    int TotalFrames,
    float TargetFps,
    float AverageFps,
    string Path
) : ISize;

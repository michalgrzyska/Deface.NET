# `Deface.NET`: Video and photo anonymization library for .NET

Deface.NET is an MIT licensed library for photo and video processing to achieve anonymized resources. It is based on [Ultraface](https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB) ONNX model for face detection and [SkiaSharp](https://github.com/mono/SkiaSharp) + [FFMpeg](https://www.ffmpeg.org/) image processing.

## Installation

Deface.NET is available via NuGet package:

    nuget install Deface.NET

## Using Deface.NET

#### With Dependency Injection (recommended):

Deface.NET can be easily added via Dependency Injection:

    services.AddDeface(settings =>
    {
        ...
    });

and then `IDefaceService` can be easily injected:

    public class SomeDIClass
    {
        private readonly IDefaceService _defaceService;

        public SomeDIClass(IDefaceService defaceService)
        {
            _defaceService = defaceService;
        }
    }

#### With Standalone Provider

Another way of using Deface.NET is to use `DefaceProvider:`

    IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
    {
        ...
    });

    var result = defaceService.ProcessImage("1.png", "1-processed.png");

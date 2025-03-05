# Deface.NET: Video and photo anonymization library for .NET

![NuGet](https://img.shields.io/nuget/v/Deface.NET.svg)

Deface.NET is an MIT licensed library for photo and video processing to achieve anonymized resources. It is based on [Ultraface](https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB) ONNX model for face detection and [SkiaSharp](https://github.com/mono/SkiaSharp) + [FFMpeg](https://www.ffmpeg.org/) image processing.

## Installation

Deface.NET is available via NuGet package:

    nuget install Deface.NET

Also, you need to install FFMpeg and FFProbe manually.

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

## GPU Compatibility

- You need to have cuDNN and CUDA Toolkit installed.
- See [NVIDIA docs for CUDA support matrix](https://docs.nvidia.com/deeplearning/cudnn/latest/reference/support-matrix.html).
- See [tested NVIDIA configurations for Deface.NET](https://github.com/michalgrzyska/Deface.NET/blob/main/docs/tested-configurations.md).

### Usage

```
services.AddDeface(settings =>
{
    ...
    settings.Hardware = Hardware.Cuda(gpuId); // for most configurations gpuId = 0 or gpuId = 1 works
    ...
});
```

## Configuration options

To find out more informatiom, read the XML comments for a given option property/value.

| Option                       | Value                                      | Required    | Additional info                        |
| ---------------------------- | ------------------------------------------ | ----------- | -------------------------------------- |
| `FFMpegPath`                 | `string`                                   | videos only | -                                      |
| `FFProbePath`                | `string`                                   | videos only |
| `LoggingLevel`               | `None`, `Basic`, `Detailed`                |             | -                                      |
| `AnonimizationShape`         | `Ellipse`, `Rectangle`                     |             | -                                      |
| `AnonimizationMethod`        | `GaussianBlur`, `Mosaic`, `Color(r, g, b)` |             | -                                      |
| `Threshold`                  | `float`                                    |             | `0 <= Threshold <= 1`                  |
| `RunDetectionEachNFrames`    | `int`                                      |             | `1 <= RunDetectionEachNFrames`         |
| `MaskScale`                  | `float`                                    |             | `1 <= MaskScale`                       |
| `ImageFormat`                | `Png`, `Jpeg(quality)`                     |             | -                                      |
| `Hardware`                   | `Cpu()`, `Cuda(gpuDeviceId)`               |             | -                                      |
| `EncodingCodec`              | `VP9`, `H264`                              |             | -                                      |
| `HideCommercialFeaturesInfo` | `bool`                                     |             | -                                      |
| `CustomBaseDirectory`        | `string`                                   |             | See FAQ's CustomBaseDirectory section. |

## Commercial Features

Deface.NET's default settings run the library in open-source mode. However, the library includes some features that are licensed differently. Ensure you have the proper license before using them. Deface.NET's creators take no responsibility for the use of commercial features without the necessary rights.

Each enabled commercial feature displays information about its usage, including additional links to the license, etc. You can hide this information by setting `HideCommercialFeaturesInfo = false` in the settings.

The current list of commercial features requiring an additional license:

| Feature     | Description                                          | How to enable                        |
| ----------- | ---------------------------------------------------- | ------------------------------------ |
| H.264 codec | Videos are now being saved as .mp4 with H.264 codec. | `EncodingCodec = EncodingCodec.H264` |

## FAQ & Known Issues

#### 1. Long paths on Windows

Videos may not be processed correctly on Windows if the destination path exceeds 260 characters. See [Microsoft Docs](https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry) to learn how to enable long paths.

#### 2. GPU performance

Currently, the Ultraface model used in Deface.NET achieves only ~30% performance improvement when running on a GPU. We are working on optimizing it.

#### 3. `CustomBaseDirectory`

In some cases (such as with Azure Functions), Deface.NET may throw a `FileNotFoundException` indicating that `Settings.CustomBaseDirectory` should be overridden. This happens because the native static method `AppContext.BaseDirectory` from the `System` namespace may not return the actual directory containing the application DLLs.

To resolve this, set the `CustomBaseDirectory` property when registering Deface.NET. In such cases, applications often provide the correct application directory as an environment variable.

# Deface.NET: Video and photo anonymization library for .NET

![NuGet](https://img.shields.io/nuget/v/Deface.NET.svg)

Deface.NET is an MIT licensed library for photo and video processing to achieve anonymized resources. It is based on [Ultraface](https://github.com/Linzaer/Ultra-Light-Fast-Generic-Face-Detector-1MB) ONNX model for face detection, custom trained YOLO-NAS-S ONNX model for license plate detection and [SkiaSharp](https://github.com/mono/SkiaSharp) + [FFMpeg](https://www.ffmpeg.org/) image processing.

## Installation

Deface.NET is available via NuGet package:

    nuget install Deface.NET

Also, you need to install FFMpeg and FFProbe manually.

## Using Deface.NET

#### With Dependency Injection (recommended):

Deface.NET can be easily added via Dependency Injection:

```csharp
services.AddDeface(settings =>
{
    ...
});
```

and then `IDefaceService` can be easily injected:

```csharp
public class SomeDIClass
{
    private readonly IDefaceService _defaceService;

    public SomeDIClass(IDefaceService defaceService)
    {
        _defaceService = defaceService;
    }
}
```

#### With Standalone Provider

Another way of using Deface.NET is to use `DefaceProvider:`

```csharp
IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    ...
});

var result = defaceService.ProcessImage("1.png", "1-processed.png");
```

## GPU Compatibility

- You need to have cuDNN and CUDA Toolkit installed.
- See [NVIDIA docs for CUDA support matrix](https://docs.nvidia.com/deeplearning/cudnn/latest/reference/support-matrix.html).
- See [tested NVIDIA configurations for Deface.NET](https://github.com/michalgrzyska/Deface.NET/blob/main/docs/tested-configurations.md).

### Usage

```csharp
services.AddDeface(settings =>
{
    ...
    settings.Hardware = Hardware.Cuda(gpuId); // for most configurations gpuId = 0 or gpuId = 1 works
    ...
});
```

## Configuration options

To find out more informatiom, read the XML comments for a given option property/value.

| Option                         | Value                                      | Required   | Additional info                        |
| ------------------------------ | ------------------------------------------ | ---------- | -------------------------------------- |
| `FFMpegPath`                   | `string`                                   | for videos | -                                      |
| `FFProbePath`                  | `string`                                   | for images |
| `LoggingLevel`                 | `None`, `Basic`, `Detailed`                |            | -                                      |
| `AnonimizationShape`           | `Ellipse`, `Rectangle`                     |            | -                                      |
| `AnonimizationMethod`          | `GaussianBlur`, `Mosaic`, `Color(r, g, b)` |            | -                                      |
| `FaceThreshold`                | `float`                                    |            | `0 <= FaceThreshold <= 1`              |
| `LicensePlateThreshold`        | `float`                                    |            | `0 <= LicensePlateThreshold <= 1`      |
| `DisableFaceDetection`         | `bool`                                     |            |                                        |
| `DisableLicensePlateDetection` | `bool`                                     |            |                                        |
| `RunDetectionEachNFrames`      | `int`                                      |            | `1 <= RunDetectionEachNFrames`         |
| `MaskScale`                    | `float`                                    |            | `1 <= MaskScale`                       |
| `ImageFormat`                  | `Png`, `Jpeg(quality)`                     |            | -                                      |
| `Hardware`                     | `Cpu()`, `Cuda(gpuDeviceId)`               |            | -                                      |
| `EncodingCodec`                | `VP9`, `H264`                              |            | -                                      |
| `HideCommercialFeaturesInfo`   | `bool`                                     |            | -                                      |
| `CustomBaseDirectory`          | `string`                                   |            | See FAQ's CustomBaseDirectory section. |

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

#### 2. GPU Performance

Currently, the Ultraface model used in Deface.NET achieves only ~30% performance improvement when running on a GPU. We are working on optimizing it.
**NOTE**: GPU is strongly recommended with license plate blurring - current model is poorly optimized for CPU usage.

#### 3. `CustomBaseDirectory`

In some cases (such as with Azure Functions), Deface.NET may throw a `FileNotFoundException` indicating that `Settings.CustomBaseDirectory` should be overridden. This happens because the native static method `AppContext.BaseDirectory` from the `System` namespace may not return the actual directory containing the application DLLs.

To resolve this, set the `CustomBaseDirectory` property when registering Deface.NET. In such cases, applications often provide the correct application directory as an environment variable.

#### 4. Missing ONNX Files

In case your application throws an exception about missing `.onnx` files (most like due to Docker container), to your `.csproj` file add:

```csharp
<Target Name="CopyResourcesToPublish" AfterTargets="Publish">
    <ItemGroup>
        <GeneratedResources Include="$(OutputPath)Resources\**" />
    </ItemGroup>

    <Copy SourceFiles="@(GeneratedResources)" DestinationFolder="$(PublishDir)Resources\%(RecursiveDir)" />
</Target>
```

#### 5. Docker Support

`nvidia/cuda:12.4.1-cudnn-runtime-ubuntu22.04` image is tested inside production environment and works correctly.

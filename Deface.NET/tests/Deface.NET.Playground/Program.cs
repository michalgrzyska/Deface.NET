using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
    options.AnonimizationShape = AnonimizationShape.Rectangle;
    options.EncodingCodec = EncodingCodec.H264;
    options.FaceThreshold = 0.8f;
    options.LicensePlateThreshold = 0.8f;
    options.Hardware = Hardware.Cuda(0);
});

// var result = defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "C://result.mp4");
var result = defaceService.ProcessImage(TestResources.Car1, "C://result.jpg");

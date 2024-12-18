using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.FFMpegPath = "C://ffmpeg//ffmpeg.exe";
    options.FFProbePath = "C://ffmpeg//ffprobe.exe";

    //options.FFMpegPath = "/usr/bin/ffmpeg";
    //options.FFProbePath = "/usr/bin/ffprobe";

    options.AnonimizationMethod = AnonimizationMethod.Mosaic;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.Hardware = Hardware.Cuda(0);
    options.EncodingCodec = EncodingCodec.H264;
});

var result = defaceService.ProcessVideo(TestResources.Video_Short_HD_1280_720_24fps, "\"C://DefaceTest//testt.mp4");

//var result = defaceService.ProcessImage(TestResources.Photo3, "C://DefaceTest//4.png");

Console.ReadKey();
using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    //options.FFMpegPath = "C://ffmpeg//ffmpeg.exe";
    //options.FFProbePath = "C://ffmpeg//ffprobe.exe";

    options.FFMpegPath = "/usr/bin/ffmpeg";
    options.FFProbePath = "/usr/bin/ffprobe";

    options.AnonimizationMethod = AnonimizationMethod.Mosaic;
    //options.RunDetectionEachNFrames = 3;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.ImageFormat = ImageFormat.Jpeg(10);
});

//var result = await defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "testt.mp4");
var result = await defaceService.ProcessVideo("/mnt/c/Users/mihax/OneDrive/Documents/GitHub/Deface.NET/Deface.NET/tests/Deface.NET.Playground/bin/Debug/net8.0/Res/Videos/short_640_360_24fps.mp4", "testt.mp4");

//var result = defaceService.ProcessImage(TestResources.Photo1, "C://DefaceTest//1.png", x => { x.AnonimizationShape = AnonimizationShape.Rectangle; });

var x = 5;
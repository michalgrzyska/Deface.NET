using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.FFMpegPath = "C://ffmpeg//ffmpeg.exe";
    options.FFProbePath = "C://ffmpeg//ffprobe.exe";

    //options.FFMpegPath = "/usr/bin/ffmpeg";
    //options.FFProbePath = "/usr/bin/ffprobe";

    options.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
});

var result = defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "\"C://DefaceTest//testt.mp4");

//var result = defaceService.ProcessImage(TestResources.Photo3, "C://DefaceTest//4.png");

var x = 5;
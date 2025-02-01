using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.EncodingCodec = EncodingCodec.H264;
    options.FFMpegPath = "C://ffmpeg//ffmpeg.exe";
    options.FFProbePath = "C://ffmpeg//ffprobe.exe";
    options.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
});

var result = defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "\"C://DefaceTest//testt");

//var result = defaceService.ProcessImage(TestResources.Photo3, "C://DefaceTest//4.png");

Console.ReadKey();
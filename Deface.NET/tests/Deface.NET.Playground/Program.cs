using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.FFMpegPath = "C://ffmpeg//ffmpeg.exe";
    options.FFProbePath = "C://ffmpeg//ffprobe.exe";

    options.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
    //options.RunDetectionEachNFrames = 3;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.ImageFormat = ImageFormat.Jpeg(10);
});

var result = await defaceService.ProcessVideo(TestResources.Video_Short_HD_1280_720_24fps, "C://DefaceTest//testt.mp4");

//var result = defaceService.ProcessImage(TestResources.Photo1, "C://DefaceTest//1.png", x => { x.AnonimizationShape = AnonimizationShape.Rectangle; });

var x = 5;
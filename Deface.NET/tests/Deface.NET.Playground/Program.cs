using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.RunDetectionEachNFrames = 2;
    options.AnonimizationShape = AnonimizationShape.Rectangle;
    options.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
});

//var result = defaceService.ProcessVideo(TestResources.Video_Short_SD_480_270_24fps, "C://DefaceTest//testt.mp4");

var result = defaceService.ProcessImage(TestResources.Photo1, "1.png");

var x = 5;
using Deface.NET;
using Deface.NET.TestResources;

var defaceService = DefaceProvider.GetDefaceService(x =>
{
    x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
    x.RunDetectionEachNFrames = 3;
});

var t1 = defaceService.ProcessVideo(Resources.Video_Short_640_360_24fps, "C://Test/test1.mp4", x =>
{
    x.AnonimizationMethod = AnonimizationMethod.Mosaic;
});
//var t2 = defaceService.ProcessImage(pathImg, "test.png");

var x = 5;
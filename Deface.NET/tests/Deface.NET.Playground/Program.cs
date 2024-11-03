using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.AnonimizationMethod = AnonimizationMethod.Color(0, 255, 0);
});

var result = await defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "C://DefaceTest//testt.mp4");

//var result = defaceService.ProcessImage(TestResources.Photo1, "1.png");

var x = 5;
using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(AppContext.BaseDirectory);
Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
    options.AnonimizationShape = AnonimizationShape.Rectangle;
    options.EncodingCodec = EncodingCodec.H264;
    options.Threshold = 0.1f;
});

//var result = defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "C://DefaceTest/testtttt");
//var result = defaceService.ProcessVideo(
//    "/mnt/c/Users/mihax/OneDrive/Documents/GitHub/Deface.NET/Deface.NET/tests/Deface.NET.TestResources/Res/Videos/short_640_360_24fps.mp4",
//    "/tmp/result.mp4"
//);

var result = defaceService.ProcessImage(TestResources.Photo5, "C://DefaceTest/test11111.jpg");

Console.ReadKey();
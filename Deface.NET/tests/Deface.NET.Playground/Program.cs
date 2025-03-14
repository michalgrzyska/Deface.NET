using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(AppContext.BaseDirectory);
Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationMethod = AnonimizationMethod.Mosaic;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.EncodingCodec = EncodingCodec.H264;
});

//var result = defaceService.ProcessVideo(TestResources.Video_Short_640_360_24fps, "C://DefaceTest/testtttt");
//var result = defaceService.ProcessVideo(
//    "/mnt/c/Users/mihax/OneDrive/Documents/GitHub/Deface.NET/Deface.NET/tests/Deface.NET.TestResources/Res/Videos/short_640_360_24fps.mp4",
//    "/tmp/result.mp4"
//);

var result = defaceService.ProcessImage(TestResources.Photo3, "C://DefaceTest/");

Console.ReadKey();
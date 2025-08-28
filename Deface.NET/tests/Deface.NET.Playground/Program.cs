using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(AppContext.BaseDirectory);
Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
    options.AnonimizationShape = AnonimizationShape.Rectangle;
    options.EncodingCodec = EncodingCodec.H264;
    options.Threshold = 0.5f;
    options.Hardware = Hardware.Cuda(0);
});

var result = defaceService.ProcessVideo(TestResources.Video_Cars_And_People1, "C://DefaceTest/cap1.mp4");
//var result = defaceService.ProcessVideo(
//    "/mnt/c/Users/mihax/OneDrive/Documents/GitHub/Deface.NET/Deface.NET/tests/Deface.NET.TestResources/Res/Videos/short_640_360_24fps.mp4",
//    "/tmp/result.mp4"
//);

//var result = defaceService.ProcessImage(TestResources.Car1, "C://DefaceTest/res.jpg");

//Console.ReadKey();
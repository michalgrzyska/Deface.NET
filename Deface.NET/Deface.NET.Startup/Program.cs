using Deface.NET;

var defaceService = DefaceProvider.GetDefaceService(x =>
{
    x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
    x.RunDetectionEachNFrames = 3;
});

//var path = Path.GetFullPath("C://Test/aga.mp4");
//var path = Path.GetFullPath("C://Test/test.mp4");
var path = Path.GetFullPath("2.mp4");
var pathImg = Path.GetFullPath("1.png");


var t1 = defaceService.ProcessVideo(path, "C://Test/test1.mp4");
//var t2 = defaceService.ProcessImage(pathImg, "test.png");

var x = 5;
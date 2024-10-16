using Deface.NET;

var defaceService = DefaceProvider.GetDefaceService();

var path = Path.GetFullPath("3.mp4");
var pathImg = Path.GetFullPath("1.png");


var t1 = defaceService.ProcessVideo(path, "test.mp4");
var t2 = defaceService.ProcessImage(pathImg, "test.png");

var x = 5;
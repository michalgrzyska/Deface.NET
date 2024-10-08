using Deface.NET;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();

//services.AddLogging(configure =>
//{
//    configure.AddConsole();
//    configure.SetMinimumLevel(LogLevel.Information);
//});

services.AddDeface(x =>
{
    x.AnonimizationMethod = AnonimizationMethod.Mosaic;
});

var serviceProvider = services.BuildServiceProvider();

var defaceService = serviceProvider.GetRequiredService<IDefaceService>();
var path = Path.GetFullPath("3.mp4");
var pathImg = Path.GetFullPath("1.png");


var t1 = defaceService.ProcessVideo(path, "test.mp4");
var t2 = defaceService.ProcessImage(pathImg, "test.png");

var x = 5;
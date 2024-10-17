using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.RunDetectionEachNFrames = 2;
});

var result = defaceService.ProcessVideo(TestResources.Video_Short_SD_480_270_24fps, "C://DefaceTest//testt.mp4");
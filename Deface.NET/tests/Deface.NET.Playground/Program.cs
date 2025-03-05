// Don't want to create a test project to test Deface.NET? Just use this playground.

using Deface.NET;
using Deface.NET.TestResources;

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    // Provide your own options here
});

var result = defaceService.ProcessVideo(TestResources.Video_Very_Short_480p, "your target path here");

Console.ReadKey();
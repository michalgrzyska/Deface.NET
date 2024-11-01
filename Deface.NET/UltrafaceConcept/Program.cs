using Deface.NET.TestResources;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using UltrafaceConcept;

//var ultraface = new Ultraface();

//var faces = ultraface.Process(TestResources.Photo1);

//foreach (var face in faces)
//{
//    Console.WriteLine(face);
//}

var video = TestResources.Video_Short_640_360_24fps;


var metadata = await FFProbe.AnalyseAsync(video);

double videoLengthInSeconds = metadata.Duration.TotalSeconds;
var fps = metadata.VideoStreams[0].FrameRate;

int totalFrames = (int)(videoLengthInSeconds * fps);
Console.WriteLine($"Total Frames: {totalFrames}");

TimeSpan timePerFrame = TimeSpan.FromSeconds(1.0 / fps);
Console.WriteLine($"Time Per Frame: {timePerFrame.TotalMilliseconds} ms");

using var streamOutThumb = new MemoryStream();
var outPipeThumbnail = new StreamPipeSink(streamOutThumb);

var counter = new Stopwatch();
counter.Start();

for (int frame = 0; frame < totalFrames; frame++)
{
    TimeSpan frameTime = TimeSpan.FromSeconds(frame * (1.0 / fps));

    await streamOutThumb.FlushAsync();

    var thumbArgs = FFMpegArguments
        .FromFileInput(video)
        .OutputToPipe(outPipeThumbnail, o =>
        {
            o.Seek(frameTime);
            o.SelectStream((int)0, 0);
            o.WithFrameOutputCount(1);
            //o.WithVideoFilters(filterOptions => filterOptions.Scale(640, 360));
            o.WithVideoCodec(VideoCodec.Png);
            o.ForceFormat("image2");
        });
    await thumbArgs.ProcessAsynchronously(true);

    Console.WriteLine($"Frame {frame + 1}: {counter.Elapsed}");

}



//using var streamOutThumb = new MemoryStream();
//var outPipeThumbnail = new StreamPipeSink(streamOutThumb);


//var thumbArgs = FFMpegArguments
//    .FromFileInput(video)
//    .OutputToPipe(outPipeThumbnail, o =>
//    {
//        o.Seek(TimeSpan.FromSeconds(metadata.Duration.TotalSeconds / 3));
//        o.SelectStream((int)0, 0);
//        o.WithFrameOutputCount(1);

//        o.WithVideoFilters(filterOptions => filterOptions.Scale(640, 360));

//        o.WithVideoCodec(VideoCodec.Png);
//        o.ForceFormat("image2");
//    });
//await thumbArgs.ProcessAsynchronously(true);

//Bitmap b = new(streamOutThumb);

//b.Save("1.png");
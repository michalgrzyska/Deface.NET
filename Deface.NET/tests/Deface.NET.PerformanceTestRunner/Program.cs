using Deface.NET.PerformanceTestRunner.Config;
using Deface.NET.PerformanceTestRunner.TestScenarios;

const string FFMpegPath = "C://ffmpeg//ffmpeg.exe";
const string FFProbePath = "C://ffmpeg//ffprobe.exe";

var data = await new PerformanceTestRunner(FFMpegPath, FFProbePath)
    .AddScenariosFrom<AnonimizationTypeScenario>()
    .Run();

ResultTable.Display(data);
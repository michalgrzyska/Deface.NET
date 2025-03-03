﻿using Deface.NET;
using Deface.NET.TestResources;

Console.WriteLine(Directory.GetCurrentDirectory());

IDefaceService defaceService = DefaceProvider.GetDefaceService(options =>
{
    options.AnonimizationMethod = AnonimizationMethod.Mosaic;
    options.AnonimizationShape = AnonimizationShape.Ellipse;
    options.Hardware = Hardware.Cuda(0);
    options.EncodingCodec = EncodingCodec.H264;
});

var result = defaceService.ProcessVideo(TestResources.Video_Very_Short_480p, "\"C://DefaceTest//testt");

//var result = defaceService.ProcessImage(TestResources.Photo3, "C://DefaceTest//4.png");

Console.ReadKey();
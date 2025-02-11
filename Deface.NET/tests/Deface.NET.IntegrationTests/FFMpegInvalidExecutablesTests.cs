﻿using Deface.NET.IntegrationTests.Resources;
using Shouldly;

namespace Deface.NET.IntegrationTests;

public class FFMpegInvalidExecutablesTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("whatever")]
    public void InvalidFFMpegPath_ShouldThrowDefaceException(string path)
    {
        var service = DefaceProvider.GetDefaceService(x =>
        {
            x.FFMpegPath = path;
        });

        var action = () => service.ProcessVideo("", "");

        action.ShouldThrow<DefaceException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("whatever")]
    public void InvalidFFProbePath_ShouldThrowDefaceException(string path)
    {
        var service = DefaceProvider.GetDefaceService(x =>
        {
            x.FFProbePath = path;
        });

        var action = () => service.ProcessVideo("", "");

        action.ShouldThrow<DefaceException>();
    }
}

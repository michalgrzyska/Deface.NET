﻿using Deface.NET.Common;
using Deface.NET.IntegrationTests.Helpers;

namespace Deface.NET.IntegrationTests;

public class InvalidVideoPathTests : BaseIntegrationTest
{
    [Fact]
    public void ProcessVideo_InvalidOutputPath_ThrowsInvalidOperationException()
    {
        var action = () => DefaceService().ProcessVideo(TestResources.TestResources.Video_Very_Short_480p, "Y://fakeLocation");

        action
            .ShouldThrow<DefaceException>()
            .WithInnerException<InvalidOperationException>(ExceptionMessages.ErrorWhileWritingFrames);
    }
}

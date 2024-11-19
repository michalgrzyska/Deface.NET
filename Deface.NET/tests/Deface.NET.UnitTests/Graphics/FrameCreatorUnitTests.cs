using Deface.NET.Graphics;
using Deface.NET.System;

namespace Deface.NET.UnitTests.Graphics;

public class FrameCreatorUnitTests
{
    private readonly FrameCreator _frameCreator;

    public FrameCreatorUnitTests()
    {
        FileSystem fileSystem = new();

        _frameCreator = new FrameCreator(fileSystem);
    }

    [Fact]
    public void FromFile_ProperImagePath_CreatesFrame()
    {
        var path = TestResources.TestResources.Photo1;

        var frame = _frameCreator.FromFile(path);

        frame.Width.Should().Be(1280);
        frame.Height.Should().Be(946);
    }

    [Fact]
    public void FromFile_InvalidPath_ThrowsDefaceException()
    {
        var path = Guid.NewGuid().ToString();

        var action = () => _frameCreator.FromFile(path);

        action.Should().Throw<DefaceException>();
    }
}

using Castle.Core.Resource;
using Deface.NET.System;
using Deface.NET.UnitTests._TestsConfig;

namespace Deface.NET.UnitTests.System;

[Collection(nameof(SettingsCollection))]
public class FileSystemTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Fact]
    public void BaseDirectory_Custom_IsSetProperly()
    {
        var customDir = "custom";
        var fileSystem = GetFileSystem(x => { x.CustomBaseDirectory = customDir; });

        fileSystem.BaseDirectory.ShouldBe(customDir);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void BaseDirectory_InvalidOrEmpty_ShouldBeDefault(string? path)
    {
        var fileSystem = GetFileSystem(x => { x.CustomBaseDirectory = path; });
        fileSystem.BaseDirectory.ShouldBe(AppContext.BaseDirectory);
    }

    [Fact]
    public void BaseDirectory_NotSet_ShouldBeDefault()
    {
        var fileSystem = GetFileSystem();
        fileSystem.BaseDirectory.ShouldBe(AppContext.BaseDirectory);
    }

    private IFileSystem GetFileSystem(Action<Settings>? action = null)
    {
        var settingsProvider = _settingsFixture.GetSettingsProvider(action);
        return new FileSystem(settingsProvider);
    }
}

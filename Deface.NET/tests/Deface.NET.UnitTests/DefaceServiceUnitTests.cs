using Deface.NET.Configuration.Provider;
using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Deface.NET.UnitTests;

public class DefaceServiceUnitTests
{
    private readonly DefaceService _service;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IVideoProcessor _videoProcessor;
    private readonly IImageProcessor _imageProcessor;

    public DefaceServiceUnitTests()
    {
        _videoProcessor = Substitute.For<IVideoProcessor>();
        _imageProcessor = Substitute.For<IImageProcessor>();
        _scopeFactory = GetServiceScopeFactory();

        _service = new DefaceService(_scopeFactory);
    }

    [Fact]
    public async Task ProcessVideo_ProperData_DependenciesInvokedCorrectly()
    {
        // Arrange

        var input = Path.GetTempFileName();
        var output = "file";

        // Act

        await _service.ProcessVideo(input, output);

        // Assert

        await _videoProcessor.Received(1).Process(Arg.Any<string>(), Arg.Any<string>());

        // Cleanup

        File.Delete(input);
    }

    [Fact]
    public async Task ProcessVideo_InputNotExisting_ThrowsFileNotFoundException()
    {
        var action = async () => await _service.ProcessVideo("test", "file");

        await action.Should().ThrowAsync<FileNotFoundException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public async Task ProcessVideo_InvalidInputString_ThrowsArgumentNullException(string? inputString)
    {
        var action = async () => await _service.ProcessVideo(inputString!, "file");

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public async Task ProcessVideo_InvalidOutputString_ThrowsArgumentNullException(string? outputString)
    {
        // Arrange

        var input = Path.GetTempFileName();

        // Act

        var action = async () => await _service.ProcessVideo(input, outputString);

        // Assert

        await action.Should().ThrowAsync<ArgumentNullException>();

        // Cleanup

        File.Delete(input);
    }

    private IServiceScopeFactory GetServiceScopeFactory()
    {
        var scopedSettingsProvider = Substitute.For<IScopedSettingsProvider>();

        ServiceCollection serviceCollection = new();

        serviceCollection.AddScoped(x => _videoProcessor);
        serviceCollection.AddScoped(x => _imageProcessor);
        serviceCollection.AddScoped(x => scopedSettingsProvider);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IServiceScopeFactory>();
    }
}

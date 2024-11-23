using Deface.NET.Configuration.Provider;
using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Deface.NET.UnitTests.DefaceServiceTests;

public partial class DefaceServiceUnitTests
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

using Deface.NET.IntegrationTests.Helpers.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Deface.NET.IntegrationTests.Helpers;

public class DefaceWithLogger
{
    public IDefaceService DefaceService { get; private set; }
    public TestLogger Logger { get; private set; }

    private DefaceWithLogger(IDefaceService defaceService, TestLogger logger)
    {
        DefaceService = defaceService;
        Logger = logger;
    }

    public static DefaceWithLogger New(Action<Settings> settingsBuilder = default)
    {
        TestLogger testLogger = new();
        ServiceCollection services = new();

        services.AddTransient<ILogger>(_ => testLogger);
        services.AddDeface(settingsBuilder);

        var serviceProvider = services.BuildServiceProvider();
        var defaceService = serviceProvider.GetRequiredService<IDefaceService>();

        return new(defaceService, testLogger);
    }
}

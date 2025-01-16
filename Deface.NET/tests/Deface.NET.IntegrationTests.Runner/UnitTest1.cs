using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

namespace Deface.NET.IntegrationTests.Runner;

public class LibraryIntegrationTests : IAsyncLifetime
{
    private FFmpegLibraryTestContainer _testContainer;

    public async Task InitializeAsync()
    {
        _testContainer = new FFmpegLibraryTestContainer();
        await _testContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _testContainer.DisposeAsync();
    }

    [Fact]
    public async Task VerifyTestsRanSuccessfullyInsideContainer()
    {

        // Assert: Verify tests completed successfully
        //Assert.Contains("Total tests:", logs);       // Adjust based on actual test runner output
        //Assert.DoesNotContain("Failed", logs);       // Ensure no test failures
    }
}


public class FFmpegLibraryTestContainer : IAsyncDisposable
{
    private readonly IContainer _container;
    private readonly IFutureDockerImage _image;

    public FFmpegLibraryTestContainer()
    {
        string dockerfileDirectory = Path.Combine(Directory.GetCurrentDirectory() + "/../../../../");

        // Build the image dynamically
        _image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(dockerfileDirectory)
            .WithName("ffmpeg_library_test_image")
            .Build();

        // Build the container
        _container = new ContainerBuilder()
            .WithImage(_image)
            .WithName("ffmpeg_library_test_container")
            .WithCleanUp(true) // Automatically remove container after the test
            //.WithOutputConsumer(Consume.RedirectStdoutAndStderrToStream())
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("Duration:")) // Match test output
            .Build();
    }

    public async Task StartAsync()
    {
        await _image.CreateAsync();
        await _container.StartAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _container.DisposeAsync();
    }
}
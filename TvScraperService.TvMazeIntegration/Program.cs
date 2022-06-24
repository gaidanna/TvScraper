using TvScraperService.TvMazeIntegration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TestTVMazeIntegrationService>();
    })
    .Build();

await host.RunAsync();

using Demo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nuits.Extensions.Hosting.Wpf;

new HostBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<MainWindowViewModel>();
    })
    .ConfigureWpf<App, MainWindow>()
    .Build()
    .RunAsync();
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Nuits.Extensions.Hosting.Wpf;

public class WpfApplication<TApplication, TWindow> : IHost
    where TApplication : Application
    where TWindow : Window

{
    private readonly IHost _host;

    internal WpfApplication(IHost host)
    {
        _host = host;
    }

    public void Dispose() => _host.Dispose();

    public Task StartAsync(CancellationToken cancellationToken = new())
        => _host.StartAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken = new())
        => _host.StopAsync(cancellationToken);

    public IServiceProvider Services => _host.Services;

    public static WpfApplicationBuilder<TApplication, TWindow> CreateBuilder() => new();

}
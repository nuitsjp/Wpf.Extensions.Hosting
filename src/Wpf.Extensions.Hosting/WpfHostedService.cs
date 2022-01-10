using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Wpf.Extensions.Hosting;

internal class WpfHostedService<TApplication, TWindow> : IHostedService
    where TApplication : Application
    where TWindow : Window
{
    private readonly TApplication _application;
    private readonly TWindow? _window;

    public WpfHostedService(TApplication application, TWindow? window)
    {
        _application = application;
        _window = window;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _application.Run(_window);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
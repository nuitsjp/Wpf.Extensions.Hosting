using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Nuits.Extensions.Hosting.Wpf;

internal class WpfHostedService<TApplication, TWindow> : IHostedService
    where TApplication : Application
    where TWindow : Window
{
    private readonly ApplicationContainer<TApplication, TWindow> _applicationContainer;

    public WpfHostedService(ApplicationContainer<TApplication, TWindow> applicationContainer)
    {
        _applicationContainer = applicationContainer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationContainer.Run();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
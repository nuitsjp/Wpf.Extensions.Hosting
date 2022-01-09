using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nuits.Extensions.Hosting.Wpf;

public class WpfApplicationBuilder<TApplication, TWindow>
    where TApplication : Application
    where TWindow : Window
{
    public WpfApplicationBuilder()
    {
        Configuration.AddJsonFile("appsettings.json");
    }
    public IServiceCollection Services { get; } = new ServiceCollection();

    public ConfigurationManager Configuration { get; } = new();

    public WpfApplication<TApplication, TWindow> Build()
    {
        var builder = Host.CreateDefaultBuilder();

        builder.ConfigureServices(services =>
        {
            foreach (var service in Services)
            {
                services.Add(service);
            }
        });

        builder.ConfigureWpf<TApplication, TWindow>();
        return new WpfApplication<TApplication, TWindow>(builder.Build());
    }
}
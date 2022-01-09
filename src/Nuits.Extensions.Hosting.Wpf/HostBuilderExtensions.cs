using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nuits.Extensions.Hosting.Wpf;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureWpf<TApplication>(this IHostBuilder hostBuilder) 
        where TApplication : Application
    {
        return hostBuilder.ConfigureWpf<TApplication>((_, _, _) => { });
    }

    public static IHostBuilder ConfigureWpf<TApplication>(this IHostBuilder hostBuilder, Action<TApplication, Window, IServiceProvider> onLoaded)
        where TApplication : Application
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);


        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<WpfHostedService<TApplication, Window>>();
            services.AddTransient<ApplicationContainer<TApplication, Window>, ApplicationContainer<TApplication>>();
            services.AddTransient(_ => new OnLoadedListener<TApplication, Window>(onLoaded));
            services.AddTransient<TApplication>();
        });
    }

    public static IHostBuilder ConfigureWpf<TApplication, TWindow>(this IHostBuilder hostBuilder)
        where TApplication : Application
        where TWindow : Window
    {
        return hostBuilder.ConfigureWpf<TApplication, TWindow>((_, _, _) => { });
    }

    public static IHostBuilder ConfigureWpf<TApplication, TWindow>(this IHostBuilder hostBuilder, Action<TApplication, TWindow, IServiceProvider> onLoaded)
        where TApplication : Application
        where TWindow : Window
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);


        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddHostedService<WpfHostedService<TApplication, TWindow>>();
            services.AddTransient<ApplicationContainer<TApplication, TWindow>>();
            services.AddTransient(_ => new OnLoadedListener<TApplication, TWindow>(onLoaded));
            services.AddSingleton<TApplication>();
            services.AddTransient<TWindow>();
        });
    }
}
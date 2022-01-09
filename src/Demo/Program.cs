using Demo;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nuits.Extensions.Hosting.Wpf;

Host.CreateDefaultBuilder(args)
    //.ConfigureAppConfiguration((context, builder) =>
    //{
    //})
    //.ConfigureHostConfiguration(builder =>
    //{
    //    //builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    //    builder.AddJsonFile("appsettings.json");
    //})
    .ConfigureLogging(logging =>
    {
        logging.AddDebug();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<MainWindowViewModel>();
        services.Configure<MySettings>(context.Configuration.GetSection("MySettings"));
    })
    .ConfigureWpf<App, MainWindow>()
    .Build()
    .RunAsync();
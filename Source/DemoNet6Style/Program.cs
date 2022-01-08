using DemoNet6Style;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Extensions.Hosting;

//Host.CreateDefaultBuilder(args)
//    .ConfigureLogging(logging =>
//    {
//        logging.AddDebug();
//    })
//    .ConfigureServices((context, services) =>
//    {
//        services.AddTransient<MainWindowViewModel>();
//        services.Configure<MySettings>(context.Configuration.GetSection("MySettings"));
//    })
//    .ConfigureWpf<App, MainWindow>()
//    .Build()
//    .RunAsync();

//var builder = WpfApplication<App, MainWindow>.CreateBuilder();

//builder.Services.AddTransient<MainWindowViewModel>();
//var foo = builder.Configuration.GetSection("MySettings");
//builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

//var app = builder.Build();
//app.RunAsync();
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);
builder.Services.AddTransient<MainWindowViewModel>();
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
var app = builder.Build((_, _, _) => { });
app.RunAsync();

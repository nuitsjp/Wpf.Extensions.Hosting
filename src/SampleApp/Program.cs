using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleApp;
using Wpf.Extensions.Hosting;

var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

builder.Services.AddTransient<MainWindowViewModel>();
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

builder.Logging.AddDebug();

var app = builder.Build();
app.Loaded += (sender, eventArgs) =>
{

};

app.RunAsync();

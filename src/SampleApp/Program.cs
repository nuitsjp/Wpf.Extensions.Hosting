using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleApp;
using Serilog;
using Wpf.Extensions.Hosting;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Configure dependency injection.
// Injecting MainWindowViewModel into MainWindow.
builder.Services.AddTransient<MainWindowViewModel>();

// Configure the settings.
// Injecting IOptions<MySettings> from appsetting.json.
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

// Configure logging.
// Using the diagnostic logging library Serilog.
builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Debug()
    .WriteTo.File(
        @"Logs\log.txt", 
        rollingInterval: RollingInterval.Day));
    
var app = builder.Build();
app.Startup += (sender, eventArgs) =>
{

};

app.RunAsync();

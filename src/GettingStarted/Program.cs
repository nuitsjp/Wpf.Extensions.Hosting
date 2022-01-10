using GettingStarted;

var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

builder.Services.AddTransient<MainWindowViewModel>();

var app = builder.Build();
app.RunAsync();

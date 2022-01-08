using SampleApp;

var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

builder.Services.AddTransient<MainWindowViewModel>();
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

builder.Logging.AddDebug();

var app = builder.Build();

app.RunAsync();

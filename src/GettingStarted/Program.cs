using GettingStarted;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Register the ViewModel to be injected into MainWindow to the DI container.
builder.Services.AddTransient<MainWindowViewModel>();

// Build and run the application.
var app = builder.Build();
await app.RunAsync();

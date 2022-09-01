using Microsoft.Extensions.Hosting;
using SampleAppWithStartupUri;
using Wpf.Extensions.Hosting;

var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

var app = builder.Build();

await app.RunAsync();

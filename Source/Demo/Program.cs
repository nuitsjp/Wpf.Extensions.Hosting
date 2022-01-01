using Demo;
using Microsoft.Extensions.Hosting;
using Nuits.Extensions.Hosting.Wpf;

new HostBuilder()
    .ConfigureWpf<App, MainWindow>()
    .Build()
    .RunAsync();
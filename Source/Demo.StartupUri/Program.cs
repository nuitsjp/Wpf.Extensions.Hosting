using Demo.StartupUri;
using Microsoft.Extensions.Hosting;
using Nuits.Extensions.Hosting.Wpf;

new HostBuilder()
    .ConfigureWpf<App>((app, window) =>
    {

    })
    .Build()
    .RunAsync();
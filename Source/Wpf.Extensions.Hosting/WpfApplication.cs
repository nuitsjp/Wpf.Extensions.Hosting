// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wpf.Extensions.Hosting.Infrastructure;

namespace Wpf.Extensions.Hosting
{
    /// <summary>
    /// The web application used to configure the HTTP pipeline, and routes.
    /// </summary>
    public sealed class WpfApplication<TApplication, TWindow> : IHost, IAsyncDisposable
        where TApplication : Application
        where TWindow : Window
    {
        private readonly IHost _host;

        internal WpfApplication(IHost host)
        {
            _host = host;
            ApplicationBuilder = new ApplicationBuilder(host.Services);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName);
        }

        /// <summary>
        /// The application's configured services.
        /// </summary>
        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// The application's configured <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        /// <summary>
        /// The application's configured <see cref="IWebHostEnvironment"/>.
        /// </summary>
        public IHostEnvironment Environment => _host.Services.GetRequiredService<IHostEnvironment>();

        /// <summary>
        /// Allows consumers to be notified of application lifetime events.
        /// </summary>
        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        /// <summary>
        /// The default logger for the application.
        /// </summary>
        public ILogger Logger { get; }

        internal IFeatureCollection ServerFeatures => _host.Services.GetRequiredService<IServer>().Features;
        internal IDictionary<string, object?> Properties => ApplicationBuilder.Properties;
        internal ApplicationBuilder ApplicationBuilder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfApplication{TApplication,TWindow}"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="WpfApplication{TApplication,TWindow}"/>.</returns>
        public static WpfApplication<TApplication, TWindow> Create(string[]? args = null, Action<TApplication, TWindow, IServiceProvider> onLoaded = null) =>
            new WpfApplicationBuilder<TApplication, TWindow>(new() { Args = args }).Build(onLoaded);

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfApplicationBuilder{TApplication,TWindow}"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="WpfApplicationBuilder{TApplication,TWindow}"/>.</returns>
        public static WpfApplicationBuilder<TApplication, TWindow> CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfApplicationBuilder{TApplication,TWindow}"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="WpfApplicationBuilder{TApplication,TWindow}"/>.</returns>
        public static WpfApplicationBuilder<TApplication, TWindow> CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfApplicationBuilder{TApplication,TWindow}"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="WpfApplicationOptions"/> to configure the <see cref="WpfApplicationBuilder{TApplication,TWindow}"/>.</param>
        /// <returns>The <see cref="WpfApplicationBuilder{TApplication,TWindow}"/>.</returns>
        public static WpfApplicationBuilder<TApplication, TWindow> CreateBuilder(WpfApplicationOptions options) =>
            new(options);

        /// <summary>
        /// Start the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="WpfApplication{TApplication,TWindow}"/>.
        /// Successful completion indicates the HTTP server is ready to accept new requests.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="WpfApplication{TApplication,TWindow}"/>.
        /// Successful completion indicates that all the HTTP server has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        /// <summary>
        /// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the entire runtime of the <see cref="WpfApplication{TApplication,TWindow}"/> from startup to shutdown.
        /// </returns>
        public Task RunAsync(string? url = null)
        {
            Listen(url);
            return HostingAbstractionsHostExtensions.RunAsync(this);
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="url">The URL to listen to if the server hasn't been configured directly.</param>
        public void Run(string? url = null)
        {
            Listen(url);
            HostingAbstractionsHostExtensions.Run(this);
        }

        /// <summary>
        /// Disposes the application.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the application.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        internal RequestDelegate BuildRequestDelegate() => ApplicationBuilder.Build();

        private void Listen(string? url)
        {
            if (url is null)
            {
                return;
            }

            var addresses = ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
            if (addresses is null)
            {
                throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesFeature)} was found.");
            }
            if (addresses.IsReadOnly)
            {
                throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesFeature.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
            }

            addresses.Clear();
            addresses.Add(url);
        }
    }
}

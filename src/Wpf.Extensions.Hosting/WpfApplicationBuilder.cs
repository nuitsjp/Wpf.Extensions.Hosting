// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wpf.Extensions.Hosting
{
    /// <summary>
    /// A builder for web applications and services.
    /// </summary>
    public sealed class WpfApplicationBuilder<TApplication, TWindow> : IWpfApplicationBuilder<TApplication, TWindow> where TApplication : Application
        where TWindow : Window
    {
        private readonly HostBuilder _hostBuilder = new();
        private readonly WpfApplicationServiceCollection _services = new();
        private readonly List<KeyValuePair<string, string>> _hostConfigurationValues;
        private readonly Action<TApplication, TWindow, IServiceProvider> _onLoaded;

        private WpfApplication<TApplication, TWindow>? _builtApplication;

        internal WpfApplicationBuilder(WpfApplicationOptions<TApplication, TWindow> options)
        {
            Services = _services;

            _onLoaded = options.OnLoaded ?? ((_, _, _) => { });

            // Run methods to configure both generic and web host defaults early to populate config from appsettings.json
            // environment variables (both DOTNET_ prefixed) and other possible default sources to prepopulate
            // the correct defaults.
            var bootstrapHostBuilder = new BootstrapHostBuilder(Services, _hostBuilder.Properties);
            bootstrapHostBuilder.ConfigureDefaults(args: options.Args);
            bootstrapHostBuilder.ConfigureHostConfiguration(options.ApplyHostConfiguration);

            Configuration = new();

            // Collect the hosted services separately since we want those to run after the user's hosted services
            _services.TrackHostedServices = true;

            // This is the application configuration
            var (hostContext, hostConfiguration) = bootstrapHostBuilder.RunDefaultCallbacks(Configuration, _hostBuilder);

            // Stop tracking here
            _services.TrackHostedServices = false;

            // Capture the host configuration values here. We capture the values so that
            // changes to the host configuration have no effect on the final application. The
            // host configuration is immutable at this point.
            _hostConfigurationValues = new(hostConfiguration.AsEnumerable());

            // Grab the WebHostBuilderContext from the property bag to use in the ConfigureWebHostBuilder
            //var webHostContext = (WebHostBuilderContext)hostContext.Properties[typeof(WebHostBuilderContext)];

            // Grab the IHostEnvironment from the webHostContext. This also matches the instance in the IServiceCollection.
            Environment = hostContext.HostingEnvironment;
            Logging = new LoggingBuilder(Services);
            Host = new ConfigureHostBuilder(hostContext, Configuration, Services);

            Services.AddSingleton<IConfiguration>(_ => Configuration);
        }

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public IHostEnvironment Environment { get; }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration { get; }

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging { get; }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; }

        /// <summary>
        /// Builds the <see cref="WpfApplication{TApplication,TWindow}"/>.
        /// </summary>
        /// <returns>A configured <see cref="WpfApplication{TApplication,TWindow}"/>.</returns>
        public WpfApplication<TApplication, TWindow> Build()
        {
            Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            Services.AddHostedService<WpfHostedService<TApplication, TWindow>>();
            Services.AddTransient<ApplicationContainer<TApplication, TWindow>>();
            Services.AddTransient(_ => new OnLoadedListener<TApplication, TWindow>(_onLoaded));
            Services.AddSingleton<TApplication>();
            Services.AddTransient<TWindow>();

            // Wire up the host configuration here. We don't try to preserve the configuration
            // source itself here since we don't support mutating the host values after creating the builder.
            _hostBuilder.ConfigureHostConfiguration(builder =>
            {
                builder.AddInMemoryCollection(_hostConfigurationValues);
            });

            var chainedConfigSource = new TrackingChainedConfigurationSource(Configuration);

            // Wire up the application configuration by copying the already built configuration providers over to final configuration builder.
            // We wrap the existing provider in a configuration source to avoid re-bulding the already added configuration sources.
            _hostBuilder.ConfigureAppConfiguration(builder =>
            {
                builder.Add(chainedConfigSource);

                foreach (var keyValue in ((IConfigurationBuilder)Configuration).Properties)
                {
                    builder.Properties[keyValue.Key] = keyValue.Value;
                }
            });

            // This needs to go here to avoid adding the IHostedService that boots the server twice (the GenericWebHostService).
            // Copy the services that were added via WpfApplicationBuilder.Services into the final IServiceCollection
            _hostBuilder.ConfigureServices((context, services) =>
            {
                // We've only added services configured by the GenericWebHostBuilder and WebHost.ConfigureWebDefaults
                // at this point. HostBuilder news up a new ServiceCollection in HostBuilder.Build() we haven't seen
                // until now, so we cannot clear these services even though some are redundant because
                // we called ConfigureWebHostDefaults on both the _deferredHostBuilder and _hostBuilder.
                foreach (var s in _services)
                {
                    services.Add(s);
                }

                // Add the hosted services that were initially added last
                // this makes sure any hosted services that are added run after the initial set
                // of hosted services. This means hosted services run before the web host starts.
                foreach (var s in _services.HostedServices)
                {
                    services.Add(s);
                }

                // Clear the hosted services list out
                _services.HostedServices.Clear();

                // Add any services to the user visible service collection so that they are observable
                // just in case users capture the Services property. Orchard does this to get a "blueprint"
                // of the service collection

                // Drop the reference to the existing collection and set the inner collection
                // to the new one. This allows code that has references to the service collection to still function.
                _services.InnerCollection = services;

                var hostBuilderProviders = ((IConfigurationRoot)context.Configuration).Providers;

                if (!hostBuilderProviders.Contains(chainedConfigSource.BuiltProvider))
                {
                    // Something removed the _hostBuilder's TrackingChainedConfigurationSource pointing back to the ConfigurationManager.
                    // This is likely a test using WebApplicationFactory. Replicate the effect by clearing the ConfingurationManager sources.
                    ((IConfigurationBuilder)Configuration).Sources.Clear();
                }

                // Make builder.Configuration match the final configuration. To do that, we add the additional
                // providers in the inner _hostBuilders's Configuration to the ConfigurationManager.
                foreach (var provider in hostBuilderProviders)
                {
                    if (!ReferenceEquals(provider, chainedConfigSource.BuiltProvider))
                    {
                        ((IConfigurationBuilder)Configuration).Add(new ConfigurationProviderSource(provider));
                    }
                }
            });

            // Run the other callbacks on the final host builder
            Host.RunDeferredCallbacks(_hostBuilder);

            _builtApplication = new WpfApplication<TApplication, TWindow>(_hostBuilder.Build());

            // Mark the service collection as read-only to prevent future modifications
            _services.IsReadOnly = true;

            // Resolve both the _hostBuilder's Configuration and builder.Configuration to mark both as resolved within the
            // service provider ensuring both will be properly disposed with the provider.
            _ = _builtApplication.Services.GetService<IEnumerable<IConfiguration>>();

            return _builtApplication;
        }

        private sealed class LoggingBuilder : ILoggingBuilder
        {
            public LoggingBuilder(IServiceCollection services)
            {
                Services = services;
            }

            public IServiceCollection Services { get; }
        }
    }
}

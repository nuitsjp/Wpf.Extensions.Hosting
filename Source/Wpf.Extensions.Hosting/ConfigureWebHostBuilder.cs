// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Extensions.Hosting.Infrastructure;

namespace Wpf.Extensions.Hosting
{
    /// <summary>
    /// A non-buildable <see cref="IWebHostBuilder"/> for <see cref="WebApplicationBuilder"/>.
    /// Use <see cref="WebApplicationBuilder.Build"/> to build the <see cref="WebApplicationBuilder"/>.
    /// </summary>
    public sealed class ConfigureWebHostBuilder : IWebHostBuilder, ISupportsStartup
    {
        private readonly IHostEnvironment _environment;
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly HostBuilderContext _context;

        internal ConfigureWebHostBuilder(HostBuilderContext webHostBuilderContext, ConfigurationManager configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _environment = webHostBuilderContext.HostingEnvironment;
            _services = services;
            _context = webHostBuilderContext;
        }

        IWebHost IWebHostBuilder.Build()
        {
            throw new NotSupportedException();
        }

        public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var previousContentRoot = _context.HostingEnvironment.ContentRootPath;
            var previousApplication = _configuration[WebHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[WebHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[WebHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[WebHostDefaults.HostingStartupExcludeAssembliesKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);

            if (!string.Equals(previousApplication, _configuration[WebHostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{_configuration[WebHostDefaults.ApplicationKey]}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (!string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[WebHostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[WebHostDefaults.ContentRootKey])}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (!string.Equals(previousEnvironment, _configuration[WebHostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[WebHostDefaults.EnvironmentKey]}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssemblies, _configuration[WebHostDefaults.HostingStartupAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{_configuration[WebHostDefaults.HostingStartupAssembliesKey]}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssembliesExclude, _configuration[WebHostDefaults.HostingStartupExcludeAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{_configuration[WebHostDefaults.HostingStartupExcludeAssembliesKey]}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }

            return this;
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureServices)
        {
            // Run these immediately so that they are observable by the imperative code
            configureServices(_context, _services);
            return this;
        }

        /// <inheritdoc />
        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((WebHostBuilderContext context, IServiceCollection services) => configureServices(services));
        }

        public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? GetSetting(string key)
        {
            return _configuration[key];
        }

        /// <inheritdoc />
        public IWebHostBuilder UseSetting(string key, string? value)
        {
            // All properties on IWebHostEnvironment are non-nullable.
            if (value is null)
            {
                return this;
            }

            var previousContentRoot = _context.HostingEnvironment.ContentRootPath;
            var previousApplication = _configuration[WebHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[WebHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[WebHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[WebHostDefaults.HostingStartupExcludeAssembliesKey];

            if (string.Equals(key, WebHostDefaults.ApplicationKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousApplication, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{value}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (string.Equals(key, WebHostDefaults.ContentRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(value), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(value)}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (string.Equals(key, WebHostDefaults.EnvironmentKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEnvironment, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{value}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (string.Equals(key, WebHostDefaults.HostingStartupAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssemblies, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{value}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }
            else if (string.Equals(key, WebHostDefaults.HostingStartupExcludeAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssembliesExclude, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{value}\". Changing the host configuration using WebApplicationBuilder.WebHost is not supported. Use WebApplication.CreateBuilder(WebApplicationOptions) instead.");
            }

            // Set the configuration value after we've validated the key
            _configuration[key] = value;

            return this;
        }

        IWebHostBuilder ISupportsStartup.Configure(Action<IApplicationBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by WebApplicationBuilder.WebHost. Use the WebApplication returned by WebApplicationBuilder.Build() instead.");
        }

        IWebHostBuilder ISupportsStartup.Configure(Action<WebHostBuilderContext, IApplicationBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by WebApplicationBuilder.WebHost. Use the WebApplication returned by WebApplicationBuilder.Build() instead.");
        }


        //IWebHostBuilder ISupportsStartup.UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<WebHostBuilderContext, TStartup> startupFactory)
        //{
        //    throw new NotSupportedException("UseStartup() is not supported by WebApplicationBuilder.WebHost. Use the WebApplication returned by WebApplicationBuilder.Build() instead.");
        //}
    }
}

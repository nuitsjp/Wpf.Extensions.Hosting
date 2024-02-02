// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wpf.Extensions.Hosting;

public interface IWpfApplicationBuilder<TApplication, TWindow> where TApplication : Application where TWindow : Window
{
    /// <summary>
    /// The <see cref="IHostBuilder"/> for the application.
    /// </summary>
    HostBuilder HostBuilder { get; }

    /// <summary>
    /// Provides information about the web hosting environment an application is running.
    /// </summary>
    IHostEnvironment Environment { get; }

    /// <summary>
    /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
    /// </summary>
    ConfigurationManager Configuration { get; }

    /// <summary>
    /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
    /// </summary>
    ILoggingBuilder Logging { get; }

    /// <summary>
    /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
    /// To build after configuration, call <see cref="Build"/>.
    /// </summary>
    ConfigureHostBuilder Host { get; }

    /// <summary>
    /// Builds the <see cref="WpfApplication{TApplication,TWindow}"/>.
    /// </summary>
    /// <returns>A configured <see cref="WpfApplication{TApplication,TWindow}"/>.</returns>
    WpfApplication<TApplication, TWindow> Build();
}
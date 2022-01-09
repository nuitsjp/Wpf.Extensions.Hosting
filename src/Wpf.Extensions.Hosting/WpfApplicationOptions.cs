// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wpf.Extensions.Hosting
{
    /// <summary>
    /// Options for configuing the behavior for <see cref="WpfApplication{TApplication,TWindow}.CreateBuilder(WpfApplicationOptions)"/>.
    /// </summary>
    public class WpfApplicationOptions<TApplication, TWindow>
        where TApplication : Application
        where TWindow : Window
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[]? Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string? EnvironmentName { get; init; }

        public Action<TApplication, TWindow, IServiceProvider>? OnLoaded { get; init; }

        internal void ApplyHostConfiguration(IConfigurationBuilder builder)
        {
            Dictionary<string, string>? config = null;

            if (EnvironmentName is not null)
            {
                config = new();
                config[HostDefaults.EnvironmentKey] = EnvironmentName;
            }

            if (config is not null)
            {
                builder.AddInMemoryCollection(config);
            }
        }
    }
}

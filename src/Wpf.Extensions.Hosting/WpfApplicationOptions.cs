// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wpf.Extensions.Hosting
{
    /// <summary>
    /// Options for configuing the behavior for <see cref="WpfApplication{TApplication,TWindow}.CreateBuilder(WpfApplicationOptions)"/>.
    /// </summary>
    public class WpfApplicationOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[]? Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string? EnvironmentName { get; init; }

        internal void ApplyHostConfiguration(IConfigurationBuilder builder)
        {
            Dictionary<string, string>? config = null;

            if (EnvironmentName is not null)
            {
                config = new()
                {
                    [HostDefaults.EnvironmentKey] = EnvironmentName
                };
            }

            if (config is not null)
            {
                builder.AddInMemoryCollection(config);
            }
        }
    }
}

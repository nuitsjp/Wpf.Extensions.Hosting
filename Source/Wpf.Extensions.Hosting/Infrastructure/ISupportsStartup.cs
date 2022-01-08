// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Wpf.Extensions.Hosting.Infrastructure
{
    /// <summary>
    /// An interface implemented by IWebHostBuilders that handle <see cref="WebHostBuilderExtensions.Configure(IWebHostBuilder, Action{IApplicationBuilder})"/>,
    /// <see cref="WebHostBuilderExtensions.UseStartup(IWebHostBuilder, Type)"/> and <see cref="WebHostBuilderExtensions.UseStartup{TStartup}(IWebHostBuilder, Func{WebHostBuilderContext, TStartup})"/>
    /// directly.
    /// </summary>
    public interface ISupportsStartup
    {
        /// <summary>
        /// Specify the startup method to be used to configure the web application.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        IWebHostBuilder Configure(Action<IApplicationBuilder> configure);

        /// <summary>
        /// Specify the startup method to be used to configure the web application.
        /// </summary>
        /// <param name="configure">The delegate that configures the <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        IWebHostBuilder Configure(Action<WebHostBuilderContext, IApplicationBuilder> configure);
    }
}

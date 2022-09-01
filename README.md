# Wpf.Extensions.Hosting

[The Japanese document is here.](README-jp.md)

Wpf.Extensions.Hosting is a library for running WPF applications on Generic Host.

Many of the modern libraries in .NET are provided for Generic Hosts. By using this library, you can take advantage of the latest and greatest set of libraries provided for .   

This library allows you to write WPF on Generic Host in a simple and intuitive way, just like .NET6.

```csharp
// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Configure dependency injection.
// Injecting MainWindowViewModel into MainWindow.
builder.Services.AddTransient<MainWindowViewModel>();

// Configure the settings.
// Injecting IOptions<MySettings> from appsetting.json.
builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));

// Configure logging.
// Using the diagnostic logging library Serilog.
builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Debug()
    .WriteTo.File(
        @"Logs\log.txt", 
        rollingInterval: RollingInterval.Day));
    
var app = builder.Build();

await app.RunAsync();
```

## Getting Started

Create a WPF project and add the package from NuGet.

NuGet :[Wpf.Extensions.Hosting](https://www.nuget.org/packages/Wpf.Extensions.Hosting)

```
Install-Package Wpf.Extensions.Hosting
```

Stop the automatic generation of the application entry point (Main method). Open the .csproj file and set EnableDefaultApplicationDefinition to false.

```xml
<Project Sdk="Microsoft.NET.Sdk">

	<PropertyGroup>
		<OutputType>WinExe</OutputType>
		<TargetFramework>net6.0-windows</TargetFramework>
		<Nullable>enable</Nullable>
		<UseWPF>true</UseWPF>
		<EnableDefaultApplicationDefinition>false</EnableDefaultApplicationDefinition>
	</PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Wpf.Extensions.Hosting" Version="0.0.3" />
	</ItemGroup>

</Project>

```

Delete the description of StartupUri from App.xaml.

```xml
<Application x:Class="GettingStarted.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:GettingStarted">
    <Application.Resources>
         
    </Application.Resources>
</Application>
```

Add a constructor to App.xaml.cs.

```csharp
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
    }
```

Create a Program.cs file and run the WPF application on the Generic Host.

```csharp
using GettingStarted;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Build and run the application.
var app = builder.Build();
await app.RunAsync();
```

## Use Dependency Injection.

The following is an example of injecting a ViewModel into MainWindow.

Create the MainWindowViewModel.
```csharp
namespace GettingStarted;

public class MainWindowViewModel
{
    public string Message => "Hello, Generic Host!";
}
```

Modify the constructor of MainWindow to receive ViewModel as an argument of the constructor and set it to DataContext.

```csharp
public MainWindow(MainWindowViewModel mainWindowViewModel)
{
    InitializeComponent();
    DataContext = mainWindowViewModel;
}
```

Register the ViewModel to the DI container in Program.cs.

```csharp
// Register the ViewModel to be injected into MainWindow to the DI container.
builder.Services.AddTransient<MainWindowViewModel>();
```

In this way, all the features of Generic Host are available.

## .NET Foundation

This project is part of the [.NET Foundation](http://www.dotnetfoundation.org/projects).

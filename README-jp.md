# Wpf.Extensions.Hosting


Wpf.Extensions.Hostingは、WPFアプリケーションをGeneric Host上で動作させるためのライブラリです。

.NETの最新のライブラリの多くは、Generic Host向けに提供されています。このライブラリを使用することで、.NET向けに提供されている最新かつ最高のライブラリ群を利用できます。  

.NET6ライクに、Generic Host上のWPFアプリケーションをシンプルかつ直感的に記述できます。

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

app.RunAsync();
```

## Getting Started

WPFプロジェクトを作成し、NuGetからパッケージを追加します。

NuGet :[Wpf.Extensions.Hosting](https://www.nuget.org/packages/Wpf.Extensions.Hosting)

```
Install-Package Wpf.Extensions.Hosting
```

アプリケーションのエントリポイント（Mainメソッド）の自動生成を停止します。.csprojファイルを開き、EnableDefaultApplicationDefinitionをfalseに設定します。

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

App.xamlからStartupUriの記述を削除します。

```xml
<Application x:Class="GettingStarted.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:GettingStarted">
    <Application.Resources>
         
    </Application.Resources>
</Application>
```

App.xaml.csにコンストラクターを追加します。

```csharp
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
    }
```

Program.csファイルを作成し、Generic Host上でWPFアプリケーションを実行します。

```csharp
using GettingStarted;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Build and run the application.
var app = builder.Build();
app.RunAsync();

```

## Dependency Injectionを使用する

MainWindowにViewModelを注入する例を示します。まずはMainWindowViewModelを作成します。
```csharp
namespace GettingStarted;

public class MainWindowViewModel
{
    public string Message => "Hello, Generic Host!";
}
```

MainWindowのコンストラクターの引数にViewModelを受け取り、DataContextに設定するように修正します。

```csharp
public MainWindow(MainWindowViewModel mainWindowViewModel)
{
    InitializeComponent();
    DataContext = mainWindowViewModel;
}
```

Program.csでViewModelをDIコンテナに登録します。

```csharp
// Register the ViewModel to be injected into MainWindow to the DI container.
builder.Services.AddTransient<MainWindowViewModel>();
```

このように、Generic Hostのすべての機能を利用することができます。

## .NET Foundation

このプロジェクトは [.NET Foundation](http://www.dotnetfoundation.org/projects) の一部です。

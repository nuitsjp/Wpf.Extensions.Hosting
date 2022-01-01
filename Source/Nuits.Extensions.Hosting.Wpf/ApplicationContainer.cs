using System.CodeDom;
using System.Windows;

namespace Nuits.Extensions.Hosting.Wpf;

internal class ApplicationContainer<TApplication, TWindow>
    where TApplication : Application
    where TWindow : Window
{
    private bool _isLoaded;
    private readonly TApplication _application;
    private readonly TWindow? _window;
    private readonly OnLoadedListener<TApplication, TWindow> _onLoaded;

    public ApplicationContainer(TApplication application, TWindow? window, OnLoadedListener<TApplication, TWindow> onLoaded)
    {
        _application = application;
        _window = window;
        _onLoaded = onLoaded;
    }

    internal void Run()
    {
        _application.Activated += OnActivated;
        _application.Run(_window);
    }

    private void OnActivated(object sender, EventArgs e)
    {
        if (_isLoaded)
            return;

        if (_window is null)
        {
            _onLoaded.OnLoad(_application, (TWindow)_application.MainWindow!);
        }
        else
        {
            _onLoaded.OnLoad(_application, _window);

        }

        _isLoaded = true;
    }
}

internal class ApplicationContainer<TApplication> : ApplicationContainer<TApplication, Window>
    where TApplication : Application
{
    public ApplicationContainer(TApplication application, OnLoadedListener<TApplication, Window> onLoaded)
        : base(application, null, onLoaded)
    {
    }
}

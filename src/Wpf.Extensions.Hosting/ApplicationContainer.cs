using System.Windows;

namespace Wpf.Extensions.Hosting;

internal class ApplicationContainer<TApplication, TWindow>
    where TApplication : Application
    where TWindow : Window
{
    private bool _isLoaded;
    private readonly TApplication _application;
    private readonly TWindow? _window;

    public ApplicationContainer(TApplication application, TWindow? window)
    {
        _application = application;
        _window = window;
    }

    public event EventHandler<ApplicationLoadedEventArgs<TApplication, TWindow>>? Loaded;

    internal void Run()
    {
        _application.Activated += OnActivated;
        _application.Run(_window);
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        if (_isLoaded)
            return;

        var window = _window ?? (TWindow)_application.MainWindow!;
        Loaded?.Invoke(this, new ApplicationLoadedEventArgs<TApplication, TWindow>(_application, window));

        _isLoaded = true;
    }
}

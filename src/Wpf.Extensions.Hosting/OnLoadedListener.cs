using System.Windows;

namespace Wpf.Extensions.Hosting;

internal class OnLoadedListener<TApplication, TWindow>
    where TApplication: Application
    where TWindow : Window
{
    private readonly Action<TApplication, TWindow, IServiceProvider> _onLoaded;

    public OnLoadedListener(Action<TApplication, TWindow, IServiceProvider> onLoaded)
    {
        _onLoaded = onLoaded;
    }

    internal void OnLoad(TApplication application, TWindow window, IServiceProvider serviceProvider) => _onLoaded(application, window, serviceProvider);
}
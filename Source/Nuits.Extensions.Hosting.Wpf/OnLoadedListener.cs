using System.Windows;

namespace Nuits.Extensions.Hosting.Wpf;

internal class OnLoadedListener<TApplication, TWindow>
    where TApplication: Application
    where TWindow : Window
{
    private readonly Action<TApplication, TWindow> _onLoaded;

    public OnLoadedListener(Action<TApplication, TWindow> onLoaded)
    {
        _onLoaded = onLoaded;
    }

    internal void OnLoad(TApplication application, TWindow window) => _onLoaded(application, window);
}
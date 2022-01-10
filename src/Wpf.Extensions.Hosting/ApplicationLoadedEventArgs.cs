using System.Windows;

namespace Wpf.Extensions.Hosting;

public class ApplicationLoadedEventArgs<TApplication, TWindow> : EventArgs
    where TApplication : Application
    where TWindow : Window
{
    public ApplicationLoadedEventArgs(TApplication application, TWindow window)
    {
        Application = application;
        Window = window;
    }

    public TApplication Application { get; }
    public TWindow Window { get; }
}
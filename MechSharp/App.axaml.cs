using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MechSharp.ViewModels;

namespace MechSharp;

public class App : Application
{
    public AppViewModel ViewModel { get; }
    public MainWindow? MainWindow { get; private set; }

    public App()
    {
        DataContext = ViewModel = new AppViewModel();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = MainWindow = new MainWindow()
            {
                DataContext = DataContext
            };

            desktop.Exit += OnExit;
        }
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        ViewModel.Save();
    }
}

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using MechSharp.ViewModels;
using MechSharp.Views;

namespace MechSharp;

public partial class App : Application
{
	public MainWindow? MainWindow { get; private set; }

	public App()
	{
		DataContext = new ApplicationViewModel(this);
	}

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		BindingPlugins.DataValidators.RemoveAt(0);

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = MainWindow = new MainWindow(this);
		}

		base.OnFrameworkInitializationCompleted();
	}
}

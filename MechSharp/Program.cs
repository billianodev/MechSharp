using System;
using System.Reflection;
using Avalonia;

namespace MechSharp;

public static class Program
{
	[STAThread]
	private static int Main(string[] args) => BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

	public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
			.UsePlatformDetect();
}
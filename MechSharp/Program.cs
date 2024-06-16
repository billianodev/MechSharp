using System;
using Avalonia;
using Billiano.Audio;

namespace MechSharp;

public static class Program
{
	[STAThread]
	private static int Main(string[] args)
	{
		try
		{
			CodecFactoryUtility.UseOgg();
			return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
			return ex.GetHashCode();
		}
	}

	public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
			.UsePlatformDetect();
}
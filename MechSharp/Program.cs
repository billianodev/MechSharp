using System;
using Avalonia;
using Billiano.Audio;

namespace MechSharp;

public static class Program
{
	public static CodecFactory CodecFactory { get; }

	static Program()
	{
		CodecFactory = CodecFactory.CreateDefault();
	}
	
	[STAThread]
	private static int Main(string[] args)
	{
		try
		{
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
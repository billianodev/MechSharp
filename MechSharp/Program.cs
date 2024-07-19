using System;
using Avalonia;
using MechSharp.Core;

namespace MechSharp;

public static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            var app = BuildAvaloniaApp();
            return app.StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
#if DEBUG
            throw;
#else
            return ex.GetHashCode();
#endif
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>();
        builder.UsePlatformDetect();
        builder.WithInterFont();
        return builder;
    }
}
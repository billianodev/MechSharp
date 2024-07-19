using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MechSharp.Core;

public static class Logger
{
    public static void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] obj)
    {
        var s = string.Format(format, obj);
        WriteLine(s);
    }

    public static void WriteLine(object? obj)
    {
        var s = obj?.ToString();
        WriteLine(s);
    }

    public static void WriteLine(string? s)
    {
        if (s is null)
        {
            return;
        }

        Console.WriteLine(s);
        Trace.WriteLine(s);
    }
}
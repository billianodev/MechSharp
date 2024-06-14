using System;
using System.Diagnostics;
using System.IO;

namespace MechSharp;

public static class Logger
{
#if DEBUG
	private static readonly StreamWriter stream;
#endif

	static Logger()
	{
#if DEBUG
		var path = Path.Combine(AppContext.BaseDirectory, $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log");
		stream = new StreamWriter(File.Create(path));
#endif
	}

	public static void WriteLine(object obj)
	{
		var s = obj.ToString();
#if DEBUG
		stream.WriteLine(s);
		stream.Flush();
#endif
		Console.WriteLine(s);
		Trace.WriteLine(s);
	}
}
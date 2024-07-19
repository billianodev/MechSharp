using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MechSharp.Utilities;

public class PathHelper
{
    public static PathHelper AppDir { get; }
    public static PathHelper UserDir { get; }
    public static PathHelper AppDataDir { get; }

    public string Dir { get; }

    static PathHelper()
    {
        AppDir = AppContext.BaseDirectory;
        UserDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        AppDataDir = OperatingSystem.IsWindows()
            ? UserDir.CombineThenCreateDirectory("AppData", "LocalLow", "MechSharp")
            : UserDir.CombineThenCreateDirectory(".MechSharp");
    }

    public PathHelper(string dir)
    {
        Dir = dir;
        Directory.CreateDirectory(dir);
    }

    public static IEnumerable<string> EnumerateDirectories(params string[] directories)
    {
        foreach (var directory in directories.Where(Directory.Exists))
        {
            foreach (var dir in Directory.EnumerateDirectories(directory))
            {
                yield return dir;
            }
        }
    }

    public static string GetPathFromAppDataDirectory(params string[] path)
    {
        return Path.Combine([AppDataDir, .. path]);
    }

    public string Combine(params string[] path)
    {
        return Path.Combine([Dir, .. path]);
    }

    public string CombineThenCreateDirectory(params string[] path)
    {
        var result = Combine(path);
        Directory.CreateDirectory(result);
        return result;
    }

    public string CombineThenCreateParentDirectory(params string[] path)
    {
        var result = Combine(path);
        var parent = Path.GetDirectoryName(result);

        if (parent is not null)
        {
            Directory.CreateDirectory(parent);
        }

        return result;
    }

    public static implicit operator PathHelper(string absolutePath)
        => new(absolutePath);

    public static implicit operator string(PathHelper path)
        => path.Dir;
}

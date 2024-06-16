using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MechSharp;

public class Soundpacks
{
    public static readonly string KeypackBuiltInPath = Path.Combine(AppContext.BaseDirectory, "sounds", "keys");
    public static readonly string KeypackCustomPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mechvibes_custom");
    
    public static readonly string MousepackBuiltInPath = Path.Combine(AppContext.BaseDirectory, "sounds", "mouse");
    public static readonly string MousepackCustomPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mousevibes_custom");

    public static IEnumerable<SoundpackInfo> LoadKeypackInfos()
    {
        return LoadAll(KeypackBuiltInPath, KeypackCustomPath).ToArray();
    }

    public static IEnumerable<SoundpackInfo> LoadMousepackInfos()
    {
		return LoadAll(MousepackBuiltInPath, MousepackCustomPath).ToArray();
    }

    private static IEnumerable<SoundpackInfo> LoadAll(params string[] directories)
    {
        foreach (var directory in EnumerateDirectories(directories))
        {
            if (SoundpackInfo.Load(directory, out var soundpack))
            {
                Logger.WriteLine($"Loaded soundpack {soundpack} in {soundpack.Dir}");

                yield return soundpack;
            }
        }
    }

    private static IEnumerable<string> EnumerateDirectories(params string[] directories)
    {
        var list = new List<IEnumerable<string>>();
        foreach (var directory in directories)
        {
            if (Directory.Exists(directory))
            {
                list.Add(Directory.EnumerateDirectories(directory));
            }
        }
        return list.SelectMany(x =>x);
    }
}
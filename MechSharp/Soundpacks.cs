using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        foreach (string directory in EnumerateDirectories(directories))
        {
            if (SoundpackInfo.Load(directory, out SoundpackInfo? soundpack))
            {
                Debug.WriteLine($"Loaded soundpack {soundpack} in {soundpack.Dir}");

                yield return soundpack;
            }
        }
    }

    private static IEnumerable<string> EnumerateDirectories(params string[] directories)
    {
        IEnumerable<string> enumerable = [];
        
        foreach (string directory in directories)
        {
            if (Directory.Exists(directory))
            {
                enumerable = enumerable.Concat(Directory.EnumerateDirectories(directory));
            }
        }
        return enumerable;
    }
}
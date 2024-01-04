﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MechSharp;

public class Soundpacks
{
    public static readonly string KeysBuiltInPath = Path.Combine(AppContext.BaseDirectory, "sounds", "keys");
    public static readonly string KeysCustomPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mechvibes_custom");
    
    public static readonly string MouseBuiltInPath = Path.Combine(AppContext.BaseDirectory, "sounds", "mouse");
    public static readonly string MouseCustomPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "mousevibes_custom");

    public static IEnumerable<SoundpackInfo> LoadKeypackInfos()
    {
        var directories = Enumerable.Concat(
            Directory.Exists(KeysBuiltInPath) ? Directory.EnumerateDirectories(KeysBuiltInPath) : [],
            Directory.Exists(KeysCustomPath) ? Directory.EnumerateDirectories(KeysCustomPath) : []
            );

        return LoadAll(directories).ToArray();
    }

    public static IEnumerable<SoundpackInfo> LoadMousepackInfo()
    {
		var directories = Enumerable.Concat(
			Directory.Exists(MouseBuiltInPath) ? Directory.EnumerateDirectories(MouseBuiltInPath) : [],
			Directory.Exists(MouseCustomPath) ? Directory.EnumerateDirectories(MouseCustomPath) : []
			);

		return LoadAll(directories).ToArray();
    }

    private static IEnumerable<SoundpackInfo> LoadAll(IEnumerable<string> directories)
    {
        foreach (var directory in directories)
        {
            if (SoundpackInfo.Load(directory, out var soundpack))
            {
                Debug.WriteLine($"Loaded soundpack {soundpack} in {soundpack.Dir}");

                yield return soundpack;
            }
        }
    }
}
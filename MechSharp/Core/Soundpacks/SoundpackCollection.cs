using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MechSharp.Models;
using MechSharp.Utilities;

namespace MechSharp.Core.Soundpacks;

public class SoundpackCollection : IEnumerable
{
    private readonly SoundpackInfo[] soundpacks;

    public SoundpackCollection(params string[] dir)
    {
        soundpacks = LoadAll(dir).ToArray();
    }

    public SoundpackInfo? Find(string? dir)
    {
        if (dir is null)
        {
            return null;
        }

        return soundpacks.FirstOrDefault(x => x.Dir == dir);
    }

    private static IEnumerable<SoundpackInfo> LoadAll(params string[] directories)
    {
        foreach (var directory in PathHelper.EnumerateDirectories(directories))
        {
            if (SoundpackInfo.TryLoad(directory, out var soundpack))
            {
                Logger.WriteLine("Loaded result {0}", soundpack);
                yield return soundpack;
            }
        }
    }

    public IEnumerator GetEnumerator()
    {
        return soundpacks.GetEnumerator();
    }
}

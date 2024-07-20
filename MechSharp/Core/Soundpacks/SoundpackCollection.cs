using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MechSharp.Models;
using MechSharp.Utilities;

namespace MechSharp.Core.Soundpacks;

public class SoundpackCollection : IEnumerable
{
    private readonly SoundpackInfo[] _soundpacks;

    public SoundpackCollection(params string[] dir)
    {
        _soundpacks = LoadAll(dir).ToArray();
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

    public SoundpackInfo? Find(string? dir)
    {
        if (dir is null)
        {
            return null;
        }

        return _soundpacks.FirstOrDefault(x => x.Dir == dir);
    }

    public IEnumerator GetEnumerator()
    {
        return _soundpacks.GetEnumerator();
    }
}

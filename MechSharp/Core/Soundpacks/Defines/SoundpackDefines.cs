using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Billiano.Audio.FireForget;

namespace MechSharp.Core.Soundpacks.Defines;

public abstract class SoundpackDefines
{
    private readonly Dictionary<int, SoundpackUpDownSource> dict;

    protected SoundpackDefines()
    {
        dict = Enumerate().ToDictionary(x => x.Id, x => x.Source);
    }

    public bool TryGetDownSound(int id, [NotNullWhen(true)] out IFireForgetSource? result)
    {
        dict.TryGetValue(id, out var source);
        return (result = source?.Down) is not null;
    }

    public bool TryGetUpSound(int id, [NotNullWhen(true)] out IFireForgetSource? result)
    {
        dict.TryGetValue(id, out var source);
        return (result = source?.Up) is not null;
    }

    protected abstract IEnumerable<SoundpackDefine> Enumerate();
}
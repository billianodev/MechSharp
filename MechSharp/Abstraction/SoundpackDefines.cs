using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Billiano.Audio.FireForget;
using MechSharp.Core.Soundpacks;
using MechSharp.Core.Soundpacks.Defines;

namespace MechSharp.Abstraction;

public abstract class SoundpackDefines
{
    private readonly Dictionary<int, SoundpackDownUp> _dict;

    protected SoundpackDefines()
    {
        _dict = Enumerate().ToDictionary(x => x.Id, x => x.Source);
    }

    public bool TryGetDownSound(int id, [NotNullWhen(true)] out WaveCache? result)
    {
        _dict.TryGetValue(id, out var source);
        return (result = source?.Down) is not null;
    }

    public bool TryGetUpSound(int id, [NotNullWhen(true)] out WaveCache? result)
    {
        _dict.TryGetValue(id, out var source);
        return (result = source?.Up) is not null;
    }

    protected abstract IEnumerable<SoundpackDefine> Enumerate();
}
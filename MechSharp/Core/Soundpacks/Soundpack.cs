using System.Diagnostics.CodeAnalysis;
using MechSharp.Core.Soundpacks.Defines;
using MechSharp.Models;

namespace MechSharp.Core.Soundpacks;

public sealed class Soundpack(SoundpackData data, SoundpackDefines defines)
{
    public SoundpackData Data => data;
    public SoundpackDefines Defines => defines;

    public static bool TryLoadKeypack(SoundpackData data, bool keyUp, [NotNullWhen(true)] out Soundpack? result)
    {
        SoundpackDefines? defines = data.KeyDefineType switch
        {
            KeyDefineType.Single => new KeypackDefinesSingle(data, keyUp),
            KeyDefineType.Multi => new KeypackDefinesMulti(data, keyUp),
            _ => null,
        };

        return TryLoadSoundpack(data, defines, out result);
    }

    public static bool TryLoadMousepack(SoundpackData data, [NotNullWhen(true)] out Soundpack? result)
    {
        SoundpackDefines? defines = data.KeyDefineType switch
        {
            KeyDefineType.Multi => new MousepackDefinesMulti(data),
            _ => null,
        };

        return TryLoadSoundpack(data, defines, out result);
    }

    private static bool TryLoadSoundpack(SoundpackData data, SoundpackDefines? defines, [NotNullWhen(true)] out Soundpack? result)
    {
        if (defines is null)
        {
            result = null;
            return false;
        }

        result = new Soundpack(data, defines);
        return true;
    }
}

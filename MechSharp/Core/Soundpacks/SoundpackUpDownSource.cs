using Billiano.Audio.FireForget;

namespace MechSharp.Core.Soundpacks;

public record SoundpackUpDownSource(IFireForgetSource? Down, IFireForgetSource? Up)
{
    public SoundpackUpDownSource(IFireForgetSource? down) : this(down, null)
    {
    }
}
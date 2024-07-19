using Billiano.Audio.FireForget;

namespace MechSharp.Core.Soundpacks;

public record SoundpackUpDownSource(WaveCache? Down, WaveCache? Up)
{
    public SoundpackUpDownSource(WaveCache? down) : this(down, null)
    {
    }
}
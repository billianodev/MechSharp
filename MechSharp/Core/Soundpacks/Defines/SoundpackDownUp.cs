using Billiano.Audio.FireForget;

namespace MechSharp.Core.Soundpacks.Defines;

public record SoundpackDownUp
{
    public WaveCache? Down { get; set; }
    public WaveCache? Up { get; set; }

    public SoundpackDownUp(WaveCache? down)
    {
        Down = down;
    }

    public SoundpackDownUp(WaveCache? down, WaveCache? up)
    {
        Down = down;
        Up = up;
    }
}
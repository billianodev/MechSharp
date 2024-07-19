using Billiano.Audio;
using Billiano.Audio.Codecs.CSCore.MediaFoundation;
using CommunityToolkit.Mvvm.ComponentModel;
using MechSharp.Core.Soundpacks;
using MechSharp.Utilities;

namespace MechSharp.Core;

public sealed partial class SoundpacksLoader : ObservableObject
{
    public static CodecFactory Codecs { get; }

    public static string KeypackDirectories { get; }
    public static string KeypackCustomDirectory { get; }

    public static string MousepackBuiltInDirectory { get; }
    public static string MousepackCustomDirectory { get; }

    [ObservableProperty]
    public SoundpackCollection? keypacks;

    [ObservableProperty]
    public SoundpackCollection? mousepacks;

    static SoundpacksLoader()
    {
        Codecs = CodecFactory.CreateDefault();
        Codecs.TryRegisterAAC();
        Codecs.UseMediaFoundationFallback();

        KeypackDirectories = PathHelper.AppDir.CombineThenCreateDirectory("sounds", "keys");
        KeypackCustomDirectory = PathHelper.UserDir.CombineThenCreateDirectory("mechvibes_custom");

        MousepackBuiltInDirectory = PathHelper.AppDir.CombineThenCreateDirectory("sounds", "mouse");
        MousepackCustomDirectory = PathHelper.UserDir.CombineThenCreateDirectory("mousevibes_custom");
    }

    public void Load()
    {
        Keypacks = new SoundpackCollection(KeypackDirectories, KeypackCustomDirectory);
        Mousepacks = new SoundpackCollection(MousepackBuiltInDirectory, MousepackCustomDirectory);
    }
}
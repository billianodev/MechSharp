using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Billiano.Audio.CSCoreSupport;
using Billiano.Audio.FireForget;
using MechSharp.Json;
using MechSharp.Models;
using MechSharp.Utilities;
using NAudio.Wave;

namespace MechSharp.Core.Soundpacks.Defines;

public sealed class KeypackDefinesMulti(SoundpackData data, bool keyUp) : SoundpackDefines
{
    protected override IEnumerable<SoundpackDefine> Enumerate()
    {
        var defines = data.Defines.Deserialize(SoundpackDefinesJsonSerializerContext.Default.DictionaryInt32String);

        if (defines is null)
        {
            yield break;
        }

        var caches = new Dictionary<string, SoundpackUpDownSource>();

        foreach (var file in defines.Values.Distinct())
        {
            if (file is null)
            {
                continue;
            }

            var path = Path.Combine(data.Dir, file);

            if (!File.Exists(path))
            {
                continue;
            }

            using (var reader = SoundpacksLoader.Codecs.GetCodec(path).ToWaveProvider())
            {
                SoundpackUpDownSource sound;
                if (keyUp)
                {
                    WaveHelper.Split(reader, out var down, out var up);
                    sound = new SoundpackUpDownSource(down.ToFireForgetSource(), up.ToFireForgetSource());
                }
                else
                {
                    var source = reader.ToFireForgetSource();
                    sound = new SoundpackUpDownSource(source);
                }

                caches.Add(file, sound);
            }
        }

        foreach ((var id, var file) in defines)
        {
            if (file is null)
            {
                continue;
            }

            if (caches.TryGetValue(file, out var result))
            {
                yield return new SoundpackDefine(id, result);
            }
        }
    }
}
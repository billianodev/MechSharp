using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Billiano.Audio.CSCoreSupport;
using Billiano.Audio.FireForget;
using MechSharp.Json;
using MechSharp.Models;
using MechSharp.Utilities;

namespace MechSharp.Core.Soundpacks.Defines;

public sealed class KeypackDefinesSingle(SoundpackData data, bool keyUp) : SoundpackDefines
{
    protected override IEnumerable<SoundpackDefine> Enumerate()
    {
        var defines = data.Defines.Deserialize(SoundpackDefinesJsonSerializerContext.Default.DictionaryInt32SoundpackDefinesRange);

        if (defines is null)
        {
            yield break;
        }

        var path = Path.Combine(data.Dir, data.Sound);

        if (!File.Exists(path))
        {
            yield break;
        }

        var caches = new Dictionary<SoundpackDefinesRange, SoundpackUpDownSource>();

        using (var reader = SoundpacksLoader.Codecs.GetCodec(path).ToWaveProvider())
        {
            foreach (var range in defines.Values.Distinct())
            {
                if (range is null)
                {
                    continue;
                }

                var slice = WaveHelper.Cut(reader, range.Start, range.Length);

                SoundpackUpDownSource sound;
                if (keyUp)
                {
                    WaveHelper.Split(slice, out var down, out var up);
                    sound = new SoundpackUpDownSource(down.ToFireForgetSource(), up.ToFireForgetSource());
                }
                else
                {
                    var source = slice.ToFireForgetSource();
                    sound = new SoundpackUpDownSource(source);
                }

                caches.Add(range, sound);
            }
        }

        foreach ((var id, var range) in defines)
        {
            if (range is null)
            {
                continue;
            }

            if (caches.TryGetValue(range, out var result))
            {
                yield return new SoundpackDefine(id, result);
            }
        }
    }
}
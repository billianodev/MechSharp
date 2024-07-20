using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Billiano.Audio;
using Billiano.Audio.FireForget;
using MechSharp.Abstraction;
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

        var caches = new Dictionary<SoundpackDefinesRange, SoundpackDownUp>();

        using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
        {
            var buffer = reader.ReadToEnd();

            foreach (var range in defines.Values.Distinct())
            {
                if (range is null)
                {
                    continue;
                }

                var slice = WaveHelper.Cut(buffer, reader.WaveFormat, range.Start, range.Length);

                SoundpackDownUp sound;
                if (keyUp)
                {
                    WaveHelper.Split(slice, reader.WaveFormat, out var first, out var second);
                    var down = new WaveCache(first, reader.WaveFormat);
                    var up = new WaveCache(second, reader.WaveFormat);
                    sound = new SoundpackDownUp(down, up);
                }
                else
                {
                    var source = new WaveCache(slice, reader.WaveFormat);
                    sound = new SoundpackDownUp(source);
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
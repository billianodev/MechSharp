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

public sealed class KeypackDefinesMulti(SoundpackData data, bool keyUp) : SoundpackDefines
{
    protected override IEnumerable<SoundpackDefine> Enumerate()
    {
        var defines = data.Defines.Deserialize(SoundpackDefinesJsonSerializerContext.Default.DictionaryInt32String);

        if (defines is null)
        {
            yield break;
        }

        var caches = new Dictionary<string, SoundpackDownUp>();

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

            using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
            {
                var buffer = reader.ReadToEnd();

                SoundpackDownUp sound;
                if (keyUp)
                {
                    WaveHelper.Split(buffer, reader.WaveFormat, out var first, out var second);
                    var down = new WaveCache(first, reader.WaveFormat);
                    var up = new WaveCache(second, reader.WaveFormat);
                    sound = new SoundpackDownUp(down, up);
                }
                else
                {
                    var source = new WaveCache(buffer, reader.WaveFormat);
                    sound = new SoundpackDownUp(source);
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
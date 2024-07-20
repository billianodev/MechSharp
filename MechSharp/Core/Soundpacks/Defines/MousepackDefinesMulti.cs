using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Billiano.Audio.FireForget;
using MechSharp.Abstraction;
using MechSharp.Json;
using MechSharp.Models;

namespace MechSharp.Core.Soundpacks.Defines;

public sealed class MousepackDefinesMulti(SoundpackData data) : SoundpackDefines
{
    protected override IEnumerable<SoundpackDefine> Enumerate()
    {
        var defines = data.Defines.Deserialize(SoundpackDefinesJsonSerializerContext.Default.DictionaryStringString);

        if (defines is null)
        {
            yield break;
        }

        var caches = new Dictionary<int, SoundpackDownUpSource>();

        foreach ((var id, var file) in defines)
        {
            if (int.TryParse(id, out var numId))
            {
                if (!caches.TryGetValue(numId, out var result))
                {
                    caches.Add(numId, result = new SoundpackDownUpSource());
                }

                if (id.StartsWith('0'))
                {
                    result.Up = file;
                }
                else
                {
                    result.Down = file;
                }
            }
        }

        foreach ((var id, var info) in caches)
        {
            WaveCache? down = null;
            WaveCache? up = null;

            if (info.Down is not null)
            {
                var path = Path.Combine(data.Dir, info.Down);

                if (File.Exists(path))
                {
                    using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
                    {
                        down = reader.ToWaveCache();
                    }
                }
            }

            if (info.Up is not null)
            {
                var path = Path.Combine(data.Dir, info.Up);

                if (File.Exists(path))
                {
                    using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
                    {
                        up = reader.ToWaveCache();
                    }
                }
            }

            var sound = new SoundpackDownUp(down, up);
            yield return new SoundpackDefine(id, sound);
        }
    }
}
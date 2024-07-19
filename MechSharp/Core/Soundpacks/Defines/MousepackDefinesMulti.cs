using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Billiano.Audio.FireForget;
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

        var caches = new Dictionary<int, (string?, string?)>();

        foreach ((var id, var file) in defines)
        {
            if (int.TryParse(id, out var numId))
            {
                if (!caches.TryGetValue(numId, out var result))
                {
                    caches.Add(numId, result = (null, null));
                }

                if (id.StartsWith('0'))
                {
                    result.Item2 = file;
                }
                else
                {
                    result.Item1 = file;
                }

                caches[numId] = result;
            }
        }

        foreach ((var id, (var down, var up)) in caches)
        {
            IFireForgetSource? downSource = null;
            IFireForgetSource? upSource = null;

            if (down is not null)
            {
                var path = Path.Combine(data.Dir, down);

                if (File.Exists(path))
                {
                    using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
                    {
                        downSource = reader.ToFireForgetSource();
                    }
                }
            }

            if (up is not null)
            {
                var path = Path.Combine(data.Dir, up);

                if (File.Exists(path))
                {
                    using (var reader = SoundpacksLoader.Codecs.GetCodec(path))
                    {
                        upSource = reader.ToFireForgetSource();
                    }
                }
            }

            var sound = new SoundpackUpDownSource(downSource, upSource);
            yield return new SoundpackDefine(id, sound);
        }
    }
}
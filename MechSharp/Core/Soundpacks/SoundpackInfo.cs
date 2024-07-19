using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using MechSharp.Core;

namespace MechSharp.Models;

public class SoundpackInfo
{
#nullable disable
    public string Id { get; set; }
    public string Name { get; set; }
    public string Dir { get; set; }
#nullable restore

    public static bool TryLoad(string dir, [NotNullWhen(true)] out SoundpackInfo? result)
    {
        return TryLoad(SoundpackInfoJsonContext.Default.SoundpackInfo, dir, out result);
    }

    protected static bool TryLoad<T>(JsonTypeInfo<T> jsonTypeInfo, string dir, [NotNullWhen(true)] out T? result) where T : SoundpackInfo
    {
        result = null;

        try
        {
            var file = Path.Combine(dir, "config.json");

            if (File.Exists(file))
            {
                using (var stream = File.OpenRead(file))
                {
                    result = JsonSerializer.Deserialize(stream, jsonTypeInfo);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }

        if (result is not null)
        {
            result.Dir = dir;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return Name;
    }
}

[JsonSerializable(typeof(SoundpackInfo))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, UseStringEnumConverter = true)]
public partial class SoundpackInfoJsonContext : JsonSerializerContext;
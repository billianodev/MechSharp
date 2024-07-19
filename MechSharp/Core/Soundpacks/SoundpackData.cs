using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using MechSharp.Core.Soundpacks.Defines;

namespace MechSharp.Models;

public sealed class SoundpackData : SoundpackInfo
{
#nullable disable
    public string Sound { get; set; }
    public KeyDefineType KeyDefineType { get; set; }
    public JsonElement Defines { get; set; }
#nullable restore

    public static bool TryLoad(SoundpackInfo info, [NotNullWhen(true)] out SoundpackData? result)
    {
        return TryLoad(SoundpackDataJsonContext.Default.SoundpackData, info.Dir, out result);
    }
}

[JsonSerializable(typeof(SoundpackData))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, UseStringEnumConverter = true)]
public partial class SoundpackDataJsonContext : JsonSerializerContext;

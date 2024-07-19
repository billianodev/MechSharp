using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MechSharp.Core.Soundpacks.Defines;
using MechSharp.Json;

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
        return TryLoad(SoundpackInfoJsonSerializerContext.Default.SoundpackData, info.Dir, out result);
    }
}
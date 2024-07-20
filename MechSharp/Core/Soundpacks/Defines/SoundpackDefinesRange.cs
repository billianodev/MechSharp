using System.Text.Json.Serialization;
using MechSharp.Json;

namespace MechSharp.Core.Soundpacks.Defines;

[JsonConverter(typeof(SoundpackDefinesRangeJsonConverter))]
public record SoundpackDefinesRange(int Start, int Length);
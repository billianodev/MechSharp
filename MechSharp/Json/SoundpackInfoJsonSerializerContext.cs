using System.Text.Json.Serialization;
using MechSharp.Models;

namespace MechSharp.Json;

[JsonSerializable(typeof(SoundpackInfo))]
[JsonSerializable(typeof(SoundpackData))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower, UseStringEnumConverter = true)]
public partial class SoundpackInfoJsonSerializerContext : JsonSerializerContext;
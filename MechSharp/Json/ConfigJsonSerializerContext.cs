using System.Text.Json.Serialization;
using MechSharp.Core;

namespace MechSharp.Json;

[JsonSerializable(typeof(Config))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class ConfigJsonSerializerContext : JsonSerializerContext;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using MechSharp.Core.Soundpacks.Defines;

namespace MechSharp.Json;

[JsonSerializable(typeof(Dictionary<int, string>))]
[JsonSerializable(typeof(Dictionary<int, SoundpackDefinesRange>))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, SoundpackDefinesRange>))]
public partial class SoundpackDefinesJsonSerializerContext : JsonSerializerContext;
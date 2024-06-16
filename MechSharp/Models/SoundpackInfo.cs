using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using MechSharp.Utilities;

namespace MechSharp;

public class SoundpackInfo
{
	[JsonIgnore]
	public string? Dir { get; set; }

	public required string Id { get; init; }
	public required string Name { get; init; }
	public required string KeyDefineType { get; init; }
	public required string? Sound { get; init; }
	public required JsonElement Defines { get; init; }

	public static bool Load(string dir, [NotNullWhen(true)] out SoundpackInfo? soundpack)
	{
		soundpack = null;
		try
		{
			var file = Path.Combine(dir, "config.json");
			if (File.Exists(file))
			{
				using (var stream = File.OpenRead(file))
				{
					soundpack = JsonSerializer.Deserialize(stream, SoundpackInfoContext.Default.SoundpackInfo);
				}
			}
		}
		catch (Exception)
		{
		}
		if (soundpack != null)
		{
			soundpack.Dir = dir;
		}
		return soundpack != null;
	}

	public override string ToString() => Name;
}

[JsonSerializable(typeof(SoundpackInfo))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<int, string>))]
[JsonSerializable(typeof(Dictionary<int, (int, int)?>))]
[JsonSourceGenerationOptions(
	Converters = [typeof(TupleConverter<int, int>)],
	PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
public partial class SoundpackInfoContext : JsonSerializerContext
{
}
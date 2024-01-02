using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MechSharp;

public class Config
{
	private static readonly string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow", "MechSharp");
	private static readonly string path = Path.Combine(directory, "config.json");

	public string? Keypack { get; set; }
	public string? Mousepack { get; set; }
	public float KeypackVolume { get; set; } = 1f;
	public float MousepackVolume { get; set; } = 1f;
	public bool IsMuteEnabled { get; set; } = false;
	public bool IsKeypackEnabled { get; set; } = true;
	public bool IsKeyUpEnabled { get; set; } = true;
	public bool IsRandomEnabled { get; set; } = true;
	public bool IsMousepackEnabled { get; set; } = true;

	public static Config Load()
	{
		Config? result = null;
		try
		{
			if (File.Exists(path))
			{
				using (var stream = File.OpenRead(path))
				{
					result = JsonSerializer.Deserialize(stream, ConfigContext.Default.Config);
				}
			}
		}
		catch
		{
		}
		return result ?? new Config();
	}

	public static void Load(out Config config)
	{
		config = Load();
	}

	public void Save()
	{
		try
		{
			Directory.CreateDirectory(directory);

			using (var stream = File.Create(path))
			{
				JsonSerializer.Serialize(stream, this, ConfigContext.Default.Config);
			}
		}
		catch
		{
		}
	}
}

[JsonSerializable(typeof(Config))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class ConfigContext : JsonSerializerContext
{
}
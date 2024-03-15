using System;
using System.Diagnostics;
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
				using (FileStream stream = File.OpenRead(path))
				{
					result = JsonSerializer.Deserialize(stream, ConfigContext.Default.Config);
				}
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
		return result ?? new Config();
	}

	public void Save()
	{
		try
		{
			Directory.CreateDirectory(directory);

			using (FileStream stream = File.Create(path))
			{
				JsonSerializer.Serialize(stream, this, ConfigContext.Default.Config);
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
	}
}

[JsonSerializable(typeof(Config))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class ConfigContext : JsonSerializerContext
{
}
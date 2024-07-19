using System;
using System.IO;
using System.Text.Json;
using MechSharp.Abstraction;
using MechSharp.Json;
using MechSharp.Utilities;
using MechSharp.ViewModels;

namespace MechSharp.Core;

public sealed class Config : IConfig
{
    private static string Path { get; }

    public string? Keypack { get; set; }
    public string? Mousepack { get; set; }
    public float KeypackVolume { get; set; }
    public float MousepackVolume { get; set; }
    public bool IsMuted { get; set; }
    public bool IsKeypackEnabled { get; set; }
    public bool IsKeyUpEnabled { get; set; }
    public bool IsRandomEnabled { get; set; }
    public bool IsMousepackEnabled { get; set; }

    static Config()
    {
        Path = PathHelper.AppDataDir.Combine("Config.json");
    }

    public static void Load(AppViewModel viewModel, SoundpacksLoader loader)
    {
        try
        {
            if (!File.Exists(Path))
            {
                return;
            }

            using (var stream = File.OpenRead(Path))
            {
                var result = JsonSerializer.Deserialize(stream, ConfigJsonSerializerContext.Default.Config);

                if (result is null)
                {
                    return;
                }

                viewModel.Keypack = loader.Keypacks?.Find(result.Keypack);
                viewModel.Mousepack = loader.Mousepacks?.Find(result.Mousepack);
                viewModel.KeypackVolume = result.KeypackVolume;
                viewModel.MousepackVolume = result.MousepackVolume;
                viewModel.IsMuted = result.IsMuted;
                viewModel.IsKeypackEnabled = result.IsKeypackEnabled;
                viewModel.IsKeyUpEnabled = result.IsKeyUpEnabled;
                viewModel.IsRandomEnabled = result.IsRandomEnabled;
                viewModel.IsMousepackEnabled = result.IsMousepackEnabled;
            }
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }
    }

    public static void Save(AppViewModel viewModel)
    {
        var config = new Config()
        {
            Keypack = viewModel.Keypack?.Dir,
            Mousepack = viewModel.Mousepack?.Dir,
            KeypackVolume = viewModel.KeypackVolume,
            MousepackVolume = viewModel.MousepackVolume,
            IsMuted = viewModel.IsMuted,
            IsKeypackEnabled = viewModel.IsKeypackEnabled,
            IsKeyUpEnabled = viewModel.IsKeyUpEnabled,
            IsRandomEnabled = viewModel.IsRandomEnabled,
            IsMousepackEnabled = viewModel.IsMousepackEnabled,
        };

        try
        {
            using (var stream = File.Create(Path))
            {
                JsonSerializer.Serialize(stream, config, ConfigJsonSerializerContext.Default.Config);
            }
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }
    }
}
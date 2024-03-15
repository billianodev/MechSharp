using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Billiano.NAudio;
using Billiano.AutoLaunch;
using MechSharp.Utilities;
using MechSharp.Views;
using SharpHook;
using SharpHook.Native;

namespace MechSharp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public bool IsKeyDown { get; private set; }

	private readonly App app;
	private readonly MainWindow window;

	private readonly StartupManager startupManager;
	
	private readonly AudioEngine audioEngine;
	private readonly SimpleGlobalHook hook;
	private readonly HashSet<KeyCode> pressedKeys;

	private readonly Config config;
	private Soundpack? keypack;
	private Soundpack? mousepack;

	public MainWindowViewModel(App app, MainWindow window)
	{
		this.app = app;
		this.window = window;

		startupManager = new();

		audioEngine = new();
		hook = new(true);
		hook.KeyPressed += Hook_KeyPressed;
		hook.KeyReleased += Hook_KeyReleased;
		hook.MousePressed += Hook_MousePressed;
		hook.MouseReleased += Hook_MouseReleased;
		pressedKeys = [];

		config = Config.Load();
	}

	public void Load()
	{
		IEnumerable<SoundpackInfo> keypacks = LoadKeypackInfos();
		IEnumerable<SoundpackInfo> mousepacks = LoadMousepackInfos();

		window.KeypackVolumeSlider.Value = config.KeypackVolume;
		window.MousepackVolumeSlider.Value = config.MousepackVolume;
		window.MuteCheckBox.IsChecked = config.IsMuteEnabled;
		window.KeypackCheckBox.IsChecked = config.IsKeypackEnabled;
		window.KeyUpCheckBox.IsChecked = config.IsKeyUpEnabled;
		window.MousepackCheckBox.IsChecked = config.IsMousepackEnabled;
		window.RandomCheckBox.IsChecked = config.IsRandomEnabled;

		foreach (SoundpackInfo info in keypacks)
		{
			if (info.Dir == config.Keypack)
			{
				window.KeypackSelector.SelectedItem = info;
				break;
			}
		}
		
		foreach (SoundpackInfo info in mousepacks)
		{
			if (info.Dir == config.Mousepack)
			{
				window.MousepackSelector.SelectedItem = info;
				break;
			}
		}

		window.StartupCheckBox.IsEnabled = startupManager != null;
		window.StartupCheckBox.IsChecked = startupManager?.Get();
		hook.RunAsync();
	}

	public void Save()
	{
		config.Keypack = keypack?.Info.Dir;
		config.Mousepack = mousepack?.Info.Dir;
		config.Save();
	}

	public IEnumerable<SoundpackInfo> LoadKeypackInfos()
	{
		try
		{
			IEnumerable<SoundpackInfo> keypacks = Soundpacks.LoadKeypackInfos();
			window.KeypackSelector.ItemsSource = keypacks;
			return keypacks;
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
		return [];
	}

	public IEnumerable<SoundpackInfo> LoadMousepackInfos()
	{
		try
		{
			IEnumerable<SoundpackInfo> mousepacks = Soundpacks.LoadMousepackInfos();
			window.MousepackSelector.ItemsSource = mousepacks;
			return mousepacks;
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
		return [];
	}

	public void LoadKeypack(SoundpackInfo? info)
	{
		if (!config.IsKeypackEnabled || info == null)
		{
			keypack = null;
			window.KeypackSelector.SelectedItem = null;
			return;
		}

		Task.Run(() =>
		{
			try
			{
				keypack = Soundpack.LoadKeypack(info, audioEngine.WaveFormat, config.IsKeyUpEnabled);
			}
			catch (Exception ex)
			{
				Logger.WriteLine(ex);
			}
		});
	}

	public void LoadMousepack(SoundpackInfo? info)
	{
		if (!config.IsMousepackEnabled || info == null)
		{
			mousepack = null;
			window.MousepackSelector.SelectedItem = null;
			return;
		}

		Task.Run(() =>
		{
			try
			{
				mousepack = Soundpack.LoadMousepack(info, audioEngine.WaveFormat);
			}
			catch (Exception ex)
			{
				Logger.WriteLine(ex);
			}
		});
	}

	public void SetKeypackVolume(double value)
	{
		config.KeypackVolume = (float)value;
	}

	public void SetMousepackVolume(double value)
	{
		config.MousepackVolume = (float)value;
	}

	public void ToggleMute(bool value)
	{
		config.IsMuteEnabled = value;
		pressedKeys.Clear();
	}

	public void ToggleKeypack(bool value)
	{
		config.IsKeypackEnabled = value;
		pressedKeys.Clear();
		LoadKeypack(keypack?.Info);
	}

	public void ToggleKeyUp(bool value)
	{
		config.IsKeyUpEnabled = value;
		LoadKeypack(keypack?.Info);
	}

	public void ToggleRandom(bool value)
	{
		config.IsRandomEnabled = value;
	}

	public void ToggleMousepack(bool value)
	{
		config.IsMousepackEnabled = value;
		LoadMousepack(mousepack?.Info);
	}

	public void ToggleStartup(bool value)
	{
		startupManager.Set(value);
	}

	#region Hook Callback
	private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
	{
		if (config.IsMuteEnabled || !config.IsKeypackEnabled)
		{
			return;
		}
		if (!pressedKeys.Add(e.Data.KeyCode))
		{
			return;
		}
		if (!IsKeyDown)
		{
			IsKeyDown = true;
			PropertyChanged?.Invoke(this, new(nameof(IsKeyDown)));
		}
		try
		{
			int key = MechvibesKey.Map(e.Data.KeyCode, config.IsRandomEnabled);
			if (key != MechvibesKey.MkNone && keypack?.TryGetValue(key, out SampleCache? sample) == true)
			{
				audioEngine.Play(sample, config.KeypackVolume);
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
	}

	private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
	{
		if (config.IsMuteEnabled || !config.IsKeypackEnabled)
		{
			return;
		}

		pressedKeys.Remove(e.Data.KeyCode);

		if (pressedKeys.Count == 0 && IsKeyDown)
		{
			IsKeyDown = false;
			PropertyChanged?.Invoke(this, new(nameof(IsKeyDown)));
		}
		if (!config.IsKeyUpEnabled)
		{
			return;
		}
		try
		{
			int key = -MechvibesKey.Map(e.Data.KeyCode, config.IsRandomEnabled);
			if (key != MechvibesKey.MkNone && keypack?.TryGetValue(key, out SampleCache? sample) == true)
			{
				audioEngine.Play(sample, config.KeypackVolume);
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
	}

	private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
	{
		if (config.IsMuteEnabled || !config.IsMousepackEnabled)
		{
			return;
		}
		try
		{
			int key = MousevibesButton.Map(e.Data.Button);
			if (key != MousevibesButton.MbNone && mousepack?.TryGetValue(key, out SampleCache? sample) == true)
			{
				audioEngine.Play(sample, config.MousepackVolume);
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
	}

	private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
	{
		if (config.IsMuteEnabled || !config.IsMousepackEnabled)
		{
			return;
		}
		try
		{
			int key = MousevibesButton.Map(e.Data.Button);
			if (key != MousevibesButton.MbNone && mousepack?.TryGetValue(key, out SampleCache? sample) == true)
			{
				audioEngine.Play(sample, config.MousepackVolume);
			}
		}
		catch (Exception ex)
		{
			Logger.WriteLine(ex);
		}
	}
	#endregion
}

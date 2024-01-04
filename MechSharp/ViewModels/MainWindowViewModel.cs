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
using System.Diagnostics;

namespace MechSharp.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	public bool IsKeyDown { get; private set; }

	private readonly App _app;
	private readonly MainWindow _window;

	private readonly StartupManager _startupManager;
	
	private readonly AudioEngine _audioEngine;
	private readonly SimpleGlobalHook _hook;
	private readonly HashSet<KeyCode> _pressedKeys;

	private readonly Config _config;
	private Soundpack? _keypack;
	private Soundpack? _mousepack;

	public MainWindowViewModel(App app, MainWindow window)
	{
		try
		{
			_app = app;
			_window = window;

			_startupManager = new StartupManager();

			_audioEngine = new AudioEngine();
			_hook = new SimpleGlobalHook(true);
			_hook.KeyPressed += Hook_KeyPressed;
			_hook.KeyReleased += Hook_KeyReleased;
			_hook.MousePressed += Hook_MousePressed;
			_hook.MouseReleased += Hook_MouseReleased;
			_pressedKeys = [];

			_config = Config.Load();
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			throw;
		}
	}

	public void Load()
	{
		var keypacks = LoadKeypackInfos();
		var mousepacks = LoadMousepackInfos();

		_window.KeypackVolumeSlider.Value = _config.KeypackVolume;
		_window.MousepackVolumeSlider.Value = _config.MousepackVolume;
		_window.MuteCheckBox.IsChecked = _config.IsMuteEnabled;
		_window.KeypackCheckBox.IsChecked = _config.IsKeypackEnabled;
		_window.KeyUpCheckBox.IsChecked = _config.IsKeyUpEnabled;
		_window.MousepackCheckBox.IsChecked = _config.IsMousepackEnabled;
		_window.RandomCheckBox.IsChecked = _config.IsRandomEnabled;

		foreach (var info in keypacks)
		{
			if (info.Dir == _config.Keypack)
			{
				_window.KeypackSelector.SelectedItem = info;
				break;
			}
		}

		foreach (var info in mousepacks)
		{
			if (info.Dir == _config.Mousepack)
			{
				_window.MousepackSelector.SelectedItem = info;
			}
		}

		_window.StartupCheckBox.IsEnabled = _startupManager != null;
		_window.StartupCheckBox.IsChecked = _startupManager?.Get();
		_hook.RunAsync();
	}

	public void Save()
	{
		_config.Keypack = _keypack?.Info.Dir;
		_config.Mousepack = _mousepack?.Info.Dir;
		_config.Save();
	}

	public IEnumerable<SoundpackInfo> LoadKeypackInfos()
	{
		try
		{
			var keypacks = Soundpacks.LoadKeypackInfos();
			_window.KeypackSelector.ItemsSource = keypacks;
			return keypacks;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
		return [];
	}

	public IEnumerable<SoundpackInfo> LoadMousepackInfos()
	{
		try
		{
			var mousepacks = Soundpacks.LoadMousepackInfo();
			_window.MousepackSelector.ItemsSource = mousepacks;
			return mousepacks;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
		return [];
	}

	public void LoadKeypack(SoundpackInfo? info)
	{
		if (!_config.IsKeypackEnabled || info == null)
		{
			_keypack = null;
			_window.KeypackSelector.SelectedItem = null;
			return;
		}

		Task.Run(() =>
		{
			try
			{
				_keypack = Soundpack.LoadKeypack(info, _audioEngine.WaveFormat, _config.IsKeyUpEnabled);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});
	}

	public void LoadMousepack(SoundpackInfo? info)
	{
		if (!_config.IsMousepackEnabled || info == null)
		{
			_mousepack = null;
			_window.MousepackSelector.SelectedItem = null;
			return;
		}

		Task.Run(() =>
		{
			try
			{
				_mousepack = Soundpack.LoadMousepack(info, _audioEngine.WaveFormat);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});
	}

	public void SetKeypackVolume(double value)
	{
		_config.KeypackVolume = (float)value;
	}

	public void SetMousepackVolume(double value)
	{
		_config.MousepackVolume = (float)value;
	}

	public void ToggleMute(bool value)
	{
		_config.IsMuteEnabled = value;
		_pressedKeys.Clear();
	}

	public void ToggleKeypack(bool value)
	{
		_config.IsKeypackEnabled = value;
		_pressedKeys.Clear();
		LoadKeypack(_keypack?.Info);
	}

	public void ToggleKeyUp(bool value)
	{
		_config.IsKeyUpEnabled = value;
		LoadKeypack(_keypack?.Info);
	}

	public void ToggleRandom(bool value)
	{
		_config.IsRandomEnabled = value;
	}

	public void ToggleMousepack(bool value)
	{
		_config.IsMousepackEnabled = value;
		LoadMousepack(_mousepack?.Info);
	}

	public void ToggleStartup(bool value)
	{
		_startupManager.Set(value);
	}

	#region Hook Callback
	private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
	{
		if (_config.IsMuteEnabled || !_config.IsKeypackEnabled) return;

		if (!_pressedKeys.Add(e.Data.KeyCode)) return;

		if (!IsKeyDown)
		{
			IsKeyDown = true;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsKeyDown)));
		}

		try
		{
			var key = MechvibesMapper.MapKeyCode(e.Data.KeyCode, _config.IsRandomEnabled);
			if (key != null && _keypack?.TryGetValue(key, out var wave) == true)
			{
				_audioEngine.Play(wave, _config.KeypackVolume);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}

	private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
	{
		if (_config.IsMuteEnabled || !_config.IsKeypackEnabled) return;

		_pressedKeys.Remove(e.Data.KeyCode);

		if (_pressedKeys.Count == 0 && IsKeyDown)
		{
			IsKeyDown = false;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsKeyDown)));
		}

		if (!_config.IsKeyUpEnabled) return;

		try
		{
			var key = MechvibesMapper.MapKeyCodeUp(e.Data.KeyCode, _config.IsRandomEnabled);
			if (key != null && _keypack?.TryGetValue(key, out var wave) == true)
			{
				_audioEngine.Play(wave, _config.KeypackVolume);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}

	private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
	{
		if (_config.IsMuteEnabled || !_config.IsMousepackEnabled) return;
		try
		{
			var key = MechvibesMapper.MapMouseButton(e.Data.Button);
			if (key != null && _mousepack?.TryGetValue(key, out var wave) == true)
			{
				_audioEngine.Play(wave, _config.MousepackVolume);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}

	private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
	{
		if (_config.IsMuteEnabled || !_config.IsMousepackEnabled) return;
		try
		{
			var key = MechvibesMapper.MapMouseButtonUp(e.Data.Button);
			if (key != null && _mousepack?.TryGetValue(key, out var wave) == true)
			{
				_audioEngine.Play(wave, _config.MousepackVolume);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}
	#endregion
}

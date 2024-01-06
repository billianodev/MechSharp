using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MechSharp.ViewModels;

namespace MechSharp.Views;

public partial class MainWindow : Window
{
	private const string GitHub = "https://github.com/billiano-dev/MechSharp";
	private const string Kofi = "https://ko-fi.com/billiano";
	private const string Mechvibes = "https://mechvibes.com";

	public MainWindowViewModel? ViewModel { get; }

	public MainWindow()
	{
		InitializeComponent();
	}

	public MainWindow(App app) : this()
	{
		DataContext = ViewModel = new(app, this);
	}

	public static void Open(string url)
	{
		try
		{
			Process.Start(new ProcessStartInfo()
			{
				FileName = url,
				UseShellExecute = true,
				Verb = "open"
			});
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		ViewModel?.Load();
	}

	protected override void OnClosing(WindowClosingEventArgs e)
	{
		if (OperatingSystem.IsWindows() && e.CloseReason == WindowCloseReason.WindowClosing && !e.IsProgrammatic)
		{
			Hide();
			e.Cancel = true;
		}
		ViewModel?.Save();
	}

	#region Menu

	private void OpenKeypacksMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Open(Soundpacks.KeypackCustomPath);
	}

	private void OpenMousepacksMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Open(Soundpacks.MousepackCustomPath);
	}

	private void RefreshMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		ViewModel?.LoadKeypackInfos();
		ViewModel?.LoadMousepackInfos();
	}

	private void ExitMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private void GitHubMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Open(GitHub);
	}

	private void KofiMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Open(Kofi);
	}

	private void MechvibesMenuItem_Click(object? sender, RoutedEventArgs e)
	{
		Open(Mechvibes);
	}

	#endregion

	private void KeypackSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		ViewModel?.LoadKeypack(KeypackSelector.SelectedItem as SoundpackInfo);
	}

	private void MousepackSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		ViewModel?.LoadMousepack(MousepackSelector.SelectedItem as SoundpackInfo);
	}

	private void KeypackVolumeSlider_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
	{
		ViewModel?.SetKeypackVolume(e.NewValue);
	}

	private void MousepackVolumeSlider_ValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
	{
		ViewModel?.SetMousepackVolume(e.NewValue);
	}

	private void MuteCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleMute(MuteCheckBox.IsChecked == true);
	}

	private void KeypackCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleKeypack(KeypackCheckBox.IsChecked == true);
	}

	private void KeyUpCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleKeyUp(KeyUpCheckBox.IsChecked == true);
	}

	private void RandomCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleRandom(RandomCheckBox.IsChecked == true);
	}

	private void MousepackCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleMousepack(MousepackCheckBox.IsChecked == true);
	}

	private void StartupCheckBox_Checked(object? sender, RoutedEventArgs e)
	{
		ViewModel?.ToggleStartup(StartupCheckBox.IsChecked == true);
	}
}

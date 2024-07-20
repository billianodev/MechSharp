using System;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using MechSharp.Core;
using MechSharp.Utilities;

namespace MechSharp.ViewModels;

public partial class MenuViewModel : ViewModelBase
{
    private static Uri GitHubUri { get; }
    private static Uri KofiUri { get; }
    private static Uri MechvibesUri { get; }

    static MenuViewModel()
    {
        GitHubUri = new Uri("https://github.com/billianodev/MechSharp");
        KofiUri = new("https://ko-fi.com/G2G1SRUJG");
        MechvibesUri = new("https://mechvibes.com");
    }

    [RelayCommand]
    private static void RefreshSoundpacks()
    {
        if (App?.ViewModel is AppViewModel viewModel)
        {
            viewModel.Save();
            viewModel.Load();
        }
    }

    [RelayCommand]
    private static Task OpenKeypacksFolder()
    {
        return TopLevel?.Launcher.LaunchDirectoryAsync(SoundpacksLoader.KeypackCustomDirectory) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenMousepacksFolder()
    {
        return TopLevel?.Launcher.LaunchDirectoryAsync(SoundpacksLoader.MousepackCustomDirectory) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static void Exit()
    {
        if (App?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.TryShutdown();
        }
    }

    [RelayCommand]
    private static Task OpenGitHubLink()
    {
        return TopLevel?.Launcher.LaunchUriAsync(GitHubUri) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenKofiLink()
    {
        return TopLevel?.Launcher.LaunchUriAsync(KofiUri) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenMechvibesLink()
    {
        return TopLevel?.Launcher.LaunchUriAsync(MechvibesUri) ?? Task.CompletedTask;
    }
}

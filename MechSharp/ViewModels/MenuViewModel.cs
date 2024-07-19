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
    private static Task OpenKeypacks()
    {
        return TopLevel?.Launcher.LaunchDirectoryAsync(SoundpacksLoader.KeypackCustomDirectory) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenMousepacks()
    {
        return TopLevel?.Launcher.LaunchDirectoryAsync(SoundpacksLoader.MousepackCustomDirectory) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static void Refresh()
    {
        if (App?.ViewModel is not null)
        {
            App.ViewModel.Save();
            App.ViewModel.Load();
        }
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
    private static Task OpenGitHub()
    {
        return TopLevel?.Launcher.LaunchUriAsync(GitHubUri) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenKofi()
    {
        return TopLevel?.Launcher.LaunchUriAsync(KofiUri) ?? Task.CompletedTask;
    }

    [RelayCommand]
    private static Task OpenMechvibes()
    {
        return TopLevel?.Launcher.LaunchUriAsync(MechvibesUri) ?? Task.CompletedTask;
    }
}

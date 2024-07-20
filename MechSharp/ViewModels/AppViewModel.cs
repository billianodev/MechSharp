using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Billiano.AutoLaunch;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MechSharp.Abstraction;
using MechSharp.Core;
using MechSharp.Models;

namespace MechSharp.ViewModels;

public partial class AppViewModel : ViewModelBase, IConfig<SoundpackInfo>
{
    private readonly AutoLaunchManager _autoLaunchManager;
    private readonly MechPlayer _mechPlayer;
    private readonly object _lockObject;

    [ObservableProperty]
    private SoundpackInfo? _keypack;

    [ObservableProperty]
    private SoundpackInfo? _mousepack;

    [ObservableProperty]
    private float _keypackVolume = 1f;

    [ObservableProperty]
    private float _mousepackVolume = 1f;

    [ObservableProperty]
    private bool _isKeypackEnabled = true;

    [ObservableProperty]
    private bool _isKeyUpEnabled = true;

    [ObservableProperty]
    private bool _isRandomEnabled = true;

    [ObservableProperty]
    private bool _isMousepackEnabled = true;

    public bool IsEnableAtStartup
    {
        get => _autoLaunchManager.Get();
        set => _autoLaunchManager.Set(value);
    }

    public InputManager InputManager { get; }
    public SoundpacksLoader SoundpacksLoader { get; }

    public AppViewModel()
    {
        InputManager = new InputManager();
        SoundpacksLoader = new SoundpacksLoader();

        _autoLaunchManager = new AutoLaunchManager();
        _mechPlayer = new MechPlayer(this);
        _lockObject = new object();

        if (Design.IsDesignMode)
        {
            return;
        }

        InputManager.Register(_mechPlayer);
        InputManager.Run();

        _mechPlayer.Run();

        Load();
    }

    public void Load()
    {
        SoundpacksLoader.Load();

        lock (_lockObject)
        {
            Config.Load(this, SoundpacksLoader);
        }

        UpdateKeypack();
        UpdateMousepack();
    }

    public void Save()
    {
        Config.Save(this);
    }

    private void UpdateKeypack()
    {
        if (Monitor.IsEntered(_lockObject))
        {
            return;
        }

        _mechPlayer.LoadKeypack(IsKeypackEnabled ? Keypack : null, IsKeyUpEnabled);
    }

    private void UpdateMousepack()
    {
        if (Monitor.IsEntered(_lockObject))
        {
            return;
        }

        _mechPlayer.LoadMousepack(IsMousepackEnabled ? Mousepack : null);
    }

    [RelayCommand]
    private static void Show()
    {
        TopLevel?.Show();
    }

    [RelayCommand]
    private static void Exit()
    {
        if (App?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.TryShutdown();
        }
    }

    partial void OnKeypackChanged(SoundpackInfo? value)
    {
        UpdateKeypack();
    }

    partial void OnMousepackChanged(SoundpackInfo? value)
    {
        UpdateMousepack();
    }

    partial void OnIsKeypackEnabledChanged(bool value)
    {
        UpdateKeypack();
    }

    partial void OnIsKeyUpEnabledChanged(bool value)
    {
        UpdateKeypack();
    }

    partial void OnIsMousepackEnabledChanged(bool value)
    {
        UpdateMousepack();
    }
}
using System.Threading;
using Avalonia.Controls;
using Billiano.AutoLaunch;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MechSharp.Abstraction;
using MechSharp.Core;
using MechSharp.Models;

namespace MechSharp.ViewModels;

public partial class AppViewModel : ViewModelBase, IConfig
{
    [ObservableProperty]
    private SoundpackInfo? _keypack;

    [ObservableProperty]
    private SoundpackInfo? _mousepack;

    [ObservableProperty]
    private float _keypackVolume = 1f;

    [ObservableProperty]
    private float _mousepackVolume = 1f;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsKeypackControlEnabled))]
    [NotifyPropertyChangedFor(nameof(IsMousepackControlEnabled))]
    private bool _isMuted;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsKeypackControlEnabled))]
    private bool _isKeypackEnabled = true;

    [ObservableProperty]
    private bool _isKeyUpEnabled = true;

    [ObservableProperty]
    private bool _isRandomEnabled = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMousepackControlEnabled))]
    private bool _isMousepackEnabled = true;

    public bool IsEnabledAtStartup
    {
        get => _autoLaunchManager.Get();
        set => _autoLaunchManager.Set(value);
    }

    public bool IsKeypackControlEnabled => !IsMuted && IsKeypackEnabled;
    public bool IsMousepackControlEnabled => !IsMuted && IsMousepackEnabled;

    public InputManager InputManager { get; }
    public SoundpacksLoader SoundpacksLoader { get; }

    private readonly AutoLaunchManager _autoLaunchManager;
    private readonly MechPlayer _mechPlayer;

    private readonly object _lockObject = new();

    public AppViewModel()
    {
        InputManager = new InputManager();
        SoundpacksLoader = new SoundpacksLoader();

        _autoLaunchManager = new AutoLaunchManager();
        _mechPlayer = new MechPlayer(this);

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

        if (IsMuted || !IsKeypackEnabled)
        {
            _mechPlayer.LoadKeypack(null, IsKeyUpEnabled);
            return;
        }

        _mechPlayer.LoadKeypack(Keypack, IsKeyUpEnabled);
    }

    private void UpdateMousepack()
    {
        if (Monitor.IsEntered(_lockObject))
        {
            return;
        }

        if (IsMuted || !IsMousepackEnabled)
        {
            _mechPlayer.LoadMousepack(null);
            return;
        }

        _mechPlayer.LoadMousepack(Mousepack);
    }

    [RelayCommand]
    private static void Focus()
    {
        TopLevel?.Show();
    }

    partial void OnKeypackChanged(SoundpackInfo? value)
    {
        UpdateKeypack();
    }

    partial void OnMousepackChanged(SoundpackInfo? value)
    {
        UpdateMousepack();
    }

    partial void OnIsMutedChanged(bool value)
    {
        UpdateKeypack();
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
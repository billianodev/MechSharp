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
    private SoundpackInfo? keypack;

    [ObservableProperty]
    private SoundpackInfo? mousepack;

    [ObservableProperty]
    private float keypackVolume = 1f;

    [ObservableProperty]
    private float mousepackVolume = 1f;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsKeypackControlEnabled))]
    [NotifyPropertyChangedFor(nameof(IsMousepackControlEnabled))]
    private bool isMuted;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsKeypackControlEnabled))]
    private bool isKeypackEnabled = true;

    [ObservableProperty]
    private bool isKeyUpEnabled = true;

    [ObservableProperty]
    private bool isRandomEnabled = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMousepackControlEnabled))]
    private bool isMousepackEnabled = true;

    public bool IsEnabledAtStartup
    {
        get => AutoLaunchManager.Get();
        set => AutoLaunchManager.Set(value);
    }

    public bool IsKeypackControlEnabled => !IsMuted && IsKeypackEnabled;
    public bool IsMousepackControlEnabled => !IsMuted && IsMousepackEnabled;

    public AutoLaunchManager AutoLaunchManager { get; }
    public SoundpacksLoader SoundpackLoader { get; }
    public InputManager InputManager { get; }
    public MechPlayer MechPlayer { get; }

    private readonly object lockObject = new();

    public AppViewModel()
    {
        AutoLaunchManager = new AutoLaunchManager();
        SoundpackLoader = new SoundpacksLoader();
        InputManager = new InputManager();
        MechPlayer = new MechPlayer(this, InputManager);

        if (Design.IsDesignMode)
        {
            return;
        }

        Load();
    }

    public void Load()
    {
        SoundpackLoader.Load();

        lock (lockObject)
        {
            Config.Load(this, SoundpackLoader);
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
        if (Monitor.IsEntered(lockObject))
        {
            return;
        }

        if (IsMuted || !IsKeypackEnabled)
        {
            MechPlayer.LoadKeypack(null, IsKeyUpEnabled);
            return;
        }

        MechPlayer.LoadKeypack(Keypack, IsKeyUpEnabled);
    }

    private void UpdateMousepack()
    {
        if (Monitor.IsEntered(lockObject))
        {
            return;
        }

        if (IsMuted || !IsMousepackEnabled)
        {
            MechPlayer.LoadMousepack(null);
            return;
        }

        MechPlayer.LoadMousepack(Mousepack);
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
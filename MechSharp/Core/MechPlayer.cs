using Billiano.Audio.FireForget;
using Billiano.Audio.PortAudio;
using MechSharp.Abstraction;
using MechSharp.Core.Soundpacks;
using MechSharp.Models;
using MechSharp.Utilities;
using NAudio.Wave;
using SharpHook.Native;

namespace MechSharp.Core;

public sealed class MechPlayer : IInputReceiver
{
    private readonly PortAudioOut _playerBackend;
    private readonly FireForgetPlayer _player;

    private readonly IRuntimeConfig _config;

    private Soundpack? _keypack;
    private Soundpack? _mousepack;

    public MechPlayer(IRuntimeConfig config)
    {
        _playerBackend = new PortAudioOut();
        _player = new ResamplingFireForgetPlayer(_playerBackend, WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));

        _config = config;
    }

    public void Run()
    {
        _player.Run();
    }

    public void LoadKeypack(SoundpackInfo? info, bool keyUp)
    {
        if (info is null)
        {
            _keypack = null;
            return;
        }

        if (SoundpackData.TryLoad(info, out var data))
        {
            if (Soundpack.TryLoadKeypack(data, keyUp, out var keypack))
            {
                _keypack = keypack;
            }
        }
    }

    public void LoadMousepack(SoundpackInfo? info)
    {
        if (info is null)
        {
            _mousepack = null;
            return;
        }

        if (SoundpackData.TryLoad(info, out var data))
        {
            if (Soundpack.TryLoadMousepack(data, out var mousepack))
            {
                _mousepack = mousepack;
            }
        }
    }

    public void OnKeyPressed(InputManager sender, KeyCode keyCode)
    {
        if (_keypack is null)
        {
            return;
        }

        var code = MechvibesKey.Map(keyCode, _config.IsRandomEnabled);
        if (_keypack.Defines.TryGetDownSound(code, out var result))
        {
            _player.Play(result, _config.KeypackVolume);
        }
    }

    public void OnKeyReleased(InputManager sender, KeyCode keyCode)
    {
        if (_keypack is null)
        {
            return;
        }

        var code = MechvibesKey.Map(keyCode, _config.IsRandomEnabled);
        if (_keypack.Defines.TryGetUpSound(code, out var result))
        {
            _player.Play(result, _config.KeypackVolume);
        }
    }

    public void OnMousePressed(InputManager sender, MouseButton mouseButton)
    {
        if (_mousepack is null)
        {
            return;
        }

        var code = MousevibesButton.Map(mouseButton);
        if (_mousepack.Defines.TryGetDownSound(code, out var result))
        {
            _player.Play(result, _config.MousepackVolume);
        }
    }

    public void OnMouseReleased(InputManager sender, MouseButton mouseButton)
    {
        if (_mousepack is null)
        {
            return;
        }

        var code = MousevibesButton.Map(mouseButton);
        if (_mousepack.Defines.TryGetUpSound(code, out var result))
        {
            _player.Play(result, _config.MousepackVolume);
        }
    }
}

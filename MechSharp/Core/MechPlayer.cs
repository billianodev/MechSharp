using System;
using Billiano.Audio.FireForget;
using Billiano.Audio.PortAudio;
using MechSharp.Abstraction;
using MechSharp.Core.Soundpacks;
using MechSharp.Models;
using MechSharp.Utilities;
using NAudio.Wave;
using SharpHook.Native;

namespace MechSharp.Core;

public sealed class MechPlayer : IInputReceiver, IDisposable
{
    private readonly PortAudioOut playerBackend;
    private readonly FireForgetPlayer player;

    private readonly IRuntimeConfig config;

    private Soundpack? keypack;
    private Soundpack? mousepack;

    public MechPlayer(IRuntimeConfig config, InputManager input)
    {
        playerBackend = new PortAudioOut();
        player = new FireForgetPlayer(playerBackend, new WaveFormat(44100, 1));

        this.config = config;

        input.Register(this);
    }

    public void LoadKeypack(SoundpackInfo? info, bool keyUp)
    {
        if (info is null)
        {
            keypack = null;
            return;
        }

        if (SoundpackData.TryLoad(info, out var data))
        {
            if (Soundpack.TryLoadKeypack(data, keyUp, out var keypack))
            {
                this.keypack = keypack;
            }
        }
    }

    public void LoadMousepack(SoundpackInfo? info)
    {
        if (info is null)
        {
            mousepack = null;
            return;
        }

        if (SoundpackData.TryLoad(info, out var data))
        {
            if (Soundpack.TryLoadMousepack(data, out var mousepack))
            {
                this.mousepack = mousepack;
            }
        }
    }

    public void Dispose()
    {
        player.Dispose();
        playerBackend.Dispose();
    }

    public void OnKeyPressed(InputManager sender, KeyCode keyCode)
    {
        if (keypack is null)
        {
            return;
        }

        var code = MechvibesKey.Map(keyCode, config.IsRandomEnabled);
        if (keypack.Defines.TryGetDownSound(code, out var result))
        {
            player.Play(result, config.KeypackVolume);
        }
    }

    public void OnKeyReleased(InputManager sender, KeyCode keyCode)
    {
        if (keypack is null)
        {
            return;
        }

        var code = MechvibesKey.Map(keyCode, config.IsRandomEnabled);
        if (keypack.Defines.TryGetUpSound(code, out var result))
        {
            player.Play(result, config.KeypackVolume);
        }
    }

    public void OnMousePressed(InputManager sender, MouseButton mouseButton)
    {
        if (mousepack is null)
        {
            return;
        }

        var code = MousevibesButton.Map(mouseButton);
        if (mousepack.Defines.TryGetDownSound(code, out var result))
        {
            player.Play(result, config.MousepackVolume);
        }
    }

    public void OnMouseReleased(InputManager sender, MouseButton mouseButton)
    {
        if (mousepack is null)
        {
            return;
        }

        var code = MousevibesButton.Map(mouseButton);
        if (mousepack.Defines.TryGetUpSound(code, out var result))
        {
            player.Play(result, config.MousepackVolume);
        }
    }
}

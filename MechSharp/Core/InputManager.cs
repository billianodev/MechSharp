using System;
using System.Collections.Generic;
using SharpHook;
using SharpHook.Native;

namespace MechSharp.Core;

public sealed class InputManager : IDisposable
{
    private readonly SimpleGlobalHook hook;
    private readonly HashSet<KeyCode> pressedKeys;
    private readonly List<IInputReceiver> receivers;

    public IReadOnlyCollection<KeyCode> PressedKeys => pressedKeys;

    public InputManager()
    {
        hook = new SimpleGlobalHook(true);
        hook.KeyPressed += Hook_KeyPressed;
        hook.KeyReleased += Hook_KeyReleased;
        hook.MousePressed += Hook_MousePressed;
        hook.MouseReleased += Hook_MouseReleased;

        pressedKeys = [];
        receivers = [];

        hook.RunAsync();
    }

    public InputManager(params IInputReceiver[] receiver) : this()
    {
        receivers.AddRange(receiver);
    }

    public void Register(IInputReceiver receiver)
    {
        receivers.Add(receiver);
    }

    public void Reset()
    {
        pressedKeys.Clear();
    }

    public void Dispose()
    {
        hook.Dispose();
    }

    private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (pressedKeys.Add(e.Data.KeyCode))
        {
            foreach (var receiver in receivers)
            {
                receiver.OnKeyPressed(this, e.Data.KeyCode);
            }
        }
    }

    private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        if (pressedKeys.Remove(e.Data.KeyCode))
        {
            foreach (var receiver in receivers)
            {
                receiver.OnKeyReleased(this, e.Data.KeyCode);
            }
        }
    }

    private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
    {
        foreach (var receiver in receivers)
        {
            receiver.OnMousePressed(this, e.Data.Button);
        }
    }

    private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
    {
        foreach (var receiver in receivers)
        {
            receiver.OnMouseReleased(this, e.Data.Button);
        }
    }
}

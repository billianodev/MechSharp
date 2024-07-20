using System.Collections.Generic;
using SharpHook;
using SharpHook.Native;

namespace MechSharp.Core;

public sealed class InputManager
{
    private readonly SimpleGlobalHook _hook;
    private readonly HashSet<KeyCode> _pressedKeys;
    private readonly List<IInputReceiver> _receivers;

    public IReadOnlyCollection<KeyCode> PressedKeys => _pressedKeys;

    public InputManager()
    {
        _hook = new SimpleGlobalHook(true);
        _hook.KeyPressed += Hook_KeyPressed;
        _hook.KeyReleased += Hook_KeyReleased;
        _hook.MousePressed += Hook_MousePressed;
        _hook.MouseReleased += Hook_MouseReleased;

        _pressedKeys = [];
        _receivers = [];
    }

    public InputManager(params IInputReceiver[] receiver) : this()
    {
        _receivers.AddRange(receiver);
    }

    public void Run()
    {
        _hook.RunAsync();
    }

    public void Register(IInputReceiver receiver)
    {
        _receivers.Add(receiver);
    }

    private void Hook_KeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        if (_pressedKeys.Add(e.Data.KeyCode))
        {
            foreach (var receiver in _receivers)
            {
                receiver.OnKeyPressed(this, e.Data.KeyCode);
            }
        }
    }

    private void Hook_KeyReleased(object? sender, KeyboardHookEventArgs e)
    {
        if (_pressedKeys.Remove(e.Data.KeyCode))
        {
            foreach (var receiver in _receivers)
            {
                receiver.OnKeyReleased(this, e.Data.KeyCode);
            }
        }
    }

    private void Hook_MousePressed(object? sender, MouseHookEventArgs e)
    {
        foreach (var receiver in _receivers)
        {
            receiver.OnMousePressed(this, e.Data.Button);
        }
    }

    private void Hook_MouseReleased(object? sender, MouseHookEventArgs e)
    {
        foreach (var receiver in _receivers)
        {
            receiver.OnMouseReleased(this, e.Data.Button);
        }
    }
}

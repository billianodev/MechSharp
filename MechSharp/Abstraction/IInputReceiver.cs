using SharpHook.Native;

namespace MechSharp.Core;

public interface IInputReceiver
{
    void OnKeyPressed(InputManager sender, KeyCode keyCode);
    void OnKeyReleased(InputManager sender, KeyCode keyCode);
    void OnMousePressed(InputManager sender, MouseButton mouseButton);
    void OnMouseReleased(InputManager sender, MouseButton mouseButton);
}
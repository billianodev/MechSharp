using CommunityToolkit.Mvvm.ComponentModel;
using MechSharp.Core;
using SharpHook.Native;

namespace MechSharp.ViewModels;

public partial class LogoViewModel : ViewModelBase, IInputReceiver
{
    [ObservableProperty]
    private bool _isKeyPressed;

    public LogoViewModel()
    {
        App?.ViewModel.InputManager.Register(this);
    }

    public void OnKeyPressed(InputManager sender, KeyCode keyCode)
    {
        IsKeyPressed = true;
    }

    public void OnKeyReleased(InputManager sender, KeyCode keyCode)
    {
        if (sender.PressedKeys.Count == 0)
        {
            IsKeyPressed = false;
        }
    }

    public void OnMousePressed(InputManager sender, MouseButton mouseButton)
    {
    }

    public void OnMouseReleased(InputManager sender, MouseButton mouseButton)
    {
    }
}

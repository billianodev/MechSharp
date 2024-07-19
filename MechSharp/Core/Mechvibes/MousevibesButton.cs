using SharpHook.Native;

namespace MechSharp.Utilities;

public static class MousevibesButton
{
    public const int MbNone = 0;
    public const int Mb1 = 1;
    public const int Mb2 = 2;
    public const int Mb3 = 3;

    public static int Map(MouseButton button)
    {
        return button switch
        {
            MouseButton.Button1 => Mb1,
            MouseButton.Button2 => Mb2,
            MouseButton.Button3 => Mb3,
            _ => MbNone
        };
    }
}

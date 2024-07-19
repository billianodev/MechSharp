using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MechSharp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeHook();
    }

    private void InitializeHook()
    {
        KeyDownEvent.AddClassHandler<Window>(OnKeyDown, RoutingStrategies.Direct | RoutingStrategies.Tunnel);
        KeyUpEvent.AddClassHandler<Window>(OnKeyUp, RoutingStrategies.Direct | RoutingStrategies.Tunnel);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);

        if (!System.OperatingSystem.IsWindows() || e.CloseReason != WindowCloseReason.WindowClosing)
        {
            return;
        }

        e.Cancel = true;
        Hide();
    }

    private void OnKeyDown(Window sender, KeyEventArgs e)
    {
        e.Handled = true;
    }

    private void OnKeyUp(Window sender, KeyEventArgs e)
    {
        e.Handled = true;
    }
}

using Avalonia.Controls;
using MechSharp.ViewModels;

namespace MechSharp.Views;

public partial class LogoView : UserControl
{
    public LogoView()
    {
        DataContext = new LogoViewModel();
        InitializeComponent();
    }
}

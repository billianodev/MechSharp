using Avalonia.Controls;
using MechSharp.ViewModels;

namespace MechSharp.Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        DataContext = new MenuViewModel();
        InitializeComponent();
    }
}

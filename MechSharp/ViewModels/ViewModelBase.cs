using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MechSharp.ViewModels;

public class ViewModelBase : ObservableObject
{
    public static App? App => Application.Current as App;
    public static MainWindow? TopLevel => App?.MainWindow;
}

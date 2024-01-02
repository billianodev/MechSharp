namespace MechSharp.ViewModels;

public class ApplicationViewModel(App app)
{
	public void ShowTrayIcon_Command()
	{
		app.MainWindow?.Show();
	}
}
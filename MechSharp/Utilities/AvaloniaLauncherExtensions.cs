using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace MechSharp.Utilities;

public static class AvaloniaLauncherExtensions
{
    public static Task LaunchDirectoryAsync(this ILauncher launcher, string folderPath)
    {
        var directory = new DirectoryInfo(folderPath);
        return launcher.LaunchDirectoryInfoAsync(directory);
    }
}

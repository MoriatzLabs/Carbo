using System.Runtime.InteropServices;
using System.Text;

namespace Carbo.Services;

/// <summary>
/// Detects the currently active foreground window title using Win32 APIs.
/// Used for automatic project context detection.
/// </summary>
public static class AppDetector
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    public static string GetActiveWindowTitle()
    {
        var buffer = new StringBuilder(512);
        var handle = GetForegroundWindow();
        GetWindowText(handle, buffer, buffer.Capacity);
        return buffer.ToString();
    }

    public static string GetActiveProcessName()
    {
        var handle = GetForegroundWindow();
        GetWindowThreadProcessId(handle, out var pid);

        try
        {
            var process = System.Diagnostics.Process.GetProcessById((int)pid);
            return process.ProcessName;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Maps the active window title to a known project based on keywords.
    /// Returns null if no match is found.
    /// </summary>
    public static string? InferProjectFromWindow(IEnumerable<string> projects)
    {
        var title = GetActiveWindowTitle().ToLowerInvariant();
        var processName = GetActiveProcessName().ToLowerInvariant();

        foreach (var project in projects)
        {
            var key = project.ToLowerInvariant();
            if (title.Contains(key) || processName.Contains(key))
                return project;
        }

        return null;
    }
}

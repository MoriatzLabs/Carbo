using System.Diagnostics;

namespace Carbo.Services;

/// <summary>
/// Launches external processes and captures stdout.
/// </summary>
public static class ProcessSpawner
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

    public static async Task<string> Run(
        string executable,
        string arguments,
        TimeSpan? timeout = null)
    {
        var psi = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        var effectiveTimeout = timeout ?? DefaultTimeout;
        var completed = await Task.WhenAny(
            Task.Run(() => process.WaitForExit((int)effectiveTimeout.TotalMilliseconds)),
            Task.Delay(effectiveTimeout));

        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException($"Process '{executable}' timed out after {effectiveTimeout.TotalSeconds}s.");
        }

        var stdout = await outputTask;
        var stderr = await errorTask;

        if (process.ExitCode != 0 && !string.IsNullOrWhiteSpace(stderr))
            throw new InvalidOperationException($"Process failed (exit {process.ExitCode}): {stderr.Trim()}");

        return stdout.Trim();
    }
}

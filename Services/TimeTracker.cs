using System.Text.Json;
using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// Background service that records time spent per project.
/// Polls the active window every 15 seconds and logs entries to time_log.json.
/// Fires a subtle haptic every 15 minutes as a passive tracking tick.
/// </summary>
public class TimeTracker
{
    private static string _activeProject = "Untracked";
    private System.Timers.Timer? _pollTimer;
    private System.Timers.Timer? _hapticTimer;

    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Carbo", "time_log.json");

    public void Start()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);

        // Poll active window every 15 seconds
        _pollTimer = new System.Timers.Timer(15_000);
        _pollTimer.Elapsed += (_, _) => RecordTick();
        _pollTimer.AutoReset = true;
        _pollTimer.Start();

        // Subtle haptic every 15 minutes
        _hapticTimer = new System.Timers.Timer(15 * 60 * 1000);
        _hapticTimer.Elapsed += async (_, _) =>
        {
            // Haptic is triggered via plugin-level API; log only here
            await Task.CompletedTask; // placeholder — haptic call goes through SDK
        };
        _hapticTimer.AutoReset = true;
        _hapticTimer.Start();
    }

    public void Stop()
    {
        _pollTimer?.Stop();
        _pollTimer?.Dispose();
        _hapticTimer?.Stop();
        _hapticTimer?.Dispose();
    }

    public static void SetActiveProject(string project)
    {
        _activeProject = project;
    }

    public static string GetActiveProject() => _activeProject;

    private void RecordTick()
    {
        var entry = new TimeEntry
        {
            Timestamp = DateTime.Now,
            Project = _activeProject,
            WindowTitle = AppDetector.GetActiveWindowTitle(),
            ProcessName = AppDetector.GetActiveProcessName(),
            DurationSeconds = 15
        };

        AppendEntry(entry);
    }

    private static void AppendEntry(TimeEntry entry)
    {
        try
        {
            var entries = LoadEntries();
            entries.Add(entry);

            // Keep last 30 days of data
            var cutoff = DateTime.Now.AddDays(-30);
            entries = entries.Where(e => e.Timestamp >= cutoff).ToList();

            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = false });
            File.WriteAllText(LogPath, json);
        }
        catch
        {
            // Silently swallow — tracking must never crash the plugin
        }
    }

    public static List<TimeEntry> LoadEntries()
    {
        if (!File.Exists(LogPath)) return new List<TimeEntry>();

        try
        {
            var json = File.ReadAllText(LogPath);
            return JsonSerializer.Deserialize<List<TimeEntry>>(json) ?? new List<TimeEntry>();
        }
        catch
        {
            return new List<TimeEntry>();
        }
    }

    /// <summary>Returns total seconds per project for the given date range.</summary>
    public static Dictionary<string, int> GetProjectTotals(DateTime from, DateTime to)
    {
        return LoadEntries()
            .Where(e => e.Timestamp >= from && e.Timestamp <= to)
            .GroupBy(e => e.Project)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.DurationSeconds));
    }
}

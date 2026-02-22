using System.Text.Json;
using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// Background service that records time spent per project.
/// Polls the active window every 15 seconds and logs entries to time_log.json.
/// </summary>
public class TimeTracker
{
    // _activeProject is written by SetActiveProject (SDK/UI thread) and read by the
    // poll timer callback (thread-pool thread). volatile ensures visibility without a lock.
    private static volatile string _activeProject = "Untracked";

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

        // Haptic timer placeholder — actual haptic call wired up via SDK in plugin host
        _hapticTimer = new System.Timers.Timer(15 * 60 * 1000);
        _hapticTimer.Elapsed += (_, _) => { /* SDK haptic trigger goes here */ };
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

    /// <summary>Sets the currently active project for time tracking.</summary>
    public static void SetActiveProject(string project)
    {
        _activeProject = project;
    }

    /// <summary>Returns the name of the currently active project.</summary>
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

    /// <summary>Loads all time entries from the local log file.</summary>
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

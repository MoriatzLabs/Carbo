using Logi.Actions.SDK.Commands;
using Logi.Actions.SDK.Haptics;
using Carbo.Services;

namespace Carbo.Commands.TimeTracking;

/// <summary>
/// Thumb-wheel adjustment that cycles through configured projects and
/// logs active time to the current selection.
/// </summary>
public class TimeLogCommand : PluginDynamicAdjustment
{
    private int _currentIndex = 0;
    private List<string> _projects = new() { "Backend API", "Pitch Deck", "Investor Calls" };

    public TimeLogCommand()
    {
        DisplayName = "Time Log";
        Description = "Switch project context with the thumb wheel";
        Icon = "metadata/icons/time_log.png";

        // Load projects from settings if available
        var setting = GetSettingValue("projects_list");
        if (!string.IsNullOrWhiteSpace(setting))
        {
            _projects = setting
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length > 0)
                .ToList();
        }
    }

    protected override async Task<string> AdjustUp()
    {
        _currentIndex = Math.Min(_currentIndex + 1, _projects.Count - 1);
        await TriggerHaptic(HapticWaveform.DampStateChange);
        TimeTracker.SetActiveProject(_projects[_currentIndex]);
        return _projects[_currentIndex];
    }

    protected override async Task<string> AdjustDown()
    {
        _currentIndex = Math.Max(_currentIndex - 1, 0);
        await TriggerHaptic(HapticWaveform.DampStateChange);
        TimeTracker.SetActiveProject(_projects[_currentIndex]);
        return _projects[_currentIndex];
    }

    protected override string GetCurrentValue()
    {
        return _projects.ElementAtOrDefault(_currentIndex) ?? "None";
    }
}

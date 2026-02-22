using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.TimeTracking;

/// <summary>
/// Opens the local Carbo time-tracking dashboard in the default browser.
/// </summary>
public class StatsViewCommand : CarboCommand
{
    public StatsViewCommand()
    {
        DisplayName = "Stats View";
        Description = "Open time tracking dashboard in browser";
        GroupName = "";
        Icon = "metadata/icons/stats_view.png";
    }

    protected override async Task RunCommand()
    {
        await SendHaptic(HapticWaveform.SharpCollision);

        var dashboardPath = Path.Combine(AppContext.BaseDirectory, "Dashboard", "index.html");

        if (!File.Exists(dashboardPath))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Stats View", "Dashboard file not found.");
            return;
        }

        await ProcessSpawner.Run("powershell", $"-command Start-Process '{dashboardPath}'");
        await SendHaptic(HapticWaveform.Completed);
    }
}

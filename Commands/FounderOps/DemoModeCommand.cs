using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Toggle that activates Demo Mode: enables Slack DND, suppresses Windows
/// notifications, and optionally opens the pitch deck.
/// </summary>
public class DemoModeCommand : CarboCommand
{
    private static bool _isActive = false;

    public DemoModeCommand()
    {
        DisplayName = "Demo Mode";
        Description = "Toggle demo mode — silences distractions for investor presentations";
        GroupName = "";
        Icon = "metadata/icons/demo_mode.png";
    }

    protected override async Task RunCommand()
    {
        _isActive = !_isActive;

        if (_isActive)
        {
            await SendHaptic(HapticWaveform.HappyAlert);
            await ActivateDemoMode();
            Notify("Demo Mode ON", "Slack silenced. Notifications suppressed. You're presenter-ready.");
        }
        else
        {
            await SendHaptic(HapticWaveform.Wave);
            await DeactivateDemoMode();
            Notify("Demo Mode OFF", "Restored to normal mode.");
        }
    }

    private async Task ActivateDemoMode()
    {
        var slackToken = GetSettingValue("slack_token");
        if (!string.IsNullOrEmpty(slackToken))
        {
            await SlackService.SetDnd(slackToken, 120);
        }

        // Suppress Windows Focus Assist via registry
        await ProcessSpawner.Run("powershell", "-command \"Set-ItemProperty -Path 'HKCU:\\Software\\Microsoft\\Windows\\CurrentVersion\\CloudStore\\Store\\DefaultAccount\\Current\\default$windows.data.notifications.quiethourssettings\\windows.data.notifications.quiethourssettings' -Name 'Data' -Value ([byte[]](0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00))\"");

        var deckPath = GetSettingValue("pitch_deck_path");
        if (!string.IsNullOrEmpty(deckPath) && File.Exists(deckPath))
        {
            await ProcessSpawner.Run("powershell", $"-command Start-Process '{deckPath}'");
        }
    }

    private async Task DeactivateDemoMode()
    {
        var slackToken = GetSettingValue("slack_token");
        if (!string.IsNullOrEmpty(slackToken))
        {
            await SlackService.EndDnd(slackToken);
        }
    }
}

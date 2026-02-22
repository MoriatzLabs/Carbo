using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.TimeTracking;

/// <summary>
/// Toggle that starts/stops a Pomodoro-style focus session.
/// Activates Slack DND and suppresses notifications during the session.
/// </summary>
public class FocusModeCommand : CarboCommand
{
    // volatile fields: _isActive and _sessionStart are written by RunCommand (SDK thread)
    // and read/written by the timer callback (thread pool thread).
    private static volatile bool _isActive = false;
    private static DateTime _sessionStart;
    private static System.Timers.Timer? _endTimer;
    private static readonly object _timerLock = new();

    public FocusModeCommand()
    {
        DisplayName = "Focus Mode";
        Description = "Toggle Pomodoro focus session with Slack DND";
        GroupName = "";
        Icon = "metadata/icons/focus_mode.png";
    }

    protected override async Task RunCommand()
    {
        _isActive = !_isActive;

        if (_isActive)
        {
            _sessionStart = DateTime.Now;
            var minutesStr = GetSettingValue("focus_duration_minutes") ?? "25";
            var minutes = int.TryParse(minutesStr, out var m) ? m : 25;

            await SendHaptic(HapticWaveform.HappyAlert);

            var slackToken = GetSettingValue("slack_token");
            if (!string.IsNullOrEmpty(slackToken))
                await SlackService.SetDnd(slackToken, minutes);

            lock (_timerLock)
            {
                _endTimer?.Dispose();
                _endTimer = new System.Timers.Timer(minutes * 60 * 1000);
                _endTimer.Elapsed += OnTimerElapsed;
                _endTimer.AutoReset = false;
                _endTimer.Start();
            }

            Notify("Focus Mode ON", $"{minutes}-min session started. Slack silenced.");
        }
        else
        {
            await EndSession();
        }
    }

    // Synchronous elapsed handler — delegates async work to Task.Run to avoid async void.
    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Task.Run(async () =>
        {
            try { await OnSessionEnd(); }
            catch { /* must not crash the timer thread */ }
        });
    }

    private async Task EndSession()
    {
        lock (_timerLock)
        {
            _endTimer?.Stop();
            _endTimer?.Dispose();
            _endTimer = null;
        }

        _isActive = false;
        var elapsed = (int)(DateTime.Now - _sessionStart).TotalMinutes;

        var slackToken = GetSettingValue("slack_token");
        if (!string.IsNullOrEmpty(slackToken))
            await SlackService.EndDnd(slackToken);

        await SendHaptic(HapticWaveform.Wave);
        Notify("Focus Mode OFF", $"Session ended after {elapsed} min. Great work!");
    }

    private async Task OnSessionEnd()
    {
        await SendHaptic(HapticWaveform.Firework);
        await EndSession();
        Notify("Focus Complete!", "Pomodoro session finished. Take a break.");
    }
}

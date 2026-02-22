using Logi.Actions.SDK.Commands;
using Logi.Actions.SDK.Haptics;
using Carbo.Services;

namespace Carbo.Commands.Base;

/// <summary>
/// Base class for all Carbo commands. Provides shared helpers for haptics,
/// clipboard, CLI spawning, and notifications.
/// </summary>
public abstract class CarboCommand : PluginDynamicCommand
{
    protected async Task SendHaptic(HapticWaveform waveform)
    {
        await TriggerHaptic(waveform);
    }

    protected async Task<string> ReadClipboard()
    {
        return await ClipboardService.Get();
    }

    protected async Task WriteClipboard(string text)
    {
        await ClipboardService.Set(text);
    }

    protected async Task<string> RunCLI(string executable, string arguments)
    {
        return await ProcessSpawner.Run(executable, arguments);
    }

    protected void Notify(string title, string body)
    {
        NotificationService.Show(title, body);
    }
}

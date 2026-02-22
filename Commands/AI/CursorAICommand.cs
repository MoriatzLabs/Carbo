using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.AI;

/// <summary>
/// Opens the current project in Cursor and sends a prompt via CLI.
/// </summary>
public class CursorAICommand : CarboCommand
{
    public CursorAICommand()
    {
        DisplayName = "Cursor AI";
        Description = "Open current project in Cursor AI editor";
        GroupName = "";
        Icon = "metadata/icons/cursor_ai.png";
    }

    protected override async Task RunCommand()
    {
        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("cursor_cli_path") ?? "cursor";
        var activeWindow = AppDetector.GetActiveWindowTitle();

        // Try to open the active project directory
        await RunCLI(cliPath, ".");

        await SendHaptic(HapticWaveform.Completed);
        Notify("Cursor AI", $"Opened in Cursor — context: {activeWindow}");
    }
}

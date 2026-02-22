using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;

namespace Carbo.Commands.AI;

/// <summary>
/// Sub-bubble under "AI Tools" — diagnoses and fixes bugs in clipboard code via Claude.
/// </summary>
public class CursorFixBugCommand : CarboCommand
{
    public CursorFixBugCommand()
    {
        DisplayName = "Fix Bug";
        Description = "Diagnose and fix bugs in clipboard code";
        GroupName = "AI Tools";
        Icon = "metadata/icons/cursor_fix.png";
    }

    protected override async Task RunCommand()
    {
        var code = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(code))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Carbo", "Nothing to fix — copy buggy code first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("claude_cli_path") ?? "claude";
        var escaped = code.Replace("\"", "\\\"");
        var result = await RunCLI(cliPath,
            $"--print --prompt \"Identify and fix all bugs in this code. Explain each bug briefly as an inline comment, then return the corrected code:\\n{escaped}\"");

        await WriteClipboard(result);
        await SendHaptic(HapticWaveform.Completed);
        Notify("Fix Bug", "Fixed code is in clipboard.");
    }
}

using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.AI;

/// <summary>
/// Sends the current clipboard selection to the Claude Code CLI and writes
/// the response back to the clipboard.
/// </summary>
public class ClaudeCodeCommand : CarboCommand
{
    public ClaudeCodeCommand()
    {
        DisplayName = "Claude Code";
        Description = "Send selected code to Claude Code CLI for AI assistance";
        GroupName = "";
        Icon = "metadata/icons/claude_code.png";
    }

    protected override async Task RunCommand()
    {
        var code = await ReadClipboard();

        if (string.IsNullOrWhiteSpace(code))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Carbo", "Clipboard is empty — copy some code first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("claude_cli_path") ?? "claude";
        var escaped = code.Replace("\"", "\\\"");
        var result = await RunCLI(cliPath, $"--print --prompt \"Review and improve this code:\\n{escaped}\"");

        await WriteClipboard(result);
        await SendHaptic(HapticWaveform.Completed);
        Notify("Claude Code", "Result ready — paste from clipboard.");
    }
}

using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;

namespace Carbo.Commands.AI;

/// <summary>
/// Sub-bubble under "AI Tools" — generates unit tests for clipboard code via Claude,
/// formatted for Cursor to paste directly.
/// </summary>
public class CursorTestsCommand : CarboCommand
{
    public CursorTestsCommand()
    {
        DisplayName = "Generate Tests";
        Description = "Generate unit tests for clipboard code";
        GroupName = "AI Tools";
        Icon = "metadata/icons/cursor_tests.png";
    }

    protected override async Task RunCommand()
    {
        var code = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(code))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Carbo", "Nothing to test — copy code first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("claude_cli_path") ?? "claude";
        var escaped = code.Replace("\"", "\\\"");
        var result = await RunCLI(cliPath,
            $"--print --prompt \"Write comprehensive unit tests for this code. Cover edge cases, happy paths, and error conditions. Return only the test code:\\n{escaped}\"");

        await WriteClipboard(result);
        await SendHaptic(HapticWaveform.Completed);
        Notify("Generate Tests", "Tests copied to clipboard — paste into your test file.");
    }
}

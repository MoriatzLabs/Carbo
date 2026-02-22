using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;

namespace Carbo.Commands.AI;

/// <summary>
/// Sub-bubble under "AI Tools" — explains clipboard code via Claude.
/// </summary>
public class ClaudeExplainCommand : CarboCommand
{
    public ClaudeExplainCommand()
    {
        DisplayName = "Explain";
        Description = "Get a plain-English explanation of clipboard code";
        GroupName = "AI Tools";
        Icon = "metadata/icons/ai_explain.png";
    }

    protected override async Task RunCommand()
    {
        var code = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(code))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Carbo", "Nothing to explain — copy code first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("claude_cli_path") ?? "claude";
        var prompt = $"Explain this code clearly and concisely in plain English. Include what it does, how it works, and any notable patterns:\n{code}";
        var result = await ProcessSpawner.RunWithFileInput(cliPath, "--print --prompt-file", prompt);

        await WriteClipboard(result);
        await SendHaptic(HapticWaveform.Completed);
        Notify("Claude Explain", "Explanation copied to clipboard.");
    }
}

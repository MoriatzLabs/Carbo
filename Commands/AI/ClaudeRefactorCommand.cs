using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;

namespace Carbo.Commands.AI;

/// <summary>
/// Sub-bubble under "AI Tools" — refactors clipboard code via Claude.
/// </summary>
public class ClaudeRefactorCommand : CarboCommand
{
    public ClaudeRefactorCommand()
    {
        DisplayName = "Refactor";
        Description = "Refactor clipboard code with Claude";
        GroupName = "AI Tools";
        Icon = "metadata/icons/ai_refactor.png";
    }

    protected override async Task RunCommand()
    {
        var code = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(code))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Carbo", "Nothing to refactor — copy code first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var cliPath = GetSettingValue("claude_cli_path") ?? "claude";
        var prompt = $"Refactor this code for clarity, performance, and best practices. Return only the refactored code with no explanation:\n{code}";
        var result = await ProcessSpawner.RunWithFileInput(cliPath, "--print --prompt-file", prompt);

        await WriteClipboard(result);
        await SendHaptic(HapticWaveform.Completed);
        Notify("Claude Refactor", "Refactored code is in clipboard.");
    }
}

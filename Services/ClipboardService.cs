using System.Runtime.InteropServices;

namespace Carbo.Services;

/// <summary>
/// Cross-process clipboard access via PowerShell to avoid STA thread requirements.
/// </summary>
public static class ClipboardService
{
    public static async Task<string> Get()
    {
        var result = await ProcessSpawner.Run("powershell", "-command Get-Clipboard");
        return result?.Trim() ?? string.Empty;
    }

    public static async Task Set(string text)
    {
        // Encode to base64 to safely pass arbitrary text through the shell
        var bytes = System.Text.Encoding.Unicode.GetBytes(text);
        var b64 = Convert.ToBase64String(bytes);
        var script = $"[System.Text.Encoding]::Unicode.GetString([System.Convert]::FromBase64String('{b64}')) | Set-Clipboard";
        await ProcessSpawner.Run("powershell", $"-command \"{script}\"");
    }
}

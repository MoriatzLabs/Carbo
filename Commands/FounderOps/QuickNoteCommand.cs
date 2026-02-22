using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Sub-bubble under "Founder Ops" — appends clipboard content to a local
/// quick-notes file with a timestamp.
/// </summary>
public class QuickNoteCommand : CarboCommand
{
    private static readonly string NotesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "Carbo", "quick_notes.md");

    public QuickNoteCommand()
    {
        DisplayName = "Quick Note";
        Description = "Save clipboard as a timestamped note";
        GroupName = "Founder Ops";
        Icon = "metadata/icons/quick_note.png";
    }

    protected override async Task RunCommand()
    {
        var content = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(content))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Quick Note", "Clipboard is empty — nothing to save.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        Directory.CreateDirectory(Path.GetDirectoryName(NotesPath)!);

        var entry = $"\n## {DateTime.Now:yyyy-MM-dd HH:mm}\n\n{content.Trim()}\n";
        await File.AppendAllTextAsync(NotesPath, entry);

        await SendHaptic(HapticWaveform.Knock);
        Notify("Quick Note", "Saved to quick_notes.md.");
    }
}

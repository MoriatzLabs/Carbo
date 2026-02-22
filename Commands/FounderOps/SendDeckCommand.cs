using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Sub-bubble under "Founder Ops" — drafts a Gmail with the pitch deck attached
/// to the email address currently in the clipboard.
/// </summary>
public class SendDeckCommand : CarboCommand
{
    public SendDeckCommand()
    {
        DisplayName = "Send Deck";
        Description = "Draft a deck email to the address in clipboard";
        GroupName = "Founder Ops";
        Icon = "metadata/icons/send_deck.png";
    }

    protected override async Task RunCommand()
    {
        var toAddress = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(toAddress) || !toAddress.Contains('@'))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Send Deck", "Copy a valid email address to clipboard first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var deckPath = GetSettingValue("pitch_deck_path") ?? "";
        var subject = "Carbo — Pitch Deck";
        var body = """
            Hi,

            As discussed, please find the Carbo pitch deck attached.

            Happy to answer any questions or schedule a call at your convenience.

            Best,
            [Your name]
            """;

        try
        {
            await GmailService.CreateDraftAsync(toAddress.Trim(), subject, body, deckPath);
            await SendHaptic(HapticWaveform.Completed);
            Notify("Send Deck", $"Draft created for {toAddress.Trim()} — review and send in Gmail.");
        }
        catch (Exception ex)
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Send Deck", $"Failed: {ex.Message}");
        }
    }
}

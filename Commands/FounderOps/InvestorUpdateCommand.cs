using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Creates a Gmail draft investor update email with the pitch deck attached,
/// addressed to the most recent investor thread contact.
/// </summary>
public class InvestorUpdateCommand : CarboCommand
{
    public InvestorUpdateCommand()
    {
        DisplayName = "Investor Update";
        Description = "Draft an investor update email with deck attached";
        GroupName = "";
        Icon = "metadata/icons/investor_update.png";
    }

    protected override async Task RunCommand()
    {
        await SendHaptic(HapticWaveform.SharpCollision);

        try
        {
            var deckPath = GetSettingValue("pitch_deck_path") ?? "";
            var subject = $"Carbo Update — {DateTime.Now:MMMM yyyy}";
            var body = BuildUpdateTemplate();

            await GmailService.CreateDraftAsync(
                to: "",           // left blank so user fills in recipient
                subject: subject,
                body: body,
                attachmentPath: deckPath
            );

            await SendHaptic(HapticWaveform.Completed);
            Notify("Investor Update", "Draft created in Gmail — add recipient and send.");
        }
        catch (Exception ex)
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Investor Update", $"Failed to create draft: {ex.Message}");
        }
    }

    private static string BuildUpdateTemplate()
    {
        return $"""
            Hi [Name],

            Quick update on Carbo — {DateTime.Now:MMMM yyyy}:

            **Progress**
            - [Key metric or milestone]
            - [Key metric or milestone]
            - [Key metric or milestone]

            **This month's focus**
            [1-2 sentences on current sprint]

            **Ask**
            [Optional: intro request, advice, or FYI only]

            Deck attached. Happy to jump on a call — what's your availability?

            Best,
            [Your name]
            """;
    }
}

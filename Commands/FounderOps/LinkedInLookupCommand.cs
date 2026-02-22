using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Sub-bubble under "Founder Ops" — looks up a LinkedIn profile for the
/// name currently in the clipboard and copies a summary.
/// </summary>
public class LinkedInLookupCommand : CarboCommand
{
    public LinkedInLookupCommand()
    {
        DisplayName = "LinkedIn Lookup";
        Description = "Look up LinkedIn profile for name in clipboard";
        GroupName = "Founder Ops";
        Icon = "metadata/icons/linkedin_lookup.png";
    }

    protected override async Task RunCommand()
    {
        var name = await ReadClipboard();
        if (string.IsNullOrWhiteSpace(name))
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("LinkedIn Lookup", "Copy a person's name to the clipboard first.");
            return;
        }

        await SendHaptic(HapticWaveform.SharpCollision);

        var profile = await LinkedInService.LookupAsync(name.Trim());

        if (profile == null)
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("LinkedIn Lookup", $"No profile found for \"{name}\".");
            return;
        }

        var summary = $"{profile.Name} — {profile.Headline}\n{profile.Company}\n{profile.ProfileUrl}";
        await WriteClipboard(summary);
        await SendHaptic(HapticWaveform.Completed);
        Notify("LinkedIn Lookup", $"Found {profile.Name} — summary in clipboard.");
    }
}

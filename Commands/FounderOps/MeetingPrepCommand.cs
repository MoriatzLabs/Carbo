using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Models;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Fetches the next calendar event, pulls attendee context from Gmail,
/// and surfaces a meeting brief via a toast notification.
/// </summary>
public class MeetingPrepCommand : CarboCommand
{
    public MeetingPrepCommand()
    {
        DisplayName = "Meeting Prep";
        Description = "Get AI-powered brief for your next meeting";
        GroupName = "";
        Icon = "metadata/icons/meeting_prep.png";
    }

    protected override async Task RunCommand()
    {
        await SendHaptic(HapticWaveform.SharpCollision);

        MeetingBrief? brief = null;
        try
        {
            brief = await GoogleCalendarService.GetNextMeetingBriefAsync();
        }
        catch (Exception ex)
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Meeting Prep", $"Could not load calendar: {ex.Message}");
            return;
        }

        if (brief == null)
        {
            await SendHaptic(HapticWaveform.Knock);
            Notify("Meeting Prep", "No upcoming meetings found.");
            return;
        }

        await SendHaptic(HapticWaveform.Ringing);

        var summary = $"{brief.Title} — {brief.StartTime:HH:mm}\n" +
                      $"Attendees: {string.Join(", ", brief.Attendees)}\n" +
                      (string.IsNullOrEmpty(brief.LastEmailSubject)
                          ? ""
                          : $"Last email: {brief.LastEmailSubject}");

        Notify("Meeting Prep", summary);
        await WriteClipboard(summary);
    }
}

using Logi.Actions.SDK.Haptics;
using Carbo.Commands.Base;
using Carbo.Services;

namespace Carbo.Commands.FounderOps;

/// <summary>
/// Sub-bubble under "Founder Ops" — compiles today's calendar events and unread
/// Gmail threads into a digest and puts it in the clipboard.
/// </summary>
public class DailyDigestCommand : CarboCommand
{
    public DailyDigestCommand()
    {
        DisplayName = "Daily Digest";
        Description = "Compile today's calendar and email digest to clipboard";
        GroupName = "Founder Ops";
        Icon = "metadata/icons/daily_digest.png";
    }

    protected override async Task RunCommand()
    {
        await SendHaptic(HapticWaveform.SharpCollision);

        try
        {
            var calEvents = await GoogleCalendarService.GetTodayEventsAsync();
            var unread = await GmailService.GetUnreadSummaryAsync(maxResults: 5);

            var lines = new List<string> { $"# Daily Digest — {DateTime.Now:dddd, MMMM d}", "" };

            lines.Add("## Calendar");
            if (calEvents.Count == 0)
            {
                lines.Add("No meetings today.");
            }
            else
            {
                foreach (var ev in calEvents)
                    lines.Add($"- {ev.Start:HH:mm} — {ev.Title}");
            }

            lines.Add("");
            lines.Add("## Unread Email");
            if (unread.Count == 0)
            {
                lines.Add("Inbox zero!");
            }
            else
            {
                foreach (var thread in unread)
                    lines.Add($"- [{thread.From}] {thread.Subject}");
            }

            var digest = string.Join("\n", lines);
            await WriteClipboard(digest);
            await SendHaptic(HapticWaveform.Completed);
            Notify("Daily Digest", $"{calEvents.Count} meetings, {unread.Count} unread threads — digest in clipboard.");
        }
        catch (Exception ex)
        {
            await SendHaptic(HapticWaveform.Mad);
            Notify("Daily Digest", $"Failed: {ex.Message}");
        }
    }
}

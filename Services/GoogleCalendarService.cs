using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// Wraps the Google Calendar API. Auth credentials are loaded from the
/// OAuth token stored by Logi Options+ ExternalServiceLogin.
/// </summary>
public static class GoogleCalendarService
{
    private const string ApplicationName = "Carbo";

    private static async Task<CalendarService> CreateServiceAsync()
    {
        // Credential path managed by Logi Options+ OAuth flow
        var credPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Carbo", "google_token.json");

        UserCredential credential;
        using var stream = new FileStream(
            Path.Combine(AppContext.BaseDirectory, "client_secrets.json"),
            FileMode.Open, FileAccess.Read);

        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { CalendarService.Scope.CalendarReadonly },
            "user",
            CancellationToken.None,
            new Google.Apis.Util.Store.FileDataStore(credPath, true));

        return new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }

    public static async Task<MeetingBrief?> GetNextMeetingBriefAsync()
    {
        var service = await CreateServiceAsync();

        var request = service.Events.List("primary");
        request.TimeMinDateTimeOffset = DateTimeOffset.UtcNow;
        request.TimeMaxDateTimeOffset = DateTimeOffset.UtcNow.AddHours(8);
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.MaxResults = 1;

        var events = await request.ExecuteAsync();
        var next = events.Items?.FirstOrDefault();
        if (next == null) return null;

        var startTime = next.Start.DateTimeDateTimeOffset?.LocalDateTime
                        ?? DateTime.Parse(next.Start.Date ?? DateTime.Now.ToString("yyyy-MM-dd"));

        var attendees = next.Attendees?
            .Where(a => !a.Organizer.GetValueOrDefault())
            .Select(a => a.DisplayName ?? a.Email)
            .ToList() ?? new List<string>();

        return new MeetingBrief
        {
            Title = next.Summary ?? "Untitled Meeting",
            StartTime = startTime,
            Attendees = attendees,
            MeetingLink = next.HangoutLink ?? ""
        };
    }

    public static async Task<List<MeetingBrief>> GetTodayEventsAsync()
    {
        var service = await CreateServiceAsync();

        var today = DateTime.Today;
        var request = service.Events.List("primary");
        request.TimeMinDateTimeOffset = new DateTimeOffset(today);
        request.TimeMaxDateTimeOffset = new DateTimeOffset(today.AddDays(1).AddSeconds(-1));
        request.SingleEvents = true;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var events = await request.ExecuteAsync();

        return (events.Items ?? new List<Event>())
            .Select(e => new MeetingBrief
            {
                Title = e.Summary ?? "Untitled",
                StartTime = e.Start.DateTimeDateTimeOffset?.LocalDateTime
                            ?? DateTime.Parse(e.Start.Date ?? today.ToString("yyyy-MM-dd"))
            })
            .ToList();
    }
}

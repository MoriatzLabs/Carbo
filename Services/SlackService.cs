using System.Net.Http;

namespace Carbo.Services;

/// <summary>
/// Controls Slack Do Not Disturb via the Slack Web API.
/// </summary>
public static class SlackService
{
    private static readonly HttpClient Http = new();
    private const string BaseUrl = "https://slack.com/api";

    /// <summary>Enables DND snooze for <paramref name="minutes"/> minutes.</summary>
    public static async Task SetDnd(string token, int minutes)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "token", token },
            { "num_minutes", minutes.ToString() }
        });

        var response = await Http.PostAsync($"{BaseUrl}/dnd.setSnooze", content);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>Ends the active DND snooze.</summary>
    public static async Task EndDnd(string token)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "token", token }
        });

        var response = await Http.PostAsync($"{BaseUrl}/dnd.endSnooze", content);
        response.EnsureSuccessStatusCode();
    }
}

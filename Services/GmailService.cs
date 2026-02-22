using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// Wraps the Gmail API for creating drafts and reading unread threads.
/// </summary>
public static class GmailService
{
    private const string ApplicationName = "Carbo";

    private static async Task<Google.Apis.Gmail.v1.GmailService> CreateServiceAsync()
    {
        var credPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Carbo", "google_token.json");

        var secretsPath = Path.Combine(AppContext.BaseDirectory, "client_secrets.json");
        if (!File.Exists(secretsPath))
            throw new FileNotFoundException("Google OAuth secrets file not found. Place client_secrets.json in the plugin directory.", secretsPath);

        using var stream = new FileStream(secretsPath, FileMode.Open, FileAccess.Read);

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[]
            {
                Google.Apis.Gmail.v1.GmailService.Scope.GmailCompose,
                Google.Apis.Gmail.v1.GmailService.Scope.GmailReadonly
            },
            "user",
            CancellationToken.None,
            new Google.Apis.Util.Store.FileDataStore(credPath, true));

        return new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName
        });
    }

    /// <summary>Creates a Gmail draft with an optional attachment.</summary>
    public static async Task CreateDraftAsync(
        string to,
        string subject,
        string body,
        string? attachmentPath = null)
    {
        var service = await CreateServiceAsync();
        var rawMessage = BuildRawMessage(to, subject, body, attachmentPath);

        var draft = new Draft
        {
            Message = new Message { Raw = Base64UrlEncode(rawMessage) }
        };

        await service.Users.Drafts.Create(draft, "me").ExecuteAsync();
    }

    /// <summary>Returns summaries of up to <paramref name="maxResults"/> unread inbox threads.</summary>
    public static async Task<List<EmailSummary>> GetUnreadSummaryAsync(int maxResults = 10)
    {
        var service = await CreateServiceAsync();

        var listRequest = service.Users.Messages.List("me");
        listRequest.Q = "is:unread in:inbox";
        listRequest.MaxResults = maxResults;

        var listResponse = await listRequest.ExecuteAsync();
        var summaries = new List<EmailSummary>();

        if (listResponse.Messages == null) return summaries;

        foreach (var msgRef in listResponse.Messages)
        {
            var msg = await service.Users.Messages.Get("me", msgRef.Id).ExecuteAsync();
            var headers = msg.Payload?.Headers ?? new List<MessagePartHeader>();

            summaries.Add(new EmailSummary
            {
                From = GetHeader(headers, "From"),
                Subject = GetHeader(headers, "Subject"),
                Date = GetHeader(headers, "Date")
            });
        }

        return summaries;
    }

    private static string GetHeader(IList<MessagePartHeader> headers, string name)
        => headers.FirstOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;

    private static byte[] BuildRawMessage(string to, string subject, string body, string? attachmentPath)
    {
        using var message = new MailMessage();
        message.To.Add(string.IsNullOrWhiteSpace(to) ? "draft@placeholder.invalid" : to);
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = false;

        if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
        {
            var attachment = new Attachment(attachmentPath, MediaTypeNames.Application.Octet);
            message.Attachments.Add(attachment);
        }

        var mime = RenderMimeMessage(message);
        return Encoding.UTF8.GetBytes(mime);
    }

    private static string RenderMimeMessage(MailMessage message)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"To: {message.To}");
        sb.AppendLine($"Subject: {message.Subject}");
        sb.AppendLine("MIME-Version: 1.0");
        sb.AppendLine("Content-Type: text/plain; charset=utf-8");
        sb.AppendLine();
        sb.Append(message.Body);
        return sb.ToString();
    }

    private static string Base64UrlEncode(byte[] data)
        => Convert.ToBase64String(data)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
}

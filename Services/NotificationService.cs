using Microsoft.Toolkit.Uwp.Notifications;

namespace Carbo.Services;

/// <summary>
/// Windows toast notifications for Carbo events.
/// </summary>
public static class NotificationService
{
    public static void Show(string title, string body)
    {
        try
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(body)
                .Show();
        }
        catch
        {
            // Toast notifications may not be available in all environments
        }
    }

    public static void ShowWithAction(string title, string body, string actionText, string actionArgs)
    {
        try
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(body)
                .AddButton(new ToastButton()
                    .SetContent(actionText)
                    .AddArgument("action", actionArgs))
                .Show();
        }
        catch { }
    }
}

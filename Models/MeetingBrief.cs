namespace Carbo.Models;

public class MeetingBrief
{
    public string Title { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public List<string> Attendees { get; set; } = new();
    public string MeetingLink { get; set; } = string.Empty;
    public string LastEmailSubject { get; set; } = string.Empty;
}

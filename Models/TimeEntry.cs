namespace Carbo.Models;

public class TimeEntry
{
    public DateTime Timestamp { get; set; }
    public string Project { get; set; } = string.Empty;
    public string WindowTitle { get; set; } = string.Empty;
    public string ProcessName { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
}

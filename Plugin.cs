using Logi.Actions.SDK;
using Logi.Actions.SDK.Commands;
using Logi.Actions.SDK.Settings;
using Carbo.Commands.AI;
using Carbo.Commands.FounderOps;
using Carbo.Commands.TimeTracking;
using Carbo.Services;

namespace Carbo;

public class CarboPlugin : LogiPlugin
{
    private TimeTracker? _timeTracker;

    public override void Initialize()
    {
        // --- AI Tools ---
        RegisterCommand(new ClaudeCodeCommand());
        RegisterCommand(new CursorAICommand());
        RegisterCommand(new ClaudeRefactorCommand());
        RegisterCommand(new ClaudeExplainCommand());
        RegisterCommand(new CursorTestsCommand());
        RegisterCommand(new CursorFixBugCommand());

        // --- Founder Ops ---
        RegisterCommand(new MeetingPrepCommand());
        RegisterCommand(new DemoModeCommand());
        RegisterCommand(new InvestorUpdateCommand());
        RegisterCommand(new LinkedInLookupCommand());
        RegisterCommand(new SendDeckCommand());
        RegisterCommand(new QuickNoteCommand());
        RegisterCommand(new DailyDigestCommand());

        // --- Time Tracking ---
        RegisterCommand(new TimeLogCommand());
        RegisterCommand(new FocusModeCommand());
        RegisterCommand(new StatsViewCommand());

        // --- Settings ---
        RegisterSetting(new PluginSetting
        {
            Name = "ai_provider_1",
            DisplayName = "AI Provider (Bubble 1)",
            Type = SettingType.Dropdown,
            Default = "Claude Code",
            Options = new[] { "Claude Code", "Cursor", "Custom CLI" }
        });

        RegisterSetting(new PluginSetting
        {
            Name = "claude_cli_path",
            DisplayName = "Claude CLI Path",
            Type = SettingType.Text,
            Default = "claude"
        });

        RegisterSetting(new PluginSetting
        {
            Name = "cursor_cli_path",
            DisplayName = "Cursor CLI Path",
            Type = SettingType.Text,
            Default = "cursor"
        });

        RegisterSetting(new PluginSetting
        {
            Name = "gmail_account",
            DisplayName = "Gmail Account",
            Type = SettingType.ExternalServiceLogin,
            ServiceProvider = "Google"
        });

        RegisterSetting(new PluginSetting
        {
            Name = "pitch_deck_path",
            DisplayName = "Pitch Deck Path",
            Type = SettingType.Text,
            Default = ""
        });

        RegisterSetting(new PluginSetting
        {
            Name = "slack_token",
            DisplayName = "Slack Token",
            Type = SettingType.Text,
            Default = ""
        });

        RegisterSetting(new PluginSetting
        {
            Name = "focus_duration_minutes",
            DisplayName = "Focus Session Duration (minutes)",
            Type = SettingType.Dropdown,
            Default = "25",
            Options = new[] { "15", "25", "45", "60", "90" }
        });

        RegisterSetting(new PluginSetting
        {
            Name = "projects_list",
            DisplayName = "Projects (comma-separated)",
            Type = SettingType.Text,
            Default = "Backend API,Pitch Deck,Investor Calls"
        });

        // --- Background Services ---
        _timeTracker = new TimeTracker();
        _timeTracker.Start();
    }

    public override void Shutdown()
    {
        _timeTracker?.Stop();
    }
}

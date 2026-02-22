# Carbo — Claude Code Instructions

## What This Project Is

Carbo is a **Logitech Actions SDK plugin** for the MX Master 4, built in C# (.NET 8, Windows).
It gives founders one-click access to Claude Code, Cursor, Gmail, Google Calendar, Slack DND,
and time tracking — all from the mouse's Actions Ring and thumb wheel.

---

## Project Layout

```
Carbo/
├── Plugin.cs                     # Entry point — registers all commands + settings
├── Carbo.csproj                  # .NET 8 Windows library, NuGet refs
├── Commands/
│   ├── _Base/CarboCommand.cs     # Abstract base: haptic, clipboard, CLI, notify helpers
│   ├── AI/                       # Claude Code, Cursor, Refactor, Explain, Tests, Fix
│   ├── FounderOps/               # Meeting prep, Demo mode, Investor update, LinkedIn, etc.
│   └── TimeTracking/             # TimeLog (thumb wheel), FocusMode, StatsView
├── Services/                     # Stateless service classes (Calendar, Gmail, Slack, etc.)
├── Models/                       # Plain C# records/classes (MeetingBrief, TimeEntry, etc.)
├── metadata/
│   ├── plugin.json               # Logi manifest — pluginId, version, usesApplicationApiOnly
│   └── icons/                    # 256x256 PNGs, one per command
├── Dashboard/index.html          # Local time-tracking chart opened by StatsViewCommand
└── Data/time_log.json            # Rolling 30-day time log, written by TimeTracker
```

---

## SDK Fundamentals

- All top-level Actions Ring bubbles extend `PluginDynamicCommand` (or `CarboCommand` which
  wraps it). Thumb-wheel adjustments extend `PluginDynamicAdjustment`.
- `GroupName = ""` → top-level bubble. `GroupName = "AI Tools"` → nested sub-bubble.
- Commands are registered in `Plugin.Initialize()` — order matters (ring order).
- Settings are registered with `RegisterSetting(new PluginSetting { ... })` and read with
  `GetSettingValue("key")`.
- `usesApplicationApiOnly: true` in `plugin.json` is required — plugin must work in all apps.

---

## C# Conventions for This Repo

### Style
- **File-scoped namespaces** — `namespace Carbo.Commands.AI;` not `namespace Carbo.Commands.AI { }`.
- **Primary constructors** where there is no logic — prefer for models.
- **`var`** for locals when the type is obvious from the right-hand side.
- **`async Task`** for all SDK command overrides — never `async void`.
- **Nullable enabled** — always null-check `GetSettingValue()` returns (can be null).
- **No abbreviations** — `executable` not `exe`, `arguments` not `args` in public APIs.

### Architecture
- Services are `static` classes with `static async Task` methods — they hold no state.
  Exception: `TimeTracker` is an instance class because it owns timer state.
- Commands may hold instance state (e.g., toggle booleans, current project index) but must
  be thread-safe — SDK may call `RunCommand` concurrently.
- Never catch `Exception` silently in commands — always log via `Notify()` so the user
  knows something went wrong. Swallow only in `TimeTracker.AppendEntry` (must not crash plugin).
- `ProcessSpawner.Run` has a 60s default timeout. Pass a shorter timeout for fast ops
  (e.g., clipboard reads should time out in 5s).

### Error Handling Pattern
```csharp
// Every command that calls an external service must follow this pattern:
try
{
    // ... do work
    await SendHaptic(HapticWaveform.Completed);
    Notify("Title", "Success message.");
}
catch (Exception ex)
{
    await SendHaptic(HapticWaveform.Mad);
    Notify("Title", $"Failed: {ex.Message}");
}
```

### Adding a New Command
1. Create `Commands/<Category>/<Name>Command.cs` extending `CarboCommand`.
2. Set `DisplayName`, `Description`, `GroupName`, `Icon` in the constructor.
3. Override `RunCommand()` — always `async Task`.
4. Register in `Plugin.cs` `Initialize()`.
5. Drop a 256×256 PNG at `metadata/icons/<name>.png` and reference it in `Icon`.

### Adding a New Service
1. Create `Services/<Name>Service.cs` as a `static` class.
2. All methods `public static async Task<T>`.
3. Add any required NuGet package to `Carbo.csproj`.
4. Never store secrets in code — read from `GetSettingValue()` or environment.

---

## Haptic Waveform Reference

| Waveform | When to use |
|---|---|
| `SharpCollision` | AI command sent / action dispatched |
| `Completed` | Result ready / success |
| `DampStateChange` | Thumb wheel project switch |
| `Knock` | Note saved / minor action done |
| `HappyAlert` | Focus / Demo mode ON |
| `Wave` | Focus / Demo mode OFF |
| `Mad` | Error — API unreachable, empty clipboard |
| `Firework` | Daily goal reached / pomodoro complete |
| `SubtleCollision` | Passive 15-min time tracking tick |
| `Ringing` | Upcoming meeting alert |
| `DampCollision` | Ring bubble hover |
| `SharpStateChange` | Ring bubble clicked |

---

## Secrets & Auth

- **Never commit** `client_secrets.json` or `google_token.json` — both are in `.gitignore`.
- Google OAuth credentials live in `AppData/Carbo/google_token.json` at runtime.
- Slack token is stored in Logi Options+ settings (user enters via `SettingType.Text`).
- `Wispr Flow Setup.exe` and any other installer binaries must not be committed.

---

## What NOT to Do

- Do not use `async void` anywhere — use `async Task`.
- Do not use `Thread.Sleep` — use `await Task.Delay`.
- Do not add `Console.WriteLine` — use `Notify()` for user-facing messages.
- Do not hardcode file paths — use `AppContext.BaseDirectory` or `Environment.SpecialFolder`.
- Do not commit binary files (PNGs, EXEs, PDFs) unless they are finalised plugin icons.
- Do not use wildcard NuGet versions (`Version="*"`) in production — pin to a specific version
  before shipping.

---

## Build

```bash
dotnet restore
dotnet build
```

The SDK (`Logi.Actions.SDK`) is a private NuGet from Logitech's developer portal.
Add the feed in NuGet.config before building:

```xml
<packageSources>
  <add key="Logitech" value="https://nuget.logitech.com/v3/index.json" />
</packageSources>
```

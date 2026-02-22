# Carbo

**One plugin. One mouse. Zero tab switching.**

Carbo turns the MX Master 4 into an AI founder's command center — connecting code AI (Claude Code, Cursor), founder workflows (meeting prep, demo mode, investor updates), and intelligent time tracking through the Actions Ring, thumb wheel, and haptic feedback.

> *"Eight bubbles. Twelve signals. Zero context switches."*

---

## The Problem

Jia is building an AI agent startup. It's mid-morning and she's deep in Cursor, debugging a retrieval pipeline. She spots a broken embedding function — needs to refactor it. She highlights the code, switches to her terminal, types out a Claude Code prompt, waits, copies the output, switches back to Cursor, pastes it in. Three minutes gone.

Then she has an investor call in 20 minutes — she scrambles to find the calendar invite, pulls up LinkedIn to remember what they discussed last time, digs through Gmail for the deck she sent last week. Another five minutes gone.

By lunch she's context-switched 90+ times and has no idea how her time broke down across her three active projects.

**This is the cognitive tax of building in 2026.** The tools are powerful individually. The friction lives in the gaps between them.

**Carbo closes those gaps.**

---

## What It Does

### AI at Your Fingertips
Highlight code → press a ring bubble → Claude Code or Cursor refactors it → a haptic pulse confirms the result is in your clipboard. Paste without ever leaving your editor.

### Founder Workflow Triggers
- **Meeting Prep** — One click before a call. Pulls the next event from Google Calendar, surfaces attendee context from Gmail. Toast notification lands in the corner.
- **Demo Mode** — Slack goes DND, notifications silenced, deck opens. One click to activate, one to restore.
- **Investor Update** — After a call, one click drafts a follow-up email with your deck attached. A haptic `knock` confirms the draft is ready.

### Invisible Time Tracking
Carbo watches which app is in the foreground and logs time automatically. The thumb wheel cycles through projects with a tactile click per project. End of day: full breakdown, zero timers started.

---

## Actions Ring Layout

```
            ┌─────────┐
        ╱   │ Claude  │   ╲
      ╱     │  Code   │     ╲
  ┌────┐    └─────────┘    ┌────┐
  │Meet│                   │Cur-│
  │Prep│                   │sor │
  └────┘                   └────┘
        ╲                 ╱
  ┌────┐    ● CARBO ●    ┌────┐
  │Demo│                 │Inv.│
  │Mode│                 │Upd.│
  └────┘                 └────┘
        ╱                 ╲
  ┌────┐    ┌─────────┐   ┌────┐
  │Time│    │  Focus  │   │Stats│
  │Log │    │  Mode   │   │View│
  └────┘    └─────────┘   └────┘
```

| Bubble | Type | What It Does |
|--------|------|--------------|
| Claude Code | Command | Clipboard → Claude CLI → result to clipboard → haptic |
| Cursor AI | Command | Same flow via Cursor CLI |
| Meeting Prep | Command | Calendar + Gmail → toast brief |
| Demo Mode | Toggle | Slack DND + notification silence + deck open |
| Investor Update | Command | Gmail draft with deck attached |
| Focus Mode | Toggle | Pomodoro timer + Slack DND |
| Time Log | Adjustment | Thumb wheel cycles project context |
| Stats View | Command | Opens time tracking dashboard in browser |

**Sub-folder: AI Tools** — Refactor, Explain, Generate Tests, Fix Bug (each with a different system prompt)

**Sub-folder: Founder Ops** — LinkedIn Lookup, Send Deck, Quick Note, Daily Digest

---

## Haptic Language

Carbo assigns a meaning to 12 of the MX Master 4's haptic waveforms so you stop looking at the screen:

| Feel | Waveform | Meaning |
|------|----------|---------|
| Sharp snap | `SharpCollision` | AI command sent |
| Warm pulse | `Completed` | Result ready / paste now |
| Soft detent | `DampStateChange` | Project switched (thumb wheel) |
| Knock | `Knock` | Draft ready / note saved |
| Uplift | `HappyAlert` | Focus or Demo mode ON |
| Fade | `Wave` | Session ended |
| Urgent buzz | `Mad` | Error — check Notify toast |
| Burst | `Firework` | Pomodoro complete |
| Barely-there | `SubtleCollision` | 15-min passive tick |
| Ring | `Ringing` | Meeting in 5 min |

After an hour you stop looking at the screen. You just feel it.

---

## Project Structure

```
Carbo/
├── Plugin.cs                         # Entry point — registers all commands + settings
├── Carbo.csproj                      # .NET 8 Windows library
│
├── Commands/
│   ├── _Base/CarboCommand.cs         # Abstract base: haptic, clipboard, CLI helpers
│   ├── AI/                           # ClaudeCode, CursorAI, Refactor, Explain, Tests, Fix
│   ├── FounderOps/                   # MeetingPrep, DemoMode, InvestorUpdate, LinkedIn, etc.
│   └── TimeTracking/                 # TimeLog (thumb wheel), FocusMode, StatsView
│
├── Services/                         # Stateless service classes
│   ├── ClipboardService.cs           # PowerShell-backed clipboard read/write
│   ├── ProcessSpawner.cs             # CLI process runner with timeout + RunWithFileInput
│   ├── GoogleCalendarService.cs      # Google Calendar API
│   ├── GmailService.cs               # Gmail drafts + unread summary
│   ├── SlackService.cs               # Slack DND toggle
│   ├── AppDetector.cs                # Win32 GetForegroundWindow
│   ├── TimeTracker.cs                # Background poll timer, project log
│   ├── LinkedInService.cs            # Browser-based LinkedIn lookup
│   └── NotificationService.cs        # Windows toast notifications
│
├── Models/                           # Plain C# classes
│   ├── MeetingBrief.cs
│   ├── TimeEntry.cs
│   ├── Project.cs
│   ├── LinkedInProfile.cs
│   └── EmailSummary.cs
│
├── metadata/
│   ├── plugin.json                   # Logi manifest
│   └── icons/                        # 256×256 PNGs, one per command
│
├── Dashboard/index.html              # Local time-tracking chart
└── Data/time_log.json                # Rolling 30-day log
```

---

## Prerequisites

| Requirement | Notes |
|-------------|-------|
| Windows 10/11 | Win32 APIs required |
| [Logi Options+](https://www.logitech.com/software/logi-options-plus.html) | Must be installed and running |
| MX Master 4 | Plugin targets the Actions Ring + thumb wheel + haptic panel |
| .NET 8 SDK | `dotnet build` |
| Logitech Actions SDK NuGet | Private feed — see below |
| Claude CLI (`claude`) | Install via `npm i -g @anthropic-ai/claude-code` |
| Cursor CLI | Bundled with Cursor installation |
| Google OAuth credentials | For Calendar + Gmail features |
| Slack token | For DND features |

---

## Setup

### 1. Add the Logitech NuGet feed

Create or update `NuGet.config` in the repo root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="Logitech" value="https://nuget.logitech.com/v3/index.json" />
  </packageSources>
</configuration>
```

### 2. Build

```bash
dotnet restore
dotnet build
```

### 3. Add Google OAuth credentials

1. Create a project at [console.cloud.google.com](https://console.cloud.google.com)
2. Enable **Google Calendar API** and **Gmail API**
3. Create an OAuth 2.0 Client ID (Desktop app)
4. Download `client_secrets.json` and place it in the project root
   *(It is gitignored — never commit it)*

### 4. Configure settings in Logi Options+

Once the plugin is loaded, open Logi Options+ → Carbo → Settings:

| Setting | What to set |
|---------|-------------|
| Claude CLI Path | Path to `claude` binary (default: `claude`) |
| Cursor CLI Path | Path to `cursor` binary (default: `cursor`) |
| Pitch Deck Path | Full path to your latest `.pptx` or `.pdf` deck |
| Slack Token | Your Slack user OAuth token (starts with `xoxp-`) |
| Projects | Comma-separated list: `Backend API,Pitch Deck,Investor Calls` |
| Focus Duration | 15 / 25 / 45 / 60 min (default: 25) |

### 5. Deploy to Logi Options+

```bash
dotnet publish -c Release
```

Copy the output directory to the Logi Options+ plugins folder, or use `LogiPluginTool` to package and install:

```bash
dotnet tool install -g Logi.Actions.SDK.Tool
LogiPluginTool pack
LogiPluginTool install Carbo.lplug4
```

---

## Using the Plugin

### Basic AI Flow

1. Select code in any editor and copy it (`Ctrl+C`)
2. Press the **Haptic Sense Panel** on your MX Master 4
3. The Actions Ring appears — move to **Claude Code** and click
4. Feel `SharpCollision` (sent) → wait → feel `Completed` (done)
5. `Ctrl+V` anywhere — the improved code is in your clipboard

### Before an Investor Call

1. Click **Meeting Prep** in the ring
2. A toast notification appears with the next event title, attendee name/company, and most recent email subject
3. Walk in prepared — zero tabs opened

### Demo Mode

1. Click **Demo Mode** — feel `HappyAlert`
2. Slack silences, notifications disappear, deck opens
3. Click again when done — feel `Wave`, everything restored

### Switching Projects (Thumb Wheel)

1. Scroll the thumb wheel to cycle through your project list
2. Feel `DampStateChange` click into each project
3. Time is logged automatically to the active project

### Stats Dashboard

Click **Stats View** — your default browser opens a local dashboard showing time by project for today, this week, or this month.

---

## Architecture

```
MX Master 4 Hardware
        │
        ▼
Logi Options+ / Actions SDK
        │
        ▼
Carbo Plugin (C# / .NET 8)
   ├── AI Dispatch ──► Claude CLI / Cursor CLI
   ├── Founder Ops ──► Google Calendar, Gmail, Slack
   ├── Time Tracker ──► Local JSON log
   └── Haptic Engine ──► 12 waveform vocabulary
```

**Key technical decisions:**

- `usesApplicationApiOnly: true` — the ring is available in every app, not tied to one
- Shell injection prevention — AI prompts are written to a temp file and passed via `--prompt-file`, never interpolated into the command line
- Thread-safe static state — toggle fields use `volatile`; timer disposal uses `lock`
- No `async void` — all timer callbacks delegate to `Task.Run`
- Services are stateless `static` classes; only `TimeTracker` owns timer state as an instance

---

## Hackathon Context

Built for the **Logitech Actions SDK Hackathon**.

**Judging criteria alignment:**

| Criterion | Why Carbo scores |
|-----------|-----------------|
| **Novelty** | First plugin combining code AI + founder ops + passive time tracking with a 12-waveform haptic vocabulary |
| **Impact** | AI founders are a fast-growing segment who context-switch 90+ times daily — a real, measurable pain |
| **Viability** | Free tier (AI + time tracking) → Pro ($5/mo, email/calendar/Slack integrations) → Marketplace distribution |
| **Implementation** | Actions Ring native, full haptic coverage, per-app ring profiles, custom icon set, Marketplace-compliant |

**The pitch:**

> Jia builds AI agents for a living. She context-switches 90 times before lunch. Carbo puts Claude Code, meeting prep, demo mode, and investor follow-ups inside her MX Master 4 — accessible through the Actions Ring and confirmed through haptic feedback. She refactors code with a gesture, preps for calls with one click, and sends investor follow-ups without opening Gmail.
>
> Eight bubbles. Twelve signals. Zero context switches.

---

## Contributing

See [`CLAUDE.md`](CLAUDE.md) for conventions, the error-handling pattern, how to add a new command or service, and the haptic waveform reference.

Skills for Claude Code are in [`.claude/skills/`](.claude/skills/):
- `csharp-review` — full code audit against conventions
- `add-command` — scaffold a new ring bubble
- `add-service` — scaffold a new static service

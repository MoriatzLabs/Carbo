# Carbo — Logitech Actions SDK Plugin Concept

## The Story: A Day Without Carbo

Jia is building an AI agent startup. It's mid-morning and she's deep in Cursor, debugging a retrieval pipeline. She spots a broken embedding function — needs to refactor it. She highlights the code, switches to her terminal, types out a Claude Code prompt, waits, copies the output, switches back to Cursor, pastes it in, realizes the context was wrong, goes back. Three minutes gone. Then she has an investor call in 20 minutes — she scrambles to find the calendar invite, pulls up the investor's LinkedIn to remember what they discussed last time, digs through Gmail for the deck she sent last week. Another five minutes gone. By lunch she's context-switched 90+ times and has no idea how her time broke down across her three active projects.

**This is the cognitive tax of building in 2026.** The tools are powerful individually. The friction lives in the gaps between them — every switch, every copy-paste, every manual lookup pulls you out of the problem you were actually solving.

**Carbo closes those gaps.**

---

## The Concept: Carbo

**One plugin. One mouse. Zero tab switching.**

Carbo turns the MX Master 4 into an AI founder's command center — connecting code AI (Claude Code, Cursor), founder workflows (meeting prep, demo mode, investor updates), and intelligent time tracking through the mouse's physical controls, haptic feedback, and the **Actions Ring** overlay.

The core insight: **founders don't need more tools — they need fewer context switches.** Every interaction in Carbo keeps you in your current app, in your current thought, in your flow.

---

## Why These Three Pillars

### 1. Code AI at Your Fingertips

Jia's daily loop: highlight code in Cursor → invoke Claude Code → get result → paste it back. Today that's 4+ context switches per iteration. With Carbo, she presses a bubble on the Actions Ring, the selected code goes to her AI of choice via CLI, and a haptic pulse in her palm tells her the result is on her clipboard. She pastes without ever leaving her editor.

### 2. Founder Workflow Triggers

Jia's day isn't just code. She has investor calls, customer demos, and a deck to prep. Carbo maps these real workflows to Actions Ring bubbles — each powered by direct API integrations:

- **Meeting Prep**: One click before a call. Carbo pulls the next meeting from Google Calendar, looks up the attendee on LinkedIn, and surfaces a brief in a toast notification — name, company, mutual connections, last email thread. Jia walks into every call prepared without opening a single tab.
- **Demo Mode**: Jia is about to demo her product to a prospect. One click activates Demo Mode — Slack goes to DND, notifications are silenced system-wide, and a haptic `happy_alert` confirms she's in presentation mode. When the demo ends, she toggles off and everything restores.
- **Investor Update**: After an investor call, Jia clicks one bubble. Carbo drafts a follow-up email via Gmail with the investor's name (from the calendar event), attaches her latest deck, and opens it for a quick review before sending. A haptic `knock` confirms the draft is ready. No more forgetting to follow up.

### 3. Invisible Time Tracking

Carbo watches which app is in the foreground and logs time automatically against the active project. The thumb wheel lets Jia switch project context with a tactile click per project. At the end of the day, she has a full breakdown without having started a single timer.

---

## Actions Ring Integration

The Actions Ring is Carbo's primary interface — an on-screen overlay with **8 main bubbles** and **up to 9 sub-bubbles per folder**. When a user installs Carbo, it registers its actions as available in the Actions Ring picker. Users drag-and-drop Carbo actions into their ring layout via Logi Options+.

### Carbo Actions Ring Layout (Default Profile)

The plugin ships with a recommended default layout. Users can rearrange or replace any bubble.

```
            ┌─────────┐
        ╱   │ Claude  │   ╲
      ╱     │  Code   │     ╲
  ┌────┐    └─────────┘    ┌────┐
  │Meet│                   │Cur-│
  │Prep│                   │sor │
  └────┘                   └────┘
  │                             │
  ┌────┐     ● CARBO ●     ┌────┐
  │Demo│    (center hub)   │Inv. │
  │Mode│                   │ Upd │
  └────┘                   └────┘
      ╲     ┌─────────┐     ╱
        ╲   │  Focus  │   ╱
            │  Mode   │
            └─────────┘
       ┌────┐       ┌────┐
       │Time│       │Stats│
       │Log │       │View │
       └────┘       └────┘
```

### 8 Main Bubbles

| Position | Action Name | Type | Behavior |
|----------|-------------|------|----------|
| **Top** | Claude Code | Command | Sends selected text/clipboard to Claude Code CLI, haptic `sharp_collision` on send, `completed` when result returns to clipboard |
| **Top-Right** | Cursor AI | Command | Same flow routed to Cursor's CLI/API for inline code generation |
| **Right** | Investor Update | Command | Drafts a follow-up email via Gmail (contact from last calendar event, attaches latest deck), haptic `knock` when draft is ready |
| **Bottom-Right** | Stats View | Command | Opens Carbo time dashboard (local HTML) showing today's project breakdown |
| **Bottom** | Focus Mode | Command (Toggle) | Activates Pomodoro: sets Slack to DND via API, starts timer, haptic `happy_alert`. Toggle off or auto-end with `wave` haptic |
| **Bottom-Left** | Time Log | Adjustment | Click to see current project; **scroll thumb wheel** to switch project context with `damp_state_change` haptic ticks |
| **Left** | Demo Mode | Command (Toggle) | Silences Slack + system notifications, optionally launches demo app/deck. Haptic `happy_alert` on activate, `wave` on deactivate |
| **Top-Left** | Meeting Prep | Command | Pulls next meeting from Google Calendar, looks up attendee on LinkedIn, surfaces toast with brief (name, company, last email). Haptic `completed` when ready |

### Folder: "AI Tools" (Sub-bubbles)

For power users who want more AI options, Carbo provides a folder with 4 sub-bubbles:

| Sub-bubble | Action |
|------------|--------|
| Claude Code (Refactor) | Sends with system prompt: "Refactor this code" |
| Claude Code (Explain) | Sends with system prompt: "Explain this code" |
| Cursor (Generate Tests) | Sends with system prompt: "Write tests for this" |
| Cursor (Fix Bug) | Sends with system prompt: "Find and fix bugs in this" |

### Folder: "Founder Ops" (Sub-bubbles)

| Sub-bubble | Action |
|------------|--------|
| LinkedIn Lookup | Opens the next meeting attendee's LinkedIn profile in browser |
| Send Deck | Composes a Gmail draft with the latest pitch deck (PDF/PPTX) attached |
| Quick Note | Captures clipboard → appends to a running local markdown log (timestamped) |
| Daily Digest | Compiles today's time breakdown + meetings attended and sends summary via Gmail |

### Context-Aware Profiles

Carbo leverages the Actions Ring's **per-app profile** system. The ring automatically adapts:

| Active App | Ring Adjusts To |
|-----------|----------------|
| **Cursor / VS Code / Terminal** | AI actions prominent (Claude Code, Cursor in top positions) |
| **Browser (LinkedIn, Gmail)** | Founder Ops actions prominent (Meeting Prep, Investor Update) |
| **Slack / Zoom** | Focus Mode, Demo Mode, and Meeting Prep prominent |
| **PowerPoint / Google Slides** | Demo Mode prominent, Send Deck in easy reach |
| **Any app** | Time tracking always active in bottom-left position |

---

## Icon Design: Carbo Visual System

### Design Philosophy

Carbo icons use a **monoline style** with a consistent 2px stroke weight on a transparent background. The palette is minimal: **white icons on dark ring backgrounds**, with a single accent color (**#00E5A0** — Logitech teal/green) for active states. Every icon is designed to be legible at the Actions Ring's bubble size (~48px rendered).

### Plugin Icon (Marketplace & Options+)

**File**: `metadata/Icon256x256.png`

| Spec | Value |
|------|-------|
| Size | 256 × 256 px |
| Format | PNG with transparency |
| Design | A stylized "C" formed from a diamond/carbon lattice pattern, with a small lightning bolt cutting through the center. Monochrome white on transparent. Teal (#00E5A0) accent on the bolt. |
| Concept | Carbon structure = foundation; lightning = speed/automation |

### Actions Ring Bubble Icons

Each action registered by Carbo needs an icon visible inside the ring bubble. These follow the Logi Options+ icon conventions.

| Action | Icon Description | Visual Concept |
|--------|-----------------|----------------|
| **Claude Code** | Terminal cursor `▌` inside a chat bubble outline | Code meets conversation |
| **Cursor AI** | Cursor pointer with a small sparkle/star | AI-native editor |
| **Meeting Prep** | Calendar page with a person silhouette overlay | Know who you're meeting |
| **Demo Mode** | Play button (▶) inside a shield outline | Presentation + protection |
| **Investor Update** | Envelope with a small arrow pointing up-right | Follow-up sent |
| **Focus Mode** | Shield outline with a clock face inside | Protection + time |
| **Time Log** | Stopwatch with segmented ring (project slices) | Time across projects |
| **Stats View** | Mini bar chart (3 bars ascending) | Data visualization |
| **AI Refactor** | Two rotating arrows around a code bracket | Transform code |
| **AI Explain** | Lightbulb with code bracket inside | Understanding |
| **Generate Tests** | Checkmark inside a test tube | Test creation |
| **Fix Bug** | Bug silhouette with a wrench | Debug |
| **LinkedIn Lookup** | Person silhouette with a chain-link icon | Professional context |
| **Send Deck** | Presentation slide with an arrow pointing out | Share your pitch |
| **Quick Note** | Clipboard with a small arrow pointing into it | Capture to destination |
| **Daily Digest** | Envelope with a chart inside | Summary email |

### Icon Color States

| State | Treatment |
|-------|-----------|
| **Default** | White monoline on transparent |
| **Hover** | White fill with slight glow |
| **Active / Running** | Teal (#00E5A0) fill — indicates action is in progress |
| **Error** | Red (#FF4D4D) outline pulse |
| **Complete** | Brief green (#00E5A0) flash, returns to default |

---

## Hardware: MX Master 4 Control Mapping

### Physical Controls Available

| Control | Location | Native Behavior |
|---------|----------|-----------------|
| **Haptic Sense Panel** | Thumb rest | Press to open Actions Ring |
| **Gesture Button** | Below thumb buttons | Hold + move mouse in direction |
| **Thumb Wheel** | Side, horizontal | Horizontal scroll |
| **Forward/Back Buttons** | Side, above thumb | Navigate forward/back |
| **MagSpeed Scroll Wheel** | Top | Vertical scroll, ratchet/free-spin |

### Carbo Control Mapping (Beyond Actions Ring)

The Actions Ring is the primary interface, but gestures provide quick shortcuts without opening the ring:

| Control | Action | What Happens |
|---------|--------|-------------|
| **Haptic Sense Panel press** | Open Carbo Ring | Shows the Actions Ring with Carbo's 8 bubbles |
| **Gesture + Up** | Quick: Claude Code | Shortcut — same as Claude Code bubble, bypasses ring |
| **Gesture + Down** | Quick: Cursor AI | Shortcut — same as Cursor AI bubble |
| **Gesture + Left** | Quick: Meeting Prep | Shortcut — pulls next meeting context |
| **Gesture + Right** | Quick: Investor Update | Shortcut — drafts follow-up from last calendar event |
| **Thumb Wheel Scroll** | Switch project | Cycles through projects for time tracking |
| **Forward Button (long press)** | Toggle Focus Mode | Same as Focus Mode bubble |
| **Back Button (long press)** | Toggle Demo Mode | Same as Demo Mode bubble |

This dual-mode design means: **Actions Ring for deliberate choices, gestures for muscle-memory shortcuts.** New users start with the ring; power users graduate to gestures.

---

## Haptic Feedback Design — The Tactile Language

The MX Master 4 supports **16 distinct haptic waveforms** via the Logi Actions SDK. Carbo uses them to create a *tactile vocabulary* — a language you learn unconsciously so you always know what's happening without looking at your screen.

### Waveform Assignments

| Waveform | SDK Name | Carbo Meaning | Why This Waveform |
|----------|----------|---------------|-------------------|
| **Sharp Collision** | `sharp_collision` | AI command sent successfully | Crisp, decisive — confirms your intent was captured |
| **Completed** | `completed` | AI response ready / Meeting brief ready | Warm, satisfying — the work is done, come get it |
| **Damp State Change** | `damp_state_change` | Project context switched (thumb wheel) | Soft detent — like clicking into a new gear |
| **Knock** | `knock` | Action completed (email drafted, note saved) | Distinct tap — like knocking on a door to confirm delivery |
| **Happy Alert** | `happy_alert` | Focus / Demo mode activated | Uplifting — you're entering a protected state |
| **Wave** | `wave` | Focus / Demo session ended | Gentle fade-out — easing you back to awareness |
| **Mad** | `mad` | Error: API unreachable / auth failed | Sharp, urgent — something went wrong, check it |
| **Firework** | `firework` | Daily time goal reached | Celebratory — you've put in your hours |
| **Subtle Collision** | `subtle_collision` | Passive time tracking tick (every 15 min) | Barely perceptible — ambient awareness without distraction |
| **Ringing** | `ringing` | Upcoming meeting in 5 min (Calendar alert) | Attention-grabbing — something external needs you |
| **Damp Collision** | `damp_collision` | Actions Ring bubble hover confirmation | Soft feedback as cursor lands on each bubble |
| **Sharp State Change** | `sharp_state_change` | Actions Ring bubble clicked/activated | Confirms the action was triggered |

### Haptic Flow Examples

**Scenario 1: AI Refactor via Actions Ring**
1. Press Haptic Sense Panel → Ring appears → `damp_collision` as you hover each bubble
2. Click "Claude Code" bubble → `sharp_state_change` (confirmed click) → `sharp_collision` (code sent)
3. 2-3 seconds pass... → `completed` (result is in your clipboard, paste anywhere)

**Scenario 2: Meeting Prep Before an Investor Call**
1. Jia's next meeting is in 10 min → `ringing` (calendar alert, feel it in your palm)
2. She presses Meeting Prep bubble → `sharp_state_change` (activated)
3. Carbo pulls calendar event + LinkedIn profile + last Gmail thread → `completed` (brief appears as toast)
4. Jia glances at the brief — name, company, mutual connections, last email — and walks into the call prepared

**Scenario 3: Demo Mode for a Customer Call**
1. Press Demo Mode bubble → `happy_alert` (you're in presentation mode)
2. Slack goes to DND, system notifications silenced
3. Jia runs her demo undisturbed
4. Press again to deactivate → `wave` (everything restored)

**Scenario 4: Sending an Investor Follow-up**
1. After a call, Jia clicks Investor Update → `sharp_state_change` (activated)
2. Carbo grabs the contact from the calendar event, drafts a Gmail follow-up with her deck attached → `knock` (draft ready!)
3. If Gmail auth expired → `mad` (error — check settings)

**Scenario 5: Switching Projects**
1. Scroll thumb wheel left → `damp_state_change` (now on "Backend API")
2. Scroll again → `damp_state_change` (now on "Investor Deck")
3. Reach end of list → `mad` (boundary — no more projects)

### Why Haptics Matter for Founders

The haptic motor on the MX Master 4 is audible even when you're not holding the mouse — which means it can **replace audio notifications**. Instead of a Slack ping breaking your concentration, you feel a gentle `ringing` in your palm when your next meeting is approaching. You decide if it's worth breaking flow. This is the key UX insight: **haptics give you information without demanding attention**.

---

## User Configuration

Carbo is opinionated out of the box but fully configurable via a settings panel in Logi Options+.

### Guided Onboarding — "Build Your Ring"

On first install, Carbo opens a local onboarding page that asks three questions. Based on the answers, it pre-configures the Actions Ring layout, registers only the relevant actions, and connects the right APIs.

**Question 1: What's your AI code tool?**

| Choice | Result |
|--------|--------|
| Claude Code | Top bubble = Claude Code, AI Tools folder = Claude prompts |
| Cursor | Top bubble = Cursor AI, AI Tools folder = Cursor prompts |
| Both | Top = Claude Code, Top-Right = Cursor (default layout) |
| Custom CLI | Top bubble = user-defined command, prompts configurable |

**Question 2: How do you communicate with investors and customers?**

| Choice | Result |
|--------|--------|
| Gmail + Google Calendar | Meeting Prep uses Google Calendar, Investor Update drafts via Gmail. OAuth flow triggered. |
| Outlook + Outlook Calendar | Same actions rewired to Microsoft Graph API. OAuth flow triggered. |
| I'll configure later | Communication bubbles available but unconfigured; Quick Note (local log) fills the slot |

**Question 3: What does your day look like?**

| Choice | Ring Layout |
|--------|-------------|
| **Mostly coding** | AI actions in top 3 positions, founder ops in folder, Focus Mode prominent |
| **Mostly meetings & fundraising** | Meeting Prep + Demo Mode + Investor Update in top positions, AI actions in folder |
| **Mixed (default)** | Balanced layout — AI top, founder ops sides, Focus/Time bottom |

After onboarding, the ring is ready to use immediately. Every choice can be changed later in the Logi Options+ settings panel.

### Configurable Settings (Profile Actions)

| Setting | Default | Options |
|---------|---------|---------|
| **AI Provider (Bubble 1)** | Claude Code | Claude Code, Cursor, Custom CLI command |
| **AI Provider (Bubble 2)** | Cursor | Claude Code, Cursor, Custom CLI command |
| **AI System Prompts** | Refactor / Explain / Test / Fix | Free text — user can write any prompt |
| **Google Calendar Account** | (OAuth login) | Connected via External Service Login |
| **Gmail Account** | (OAuth login) | For investor follow-ups and daily digest |
| **Slack Workspace Token** | (OAuth login) | Connected via External Service Login |
| **Pitch Deck Path** | (empty — user sets) | Path to latest PPTX/PDF deck for Send Deck action |
| **Meeting Prep Lead Time** | 5 min before | 2 / 5 / 10 / 15 min |
| **Project List** | Project A, B, C | User-defined list (up to 10 projects) |
| **Time Tracking Export** | Local JSON | Local JSON, CSV export, email digest |
| **Focus Duration** | 25 min (Pomodoro) | 15 / 25 / 45 / 60 min |
| **Haptic Intensity** | Medium | Controlled via Logi Options+ (Subtle / Low / Medium / High) |
| **Passive Time Tick** | Every 15 min | Off / 15 / 30 / 60 min |
| **Actions Ring Profile** | General | General, VS Code, Browser, Slack (auto-switches) |

All settings are persisted via the SDK's `PluginSetting` API and survive restarts. OAuth flows use the SDK's `External Service Login` feature.

---

## Architecture (Hackathon MVP)

```
┌─────────────────────────────┐
│     MX Master 4 Mouse       │
│   (Haptic Sense Panel +     │
│    Gestures + Thumb Wheel)  │
└──────────┬──────────────────┘
           │ Events
           ▼
┌─────────────────────────────┐
│  Logi Options+ / Actions    │
│  Ring / Plugin Service      │
└──────────┬──────────────────┘
           │ Actions SDK (C#)
           ▼
┌──────────────────────────────────────────────────┐
│               CARBO PLUGIN (C#)                  │
│                                                  │
│  ┌────────────┐ ┌──────────────┐ ┌──────────┐   │
│  │ AI Dispatch │ │ Founder Ops  │ │ Time     │   │
│  │ Module      │ │ Module       │ │ Tracker  │   │
│  └──────┬─────┘ └──────┬───────┘ └────┬─────┘   │
│         │              │               │         │
│  ┌──────▼─────┐ ┌──────▼────────┐ ┌───▼──────┐  │
│  │ Clipboard   │ │ REST API      │ │ App      │  │
│  │ + Process   │ │ Client        │ │ Detect   │  │
│  │ Manager     │ │ (Google Cal,  │ │ + Log    │  │
│  │             │ │  Gmail, Slack,│ │          │  │
│  │             │ │  LinkedIn)    │ │          │  │
│  └──────┬─────┘ └──────┬────────┘ └───┬──────┘  │
│         │              │               │         │
│  ┌──────▼──────────────▼───────────────▼──────┐  │
│  │           Haptic Feedback Engine            │  │
│  │     (Waveform selection + dispatch)         │  │
│  └────────────────────────────────────────────┘  │
│                                                  │
│  ┌────────────────────────────────────────────┐  │
│  │        Actions Ring Action Registry         │  │
│  │    (8 commands + 2 folders + icons)         │  │
│  └────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────┘
       │              │              │
       ▼              ▼              ▼
┌──────────┐   ┌──────────┐   ┌──────────┐
│ Claude   │   │ Google   │   │ LinkedIn │
│ Code /   │   │ Calendar │   │ (Profile │
│ Cursor   │   │ + Gmail  │   │  Lookup) │
└──────────┘   └──────────┘   └──────────┘
                    │
                    ▼
              ┌──────────┐
              │  Slack   │
              │  API     │
              └──────────┘
```

### Key Technical Decisions

- **C# Actions SDK** — mature, released 1 year ago, full haptics + Actions Ring support
- **`UsesApplicationApiOnly = true`** — Carbo is not tied to one app; it works across all profiles and is accessible from the Action Panel everywhere
- **Direct REST APIs** — Google Calendar, Gmail, Slack, and LinkedIn APIs called via `HttpClient` in C#. No middleware layer. Simple, fast, debuggable.
- **OAuth via SDK** — The Actions SDK supports `External Service Login` for OAuth flows (Google, Slack). Tokens stored securely via `PluginSetting`.
- **CLI process spawning for AI** — `claude` and `cursor` are CLI tools; we spawn them as child processes and capture stdout
- **Active window detection** — Win32 API (`GetForegroundWindow`) to detect current app for context-aware prompts, time tracking, and Actions Ring profile switching
- **Local JSON for time data** — MVP stores via SDK's `PluginData` storage; can export to CSV or email digest
- **Icon assets** — All icons ship as PNGs in the `metadata/` folder, following Logitech's monoline style

---

## MVP Scope (Hackathon — 1-2 Days)

### Day 1: Core Loop + Actions Ring
1. **Plugin scaffold** — LogiPluginTool template, `UsesApplicationApiOnly = true`
2. **Register 8 Actions Ring commands** — Each as a `PluginDynamicCommand` with custom icon
3. **AI dispatch (Claude Code bubble)** — Clipboard → CLI → result to clipboard → haptic `sharp_collision` + `completed`
4. **Meeting Prep** — Google Calendar API → next event → LinkedIn profile lookup → Gmail thread search → toast notification → haptic `completed`
5. **Basic haptic mapping** — Wire up the 8 most important waveforms
6. **Icon assets** — Create all 16 icons (8 main + 8 sub-bubble) as monoline PNGs

### Day 2: Polish + Story
7. **Demo Mode** — Toggle command: Slack DND API + system notification suppression + `happy_alert` / `wave` haptics
8. **Investor Update** — Calendar event → Gmail draft with deck attached → haptic `knock`
9. **Time tracking** — Thumb wheel as `PluginDynamicAdjustment` for project cycling with `damp_state_change` ticks
10. **AI sub-folder** — Register 4 sub-actions with different system prompts
11. **Settings UI** — Profile actions with text parameters for API keys, project names, AI provider selection
12. **Demo flow** — End-to-end story: code → AI refactor via ring → meeting prep → demo mode → investor follow-up → time tracked

---

## Judging Criteria Alignment

### Novelty ★★★★★
No existing Logitech plugin combines code AI (Claude Code, Cursor), founder workflow automation (meeting prep, demo mode, investor follow-ups via email), and passive time tracking — all surfaced through the Actions Ring with a coherent haptic language. The 12-waveform tactile vocabulary is a genuinely new interaction paradigm for a mouse.

### Impact ★★★★★
AI founders are a fast-growing, high-value user segment. They spend 8-12 hours daily across code editors, email, calendars, LinkedIn, and pitch decks. The "tab overload" problem from Logitech's own research maps perfectly to this persona. The narrative is clear: *"Your mouse is your co-pilot."*

### Viability ★★★★☆
- **Free tier**: Core AI dispatch + meeting prep + basic time tracking
- **Pro tier ($5/mo)**: Full email/calendar integration, demo mode, investor follow-ups, team time visibility
- **Distribution**: Logitech Marketplace (`.lplug4` package), open-source SDK plugin so community can extend
- **Maintenance**: Direct API integrations with stable, well-documented services (Google, Slack, LinkedIn)

### Implementation Quality ★★★★★
- **Actions Ring native** — fully integrated with Logitech's primary UI paradigm, not a workaround
- **Custom icon set** — cohesive monoline visual system at every touch point
- **Haptic feedback at every interaction** — creates a polished, premium feel
- **Per-app ring profiles** — adapts to context automatically
- User-configurable via standard Logi Options+ settings panel
- Compliant with Marketplace approval guidelines: `UsesApplicationApiOnly`, no admin install, no GPL dependencies, proper EULA

---

## The Pitch (30 seconds)

> *"Jia builds AI agents for a living. She context-switches 90 times before lunch. Carbo puts Claude Code, meeting prep, demo mode, and investor follow-ups inside her MX Master 4 — accessible through the Actions Ring and confirmed through haptic feedback. She refactors code with a gesture, preps for calls with one click, and sends investor follow-ups without opening Gmail. Eight bubbles. Twelve haptic signals. Zero context switches."*

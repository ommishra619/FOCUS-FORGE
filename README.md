# FocusForge

FocusForge is a Windows desktop productivity app for planning focused work and adding personal friction around distracting apps.

## First slice

The initial WPF shell includes a daily dashboard, task list, focus session status, and protected-app status. The app targets .NET 8 and will grow into a tray agent with local SQLite persistence and process monitoring.

Task management supports title-first capture with `Ctrl+N`, editing, optional details and time context, explicit unscheduled/someday/specific-date scheduling, daily/weekday/weekly/monthly recurrence, free-form tags, and nested checklists. Existing workspace JSON remains readable; new task metadata is added on the next save.

The Weight tracker section records dated measurements and renders them as a saved trend graph with first-to-latest change summary.

MVP app locking is personal friction: configured processes can be detected and closed while a rule is active. It is not tamper-resistant against an administrator or a user who disables the agent.

## Prerequisites

- Windows 10 or later
- .NET 8 SDK

Build from the repository root with `dotnet build FocusForge.sln`.

# agent.md — C# Avalonia File Manager

This document defines the rules and conventions for an automated coding agent contributing to this repository.

## Project Overview

This is a cross-platform **Avalonia UI** application (not WPF) using **MVVM** with the **CommunityToolkit.Mvvm** (MVVM Community Toolkit). The app is a file manager and should remain responsive on large directories and slow file systems.

## Non-Negotiables

- **Use Avalonia, not WPF**
  - UI markup must be **Avalonia XAML** (`xmlns="https://github.com/avaloniaui"`).
  - Do not introduce WPF types/namespaces: `System.Windows.*`, `Microsoft.Xaml.Behaviors`, WPF `ICommand` helpers, `DependencyObject`, etc.
  - Prefer Avalonia patterns: `StyledProperty`, `DirectProperty`, `Classes`, `PseudoClasses`, `DataTemplates`, `Styles`, `Converters`, `Behaviors` (Avalonia-compatible only).

- **Use MVVM Community Toolkit**
  - ViewModels should use `ObservableObject` / `[ObservableProperty]`, `[RelayCommand]`, `[NotifyPropertyChangedFor]`, etc.
  - Prefer `IMessenger` from the Toolkit for app-level events when appropriate.
  - Avoid custom `INotifyPropertyChanged` boilerplate unless required for performance hotspots.

- **MVVM discipline**
  - Views should not contain business logic.
  - File system operations, IO, parsing, indexing, etc. belong in services/models.
  - ViewModels coordinate UI state and call services; they should be testable without UI.

## Architecture & Patterns

- **Dependency Injection**
  - Use `Microsoft.Extensions.DependencyInjection`.
  - Register services and ViewModels in composition root (App startup).
  - For ViewModels created dynamically (e.g., directory nodes, file items), use factories:
    - `Func<T>` / `Func<Arg, T>` or explicit factory interfaces.
    - Avoid service locator patterns inside ViewModels unless there’s a strong reason.

- **Async + cancellation**
  - Use async IO APIs whenever possible.
  - Long-running operations must accept `CancellationToken`.
  - UI-triggered operations should cancel prior runs where it makes sense (e.g., directory scanning, search).
  - Never block the UI thread with `.Result`, `.Wait()`, or heavy synchronous enumeration.

- **Performance expectations**
  - Directory enumeration must be incremental / streaming where possible.
  - Avoid repeated allocation/copying of large collections.
  - Use virtualization-friendly controls and patterns (Avalonia `DataGrid`/`ItemsControl` with virtualization where applicable).
  - Prefer batched UI updates (e.g., chunk updates to `ObservableCollection`) when adding many items.

## Avalonia UI Conventions

- **Avalonia XAML namespaces**
  - Use the Avalonia namespace: `xmlns="https://github.com/avaloniaui"`.
  - Keep control templates and styles in `Styles/` or `Themes/` folders (repo-dependent).

- **Binding**
  - Prefer `{Binding}` and compiled bindings if the project is configured for them.
  - Keep binding paths stable; avoid deep binding chains in hot UI paths.

- **DataTemplates**
  - Use `DataTemplate` / `IDataTemplate` to swap UI by ViewModel type.
  - Prefer template-based UI polymorphism over type checks in code-behind.

- **Code-behind**
  - Allowed for view-only glue (e.g., focus, drag/drop wiring, visual-state tweaks) but keep it minimal.
  - No business logic in code-behind.

## File System Domain Rules

- Treat file system as hostile:
  - Handle permission issues, broken symlinks, long paths, transient IO errors.
  - Avoid throwing on common failure paths; surface errors to UI cleanly.
- Prefer `IFileSystem` abstractions (if present) to make testing easier.
- Use `FileInfo/DirectoryInfo` sparingly for large enumerations; `Directory.Enumerate*` and `FileSystemEnumerable` patterns may be preferred for performance.

## Logging & Diagnostics

- Use whatever logging is already in the repo (commonly `Microsoft.Extensions.Logging`).
- Log at appropriate levels:
  - Debug/Trace for noisy perf details,
  - Information for user-relevant operations,
  - Warning/Error for failures.

## Testing

- ViewModels and services should be unit testable without Avalonia.
- Avoid static global state that makes tests flaky.
- If UI tests exist, do not expand them unless necessary; focus on service + VM coverage.

## Coding Standards

- Prefer modern C# patterns (nullable reference types, records where suitable, pattern matching).
- Keep names clear and explicit; avoid cryptic abbreviations.
- Do not use `dynamic`.
- Do not use `new ()` without specifying the type.
- Prefer `var` over concrete types when possible.
- Keep public APIs small; prefer internal where possible.
- Avoid unnecessary allocations and LINQ in hot paths (directory scans, sorting large lists).
- Always wrap blocks with braces even when only a single line

## PR / Change Hygiene

When making changes:
- Explain intent in the commit/PR message.
- Keep changes scoped; avoid drive-by refactors.
- Add/update tests when behavior changes.
- If introducing a new dependency, justify it and keep it minimal.

## Agent Workflow Expectations

Before coding:
1. Identify existing patterns in the repo (DI setup, ViewModel base classes, navigation).
2. Prefer extending current architecture over inventing a new one.
3. If a new pattern is necessary, document it briefly in the relevant README/docs.

While coding:
- Ensure new code compiles and matches repo style.
- Avoid introducing WPF concepts or libraries.
- Keep UI responsive; assume large directories and slow disks.

After coding:
- Update docs if behavior or architecture changed.
- Include notes on cancellation, error handling, and performance implications when relevant.


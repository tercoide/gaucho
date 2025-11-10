# Gaucho - GTK4 C# Application Template

This is a GTK4 C# application template that provides a foundation for building modern desktop applications.

## Project Overview

- **Framework**: GTK4 with GIR.Core bindings
- **Language**: C# (.NET 8.0)
- **Architecture**: Modern GTK4 application with header bar, sidebar, and main content areas
- **Purpose**: Template/skeleton for rapid GTK4 application development

## Key Components

- `Program.cs`: Application entry point with GTK4 initialization
- `MainWindow.cs`: Main application window with modern GTK4 widgets
- `gaucho.csproj`: Project configuration with GIR.Core dependencies

## Development Notes

- Uses GIR.Core for GTK4 bindings instead of GtkSharp (GTK3)
- Implements modern GTK4 patterns: HeaderBar, Box layouts, CSS styling
- Includes application menu with standard actions (About, Quit)
- Ready for extension with additional windows and custom widgets

## Build & Run

```bash
dotnet restore
dotnet build
dotnet run
```

Use the VS Code build task "Build gaucho" for development.
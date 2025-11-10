# Gaucho

A GTK4 C# application template that provides a foundation for building desktop applications with a modern user interface.

## Features

- **Modern GTK4 Interface**: Clean, responsive UI with header bar and modern styling
- **Modular Architecture**: Well-organized code structure for easy extension
- **Sample Components**: Includes examples of common UI elements:
  - Header bar with application menu
  - Main content area with sidebar layout
  - Interactive buttons and dialogs
  - About dialog
- **Cross-Platform**: Runs on Linux, Windows, and macOS

## Prerequisites

- .NET 8.0 or later
- GTK4 development libraries installed on your system

### Installing GTK4 on Linux (Ubuntu/Debian)
```bash
sudo apt-get install libgtk-4-dev
```

### Installing GTK4 on Windows
- Install GTK4 runtime from https://github.com/wingtk/gvsbuild
- Or use MSYS2/MinGW-w64

### Installing GTK4 on macOS
```bash
brew install gtk4
```

## Building and Running

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

## Project Structure

```
gaucho/
├── Program.cs           # Application entry point
├── MainWindow.cs        # Main application window
├── gaucho.csproj       # Project configuration
└── README.md           # This file
```

## Customization

### Adding New Windows
1. Create a new class inheriting from `Gtk.Window` or `Gtk.ApplicationWindow`
2. Follow the pattern established in `MainWindow.cs`
3. Register the window with your application

### Adding Custom Widgets
1. Create custom widgets by inheriting from appropriate GTK base classes
2. Implement your custom logic in the derived class
3. Add the widgets to your window layouts

### Styling
- Use GTK CSS classes for custom styling
- Add CSS class names using `widget.StyleContext.AddClass("class-name")`
- Create CSS files and load them for custom themes

### Menu and Actions
- Extend the application menu in `MainWindow.cs`
- Add new menu items and connect them to action handlers
- Consider using `Gio.SimpleAction` for more complex applications

## Dependencies

- **GirCore.Gtk-4.0**: .NET bindings for GTK4 using GObject Introspection
- **.NET 8.0**: Target framework

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is provided as a template. Feel free to use it as a starting point for your own applications.

## Troubleshooting

### Common Issues

**Application doesn't start**:
- Ensure GTK3 is properly installed
- Check that all dependencies are restored with `dotnet restore`

**UI elements not displaying correctly**:
- Verify GTK3 version compatibility
- Check console output for GTK warnings

**Build errors**:
- Ensure you have the correct .NET SDK version
- Try cleaning and rebuilding: `dotnet clean && dotnet build`

For more help, check the [GTK documentation](https://www.gtk.org/docs/) and [GtkSharp documentation](https://github.com/GtkSharp/GtkSharp).
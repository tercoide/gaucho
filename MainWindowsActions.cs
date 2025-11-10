using System;
using Gio;
using Gtk;
using System.Globalization;

namespace Gaucho;

 public partial class MainWindow
// public static class Actions1
{
    // private void OnPrimaryActionClicked(Button sender, EventArgs e)
    // {
    //     ActionActivate("primary-action");
    // }

    // private void OnSecondaryActionClicked(Button sender, EventArgs e)
    // {
    //     ActionActivate("secondary-action");
    // }

    // private void OnAboutActivated(SimpleAction sender, EventArgs e)
    // {
    //     ActionActivate("about");
    // }

    // private void OnQuitActivated(SimpleAction sender, EventArgs e)
    // {
    //     ActionActivate("quit");
    // }

    private  void ShowInfoDialog(string title, string message)
    {
        UpdateStatus($"{title}: {message}");
    }

    public  void UpdateStatus(string message)
    {
        if (_statusLabel is not null)
        {
            _statusLabel.SetLabel(message);
        }

        Console.WriteLine(message);
    }

    public void ActionActivate(string actionId)
    {
        Console.WriteLine($"Action activated: {actionId}");
        switch (actionId)
        {
            case "new":
                UpdateStatus("New project action triggered");
                break;
            case "open":
                UpdateStatus("Open action triggered");
                break;
            case "save":
                UpdateStatus("Save action triggered");
                break;
            case "export":
                UpdateStatus("Export action triggered");
                break;
            case "primary-action":
                ShowInfoDialog("Primary Action", "You clicked the primary action button!");
                break;
            case "secondary-action":
                ShowInfoDialog("Secondary Action", "You clicked the secondary action button!");
                break;
            case "about":
                var aboutDialog = AboutDialog.New();
                aboutDialog.ProgramName = "Gaucho";
                aboutDialog.Version = "1.0.0";
                aboutDialog.Comments = "A GTK4 C# application template";
                aboutDialog.Copyright = "Copyright © 2025";
                aboutDialog.WebsiteLabel = "Project Website";
                aboutDialog.SetTransientFor(this);
                aboutDialog.Present();
                break;
            case "quit":
                Application?.Quit();
                break;
            case "app.quit":
                Application?.Quit();
                break;
            default:
                if (actionId.StartsWith("sector:", StringComparison.OrdinalIgnoreCase))
                {
                    var sector = actionId["sector:".Length..];
                    var readable = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sector.Replace('-', ' '));
                    UpdateStatus($"Selected sector: {readable}");
                }
                else if (actionId.StartsWith("viewport:realized:", StringComparison.OrdinalIgnoreCase))
                {
                    var viewportName = actionId["viewport:realized:".Length..];
                    UpdateStatus($"Viewport ready: {viewportName}");
                }
                else
                {
                    UpdateStatus($"Action '{actionId}' triggered.");
                }
                break;
        }
    }
}

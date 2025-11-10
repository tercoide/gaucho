using Gtk;
using Gio;

namespace Gaucho;

class Program
{
    static void Main(string[] args)
    {
    var app = Gtk.Application.New("org.gaucho.app", Gio.ApplicationFlags.FlagsNone);
        app.OnActivate += OnActivated;
        
        app.RunWithSynchronizationContext(args);
    }

    private static void OnActivated(Gio.Application sender, EventArgs e)
    {
        var application = (Gtk.Application)sender;
        var window = new MainWindow(application);
        window.Present();
    }
}
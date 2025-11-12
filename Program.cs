using Gtk;
using Gio;
using System.Threading.Tasks;

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
        
        // Show splash screen first
        var splash = new fSplash();
        splash.ShowSplash();
        
        // Create main window (but don't show it yet)
        var window = new fMain(application);
        
       
                splash.HideSplash();
                window.Present();
    
      
    }
}
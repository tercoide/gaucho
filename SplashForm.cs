using Gtk;
using Gdk;

namespace Gaucho;

public class fSplash : Window
{
    private Picture _picture = null!;
    private Box _mainBox = null!;
    
    public fSplash() : base()
    {
        InitializeComponent();
        BuildUI();
        
    }

    private void InitializeComponent()
    {
        Title = "Gaucho - Loading";
        SetDefaultSize(400, 300);
        
        // Make the window modal and remove decorations for splash effect
        Modal = true;
        Decorated = false;
        Resizable = false;
              
        // Set window position to center
        // Position = Position.Center;
        
        // Optional: Set a timeout to auto-close the splash after a few seconds
        // This will be handled by the Program.cs instead
    }

    private void BuildUI()
    {
        // Create main container box
        _mainBox = Box.New(Orientation.Vertical, 0);
        _mainBox.SetHalign(Align.Center);
        _mainBox.SetValign(Align.Center);
        _mainBox.SetHexpand(true);
        _mainBox.SetVexpand(true);
        
        // Create picture widget for the image
        _picture = Picture.New();
        
        // Try to load an image - you can change this path to your image
        string imagePath = "splash.png"; // Put your splash image in the project root
        
        try 
        {
            // Load image from file
            var texture = Gdk.Texture.NewFromFilename(imagePath);
            _picture.SetPaintable(texture);
        }
        catch 
        {
            // If image not found, create a simple label as fallback
            var fallbackLabel = Label.New("GAUCHO");
            fallbackLabel.SetMarkup("<span size='24000' weight='bold'>GAUCHO</span>");
            fallbackLabel.SetHalign(Align.Center);
            fallbackLabel.SetValign(Align.Center);
            _mainBox.Append(fallbackLabel);
            
            var versionLabel = Label.New("GTK4 Application");
            versionLabel.SetHalign(Align.Center);
            versionLabel.SetValign(Align.Center);
            versionLabel.MarginTop = 10;
            _mainBox.Append(versionLabel);
            
            SetChild(_mainBox);
            return;
        }
        
        // Configure picture settings
        _picture.SetHalign(Align.Center);
        _picture.SetValign(Align.Center);
        _picture.SetCanShrink(true);
        
        // Add picture to the box
        _mainBox.Append(_picture);
        
        // Optional: Add loading text below the image
        var loadingLabel = Label.New("Loading...");
        loadingLabel.SetHalign(Align.Center);
        loadingLabel.MarginTop = 20;
        _mainBox.Append(loadingLabel);
        
        // Set the box as the window's child
        SetChild(_mainBox);
        
        // Apply some styling
        ApplyStyling();
    }
    
    
    
    private void ApplyStyling()
    {
        // Add CSS styling for a more polished look
        var cssProvider = Gtk.CssProvider.New();
        cssProvider.LoadFromData("""
            window {
                background-color: #2d2d2d;
                color: #ffffff;
            }
            
            .splash-image {
                border-radius: 8px;
            }
            
            .loading-text {
                font-size: 14px;
                color: #cccccc;
            }
        """,0);
        
        var display = Gdk.Display.GetDefault();
        if (display != null)
        {
            Gtk.StyleContext.AddProviderForDisplay(display, cssProvider, 
                0);
        }
        
        // Add CSS classes to elements
        _picture?.AddCssClass("splash-image");
    }
    
    public void ShowSplash()
    {
        Present();
    }
    
    public void HideSplash()
    {
        Close();
    }
}
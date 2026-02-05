using System;
using Gio;
using Gtk;
using System.Globalization;
using System.Threading.Tasks;

namespace Gaucho;

 public partial class fMain
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
                FileOpen();
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

    public  async Task<bool> FileOpen(string sFile = "")
    {

        if ( sFile == "" )
        {
            // Create file chooser dialog
            using Gtk.FileDialog fileDialog = FileDialog.New();
            fileDialog.Title = "Open CAD File";
            
            // Create file filters
            var fileFilters = Gio.ListStore.New(FileFilter.GetGType());
            
            // Filter for CAD files
            var cadFilter = FileFilter.New();
            cadFilter.Name = "CAD Files (*.dwg, *.dxf)";
            cadFilter.AddPattern("*.dwg");
            cadFilter.AddPattern("*.dxf");
            cadFilter.AddPattern("*.DWG");
            cadFilter.AddPattern("*.DXF");
            fileFilters.Append(cadFilter);
            
            // Filter for DWG files
            var dwgFilter = FileFilter.New();
            dwgFilter.Name = "AutoCAD Drawing (*.dwg)";
            dwgFilter.AddPattern("*.dwg");
            dwgFilter.AddPattern("*.DWG");
            fileFilters.Append(dwgFilter);
            
            // Filter for DXF files
            var dxfFilter = FileFilter.New();
            dxfFilter.Name = "Drawing Exchange Format (*.dxf)";
            dxfFilter.AddPattern("*.dxf");
            dxfFilter.AddPattern("*.DXF");
            fileFilters.Append(dxfFilter);
            
            // Filter for all files
            var allFilter = FileFilter.New();
            allFilter.Name = "All Files";
            allFilter.AddPattern("*");
            fileFilters.Append(allFilter);
            
            fileDialog.SetFilters(fileFilters);
            fileDialog.SetDefaultFilter(cadFilter);
            fileDialog.Modal = true;

            // try
            // {
                // Present the dialog with this window as parent
                Gio.File file = await fileDialog.OpenAsync(this);
                // For now, we'll handle this synchronously - in a real app you'd want to make this method async
                if (file is null)
			        return false;

                    
                            sFile = file.GetPath() ?? "";
                            if (!string.IsNullOrEmpty(sFile))
                                {
                                    // Continue with file processing
                                    ProcessSelectedFile(sFile);
                                    return true;
                                }
                            
                    
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"Error opening file dialog: {ex.Message}");
            // }
            
            return false;
        } else {
        
        ProcessSelectedFile(sFile);
        return true;
            }
    }
    
    private static void ProcessSelectedFile(string sFile)
    {
        Console.WriteLine($"Open Selected file: {sFile}");
        
        string fDxf = "";
    
        // Check file extension and handle accordingly
        string extension = System.IO.Path.GetExtension(sFile).ToLower();
        switch (extension)
        {
            case ".dwg":
                // Handle DWG conversion
                LoadDXF(sFile);
                break;
            case ".dxf":
                fDxf = sFile;
                LoadDXF(fDxf);
                break;
        }

        Console.WriteLine($"Openned extension: {extension}");

        //main.FillWindowMenu
        // fmain.tabDrawings.Count += 1
        // fmain.tabDrawings.Text = Left(f, 12)

        // TODO: Uncomment when dependencies are available
        /*
        Gcd.debugInfo("Showing drawing", "", "", true);
        Gcd.DrawingReady = true;
        // fmain.GLArea1.Enabled = true;
         // this is what we are doing now
        Gcd.clsJob = Gcd.Tools["SELECTION"];
        Gcd.clsJob.Start();
        Gcd.clsJobPrevious = Gcd.Tools["SELECTION"];
        Gcd.clsJobPreZoom = Gcd.Tools["SELECTION"];
        Gcd.clsJobPreviousParam = 0;

        AddFilesOpen(f);

        Gcd.Redraw();

        Gcd.debugInfo("File not found", "", "", true);
        
        Config.FilesLastPath = Util.PathFromFile(f);

        // Application.Busy--; // or however busy counter is managed
        */
    }

    private static void LoadDXF(string fDxf)
    {
        
        // Application.Busy++; // or however busy counter is managed
        //NewDrawing = Gcd.NewDrawing(f)

        var drawing = new Drawing();

        // esto es necesario porque muchas funciones usan Gcd.Drawing
         Gcd.Drawing = drawing;
        Gcd.LoadCommon();  // esto va antes de abrir el DXF o dara error cargando inserts
        // el problema es que los common no tienen layer
         Gcd.debugInfo("Leidos los bloques comunes");
        if ( Dxf.LoadFile(fDxf, drawing, false, false, false, 4, true) )
        {
            // var errorDialog =  AlertDialog.New("");
            // errorDialog.Message = "There was an error opening the file";
            // errorDialog.ShowAsync(null);
            // drawing = null;
            Gcd.debugInfo("Error en la lectura del DXF", false, false, true, true);
            Gcd.Drawing = Gcd.Drawings.Values.Last();
            return;
        }
        Gcd.debugInfo("DXF read", false, false, true, true);
        // TODO: Add proper implementation when dependencies are available
        /*
        // If NewDrawing.Headers.FINGERPRINTGUID = "" Then
        //     NewDrawing.Headers.FINGERPRINTGUID = Gcd.UniqueId()
        // Endif
        Gcd.Drawings.Add(drawing, Gcd.UniqueId());

        Gcd.Drawing.FileName = f;
        Gcd.Drawing.RequiresFileRename = false;

        try 
        {
            Gcd.Drawing.HandSeed = Convert.ToInt32(Gcd.Drawing.Headers.HandSeed, 16);
        }
        catch
        {
            // Handle conversion error if needed
        }

        fmain.UpdateLayersCombo();
        fmain.UpdateLineWtCombo();
        fmain.UpdateLineTypeCombo();
        fmain.UpdatDimTypeTypeCombo();

        // veo si tengo que cerrar la pestaña de nuevo
        Drawing dTest = null;
        dTest = fmain.tabDrawings[fmain.tabDrawings.Index].Tag;
        if ( dTest != null )
        {
            if ( dTest.RequiresFileRename && ! dTest.RequiresSaving ) //estoy sobre una pestaña <new> donde no hice nada, la cierro
            {
                Gcd.Drawings.Remove(dTest.id);
                // System.Threading.Thread.Sleep(0); // Wait equivalent
                fmain.tabDrawings_Close(fmain.tabDrawings.Index);
            }
        }
        Gcd.Drawing = drawing;
        Gcd.Drawing.Sheet = Gcd.Drawing.Model;
        // aca verifico que tenga objetos 3D
        if ( Gcd.Drawing.has3dentities )
        {
            s = new Sheet();
            s.Name = "Model3D";
            s.Is3D = true;
            Gcd.Drawing.Sheets.Add(s, s.Name);
            s.model3d = Gcd.Drawing.Sheets["Model"].model3d;
        }

        fmain.TabForDrawing(drawing); //FIXME: 2025 no anda esto
        // System.Threading.Thread.Sleep(1); // Wait equivalent
        Gcd.DrawingReady = true;
        // por ahora mostramos el dibujo centrado, hasta q leamos el estado anterior
        Gcd.Regen();
        Gcd.PanToOrigin(); // FIXME: reponer cuando este el resto arreglado

        cadZoomE.Start();

        // For Each s As Sheet In NewDrawing.Sheets
        //     Gcd.Drawing.Sheet = s
        //     cadZoomE.Start()
        // Next
        Gcd.debugInfo("Graphics regenerated", "", "", true, true);
        fmain.Text = f;
        */

        return;
    }
}

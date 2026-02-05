using Gaucho;
public class Layout
{
 // Gambas class file

 // 0 (LAYOUT)
 //
 // 5 Handle
 //
 // 102 Start of persistent reactors group; always “{ACAD_REACTORS”
 //
 // 330 Soft-pointer ID/handle to owner dictionary
 //
 // 102 End of persistent reactors group, always “}”
 //
 // 330 Soft-pointer ID/handle to owner object
 //
 // 100 Subclass marker (AcDbPlotSettings)
 //
 // plotsettings object group codes
 //
 // For group codes and descriptions following the AcDbPlotSettings marker, see PLOTSETTINGS
 //
 // 100 Subclass marker (AcDbLayout)
 //
 // 1 Layout name
 //
 // 70 Flag (bit-coded) to control the following: 1 = Indicates the PSLTSCALE value for this layout when this layout is current 2 = Indicates the LIMCHECK value for this layout when this layout is current
 //
 // 71 Tab order. This number is an ordinal indicating this layout//s ordering in the tab control that is attached to the AutoCAD drawing frame window. Note that the “Model” tab always appears as the first tab regardless of its tab order
 //
 // 10/20 Minimum limits for this layout (defined by LIMMIN while this layout is current)
 //
 // 11/21 Maximum limits for this layout (defined by LIMMAX while this layout is current):
 //
 // 12/22/32 Insertion base point for this layout (defined by INSBASE while this layout is current):
 //
 // 14/24/34 Minimum extents for this layout (defined by EXTMIN while this layout is current):
 //
 // 15/25/35 Maximum extents for this layout (defined by EXTMAX while this layout is current):
 //
 // 146 Elevation
 //
 // 13/23/33 UCS origin
 //
 // 16/26/36 UCS X-axis
 //
 // 17/27/37 UCS Y axis
 //
 // 76 Orthographic type of UCS 0 = UCS is not orthographic 1 = Top; 2 = Bottom  3 = Front; 4 = Back 5 = Left; 6 = Right
 //
 // 330 ID/handle to this layout//s associated paper space block table record
 //
 // 331 ID/handle to the viewport that was last active in this layout when the layout was current
 //
 // 345 ID/handle of AcDbUCSTableRecord if UCS is a named UCS. If not present, then UCS is unnamed
 //
 // 346 ID/handle of AcDbUCSTableRecord of base UCS if UCS is orthographic (76 code is non-zero). If not present and 76 code is non-zero, then base UCS is taken to be WORLD
 //
 // 333 Shade plot ID


public static bool ImportDxf(Sheet s, Dictionary<string,string> cData)
    {


    int i ;         
    PrintStyle p ;         
    string r ;         

    p = s.pPrintStyle;

    s.id = Dxf.ReadCodeFromCol(cData, 5); // handle

    r = Dxf.ReadCodeFromCol(cData, 100, true); // Inicio de PlotSettings
    if ( r != "AcDbPlotSettings" )
    {
        Gcd.debugInfo("Bad LAYOUT data",false,false, true);
        return false;
    }

    p.Name = Dxf.ReadCodeFromCol(cData, 1, true); // name
    p.PrinterName = Dxf.ReadCodeFromCol(cData, 2, true); // Printer name
    p.PaperName = Dxf.ReadCodeFromCol(cData, 4, true); // hoja
    p.ViewName = Dxf.ReadCodeFromCol(cData, 6, true); // nombre que se ve View

    p.MarginLeft = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 40, true)); // margenes
    p.MarginBottom = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 41, true));
    p.MarginRigth = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 42, true));
    p.MarginTop = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 43, true));

    p.PaperSizeW = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 44, true)); // tamaño del papel
    p.PaperSizeH = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 45, true));

    p.PrintOffsetX = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 46, true)); // offset
    p.PrintOffsetY = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 47, true));

    p.PrintAreaX0 = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 48, true));
    p.PrintAreaY0 = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 49, true));
    p.PrintAreaX1 = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 140, true));
    p.PrintAreaY1 = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 141, true));
    p.ScaleDrawingUnit = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 142, true)); // escala
    p.ScalePaper = Gb.CDbl(Dxf.ReadCodeFromCol(cData, 143, true));

    r = Dxf.ReadCodeFromCol(cData, 70, true); // flags
    r = Dxf.ReadCodeFromCol(cData, 72, true); // unidad, que ignoro porque uso mm
    r = Dxf.ReadCodeFromCol(cData, 73, true); // rotacion
    p.PrintArea = Gb.CInt(Dxf.ReadCodeFromCol(cData, 74, true)); // area de impresion
    r = Dxf.ReadCodeFromCol(cData, 7, true); // Style Sheet
    r = Dxf.ReadCodeFromCol(cData, 75, true); // scale type , es la escala en texto
    r = Dxf.ReadCodeFromCol(cData, 76, true); // shade plot mode
    r = Dxf.ReadCodeFromCol(cData, 77, true); // shade plot resolution
    r = Dxf.ReadCodeFromCol(cData, 78, true); // dpi
    r = Dxf.ReadCodeFromCol(cData, 147, true); // realunits/paper, es la escala en un float
    r = Dxf.ReadCodeFromCol(cData, 148, true); // paper image x
    r = Dxf.ReadCodeFromCol(cData, 149, true); // paper image x

     // busco el marcador
    i = Dxf.GoToCodeFromCol(cData, 100, "AcDbLayout");

    if ( i == 0 )
    {
        Gcd.debugInfo("Bad LAYOUT data",false, true, true);
        return false;
    }
    s.Name = Dxf.ReadCodeFromCol(cData, 1, true); // name

    r = Dxf.ReadCodeFromCol(cData, 70, true); // flags
    s.TabOrder = Gb.CInt(Dxf.ReadCodeFromCol(cData, 71, true)); // orden de la tabulacion

    return true;

     // catch

    return false;

}

public static bool ExportDxf(Drawing drw)
    {


    int i ;         
    PrintStyle p ;         
    string r = "";         
    Sheet s ;         

    foreach ( var s2 in drw.Sheets)
    {
s=s2.Value;
        p = s.pPrintStyle;

        Dxf.SaveCode(0, "LAYOUT");

        Dxf.SaveCode(5, s.id);
        // fixme
        // Dxf.SaveCode(330, drw.Dictionary.Definitions["ACAD_LAYOUT"].id);
        Dxf.SaveCode(100, "AcDbPlotSettings");
        Dxf.SaveCode(1, p.Name); // name
        Dxf.SaveCode(2, p.PrinterName); // Printer name
        Dxf.SaveCode(4, p.PaperName); // hoja

        Dxf.SaveCode(40, p.MarginLeft); // margenes
        Dxf.SaveCode(41, p.MarginBottom);
        Dxf.SaveCode(42, p.MarginRigth);
        Dxf.SaveCode(43, p.MarginTop);

        Dxf.SaveCode(44, p.PaperSizeW); // tamaño del papel
        Dxf.SaveCode(45, p.PaperSizeH);

        Dxf.SaveCode(46, p.PrintOffsetX); // offset
        Dxf.SaveCode(47, p.PrintOffsetY);

        Dxf.SaveCode(48, p.PrintAreaX0);
        Dxf.SaveCode(49, p.PrintAreaY0);
        Dxf.SaveCode(140, p.PrintAreaX1);
        Dxf.SaveCode(141, p.PrintAreaY1);

        Dxf.SaveCode(142, p.ScaleDrawingUnit); // escala
        Dxf.SaveCode(143, p.ScalePaper);

        Dxf.SaveCode(70, r); // flags
        Dxf.SaveCode(72, r); // unidad, que ignoro porque uso mm
        Dxf.SaveCode(73, r); // rotacion
        Dxf.SaveCode(74, p.PrintArea); // area de impresion
        Dxf.SaveCode(7, r); // Style Sheet
        Dxf.SaveCode(75, r); // scale type , es la escala en texto
        Dxf.SaveCode(76, r); // shade plot mode
        Dxf.SaveCode(77, r); // shade plot resolution
        Dxf.SaveCode(78, r); // dpi
        Dxf.SaveCode(147, r); // realunits/paper, es la escala en un float
        Dxf.SaveCode(148, r); // paper image x
        Dxf.SaveCode(149, r); // paper image x

        Dxf.SaveCode(100, "AcDbLayout");
        Dxf.SaveCode(1, s.Name); // name

        Dxf.SaveCode(70, r); // flags
        Dxf.SaveCode(71, s.TabOrder); // orden de la tabulacion

    }
    return true;

     // catch

    return false;

}

}
using Gaucho;
using GLib;
class cadMText: EntityBase, IEntity
{
 // Gambas class file

 // GambasCAD
 // A simple CAD made in Gambas
 //
 // Copyright (C) Ing Martin P Cristia
 //
 // This program is free software; you can redistribute it and/or modify
 // it under the terms of the GNU General Public License as published by
 // the Free Software Foundation; either version 3 of the License, or
 // (at your option) any later version.
 //
 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General Public License for more details.
 //
 // You should have received a copy of the GNU General Public License
 // along with this program; if not, write to the Free Software
 // Foundation, Inc., 51 Franklin St, Fifth Floor,
 // Boston, MA  02110-1301  USA
 // Fast
public const string USEWITH = "MTEXTBUILDER";
public const string Gender = "MTEXT";
public const int DrawingOrder = 100;  // 1 a 32.724 esto debe Drawse primero y ser sobreescrito por el resto
public const int PointsToDraw = 1;              // minimal point requered for drawing something usefull
public const string CmdLineHelper = ("Inserts a multiline text");
public const string ParamType = "PAFT";
public const string ParamHelper = "Start point;Angle;Heigth;";      // a little text that is shown at the prompt waiting for user input during build
public const string ParamDefault = " ; 0 ; 10 ; ";
public const bool Regenerable = true;
public static double TextAngle ;         

public  string[] TextLines ;         
public  static List<double>  TextHeigth ;         
public  static List<double> TextPosX ;         
public  static List<double> TextBoxWidth ;         
public  static List<double> TextPosY ;         
public  static List<double> TextBoxHeight ;         
public  static List<int> TextAlignment ;         
public  static List<string> TextFont ;         
public  static List<bool> TextBold ;         
public static string CurrentTextLines ;         
public static double CurrentTextHeigth ;         
public static double OriginalTextHeigth ;         
public static double CurrentTextPosX ;         
public static double OriginalTextPosX ;         
public static double OriginalTextPosY ;         
public static double CurrentTextBoxWidth ;         
public static double CurrentTextPosY ;         
public static double CurrentTextBoxHeight ;         
public static int CurrentTextAlignment ;          // horizontal alignment 0 left/1 center/2 right
public static int CurrentTextAlignmentVertical ;          //
public static int CurrentParagraphAlignment ;          //
public static double CurrentTextRelScaleX = 1;

public static string CurrentTextFont ;         
public static bool CurrentTextBold ;         
public static double CurrentTextItalicAngle ;         
public static bool CurrentTextCrossed ;         
public static bool CurrentTextUnderline ;         
public static bool CurrentTextOverline ;         

public static double CurrentTextAngle ;         
public static int CurrentTextColor ;         

public static int Lines ;         
public static bool Line ;         
public static bool Trimmed ;         
public static bool TrimText ;         

public static int LastTextLengh ;         

 // to create the contour

public static List<double> flxText ;         

public static List<TextStyle>  TxtStyles ;          // creados a partir de {}
public static TextStyle  TxtStyle ;          // el que estoy usando

public static bool esperar ;         

 // fParam helpers
public const int TotalParams = 13;
public const int ipaTextHeight = 0;
public const int ipaTextAngle = 1;
public const int ipaGenerationFlags = 2;
public const int ipaHorizJustif = 3;
public const int ipaVertJustif = 4;
public const int ipaRelativeFactor = 5;
public const int ipaHorizAngle = 6;
public const int ipaTextVisibility = 7;
public const int ipaAttchmPoint = 8;
public const int ipaRectangleWidth = 9;
public const int ipaDrawingDirec = 10;
public const int ipaBackFillType = 11;
public const int ipaBackColor = 12;

 // stringdata helpers
public const int sdaTotalParams = 2;
public const int sdaText = 0;
public const int sdaStyle = 1;
public const int TotalPoints = 1;  // 2021 cambie esto

public static int PrintingType ;         

public static List<double> MyPolygon ;         

 // The entity handler receives a user action, and returns the number of expected parameter
 // If definitive = true, means the parameter is set
public static bool Parameter(Entity eBuild, List<string> vParam, bool Definitive= false)
    {


    double f =0.0   ;         
    int ip =0 ;         

    if ( Gcd.StepsDone == 0 ) //  posicion
    {

        if ( vParam[0] != "point" ) return false;

         eBuild.P[0] = Gb.CDbl(vParam[1]);
         eBuild.P[1] = Gb.CDbl(vParam[2]);

        if ( Definitive ) return true;

    } // angulo del texto
    else if ( Gcd.StepsDone == 1 )
    {

        if ( vParam[0] != "float" ) return false;

        eBuild.fParam[ipaTextAngle] = Gb.CDbl(vParam[1]);
        eBuild.fParam[ipaAttchmPoint] = 0;
        eBuild.fParam[ipaDrawingDirec] = 0;

        if ( Definitive ) return true;

    } // altura
    else if ( Gcd.StepsDone == 2 )
    {

        if ( vParam[0] != "float" ) return false;

        eBuild.fParam[ipaTextHeight] = Gb.CDbl(vParam[1]);

        if ( Definitive ) return true;

    } // texto
    else if ( Gcd.StepsDone == 3 )
    {

        if ( vParam[0] != "text" ) return false;

        eBuild.sParam[sdaText] = vParam[1];

        if ( Definitive ) return true;

    }
        return false;
    }



public static void Translate(Entity e, double dx, double dy, bool OnlySelected= false)
    {


    int i ;         

    if ( OnlySelected )
    {

        for ( i = 0; i <= e.Psel.Count; i += 1)
        {
            if ( e.Psel[i] )
            {

                e.P[i * 2] += dx;
                e.P[i * 2 + 1] += dy;

            }
        }

    } else {

        Puntos.Translate(e.P, dx, dy);
    }
    Puntos.Translate(e.PolyLine, dx, dy);
    Puntos.Translate(e.Polygon, dx, dy);
     //depre BuildPOI(e)

}

public static void Rotate(Entity e, double radians)
    {


    Puntos.Rotate(e.P, radians);

     // el parametro lo tengo que pasar a grados porque la rutina que dibuja el texto usa eso
    e.fParam[ipaTextAngle] += radians;
    Puntos.Rotate(e.PolyLine, radians);
    Puntos.Rotate(e.Polygon, radians);

}

public static void Scale(Entity e, double sx, double sy)
    {


             

    e.fParam[ipaTextHeight] *= sy;
    Puntos.Scale(e.P, sx, sy);
    Puntos.Scale(e.PolyLine, sx, sy);
    Puntos.Scale(e.Polygon, sx, sy);

}

 // A fat version of the line
public static void DrawSelected(Entity oE)
    {


    Glx.DrawLines(oE.PolyLine, Colors.Gradient(Gcd.GetGBColor(oE.Colour, oE.pLayer), Config.ModelBackgroundColor), Gcd.GetLineWt(oE.LineWidth, oE.pLayer));

}

 // A fat version of the line
public static void DrawRemark(Entity oE)
    {


    if ( ! oE.Visible ) return;

    Glx.DrawLines(oE.PolyLine, Colors.Gradient(Gcd.GetGBColor(oE.Colour, oE.pLayer), Config.WhiteAndBlack), Gcd.GetLineWt(oE.LineWidth, oE.pLayer));

}

public static void Draw2(Entity oE)
    {


    // paintPlus.Lines(oE.PolyLine);

}

 // // Creo los Puntos de interes
 // // Build point of interest
 //
 // Public Function BuildPOI(oE As Entity) As Integer
 //
 //     // points
 //
 //     oE.PoiPoints.Clear
 //     oE.PoiType.Clear
 //
 //     oE.poiPoints.AddRange([oe.P[0], oe.P[1]])
 //     oE.poiType.Add(Gcd.poiBasePoint)
 //     //oE.poiEntities.Add(arrIndex)
 //
 // End

public static void Draw(Entity oE)
    {


    if ( ! oE.Visible ) return;

    Glx.DrawLines(oE.PolyLine, Gcd.GetGBColor(oE.Colour, oE.pLayer), Gcd.GetLineWt(oE.LineWidth, oE.pLayer));

}

 // Builds the geometry of the entity wthout drwing it

public static void makepolyline(Entity oE)
    {


    flxText.Clear(); // limpio los glyphs

     //If oE.id = "1B4571" Then Stop
    TextAngle = oE.fParam[ipaTextAngle];
     // voy a procesar texto
    OriginalTextPosX = oE.P[0];
    OriginalTextPosY = oE.P[1];

     // // para chequear los aligments
     // gl.PointSize(3)
     // gl.Begin(gl.POINTS)
     // gl.Vertex2f(oe.P[0], oe.P[1])
     // gl.End

    CurrentTextHeigth = oE.fParam[ipaTextHeight];
    OriginalTextHeigth = CurrentTextHeigth;
    CurrentTextBoxWidth = oE.fParam[ipaRectangleWidth];
    CurrentTextRelScaleX = 1;
    if ( CurrentTextHeigth == 0 ) CurrentTextHeigth = 1;
     //CurrentTextPosY -= CurrentTextHeigth
    switch ( oE.fParam[ipaAttchmPoint])
    {

        case 1: // Top/Left
             //CurrentTextPosY -= CurrentTextHeigth
            CurrentTextAlignment = 0;
            CurrentTextAlignmentVertical = 0; // top
            break;
        case 2: // Top/Center
             //CurrentTextPosY -= CurrentTextHeigth
            CurrentTextAlignment = 1;
            CurrentTextAlignmentVertical = 0; // top
            break;
        case 3: // Top/Right
             //CurrentTextPosY -= CurrentTextHeigth
            CurrentTextAlignment = 2;
            CurrentTextAlignmentVertical = 0; // top
            break;
        case 4: // Middle/Left
             //CurrentTextPosY -= CurrentTextHeigth / 2
            CurrentTextAlignment = 0;
            CurrentTextAlignmentVertical = 1; // mid
            break;
        case 5: // Middle/Center
             //CurrentTextPosY -= CurrentTextHeigth / 2
            CurrentTextAlignment = 1;
            CurrentTextAlignmentVertical = 1; // mid
            break;
        case 6: // Middle/Right
             //CurrentTextPosY -= CurrentTextHeigth / 2
            CurrentTextAlignment = 2;
            CurrentTextAlignmentVertical = 1; // mid
            break;
        case 7: // Bottom/Left
             //CurrentTextPosY -= 0
            CurrentTextAlignment = 0;
            CurrentTextAlignmentVertical = 2; // bottom
            break;
        case 8: // Bottom/Center
             //CurrentTextPosY -= 0
            CurrentTextAlignment = 1;
            CurrentTextAlignmentVertical = 2; // bottom
            break;
        case 9: // Bottom/Right
             //CurrentTextPosY -= 0
            CurrentTextAlignment = 2;
            CurrentTextAlignmentVertical = 2; // bottom
            break;

            default:    
            break;


    }

    CurrentTextAngle = oE.fParam[ipaTextAngle] * (180 / Math.PI); // lo paso a grados
    CurrentTextColor = Gcd.GetGBColor(oE.Colour, oE.pLayer);
    TrimText = true;

     // https://ezdxf.readthedocs.io/en/stable/tutorials/mtext.html
     // https://ezdxf.readthedocs.io/en/stable/dxfentities/mtext.html#mtext-inline-codes

     // MTEXT tiene in-line formatting, ver esa web

    TxtStyle = Gcd.Drawing.CurrTextStyle;
    if (  TxtStyle == null) TxtStyle = new TextStyle();
    TxtStyles.Add(TxtStyle);

    oE.pStyle = Gcd.Drawing.TextStyles[oE.sParam[sdaStyle]];
    if (  oE.pStyle == null) oE.pStyle = Gcd.Drawing.CurrTextStyle;
    TxtStyle = oE.pStyle;

    CurrentTextFont = Util.FileWithoutExtension(oE.pStyle.sFont_3);
     //If oE.id = "1B45BD" Then esperar = True Else esperar = False
    oE.Polygon.Clear();

    cadMText.ProcessText3(oE);

}
 // Public Function BuildGeometry(oE As Entity, Optional density As Float)
 //
 //     DeExtrude(oE)
 //     Limits(oE)
 //
 // End

public static void ProcessText3(Entity oE)
    {


     // voy a hacer una gran division
     // separo el texto a imprimir en lineas \P
     // busco lo que esta entre {}
    int p1 = 0;         
    int p2 =0;         
    int p3 =0;         
     List<string> sLines ;         
    string rtf = "";         
    // string sRemains ;         
    string sToProcess ;         
     List<double> fRect ;         
    double px =0.0 ;         
    double py =0.0 ;         
    double dx =0.0 ;         
    double dy =0.0 ;         
    double dy2 =0.0 ;         
    rtf = oE.sParam[sdaText];

    Trimmed = false;

    sLines = Util.SplitComplex(rtf, "\\P");
    var lines = 0;
     //IfGb.InStr(LCase(rtf), "pxqc;11") > 0 Then Stop

     //If sLines.Count > 1 Then Stop

     // con esto los glyps vienen hacia abajo
    CurrentTextPosY = 0; //-CurrentTextHeigth * 1.4
    CurrentParagraphAlignment = -1;

    foreach (var sRemains in sLines)
    {
         //Print sRemains
        ProcessPart3(sRemains);
        lines += 1;
    }

    if ( flxText.Count > 0 )
    {
        fRect = Puntos.Limits(flxText);

         //dy = Abs(fRect[3] - fRect[1])
        dy = fRect[3] - fRect[1];
         //dy = Lines * CurrentTextHeigth * 1.4 // la altura total del texto
        dy2 = dy / Lines;
        dx = fRect[2] - fRect[0];
         // // but
         // If Trimmed Then dy = CurrentTextHeigth * 1.2
        if ( CurrentTextAlignmentVertical == 2 ) py = OriginalTextPosY + dy - fRect[3]; // BOTTOM
        if ( CurrentTextAlignmentVertical == 1 ) py = OriginalTextPosY + dy / 2 - fRect[3]; // MID
        if ( CurrentTextAlignmentVertical == 0 ) py = OriginalTextPosY - fRect[3]; //* sLines.Count     // TOP

        if ( CurrentTextAlignment == 0 ) px = OriginalTextPosX;
         //px = OriginalTextPosX
         // TODO: ver cual de los dos es valido
         //If CurrentTextAlignment = 1 Then px = OriginalTextPosX + CurrentTextBoxWidth / 2 - dx / 2
        if ( CurrentTextAlignment == 1 ) px = OriginalTextPosX; //- dx / 2

         // TODO: ver cual de los dos es valido
         //If CurrentTextAlignment = 2 Then px = OriginalTextPosX + CurrentTextBoxWidth - dx
        if ( CurrentTextAlignment == 2 ) px = OriginalTextPosX; //- dx

        Puntos.Translate(flxText, px, py);

        fRect = Puntos.Limits(flxText);
    } else {

        fRect = new List<double> { OriginalTextPosX, OriginalTextPosY, OriginalTextPosX, OriginalTextPosY };
    }

     // Armo el polygono que lo rodea
    oE.Polygon.Clear();
    oE.Polygon.AddRange(new List<double> { fRect[0], fRect[1] });
    oE.Polygon.AddRange([fRect[0], fRect[3]]);
    oE.Polygon.AddRange([fRect[2], fRect[3]]);
    oE.Polygon.AddRange([fRect[2], fRect[1]]);
    oE.Polygon.AddRange([fRect[0], fRect[1]]);

    Puntos.RotatePointsFromBase(OriginalTextPosX, OriginalTextPosY, Gb.Rad(CurrentTextAngle), flxText);

    Puntos.RotatePointsFromBase(OriginalTextPosX, OriginalTextPosY, Gb.Rad(CurrentTextAngle), oE.Polygon);

     // FIXME:  esto falla porque la entidad esta translated al momento de dibujar
    oE.PolyLine.Clear();
    oE.PolyLine.AddRange(flxText.GetRange(0, flxText.Count));
    flxText.Clear();

     // Code   Description
     // \L   Start underline
     // \l   Stop underline
     // \O   Start overline
     // \o   Stop overline
     // \K   Start strike-through
     // \k   Stop strike-through
     // \P   New paragraph (new line)
     // \p   Paragraphs properties: indentation, alignment, tabulator stops
     // \pi \pxi \pxt    Control codes for bullets, numbered paragraphs, tab stops and columns - e.g. bullets: \pxi-3,l3,t3;, tab stops: \pxt10,t12;
     // \X    Paragraph wrap on the dimension line (only in dimensions)

     // \X   Paragraph wrap on the dimension line (only in dimensions)
     // \Q   Slanting (oblique) text by angle - e.g. \Q30;
     // \H   Text height - e.g. relative \H3x; absolut \H3;
     // \W   Text width - e.g. relative \W0.8x; absolut \W0.8;
     // \T   Tracking, character spacing - e.g. relative \T0.5x; absolut \T2;
     // \F   Font selection e.g. \Fgdt;o - GDT-tolerance
     // \S   Stacking, fractions e.g. \SA^ B; space after “^” is required to avoid caret decoding, \SX/Y; \S1#4;
     // \A   Alignment
     //
     //     \A0; = bottom
     //     \A1; = center
     //     \A2; = top
     //
     // \C   Color change
     //
     //     \C1; = red
     //     \C2; = yellow
     //     \C3; = green
     //     \C4; = cyan
     //     \C5; = blue
     //     \C6; = magenta
     //     \C7; = white
     //
     // \~   Non breaking space
     // {}   Braces - define the text area influenced by the code, codes and braces can be nested up to 8 levels deep
     // \   Escape character - e.g. \{ = “{”

}

 // procesa una linea de MText con todos sus modificadores
public static string ProcessPart3(string sLine)
    {


    string s2 = "";         
    string sFont = "";         
    string s3 = "";         
    string s4 = "";         
    string sTab = "";         
    string sPrint = "";         
    int p1 = 0;         
    int p2 = 0;         
    int p3 = 0;         
    int p4 = 0;         
    int p5 = 0;         
     List<double> fAlign = new List<double>();         
     List<double> fAlignR = new List<double>();         
     List<double> fRect = new List<double>();         

    double angulo = 0.0;         
     // PaintExtents hText = null;         
    string lText = "";         
    double FontScalePrev = 0.0;         
    int iSpace = 0;         
    string s5 = "";         
    List<string> stxWords = new List<string>();         
    int i4 = 0;         
    double dx = 0.0;         
    double dy = 0.0;         
    double LineOffsetX = 0.0;         
    bool SaltoHecho = false;         

     List<double> flxWords = new List<double>();         
     List<double> flxPAragraph = new List<double>();         
     List<double> flxTemp = new List<double>();         

    s2 = Util.RemoveUnicodes(sLine);

     //IfGb.InStr(s2, "por lado") > 0 Then Stop

     // reemplazo caracteres especiales
    s2 = Gb.Replace(s2, "%%D", "°", true);
    s2 = Gb.Replace(s2, "%%P", "±", true);
    s2 = Gb.Replace(s2, "%%C", "∅", true);
    if ( Gb.InStr(s2, "%%U") > 0 ) CurrentTextUnderline = true;
    s2 = Gb.Replace(s2, "%%U", "", true);

    do { // proceso la linea hasta que no me queden mas caracteres
        if ( Gb.Left(s2, 1) == "{" ) // voy a modificar los parametros del estilo de texto
        {
            TxtStyle = new TextStyle();
            TxtStyles.Add(TxtStyle);
            s2 = Gb.Mid(s2, 2);
        }

        if ( Gb.Left(s2, 1) == "}" ) // vuelvo al estilo anterior
        {

            CurrentTextHeigth = OriginalTextHeigth;
            if ( TxtStyles.Count > 1 )
            {
                TxtStyles.RemoveAt(TxtStyles.Count - 1);
                TxtStyle = TxtStyles.Last();
            }
            s2 =Gb.Mid(s2, 2);
        }

        if ( Gb.Left(s2, 1) == "\\" )
        {
            switch ( (Gb.Mid(s2, 2, 1)))
            {

                case "U": // ej \U+0045
                    p2 =Gb.InStr(s2, "\\", 1);
                    if ( p2 > 0 )
                    {
                        s2 =Gb.Mid(s2, p2 + 1);
                    } else {
                        s2 =Gb.Mid(s2, 8);
                    }
                     // es un caracter utf, no hago nada
                     break;

                case "f": // esta indicado un cambio en la fuente
                     //  \fVerdana|b0|i0|cxxx|p34;
                    p1 =Gb.InStr(s2, "|");
                    TxtStyle.FontName = Gb.Mid(s2, 3, p1 - 3);
                    CurrentTextFont = TxtStyle.FontName.ToLower();

                    p1 =Gb.InStr(s2, "|b");
                    if ( p1 > 0 )
                    {
                        CurrentTextBold = (Gb.Mid(s2, p1 + 2, 1) == "1");
                        TxtStyle.Bold = (Gb.Mid(s2, p1 + 2, 1) == "1");
                    }
                    p1 =Gb.InStr(s2, "|i");
                    if ( p1 > 0 )
                    {
                        CurrentTextItalicAngle = (Gb.Mid(s2, p1 + 2, 1) == "1") ? 15 : 0;
                        TxtStyle.Italic = (Gb.Mid(s2, p1 + 2, 1) == "1");
                    }

                    p1 =Gb.InStr(s2, "|c"); // codepage
                    if ( p1 > 0 )
                    {

                        p2 =Gb.InStr(s2, "|", p1 + 1);
                         //  CurrentTextColor = Gb.Mid(s2, p1 + 2, p2 - p1 - 1) = "1"
                    }
                    p1 = p2 + 1;

                    p1 =Gb.InStr(s2, "|p"); // paragraph
                    p2 =Gb.InStr(s2, ";", p1);
                    if ( p1 > 0 )
                    {

                         //CurrentTextHeigth = CFloat(Gb.Mid(s2, p1 + 2, p2 - p1 - 2)) / 100
                    }

                    s2 =Gb.Mid(s2, p2 + 1);

                    break;

                case "C": // cambio de color

                    p2 =Gb.InStr(s2, ";", 1);

                    TxtStyle.cadColor = Gcd.GetGBColor(Gb.CInt(Gb.Mid(s2, 3, p2 - 3)));

                    s2 =Gb.Mid(s2, p2 + 1);

                     // Case "\\P"            // New line
                     //     CurrentTextPosY -= Glx.TextExtends("XXX", CurrentTextHeigth)[1] * 1.2
                     //     CurrentTextPosX = OriginalTextPosX
                     //     NewLine = True
                     //     s2 =Gb.Mid(s2, 3)
                     //     If Len(s2) > 0 Then ProcessPart(s2)
                     break;

                case "p": // no se que significa esto, suele venir \p12.55;
                     // \pxqc = centered
                     // \pxqr = right
                     // \pxql = left
                    if ( Gb.Left(s2, 5) == "\\pxql" ) CurrentParagraphAlignment = 0;
                    if ( Gb.Left(s2, 5) == "\\pxqc" ) CurrentParagraphAlignment = 1;
                    if ( Gb.Left(s2, 5) == "\\pxqr" ) CurrentParagraphAlignment = 2;

                    p2 =Gb.InStr(s2, ";", 1);
                    s2 =Gb.Mid(s2, p2 + 1);

                     // Case "\\t", "^I"            // tab avanzo 4-texto anterior
                     //   // TODO: verificar sin Tab=4 espacios
                     //   If LastTextLengh > 4 Then sTab = "    " Else sTab = Space(4 - LastTextLengh)
                     //   s2 = stab &Gb.Mid(s2, 3)
                     //   If Len(s2) > 0 Then ProcessPart(s2)
                     //   LastTextLengh = 0
                     break;

                case "T": //Adjusts the space between characters. Valid values range from a minimum of .75 to 4 times the original spacing between characters.
                    p2 =Gb.InStr(s2, ";", 1);
                    s2 =Gb.Mid(s2, p2 + 1);
                    break;

                case "Q": //\Q angle; changes obliquing angle
                    s2 =Gb.Mid(s2, 3);
                    break;
                case "O": //\O changes to overline
                    s2 =Gb.Mid(s2, 3);
                    CurrentTextOverline = true;
                    break;
                case "o": //\O changes to overline
                    s2 =Gb.Mid(s2, 3);
                    CurrentTextOverline = false;
                    break;
                case "L": //\O changes to underline
                    s2 =Gb.Mid(s2, 3);
                    CurrentTextUnderline = true;

                    break;
                case "l": //changes to underline
                    s2 =Gb.Mid(s2, 3);
                    CurrentTextUnderline = false;
                    break;

                case "W": // \W value; changes width factor To produce wide text
                    p2 =Gb.InStr(s2, ";", 1);
                    CurrentTextPosX += Gb.CDbl(Gb.Mid(s2, 3, p2 - 3));
                    s2 =Gb.Mid(s2, p2 + 1);
                    break;
                case "S": // fracciones
                    p2 =Gb.InStr(s2, ";", 1);

                    s2 =Gb.Mid(s2, 3, p2 - 3) + " " +Gb.Mid(s2, p2 + 1);
                    break;
                case "A": // Sets the alignment value; valid values: 0, 1, 2(bottom, center, top)

                     // extraigo el texto puro para poder calcular bien el centrado
                     //CurrentTextAlignmentVertical = CInt(Mid(s2, 3, 1))
                    p2 =Gb.InStr(s2, ";", 1);
                    s2 =Gb.Mid(s2, p2 + 1);
                     //     If CurrentTextAlignmentVertical = 1 Then // es mid
                     //
                     //
                     //     fAlign = Glx.TextExtends(s2, CurrentTextHeigth)
                     //     //fAlign[0] *= -1
                     //     //fAlign[1] *= 1
                     //
                     //     Puntos.Rotate(fAlign, CurrentTextAngle / 180 * Pi)
                     //     //CurrentTextPosX -= fAlign[0] * 0.5
                     //
                     //     // alineo Y
                     //     CurrentTextPosY += fAlign[1] * 0.5
                     //
                     //     // pero , y si esta rotado el texto???
                     //
                     //     // Else
                     //     //   Stop
                     //
                     // End If
                    break;

                case "H": // cambia la altura del texto

                    p2 =Gb.InStr(s2, ";", 1);
                    s3 =Gb.Mid(s2, 3, p2 - 3);
                    if (Gb.InStr(s3, "x") > 0 ) // cambia x veces la altura actual
                    {
                        CurrentTextHeigth *= Gb.CDbl(Gb.Left(s3, -1));
                    } else { // es una altura absoluta}
                        CurrentTextHeigth = Gb.CDbl(s3);
                    }
                    s2 =Gb.Mid(s2, p2 + 1);
                    break;

                default: // codigo no imprementado, lo paso de largo;
                    p2 =Gb.InStr(s2, ";", 1);
                    if ( p2 > 0 )
                    {
                        s2 =Gb.Mid(s2, p2 + 1);
                    } else {
                        s2 =Gb.Mid(s2, 3);
                    }
                    break;
            }
        } else { // es texto

             // es un texto, pero puede terminar en un scape o cambio de TeextStyles
            p2 =Gb.InStr(s2, "\\");
            if ( p2 == 0 ) p2 = 1000; //Else Stop
            p3 =Gb.InStr(s2, "}");
            if ( p3 == 0 ) p3 = 1000; //Else Stop
            p4 =Gb.InStr(s2, "{");
            if ( p4 == 0 ) p4 = 1000;

            if ( p3 < p2 ) p2 = p3;
            if ( p4 < p2 ) p2 = p4;

            if ( p2 < 1000 )
            {

                s5 = Gb.Left(s2, p2 - 1); // s5 es el texto puro
                s2 =Gb.Mid(s2, p2); // s2 es el resto , a procesar
            } else {
                s5 = s2;
                s2 = "";
            }

             //
             // If p2 = 1 Then // es un escape desconocido
             //     p2 =Gb.InStr(s2, ";")
             //     If p2 = 0 Then
             //         s2 =Gb.Mid(s2, 3)
             //     Else
             //         s2 =Gb.Mid(s2, p2 + 1)
             //     End If
             //     If Len(s2) > 0 Then ProcessPart(s2)
             //
             // Else
             //IfGb.InStr(s5, "C(1)") > 0 Then Stop
            s5 = Util.ProcessTabs(s5);
            s5 = Gb.Trim(s5);
            if ( Gcd.FontReplacements.ContainsKey(CurrentTextFont) )
            {
                CurrentTextFont = Gcd.FontReplacements[CurrentTextFont];
            } else {
                CurrentTextFont = "romans";
            }

            Glx.SelectFont(CurrentTextFont);
            // if ( esperar ) Stop;
             // tengo que trimar?
            if ( TrimText )
            {
                stxWords = Gb.Split(s5, " ");
                int iWordsFit =0;         
                iWordsFit = stxWords.Count - 1;
                 // voy testeando a ver cuantas palabras entran
                do { // hasta que no tenga mas palabras
                    if ( stxWords.Count == 0 )
                    {
                        s5 = "";

                        break;
                    }
                     // armo la frase con las palabras

                    s4 = "";
                    for ( iSpace = 0; iSpace <= iWordsFit - 1; iSpace += 1)
                    {
                        s4 += stxWords[iSpace] + " ";
                    }
                    s4 += stxWords[iWordsFit];

                     //veo si entra
                    fRect = Glx.TextExtends(s4, CurrentTextHeigth,0, CurrentTextItalicAngle);
                     //bug fix para fRect nulo
                    if ( fRect == null ) fRect = new List<double> {0, 0, 0, 0};
                    if ( fRect.Count == 0 ) fRect = new List<double> {0, 0, 0, 0};
                    if ( (CurrentTextBoxWidth > (fRect[2] - fRect[0])) || (iWordsFit == 0) ) //  entra o no me queda otra opcion
                    {
                        s5 = s4; // s5 es el texto a trazar

                         // armo lo que me falta, pero primero verifico que me queden palabras
                        if ( iWordsFit < stxWords.Count - 1 )
                        {
                            Line = true; // me faltaron palabras
                            Trimmed = true;
                            s2 = "";
                            for ( iSpace = iWordsFit + 1; iSpace <= stxWords.Count - 1; iSpace += 1)
                            {

                                s2 += stxWords[iSpace] + " ";
                            }
                            s2 += stxWords[stxWords.Count - 1];

                        }

                        break;
                    } else {
                         // no entra, pruebo con menos palabras
                         iWordsFit--;

                    }

                } while ( true );

            }
            if ( s5 == "" ) break;
            SaltoHecho = false;

            flxTemp = Glx.DrawTextPoly(s5, CurrentTextHeigth, 0, CurrentTextItalicAngle, CurrentTextRelScaleX);
            fRect = Puntos.Limits(flxTemp);

            if ( CurrentTextUnderline )
            {
                flxTemp.AddRange(new List<double> {fRect[0], fRect[1] - CurrentTextHeigth * 0.2, fRect[2], fRect[1] - CurrentTextHeigth * 0.2});
            }

            if ( CurrentTextOverline )
            {
                flxTemp.AddRange(new List<double> {fRect[0], fRect[3] + CurrentTextHeigth * 0.2, fRect[2], fRect[3] + CurrentTextHeigth * 0.2});
            }

            if ( CurrentTextCrossed )
            {
                flxTemp.AddRange(new List<double> {fRect[0], (fRect[3] + fRect[1]) / 2, fRect[2], (fRect[3] + fRect[1]) / 2});
            }

            flxWords.AddRange(flxTemp);
             //Puntos.Translate(flxWords, LineOffsetX, CurrentTextPosY)

             //If NewLine Or trimmed Or s2 = "" Or s2 = "}" Then         // necesito una nueva linea porque tuve que ajustar el texto
            fRect = Puntos.Limits(flxWords);
            if ( CurrentParagraphAlignment == -1 ) // no specification
            {
                if ( CurrentTextAlignment == 0 )
                {
                    dx = 0;
                }
                else if ( CurrentTextAlignment == 1 )
                {
                    dx = -(fRect[2] - fRect[0]) / 2;
                }
                else if ( CurrentTextAlignment == 2 )
                {
                    dx = -(fRect[2] - fRect[0]);
                }
            }
            if ( CurrentParagraphAlignment == 0 )
            {
                if ( CurrentTextAlignment == 0 )
                {
                    dx = 0;
                }
                else if ( CurrentTextAlignment == 1 )
                {
                    dx = -CurrentTextBoxWidth / 2;
                }
                else if ( CurrentTextAlignment == 2 )
                {
                    dx = -CurrentTextBoxWidth;
                }
            }
            if ( CurrentParagraphAlignment == 1 )
            {
                if ( CurrentTextAlignment == 0 )
                {
                    dx = CurrentTextBoxWidth / 2 - (fRect[2] - fRect[0]) / 2;
                }
                else if ( CurrentTextAlignment == 1 )
                {
                    dx = -(fRect[2] - fRect[0]) / 2;
                }
                else if ( CurrentTextAlignment == 2 )
                {
                    dx = -CurrentTextBoxWidth / 2 - (fRect[2] - fRect[0]) / 2;
                }
            }
            if ( CurrentParagraphAlignment == 2 )
            {
                if ( CurrentTextAlignment == 0 )
                {
                    dx = CurrentTextBoxWidth - (fRect[2] - fRect[0]);
                }
                else if ( CurrentTextAlignment == 1 )
                {
                    dx = CurrentTextBoxWidth / 2 - (fRect[2] - fRect[0]);
                }
                else if ( CurrentTextAlignment == 2 )
                {
                    dx = -(fRect[2] - fRect[0]);
                }
            }

             //Endif
            Puntos.Translate(flxWords, dx + LineOffsetX, CurrentTextPosY);
            LineOffsetX += fRect[2];
            flxPAragraph.AddRange(flxWords);
            flxWords.Clear();

            if ( Line ) // necesito una nueva linea porque tuve que ajustar el texto
            {
                CurrentTextPosY -= CurrentTextHeigth * 1.4;
                SaltoHecho = true;
                Lines += 1;
                LineOffsetX = 0;

            }

        }

    } while ( s2 != "" );
     // alinemaiento produce problemas porque se refiere a todo el texto y no a la parte

    flxText.AddRange(flxPAragraph);
    if ( ! SaltoHecho ) CurrentTextPosY -= CurrentTextHeigth * 1.4;

    return "";

}

public static bool SaveDxfData(Entity e)
    {


    int i ;         
    string sText ;         

     // stxExport.AddRange(["MTEXT", dxf.codEntity])
     // Los datos comunes a todas las entidades son guardados por la rutina que llama a esta
    dxf.SaveCodeInv("AcDbMText", "100");

    dxf.SaveCodeInv((e.P[0]), dxf.codX0); // insertion point
    dxf.SaveCodeInv((e.P[1]), dxf.codY0);
    dxf.SaveCodeInv("0", dxf.codZ0);

    dxf.SaveCodeInv((e.fParam[ipaTextHeight]), "40"); // heigth
    dxf.SaveCodeInv((e.fParam[ipaRectangleWidth]), "41"); // Rectang width
    dxf.SaveCodeInv(e.fParam[ipaAttchmPoint], "71"); // attchm point
    dxf.SaveCodeInv(e.fParam[ipaDrawingDirec], "72"); // drawing direction

    if ( e.sParam[0].Length > 250 )
    {
        dxf.SaveCodeInv(Gb.Left(e.sParam[sdaText], 250), "1"); // Texti = 250
        i = 251; // next offset
        do {

            if ( e.sParam[0].Length - i > 250 )
            {
                sText =Gb.Mid(e.sParam[0], i, 250);
                dxf.SaveCodeInv(sText, "3"); // Texti += 250
                i += 250;
            } else {
                sText =Gb.Mid(e.sParam[0], i);
                dxf.SaveCodeInv(sText, "3"); // Texti += 250
                break;

            }

        }while ( true );
    } else {
        dxf.SaveCodeInv(e.sParam[sdaText], "1"); // Text
    }

    dxf.SaveCodeInv(e.sParam[sdaStyle], "7"); // text style
    if ( e.Extrusion[2] != 1 )
    {
        dxf.SaveCodeInv((e.Extrusion[0]), "210");
        dxf.SaveCodeInv((e.Extrusion[1]), "220");
        dxf.SaveCodeInv((e.Extrusion[2]), "230");
    }

    dxf.SaveCodeInv((e.fParam[ipaTextAngle]), "50"); // rotation in radians

    dxf.SaveCodeInv((e.fParam[ipaBackFillType]), "90"); // Background fill style

    dxf.SaveCodeInv((e.fParam[ipaBackColor]), "63"); // Background color

    dxf.SaveCodeInv("0", "75"); // Column type
    dxf.SaveCodeInv("0", "76"); // Column count
    dxf.SaveCodeInv("0", "78"); // Column Flow Reversed
    dxf.SaveCodeInv("0", "79"); // Column Autoheight
    dxf.SaveCodeInv("0", "48"); // Column width
    dxf.SaveCodeInv("0", "49"); // Column gutter
    dxf.SaveCodeInv("0", "50"); // Column heights

    return true;

}

public static bool ImportDXF(Entity e, List<string> sClaves, List<string> sValues)
    {


    int i  =0;         
    bool TextHeigthSet = false;
    bool TextAngleSet = false;
    double ax = 0;          // angulo del texto
    double ay = 0;         
    double az = 0;         
    string s = "";         

     // Leo los datos comunes a todas las entidades

    e.P[0] = Gb.CDbl(dxf.ReadCodePlusNext(10, sClaves, sValues, 0, 101, 100));
    e.P[1] = Gb.CDbl(dxf.ReadCodePlusNext(20, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaTextHeight] = Gb.CDbl(dxf.ReadCodePlusNext(40, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaRectangleWidth] = Gb.CDbl(dxf.ReadCodePlusNext(41, sClaves, sValues, 0, 101, 100));

    e.fParam[ipaAttchmPoint] = Gb.CDbl(dxf.ReadCodePlusNext(71, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaDrawingDirec] = Gb.CDbl(dxf.ReadCodePlusNext(72, sClaves, sValues, 0, 101, 100));
    e.sParam[sdaText] = dxf.ReadCodePlusNext(1, sClaves, sValues, 0, 101, 100);
    do {
        i = dxf.ReadCodePlus(3, sClaves, sValues, ref s,0, 101, i);
        if ( s == "" ) break;
        e.sParam[sdaText] += s;
    }while ( true );
    e.sParam[sdaStyle] = dxf.ReadCodePlusNext(7, sClaves, sValues,0, 101, 100);
    ax = Gb.CDbl(dxf.ReadCodePlusNext(11, sClaves, sValues, 0, 101, 100));
    ay = Gb.CDbl(dxf.ReadCodePlusNext(21, sClaves, sValues, 0, 101, 100));
    az = Gb.CDbl(dxf.ReadCodePlusNext(31, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaTextAngle] = Gb.CDbl(dxf.ReadCodePlusNext(50, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaBackFillType] = Gb.CDbl(dxf.ReadCodePlusNext(90, sClaves, sValues, 0, 101, 100));
    e.fParam[ipaBackColor] = Gb.CDbl(dxf.ReadCodePlusNext(63, sClaves, sValues, 0, 101, 100));

    if ( (ax != 0) || (ay != 0) || (az != 0) )
    {

        e.fParam[ipaTextAngle] = Gb.Ang(ax, ay);

    }
    e.sParam[sdaStyle] = (e.sParam[sdaStyle]).ToLower();
    if ( e.sParam[sdaStyle] == "" ) e.sParam[sdaStyle] = "standard";
     //If e.id = "1B45BD" Then Stop

    return true;

}

 // Genera un conjunto de grips para ser usados por cadSelection
public static int GenerateGrips(Entity e)
    {


    int i =0;         
    // Grip g ;         
    int iCount =0 ;         
    double OffsetWidth ;         
    double OffsetHeigth ;         
    double h = 0.0;         
     List<double> flxGrip = new List<double>();         

    var g = new Grip();
    g.Shape = 0;
    g.X = e.P[0];
    g.Y = e.P[1];
    g.Action = 0; // mover
    g.AsociatedEntity = e;
    g.AsociatedPoint = -1;
    Gcd.Drawing.Sheet.Grips.Add(g);
    g.ToolTip = ("Move base");

     // GRIP PARA EL ANCHO DEL AREA DEL TEXTO
     // tengo que ver el attch point
    switch ( e.fParam[ipaAttchmPoint])
    {
        case 1:
        case 4:
        case 7: // left
            OffsetWidth = e.fParam[ipaRectangleWidth];
            break;
            
        case 2:
        case 5:
        case 8: // mid
            OffsetWidth = e.fParam[ipaRectangleWidth] / 2;
            break;
        default: // rigth
            OffsetWidth = 0;
            break;
    }
    g = new Grip();
    g.Shape = 1;
    flxGrip.Clear();
    flxGrip.AddRange([OffsetWidth, 0]);
    Puntos.Rotate(flxGrip, e.fParam[ipaTextAngle]);
    g.X = e.P[0] + flxGrip[0];
    g.Y = e.P[1] + flxGrip[1];
    g.Xr = g.X;
    g.Yr = g.Y;
    g.Action = 1; // este dato es propio de la clase
    g.AsociatedEntity = e;
    g.AsociatedPoint = 0;
    g.DrawLineToAsociatedGrip = true;
    g.ToolTip = ("Change width");
    Gcd.Drawing.Sheet.Grips.Add(g);

     // GRIP PARA CAMBIAR LA ALTURA DEL TEXTO
    g = new Grip();
    g.Shape = 1;
    flxGrip.Clear();
    flxGrip.AddRange([0, e.fParam[ipaTextHeight]]);
    Puntos.Rotate(flxGrip, e.fParam[ipaTextAngle]);
    g.X = e.P[0] + flxGrip[0];
    g.Y = e.P[1] + flxGrip[1];
    g.Xr = g.X;
    g.Yr = g.Y;
    g.Value = e.fParam[ipaTextHeight];
    g.Action = 2;
    g.AsociatedEntity = e;
    g.AsociatedPoint = 0;
    g.DrawLineToAsociatedGrip = true;
    g.ToolTip = ("Text height");
    Gcd.Drawing.Sheet.Grips.Add(g);

    g = new Grip();
    g.Shape = 2;
    flxGrip.Clear();
    flxGrip.AddRange([-e.fParam[ipaTextHeight], 0]);
    Puntos.Rotate(flxGrip, e.fParam[ipaTextAngle]);
    g.X = e.P[0] + flxGrip[0];
    g.Y = e.P[1] + flxGrip[1];
    g.Xr = g.X;
    g.Yr = g.Y;
    g.Action = 3;
    g.AsociatedEntity = e;
    g.AsociatedPoint = 0;
    g.AsociatedGrip = 0;
    g.iFillColor = Colors.Red;
    g.DrawLineToAsociatedGrip = true;
    g.ToolTip = ("Rotate text");
    Gcd.Drawing.Sheet.Grips.Add(g);
     //
     // g = New Grip
     // g.Shape = 1
     // g.X = e.P[0]
     // g.Y = e.P[1] + e.fParam[0]
     // g.Action = 1
     // g.AsociatedEntity = e
     // g.AsociatedPoint = 0
     // g.ToolTip = ("Change radius")
     // Gcd.Drawing.Sheet.Grips.Add(g)
     //
     // g = New Grip
     // g.Shape = 1
     // g.X = e.P[0]
     // g.Y = e.P[1] - e.fParam[0]
     // g.Action = 1
     // g.AsociatedEntity = e
     // g.AsociatedPoint = 0
     // g.ToolTip = ("Change radius")
     // Gcd.Drawing.Sheet.Grips.Add(g)

    return iCount;

}

public static bool GripEdit(Grip g)
    {


    Entity e ;         
    double r ;         
    double r1 ;         
    double r2 ;         
    double Xr ;         
    double Yr ;         
    double xs ;         
    double ys ;         
    double l ;         
    double d ;         
    double x ;         
    double l0 ;         
    double fAngle ;         

    e = g.AsociatedEntity;
    switch ( g.Action)
    {
        case 0: // mover la base
            e.P[0] = g.X;
            e.P[1] = g.Y;
            break;
        case 1: // cambiar el ancho
             // Angulo entre la posicion del grip original y la nueva
            fAngle = Gb.Ang((g.X - e.P[0]), (g.Y - e.P[1])) - Gb.Ang((g.Xr - e.P[0]), (g.Yr - e.P[1]));
             // la nueva altura del texto sera
            r1 = Puntos.Distancia(e.P[0], e.P[1], g.X, g.Y);

            e.fParam[ipaRectangleWidth] = r1 * Math.Cos(fAngle);
            break;              
        case 2: // cambiar la altura
             // Angulo entre la posicion del grip original y la nueva
            fAngle = Gb.Ang((g.X - e.P[0]), (g.Y - e.P[1])) - Gb.Ang((g.Xr - e.P[0]), (g.Yr - e.P[1]));
             // la nueva altura del texto sera
            r1 = Puntos.Distancia(e.P[0], e.P[1], g.X, g.Y);

            e.fParam[ipaTextHeight] = r1 * Math.Cos(fAngle);
            break;

        case 3: // rotar
            fAngle = Gb.Ang((g.X - e.P[0]), (g.Y - e.P[1])) - Gb.Ang((g.Xr - e.P[0]), (g.Yr - e.P[1]));

            fAngle = Gb.Ang((g.X - e.P[0]), (g.Y - e.P[1]));

             //rotate(e, fAngle - e.fParam[ipaTextAngle])
            e.fParam[ipaTextAngle] = fAngle - Math.PI; //- e.fParam[ipaTextAngle]
            break;

    }
    EntityBase.BuildGeometry(e);
    Gcd.Drawing.Sheet.Grips.Clear();
    GenerateGrips(e);
    return true;

}

}
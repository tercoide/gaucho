using Gaucho;
using OpenTK.Graphics.OpenGL4;
class DrawingAids
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


 // Lineas de puntos para Paint
public int dashes ;         

 // ayudas al usuario en pantalla
 public struct HelperSt
{
    public string texto;
    public int dX; //= 5 //defasaje con respecto al mouse
    public int dY; //= 5
    public int fColor; //= Color.black
    public bool ShowRect; //= True
    public int fRectangulo; // = Color.ButtonBackground
    public string fFont; //= "Arial" (ignorado)
    public int fHeight; //= 20
}

public static double ScreenDensity = 1080 / 29.5;   // pixele by cm wich gives a 1:100 scale in my monitor

public static HelperSt Helper ;          // Esto se escribe al lado del mouse
public static string ErrorMessage ;         
public static int ErrorTimer = 0;
public static string HelperDibujo ;          // Se escribe abajo "Grid  Ortho" , etc
public static string trabajoYteclado ;         

public static string txtSnapTo ;         
public static string txtFrom ;   

public static VboContainer VBO_GridPoints = new VboContainer();
public static VboContainer VBO_text = new VboContainer();

public static void CleanTexts()
    {


    Helper.texto = "";

    HelperDibujo = "";

}

public static string GetHelp(string sTable, string sProperty)
    {


    string s = "" ;         

    switch ( sTable.ToLower())
    {
        case "layer":
            switch ( sProperty.ToLower())
            {
                case "linetype":
                    s = "Changes the default line type for this layer";
                    break;
            }
            break;
    }
    if ( s == "" ) s = ("Help unavailable.");
    // fMain.txtHelp.Text = s;
    return s;

}

public static void _new()
    {


    Helper.texto = "";
    Helper.dX = 15; //defasaje con respecto al mouse
    Helper.dY = 15;
    Helper.fColor = Colors.Black;
    Helper.ShowRect = true;
    Helper.fRectangulo = Colors.Blue;
    Helper.fFont = "Arial";
    Helper.fHeight = 10;

    VBO_GridPoints = VboManager.CreateVbo(PrimitiveType.Lines);

}

 // All text options in one line
 // 0,0 is center
public static void Texting(string MyText, int posX, int posY, int txtColor= 0, int txtHeight= 12, string FontName = "romanc")
    {


    // GL.PushMatrix();

    // GL.LoadIdentity();

    // Glx.DrawText(MyText, posX, posY, 0, 10, txtColor);

    // GL.PopMatrix();

}

public static void DibujaHelper()
    {


    string sNext = "" ;         

     // If ErrorMessage <> "" Then
     //
     //     texting(ErrorMessage, 10 - fMain.GLarea1.w / 2, 30 - fMain.GLarea1.h / 2, Color.Red)
     //
     //     If ErrorTimer = 15 Then ErrorMessage = ""
     // Else
     //     ErrorTimer = 0
     //
     // End If

     // If $CmdLineHelperNext <> "" Then snext = $CmdLineHelperNext & " : "
     //
     // trabajoYteclado = " : " & $CmdLineHelper & sNext & fMain.KeysAccumulator // & PromtSlash
     //
     // fMain.txtCommand.Text = Gcd.clsJob.prompt & trabajoYteclado
     //
     // fmain.Refresh

     // Texting(trabajoYteclado, 10 - fMain.GLarea1.w / 2, 10 - fMain.GLarea1.h / 2, Gcd.flgWindowInfoColor)

    HelperDibujo = "";
    if ( Gcd.Orthogonal ) HelperDibujo = "F8-Ortho";
    if ( Gcd.Drawing.GridActive ) HelperDibujo = HelperDibujo + "  F7-Grid = " + Gcd.Drawing.GridMinorSpacing.ToString("0.00 m");
     // and so on...

    Texting(HelperDibujo, 40, 10 - Gcd.Drawing.Sheet.GlSheet.GetHeight() / 2, Config.WindowInfoColor);

     // helper del mouse
    if ( Helper.texto.Length == 0 ) return;

    Texting(Helper.texto, Helper.dX + Mouse.X - Gcd.Drawing.Sheet.GlSheet.GetWidth() / 2, Helper.dY + -Mouse.Y + Gcd.Drawing.Sheet.GlSheet.GetHeight() / 2, Colors.Cyan, 10);

}

 // Dibuja dX y dY entre dos puntos
public static void DrawDxDy(double[] p1, double[] p2, int iSizePix= 10)
    {


     // <------------3.45-------------->
     // y algo similar para Y

     // el ancho de la flecha es iSizePix/3 y el largo = iPixsize

     // Si dX es chico  >--< 3.45
    List<double> flxTextSize ;         
    int iTotalLength ;          // en pixeles
    string sTextX ;         
    string sTextY ;         
    Entity eLineaPpal ;         
    Entity        eOblique1 ;          // tambien pueden ser flechas Solid
    Entity eOblique2 ;         
    Entity eText ;         
    double ArrowSize = Gcd.Metros(iSizePix);
    double ArrowWidth = ArrowSize / 4;

    sTextX = (p2[0] - p1[0]).ToString(Config.FormatCoord);
    sTextY = (p2[1] - p1[1]).ToString(Config.FormatCoord);

    flxTextSize = Glx.TextExtends(sTextX, iSizePix);

    iTotalLength = (int)(flxTextSize[0] + (2 + 0.6) * iSizePix); // pixeles

     //If iTotalLength < Gcd.Pixels(p2[0] - p1[0]) Then // puedo dibujar todo normalmente
     // ================eje X===============================
     // flechas
    eOblique1 = Gcd.CCC["SOLID"].NewEntity([0, 0, ArrowSize, ArrowWidth, ArrowSize, -ArrowWidth, 0, 0]);

    eOblique2 = Gcd.CCC["SOLID"].NewEntity([0, 0, -ArrowSize, ArrowWidth, -ArrowSize, -ArrowWidth, 0, 0]);
    if ( p2[0] < p1[0] )
    {
        Gcd.CCC[eOblique1.Gender].Rotate(eOblique1, Math.PI);

        Gcd.CCC[eOblique2.Gender].Rotate(eOblique2, Math.PI);

    }

    Gcd.CCC[eOblique1.Gender].Translate(eOblique1, p1[0], p1[1]);
    Gcd.CCC[eOblique2.Gender].Translate(eOblique2, p2[0], p1[1]);

     // linea principal
    eLineaPpal = Gcd.CCC["LINE"].NewEntity([p1[0], p1[1], p2[0], p1[1]]);

     // texto
    eText = Gcd.CCC["MTEXT"].NewEntity([(p2[0] - p1[0]) / 2 + p1[0], p1[1]]  );
    eText.fParam[cadMText.ipaAttchmPoint] = 8;
    eText.fParam[cadMText.ipaTextHeight] = Gcd.Metros(iSizePix);

    eText.sParam[cadMText.sdaText] = sTextX;

    eOblique1.Colour = Config.WindowAidsColor;
    eOblique2.Colour = Config.WindowAidsColor;
    eLineaPpal.Colour = Config.WindowAidsColor;
    eText.Colour = Config.WindowAidsColor;

    Gcd.CCC[eOblique1.Gender].Draw(eOblique1);
    Gcd.CCC[eOblique2.Gender].Draw(eOblique2);
    Gcd.CCC[eLineaPpal.Gender].Draw(eLineaPpal);
    Gcd.CCC[eText.Gender].Draw(eText);

     // ================eje Y===============================
     // flechas
    eOblique1 = Gcd.CCC["SOLID"].NewEntity([0, 0, ArrowSize, ArrowWidth, ArrowSize, -ArrowWidth, 0, 0]);
    if ( p2[1] < p1[1] )
    {
        Gcd.CCC[eOblique1.Gender].Rotate(eOblique1, Math.PI * 3 / 2);
    }
    else
    {
        Gcd.CCC[eOblique1.Gender].Rotate(eOblique1, Math.PI / 2);
    }

    Gcd.CCC[eOblique1.Gender].Translate(eOblique1, p2[0], p1[1]);
    eOblique2 = Gcd.CCC["SOLID"].NewEntity([0, 0, -ArrowSize, ArrowWidth, -ArrowSize, -ArrowWidth, 0, 0]);
    if ( p2[1] < p1[1] )
    {
        Gcd.CCC[eOblique2.Gender].Rotate(eOblique2, Math.PI * 3 / 2);
    }
    else
    {
        Gcd.CCC[eOblique2.Gender].Rotate(eOblique2, Math.PI / 2);
    }

    Gcd.CCC[eOblique2.Gender].Translate(eOblique2, p2[0], p2[1]);
    eOblique1.Colour = Config.WindowAidsColor;
    eOblique2.Colour = Config.WindowAidsColor;

    eLineaPpal.P.Clear();
    eLineaPpal.P.AddRange([p2[0], p1[1], p2[0], p2[1]]);
    eText.sParam[cadMText.sdaText] = sTextY;
    eText.P.Clear();
    eText.P.AddRange([p2[0], (p2[1] - p1[1]) / 2 + p1[1]]);
    Gcd.CCC[eOblique1.Gender].Draw(eOblique1);
    Gcd.CCC[eOblique2.Gender].Draw(eOblique2);
    Gcd.CCC[eLineaPpal.Gender].Draw(eLineaPpal);
    Gcd.CCC[eText.Gender].Draw(eText);

     //End If

}

public static void DrawSnapText()
    {


    double x ;         
    double y ;         
    string s ;         

    if ( txtSnapTo == "" ) return;
    if ( Gcd.Drawing.iEntity.Count == 0 ) return;
    if ( Gcd.Drawing.iEntity[2] == 0 ) return;
    x = Gcd.Drawing.iEntity[0];
    y = Gcd.Drawing.iEntity[1];
    s = txtFrom + " to " + txtSnapTo;
    Glx.SelectFont(Config.GripTextOnScreenFont);
    Glx.DrawTextPoly(s, x, y, 0, Gcd.Metros(Config.GripTextOnScreenSize));

}

public static void DrawCoordenadas()
    {


     // helpers
    string l ;         
    string l2 ;         
    string lEsc ;         

    l = Gcd.Near(Gcd.Xreal(Mouse.X)).ToString("0.00") + " : " + Gcd.Near(Gcd.Yreal(Mouse.Y)).ToString("0.00");

     //    l2 = "Mouse: " & Str$(puntos.cursorX) & ": " & Str$(puntos.cursorY)

    l2 = "Zoom " + Gcd.Drawing.Sheet.ScaleZoom.ToString("0.0000"); // & " - Pan: " & Str$(Gcd.Drawing.PanX) & ", " & Str$(Gcd.Drawing.PanY)

     //FCAD.nDraws = 0

     // intento determinar la escala
     // en mi monitor, 29cm  = 1080 pixeles

     //fMain.lblCoord.Text = l
     //fMain.lblZoom.text = l2
    return;
     // TODO: borrar lo q sigue
    double e ;         
    double z ;         
    e = 1080 / 0.29; // pixeles por metro segun el tamaño real de la pantalla
    z = Gcd.Metros(1); // metros por pixel segun la ampliacion del usuario (rueda del mouse)
    lEsc = "Esc 1:" + (1 / Gcd.Drawing.Sheet.ScaleZoom * 100 * Gcd.ScreenDensity).ToString("0");

    Texting(l, 10 - Gcd.Drawing.Sheet.W / 2, Gcd.Drawing.Sheet.H / 2 - 15, Config.WindowInfoColor);
    Texting(l2, 10 - Gcd.Drawing.Sheet.W / 2, Gcd.Drawing.Sheet.H / 2 - 35, Config.WindowInfoColor);
    Texting(lEsc, Gcd.Drawing.Sheet.W / 2 - 100, 10 - Gcd.Drawing.Sheet.H / 2, Config.WindowInfoColor);

}

public static void DrawEjes()
    {


     // helper de los ejes coordenados
     // TODO:  no se porque el fondo de las dashes me sale gris y no blanco
     // paint.brush = Paint.Color(Color.RGB(0, 0, 0, 0))
     // paint.LineWidth = 1 / Gcd.Drawing.ScaleZoom
     // paint.Dash = DashDot
     // paint.MoveTo(0 - (DashDot[0] + DashDot[1] + DashDot[2] / 2), 0) // me voy al origen de coordenadas
     // paint.relLineTo(1000, 0)
     // paint.MoveTo(0, 0 - (DashDot[0] + DashDot[1] + DashDot[2] / 2)) // me voy al origen de coordenadas
     // paint.relLineTo(0, 1000)
     // paint.Dash = []
     // paint.Stroke

}

public static void RebuildGrid()
    {


    double x0 ;         
    double y0 ;         
    double x1 ;         
    double y1 ;         
    double espaciado ;         
    double x ;         
    double y ;         

    x0 = Gcd.Near(Gcd.Xreal(0));
    y0 = Gcd.Near(Gcd.Yreal(0));
    x1 = Gcd.Near(Gcd.Xreal(Gcd.Drawing.Sheet.W));
    y1 = Gcd.Near(Gcd.Yreal(Gcd.Drawing.Sheet.H));

    espaciado = Gcd.Drawing.GridMinorSpacing;

    do {
        if ( Gcd.Pixels(espaciado) < 5 ) espaciado *= 10;
    } while (Math.Abs(Gcd.Pixels(espaciado)) > 5);

    // if ( ! GL.islist(Gcd.Drawing.GLListGrid) ) Gcd.Drawing.GLListGrid = GL.GenLists(1);
    // GL.List(Gcd.Drawing.GLListGrid, GL.COMPILE);

    switch ( Gcd.Drawing.GridStyle)
    {
        case 0: // puntos
            
            for ( y = y1; y <= y0; y += espaciado)
            {
                for ( x = x0; x <= x1; x += espaciado)
                {
                    Glx.Vertex2D(x, y,Config.WhiteAndBlack);
                }
            }

            
            break;

        case 1: // cuadricula

            for ( y = y1; y <= y0; y += espaciado)
            {
                Glx.DrawLines([x0, y, x1, y], Colors.Gray, 1);
            }

            for ( x = x0; x <= x1; x += espaciado)
            {
                Glx.DrawLines([x, y0, x, y1], Colors.Gray, 1);
            }
            break;

    }
    

    Gcd.Drawing.GridCurentSpacing = espaciado;

     // aprovecho para recomponer las lineas de puntos

}

public static void DrawCaudricula()
    {


     // crossing lines each gridspace and darker each 10m

    double i =0;         
    double xgg =0;         
    double ygg =0;         
    double xg =0;         
    double yg =0;         
    double espaciado =0;         
    double starting =0;         
    int W ;         
    int H ;         

    xg = Gcd.Xreal(0); // Puntos reales aproximados al 0,0 de la pantalla
    yg = Gcd.Yreal(0);

    xg = Gcd.Near(xg); // punto en mundo real ajustado a grid
    yg = Gcd.Near(yg);

    xg = Gcd.XPix(xg) - espaciado * 2; // punto inicial en la drawing area
    yg = Gcd.YPix(yg) - espaciado * 2;

    xgg = xg;
    ygg = yg;

    W = Gcd.Drawing.Sheet.W;
    H = Gcd.Drawing.Sheet.H;

    starting = yg; // guardo el Y inicial

    // if ( ! GL.islist(Gcd.Drawing.GLListGrid) ) Gcd.Drawing.GLListGrid = GL.GenLists(1);
    // GL.List(Gcd.Drawing.GLListGrid, GL.COMPILE);

     // lineas finas
    do { // lineas horizontales
        if ( espaciado < 5 ) break;
        Glx.DrawLines([xg, yg, xg + W + espaciado, yg], Colors.Gray, 1);
        yg += espaciado - 0.5;

    } while (true);

    xg = xgg;
    yg = ygg;
    do { // verticales
        if ( espaciado < 5 ) break;
        Glx.DrawLines([xg, yg, xg, yg + H + espaciado], Colors.Gray, 1);
        xg += espaciado - 0.5;
    } while (true);

     // lineas gruesas

    

}

 // Public Sub DrawPolygons() // and other debugging stuff
 //
 //     Dim e As Entity
 //     For Each e In Gcd.Drawing.Sheet.EntitiesVisibles
 //         If e.Polygon Then Glx.Polygon(e.Polygon, Colors.Blue, 1)
 //     Next
 //
 // End

public static void Refresh()
    {


    DibujaHelper();
     // fmain.txtInput.X = fmain.W / 2
     // fmain.txtInput.Y = fmain.h / 2
     // fmain.txtInput.SetFocus()
     // //fmain.txtInput.Text = fmain.KeysAccumulator
     // fmain.txtInput.Visible = True

}

public static void DrawGrips(List<Grip> GlxGrips)
    {


    Grip g ;         
    double side ;         
    double half ;         

    side = Gcd.Metros(Config.GripSize);
    half = side / 2;

    foreach ( var g1 in GlxGrips)
    {
         //GL.Rotatef(g.AnGLe, 0, 0, 1)
         g = g1;
        switch ( g.Shape)
        {
            case 0:
                Glx.Rectangle2D(g.X - half, g.Y - half, side, side, g.iFillColor, 0, 0, 0, g.iColor, 1, [], 1);
                break;
            case 1:
                Glx.Rombo2D(g.X, g.Y, side, g.iFillColor, g.iFillColor2);
                break;
            case 2:
                Glx.CIRCLE([g.X, g.Y], half, g.iFillColor, true);
                break;

        }
         //GL.Rotatef(-g.AnGLe, 0, 0, 1)
        if ( g.DrawLineToAsociatedGrip )
        {
            Glx.PolyLines([g.X, g.Y, GlxGrips[g.AsociatedGrip].X, GlxGrips[g.AsociatedGrip].Y], Config.GripLineColor);
        }
    }

}

}
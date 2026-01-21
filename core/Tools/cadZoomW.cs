using Gaucho;
using Gtk;

class cadZoomW :  ToolsBase
{
 
 // Tool maintained by Terco


const string Gender = "ZOOMW";

public static bool Start(string ElemToBuild, int _Mode= 0)
    {

     // Modes:
     //       0 = Move, all points in the element must be selected, or click on it.
     //       1 = Stretch, selection may be partial, each element is called to see if the support stretching

    Mode = _Mode;
    PoiChecking = false;
    Gcd.flgSearchingAllowed = false;
return true;
    }

public static void MouseDown()
    {


    if ( Mouse.Left )
    {
       SelStartX = Mouse.X;
       SelStartY = Mouse.Y;
       SelEndX =SelStartX;
       SelEndY=SelStartY;

       SelStartXr = Gcd.Xreal(SelStartX);
       SelStartYr = Gcd.Yreal(SelStartY);

       Active = true;
    }

    }

    public static void MouseUp()
    {


        SelEndX = Mouse.X;
        SelEndY = Mouse.Y;
        Active = false;

        // corrijo para start<end
        if (SelStartX >SelEndX ) Gb.Swap (ref SelStartX, ref SelEndX);
        if (SelStartY <SelEndY ) Gb.Swap (ref SelStartY, ref SelEndY); // this is FLIPPED

            // Paso a coordenadas reales
        SelStartXr = Gcd.Xreal(SelStartX);
        SelStartYr = Gcd.Yreal(SelStartY);
        SelEndXr = Gcd.Xreal(SelEndX);
        SelEndYr = Gcd.Yreal(SelEndY);

     // veo si el rectangulo es suficientemente grande como para representar una seleccion por rectangulo
    if ( (SelEndX -SelStartX + (-SelEndY +SelStartY)) < 10 ) // es un rectangulo minusculo
    {

        //DrawingAIds.ErrorMessage = ("Window is too small");

    }
         // engañamos a estas vars

        Gcd.Drawing.Xmayor =SelEndXr;
        Gcd.Drawing.Xmenor =SelStartXr;

        Gcd.Drawing.Ymayor =SelEndYr;
        Gcd.Drawing.Ymenor =SelStartYr;

        //cadZoomE.Start(0, 1);
       Finish();

    }



    public static void MouseMove()
    {


        SelEndX = Mouse.X;

        SelEndY = Mouse.Y;

        SelEndXr = Gcd.Xreal(SelEndX);
        SelEndYr = Gcd.Yreal(SelEndY);

            Gcd.Redraw();

    }

public static void Draw() // esta rutina es llamada por FCAD en el evento DrawingArea_Draw
    {

     // por ultimo, y para que se vea arriba, la seleccion

    if ( !Active ) return;

    double[] xyStart ;         
    double[] xyEnd ;         

    Glx.Rectangle2D(SelStartXr,SelStartYr,SelEndXr -SelStartXr,SelEndYr -SelStartYr,0,0,0,0, Colors.Blue, 1,[], 2);

}

public static void Finish()
    {


    Gcd.clsJob = Gcd.clsJobPrevious;
    // Gcd.clsJobPrevious = Gcd.CCC[Gender];
   // DrawingAIds.CleanTexts;

   Active = false;

    Gcd.flgNewPosition = true;
    Gcd.flgSearchingAllowed = true;

}

}
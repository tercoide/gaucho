using Gaucho;

class cadZoomW :  ToolsBase
{
 
 // Tool maintained by Terco

 // this is the Main Job, either we are doing this or other job
const string Gender = "ZOOMW";

public static bool Start(string ElemToBuild, int _Mode= 0)
    {

     // Modes:
     //       0 = Move, all points in the element must be selected, or click on it.
     //       1 = Stretch, selection may be partial, each element is called to see if the support stretching

    Mode = _Mode;
    PoiChecking = false;
    Gcd.flgSearchingAllowed = false;

}

public static void MouseDown()
    {


    if ( Mouse.Left )
    {
       SelStartX = mouse.X;
       SelStartY = mouse.Y;
       SelEndX =SelStartX;
       SelEndy =SelStartY;

       SelStartXr = gcd.Xreal(SelStartX);
       SelStartYr = gcd.Yreal(SelStartY);

       Active = true;
    }

}

public static void MouseUp()
    {


   SelEndX = mouse.x;
   SelEndy = mouse.Y;
   Active = false;

     // corrijo para start<end
    if (SelStartX >SelEndX ) Swap (SelStartX,SelEndX);
    if (SelStartY <SelEndY ) Swap (SelStartY,SelEndY); // this is FLIPPED

     // Paso a coordenadas reales
   SelStartXr = gcd.Xreal(SelStartX);
   SelStartYr = gcd.Yreal(SelStartY);
   SelEndXr = gcd.Xreal(SelEndX);
   SelEndyr = gcd.Yreal(SelEndy);

     // veo si el rectangulo es suficientemente grande como para representar una seleccion por rectangulo
    if ( (SelEndX -SelStartX + (-SelEndy +SelStartY)) < 10 ) // es un rectangulo minusculo
    {

        DrawingAIds.ErrorMessage = ("Window is too small");

    }
         // engañamos a estas vars

        gcd.Drawing.Xmayor =SelEndXr;
        gcd.Drawing.Xmenor =SelStartXr;

        gcd.Drawing.Ymayor =SelEndYr;
        gcd.Drawing.Ymenor =SelStartYr;

        cadZoomE.Start(0, 1);
       Finish();

    }

}

public static void MouseMove()
    {


   SelEndX = mouse.x;

   SelEndy = mouse.Y;

   SelEndXr = gcd.Xreal(SelEndX);
   SelEndYr = gcd.Yreal(SelEndY);

    gcd.Redraw();

}

public static void Draw() // esta rutina es llamada por FCAD en el evento DrawingArea_Draw
    {

     // por ultimo, y para que se vea arriba, la seleccion

    if ( !Active ) return;

    double[] xyStart ;         
    double[] xyEnd ;         

    glx.Rectangle2D(SelStartXr,SelStartYr,SelEndXr -SelStartXr,SelEndYr -SelStartYr,,,,, Color.DarkBlue, 1,, 2);

}

public static void Finish()
    {


    gcd.clsJob = gcd.clsJobPrevious;
    gcd.clsJobPrevious = this;

    DrawingAIds.CleanTexts;

   Active = false;

    gcd.flgPosition = true;
    gcd.flgSearchingAllowed = true;

}

}
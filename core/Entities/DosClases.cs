// A diferencia de Gambas, en C# puede haber mas de una clase por archivo.
// Y el nombre del archivo no es el nombre de la clase necesariamente.

// Esta es la definicion de la clase. 
// Equivale a un archivo cadLine.class y a otro cadCircle.class en Gb.

using Gaucho;
using OpenTK.Graphics.OpenGL;

namespace Gaucho
{
    // Arma la clase cadLine que implementa la interfaz IEntity.
    // Una interfaz es un contrato que obliga a la clase a implementar ciertos metodos.
    // Entonces puedo crear una instancia de la clase y asignarla a una variable del tipo de la interfaz.
    // En Gb usabamos clsJob , que era una variable global.

    public class cadLine: EntityBase,IEntity
    {
        public override string Gender { get; } = "LINE"; // Override para sobreescribir la propiedad virtual de la clase base.
        
        public new bool Regenerable { get; set; }
        
        public int DrawingOrder { get; } = 1;         // 1 = draws first
        public string CmdLineHelper { get; } = "a line";
        public string ParamType { get; } = "PP";                              // that is Point, Point; could be Color Text, etc
        public string ParamHelper { get; } = "Start point;End point";
        public int TotalPoints { get; } = 2;
        // Constructor, equivalente a _New en Gambas.
        public cadLine()
        {
            Console.WriteLine("Creo una linea");
        }

        public void Draw()
        {
            Console.WriteLine("Dibujo una linea");
            PrivateMethod();
            return;
        }

        private void PrivateMethod()
        {
            Console.WriteLine("Metodo privado de cadLine");
            return;
        }

        public bool ImportDXF(Entity e, List<string> sClaves, List<string> sValues)
        {


            int i ;         

            e.P.Clear();
            for ( i = 0; i < sClaves.Count; i += 1)
            {

                if ( sClaves[i] == "10" ) e.P.Add(Gb.CDbl(sValues[i]));
                if ( sClaves[i] == "20" ) e.P.Add(Gb.CDbl(sValues[i]));
                if ( sClaves[i] == "11" ) e.P.Add(Gb.CDbl(sValues[i]));
                if ( sClaves[i] == "21" ) e.P.Add(Gb.CDbl(sValues[i]));

            }
            return true;

            // catch

            return false;

        }

        public  bool NewParameter(Entity eBuild, List<string> vParam, bool Definitive= false)
    {


     // la linea solo recibe puntos

    if ( vParam[0] != "point" ) return false;

    if ( Gcd.StepsDone == 0 )
    {
         eBuild.P[0] = Gb.CDbl(vParam[1]);
         eBuild.P[1] = Gb.CDbl(vParam[2]);
        eBuild.P[2] = eBuild.P[0];
        eBuild.P[3] = eBuild.P[1];
        if ( Definitive ) return true;
    }
    else if ( Gcd.StepsDone == 1 )
    {

         eBuild.P[2] = Gb.CDbl(vParam[1]);
         eBuild.P[3] = Gb.CDbl(vParam[2]);
        if ( Definitive ) return true;

    }
    return false;

}

public static bool SaveDxfData(Entity e)
    {


     // stxExport.insert(["LINE", dxf.codEntity])
     // Los datos comunes a todas las entidades son guardados por la rutina que llama a esta
    Dxf.SaveCodeInv("AcDbLine", "100");
    Dxf.SaveCodeInv((e.P[0]).ToString(), Dxf.codX0);
    Dxf.SaveCodeInv((e.P[1]).ToString(), Dxf.codY0);
    Dxf.SaveCodeInv((e.P[2]).ToString(), Dxf.codX1);
    Dxf.SaveCodeInv((e.P[3]).ToString(), Dxf.codY1);
    if ( e.Extrusion[2] != 1 )
    {
        Dxf.SaveCodeInv((e.Extrusion[0]).ToString(), "210");
        Dxf.SaveCodeInv((e.Extrusion[1]).ToString(), "220");
        Dxf.SaveCodeInv((e.Extrusion[2]).ToString(), "230");
    }
    return true;

}



 // Return if that position is over the entity within the tolerance

public bool OverMe(Entity e, double Xr , double Yr, double tolerance ) 
{
     //If Abs(puntos.PointToLineDistance([xr, yr], e.p)) <= tolerance Then Return True Else Return False
    if ( Puntos.PointOverLine([Xr, Yr], e.P, tolerance) ){ return true; } else { return false;}



    }
    }


    public class cadCircle: EntityBase,IEntity
    {
        public override string Gender { get; } = "CIRCLE";
        
        public new bool Regenerable { get; set; }
        
        public cadCircle()
        {
            Console.WriteLine("Creo un circulo");
        }

        public new void  Draw()
        {
            Console.WriteLine("Dibujo un circulo");
            return;
        }

        public void BuildGeometry(Entity e)
        {
            Console.WriteLine("Construyo la geometria del circulo");
            return;
        }
    }

}
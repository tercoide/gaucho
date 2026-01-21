// Esta seria la clase base de las entidades. Sera heredada por ellas y tendra una
// interface que es una clase que representa alguna entidad y responde a 
// otras clases que necesitan cosas de las entidades, como dibujarse con Draw.
namespace Gaucho
{
    public class EntityBase : IEntity
    {
        // public  void EntityBase()
        // {

        // }
public bool Regenerable { get; set; } = false;
        public static void Draw()
        {
            Console.WriteLine("Llamada a diibujar desde la EntityBase");
            return;
        }
 public  void Draw2()
        {
            Console.WriteLine("Imprimo desde Base");
            return;
        }

    }


    public interface IEntity
    {
        public bool Regenerable { get; set; }
        public void Draw(Entity e) { }

        public void Draw2(Entity e)

        {
            Console.WriteLine("Imprimo desde la interface IEntity");
            return;
        }
        
        public void SaveDxfData(Entity e ) { }

         public void DrawSelected(Entity e) { }

          public void DrawRemark(Entity e) { }
           public void Finish(Entity e) { }

            public void Translate(Entity e, double dx, double dy,bool OnlyPointSelected=false) { }

             public void Scale(Entity e, double sx, double sy) { }
              public void Rotate(Entity e, double radians) { }
               public void GenerateGrips(Entity e) { }

               public Block RebuildBlock(Entity e, int  iMode  = 0, float fAngle = -1) { return null; }
                public bool SelFullPoly(Entity e, List<double> poly) { return false;  }
                 public bool SelPartialPoly(Entity e, List<double> poly) { return false;}
                 public void BuildGeometry(Entity e) { }
                 public bool SelPartial(Entity eTesting, double X1real, double Y1real, double X2real, double Y2real) { return false;  }
                  public bool SelFull(Entity eTesting, double X1real, double Y1real, double X2real, double Y2real) { return false;  }  
    
    

}
}

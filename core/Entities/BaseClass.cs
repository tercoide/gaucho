// Esta seria la clase base de las entidades. Sera heredada por ellas y tendra una
// interface que es una clase que representa alguna entidad y responde a 
// otras clases que necesitan cosas de las entidades, como dibujarse con Draw.
namespace Gaucho
{
    public class EntityBase //: IEntity
    {
        // public  void EntityBase()
        // {

        // }
        public string ParamHelper { get ;  } 
        public string ParamType { get;  } 

        public string ParamDefault { get;  }

        public string Prompt { get;  }

        public bool Regenerable { get; set; } = false;

        public int LastMode {get; set;} = -1;

        public int iiiMode { get;  } =0;

       
 public  void Draw2()
        {
            Console.WriteLine("Imprimo desde Base");
            return;
        }

        public void BuildGeometry(Entity e)
        {
            Console.WriteLine("Construyo la geometria desde Base");
            return;
        }

    }


    public interface IEntity
    {

        public Entity NewEntity(List<double>? fPoints = null, bool bNewid = false) { return new Entity(); }
        public Entity ClonEntity(Entity e, bool NewId = false) { return e; }
        public bool Regenerable { get; set; }

        public string ParamHelper { get ;  } 
        public string ParamType { get;  } 

        public string ParamDefault { get;  }

        public string Prompt { get;  }

        public int LastMode {get; set;} 

        public int iiiMode { get;  }

        public void Draw(Entity e) { }

        public void Draw2(Entity e)

        {
            Console.WriteLine("Imprimo desde la interface IEntity");
            return;
        }
        
        public void SaveDxfData(Entity e ) { }
        public bool ImportDXF(Entity e , ref List<string> keys, ref List<string> values) { return false;  }

         public void DrawSelected(Entity e) { }

          public void DrawRemark(Entity e) { }
              public void DrawShadow(Entity e) { }
              public void DrawEditing(Grip g) { }
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
    
    public bool OverMe(Entity eTesting, double Xreal, double Yreal, double tolerance) { return false;  }
public void GripEdit(Grip g) {return;  }

public bool NewParameter(Entity e, List<string> sParam, bool Definitive = false) { return false;  }  

}

}

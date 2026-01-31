using Gaucho;
using System.Collections.Generic;

public class Entity
{
    public string Gender="";                 // Type of entity
    public List<double> P = new List<double>();                     // X,Y pairs
    public List<string> sParam = new List<string>();               // Array of Strings
    public List<double> fParam = new List<double>();                // Array of doubles
    public List<int> iParam = new List<int>();              // Array of integers
    public int Colour;                    // CAD color code
    public int LineWidth = -3;            // This is it
    public Block? pBlock;                   // Pointer to Block or Hatch (or null)
    public LineType? LType;                 // 0=continuous, 1=dashed from flxDash
    public Layer? pLayer;
    public TextStyle? pStyle;                  //
    public DimStyle? pDimStyle;                //
    public bool Visible = true;            // Si esta entidad puede ser mostrada independientmente
    public bool PaperSpace = false;        // Si esta entidad esta en algun paper space
                                           //public Sheet As Sheet                       // La hoja o Layout donde esta dibujada
                                           //public Trackable As Boolean = True          // Si esta entidad puede ser buscada en pantalla con el mouse
    public string id="";                       // Unique identifier
    public Block? Container;                 // Apunta al contenedor de esta entidad, util para UnDo y Redo
    //public Collection Group;                // Grupos temporales de entidades. Esto apunta al grupo donde estoy o Null
    public List<bool> Psel = new List<bool>();                // Selected points
    public List<double> Polygon = new List<double>();               // This is a poligon built at element//s contruction that defines a poligon used for detecting points inside me
    public List<double> PolyLine = new List<double>();              // This is a polyline built at element//s contruction that defines a poligon used for detecting points inside me
    public List<double> Limits = new List<double> {0, 0, 0, 0};                    // Point of interest
    public List<double> Extrusion = new List<double> {0, 0, 0};
    //public PoiType As New Integer[]
    public int glDrwList;                 // OpenGL drawing list
    public int glDrwListSel;              // OpenGL drawing list for selected item
    public int glDrwListRemark;           // OpenGL drawing list for remarked item
                                          //public glDrwListPOI As Integer              // OpenGL drawing list for POI (grips)
    public bool Regenerable = false;
    public bool Generated = false;

}
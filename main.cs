using System.Reflection;
namespace Gaucho
{
    public static class Starter
    {
        // Gambas module file

        // GambasCAD
        // Software para diseño CAD
        //
        // Copyright (C) Ing Martin P Cristia
        //
        // This program is free software; you can redistribute it and/or modify
        // it under the terms of the GNU General Public License as published by
        // the Free Software Foundation; either version 2 of the License, or
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

        //TINCHO 2023.05.22 > Config.class implementation
        // Eviado a la clase Config.class
        // Public dirResources As String = "/usr/share/gambascad"
        // Public dirDwgIn As String = User.Home &/ ".config/GambasCAD/DwgIn"
        // Public dirDxfIn As String = User.Home &/ ".config/GambasCAD/DxfIn"
        // Public dirDwgOut As String = User.Home &/ ".config/GambasCAD/DwgOut"
        // Public dirDxfOut As String = User.Home &/ ".config/GambasCAD/DxfOut"
        // Public dirTemplates As String = User.Home &/ ".config/GambasCAD/templates"
        // Public dirBlocks As String      // path to Blocks
        //TINCHO 2023.05.22 > Config.class implementation

        // file conversion
        public static bool convLibreDWG = false;
        public static bool convODA = false;
        public static bool convOdaAppImage = false;
        public static bool DebugMode = true;
        
        public static string[] args = [];

        public static void main()
        {
            // Config.Home = Environment.GetEnvironmentVariable("HOME") ??
            //           Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Config.ConfigFile = System.IO.Path.Combine(Config.Home, ".config", "gaucho", "config.json");

            string sFile = "";

            //TINCHO 2023.05.22 > Config.class implementation
            // Estableciendo los parametros de configuración

            Config.Load();

            // Config.Root = System.IO.Path.Combine(Config.Home, ".config", "gaucho");
            // Config.Depot = System.IO.Path.Combine(Config.Root, "config.json");
            // Config.Log = System.IO.Path.Combine(Config.Root, "log.txt");
            // Config.dirDwgIn = System.IO.Path.Combine(Config.Root, "dwgin");
            // Config.dirDxfIn = System.IO.Path.Combine(Config.Root, "dxfin");
            // Config.dirDwgOut = System.IO.Path.Combine(Config.Root, "dwgout");
            // Config.dirDxfOut = System.IO.Path.Combine(Config.Root, "dxfout");
            // Config.dirTemplates = System.IO.Path.Combine(Config.Root, "templates");
            // //Config.dirBlocks = System.IO.Path.Combine(Config.Root, "blocks");
            // Config.dirPrintStyles = System.IO.Path.Combine(Config.Root, "printstyles");
            // Config.dirPatterns = System.IO.Path.Combine(Config.Root, "patterns");
            // Config.dirResources = System.IO.Path.Combine(Config.Root, "resources");

            if (Config.SplitterH.Length == 0)
            {
                Config.SplitterH = [144, 500, 144];
            }

            if (Config.SplitterV.Length == 0)
            {
                Config.SplitterV = [400, 80];
            }

            //LoadPatterns2

            Config.Save();

            // Public dirResources As String = "/usr/share/gambascad"
            // Public dirDwgIn As String = User.Home &/ ".config/GambasCAD/DwgIn"
            // Public dirDxfIn As String = User.Home &/ ".config/GambasCAD/DxfIn"
            // Public dirDwgOut As String = User.Home &/ ".config/GambasCAD/DwgOut"
            // Public dirDxfOut As String = User.Home &/ ".config/GambasCAD/DxfOut"
            // Public dirTemplates As String = User.Home &/ ".config/GambasCAD/templates"
            // Public dirBlocks As String      // path to Blocks

            

            

            // fSplash.Visible = true;

            // fSplash.Show;

           

            //TINCHO 2023.05.22 > Config.class implementation
            // Anualdo: Los recursos se copiaran una unica vez, en la primera ejecucion, en el directorio del usuario.
            // necesitamo saber desde donde estamos corriendo
            //If Application.Path Like "/usr/bin*" Then
            //    DebugMode = false
            //    Gcd.dirResources = "/usr/share/gambascad"
            //Else
            //    DebugMode = true
            //    Gcd.dirResources = Application.Path
            //Endif
            //TINCHO 2023.05.22 > Config.class implementation

            // Inicializo el programa
            Initialize(); // general init

            //MyLog = Open User.Home &/ ".config/gambascad/log.txt" For Write Create
            //TINCHO 2023.05.22 > Config.class implementation
            
            if (File.Exists(Config.Log))
            {
                File.Delete(Config.Log);
            }   

            Gcd.debugInfo("Init program - Version 0.02 - GTK4" , false, false, true);
            Gcd.debugInfo("Debug mode = " + DebugMode.ToString(), false, false, true);

            //fMain.tabFile.Index = 0

            // leo la configuracion inicial
            //Utils.LoadClass(Config, Config.ConfigFile) // Deshabilitado, con la nueva configuracion no es necesario.
            InitColors(); // CAD color init
            InitClasses();
            // InitMenus(); // fMain menus
            // loadPrintStyles();
            LoadPatterns();
            // LoadCursors();

            // TODO: DATO INTERESANTE SI LLAMO A LA SIGUIENTE LINEA DESPUES DE Fmain.Run , los graficos se ven opacos
            Gcd.main(); // drawing specific init

            // armo el combo de colores
            // fLayersOnScreen.Run  // FIXME:
            Gcd.debugInfo("LayersOnScreen initialized OK", false, false, true);

            // fMain.Run();
            // //Wait
            // fMain.Refresh();
            //Gcd.debugInfo("FMain initialized OK",,, true)

            // fSplash.HIde;

            // Wait;

            // if (args.Length  > 1)
            // {
            //     sFile = args[1];
            //     actions.FileOpen(sfile);
            // }
            // else
            // {
            //     actions.FileNew;
            // }
            // Gcd.clsJob.start();
            // bloques common
            Gcd.LoadCommon();

            // glx.Resize(fmain.glarea1)
            // fmain.glarea1.Refresh
            Gcd.Redraw();

        }

        public static void Initialize()
        {



            // string[] aDirs = { System.IO.Path.Combine(Config.dirResources, "minimal") }; // , Config.dirPatterns
            // string[] aTemp = [Config.dirDwgIn]; //, Config.dirPatterns]


            // // TERCO lo siguiente me borra los patrones de Hatch, elimino esa parte
            // //TINCHO 2023.05.22 > Config.class implementation
            // // Checking that the necessary directories exist
            // foreach (var sDir in aDirs)
            // {
            //     if (!File.Exists(sDir))
            //     {
            //         Gb.Shell("mkdir -p " + sDir);
            //     }
            //     else
            //     {
            //         Gb.Shell("rm -R " + sDir + "/*");
            //     }
            // }
            // // Copio lo patterns
            // if (!File.Exists(Config.dirPatterns))
            // {
            //     Gb.Shell("mkdir -p " + Config.dirPatterns);
            // }
            // foreach (var aFile in Directory.GetFiles("./patterns").ToArray())
            // {
            //     //TINCHO 2023.05.22 > Config.class implementation
            //     if (!File.Exists(System.IO.Path.Combine(Config.dirPatterns, aFile)))
            //     {
            //         File.Copy(System.IO.Path.Combine("./patterns", aFile), System.IO.Path.Combine(Config.dirPatterns, aFile));
            //     }

            // }
            // // Copio lo templates
            // foreach (string aFile in Directory.GecleqtFiles("./minimal").ToArray())
            // {
            //     //TINCHO 2023.05.22 > Config.class implementation
            //     File.Copy(System.IO.Path.Combine("./minimal", aFile), System.IO.Path.Combine(Config.dirTemplates, aFile));
            // }

            // External programs availability
            // libredwg
            if (File.Exists("/usr/local/lib/libredwg.so")) convLibreDWG = true;

            // ODA
            if (File.Exists("/usr/bin/ODAFileConverter")) convODA = true;

            if (Config.FileConversion == "ODA")
            {
                if (convODA)
                {
                    //nothing
                }
                else if (convLibreDWG)
                {
                    Config.FileConversion = "LibreDWG";
                }
                else
                {
                    // none available
                    Config.FileConversion = "";
                }
            }
            else if (Config.FileConversion == "LibreDWG")
            {
                if (convLibreDWG)
                {
                    //nothing
                }
                else if (convODA)
                {
                    Config.FileConversion = "ODA";
                }
                else
                {
                    // none available
                    Config.FileConversion = "";
                }
            } // none selected
            else
            {
                if (convODA)
                {
                    Config.FileConversion = "ODA";
                }
                else if (convLibreDWG)
                {
                    Config.FileConversion = "LibreDWG";
                }
            }

            //TINCHO 2023.05.22 > Config.class implementation
            //dirBlocks = Config.BlocksLibraryPath

        }

        public static void LoadPatterns()
        {


            string[] s;
           
            string spd = "";
            HatchPattern? p = null;

            spd = Config.dirPatterns;
            if (!Directory.Exists(spd)) return;
            // {
            //     Gb.Shell("mkdir -p " + spd);
            // }
            s = Gb.Dir(spd, "*.pat");
            if (s.Length == 0)
            {

                foreach (string sp in Directory.GetFiles(System.IO.Path.Combine(Config.Root, "patterns"), "*.pat"))
                {
                    // p = New HatchPattern
                    // Utils.LoadClass2(p, spd &/ sp)
                    // Gcd.HatchPatterns.Add(p, p.Name)
                    try
                    {
                        File.Copy(System.IO.Path.Combine(Config.Root + "/patterns", sp), System.IO.Path.Combine(spd, sp));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error copying pattern file: " + ex.Message);
                    }
                }

            }
            s = Directory.GetFiles(spd, "*.pat");
            foreach (string sp in s.ToList())
            {
                // p = New HatchPattern
                // Utils.LoadClass2(p, spd &/ sp)
                // Gcd.HatchPatterns.Add(p, p.Name)
                Gcd.ImportPAT(System.IO.Path.Combine(spd, sp));
            }

        }

        // public void LoadPatterns2()
        //     {


        //     string[] s ;         
        //     string sp ;         
        //     string spd ;         
        //     HatchPattern p ;         

        //     spd = Config.dirPatterns;
        //     s = Directory.GetFiles(spd).ToList();
        //     foreach ( string sp in s)
        //     {
        //         p = new HatchPattern();
        //         Utils.LoadClass2(p, System.IO.Path.Combine(spd, sp));
        //         Gcd.HatchPatterns.Add(p, p.Name);
        //     }

        // }

        // public void SavePatterns()
        //     {


        //     string[] s ;         
        //     string sp ;         
        //     HatchPattern p ;         

        //     sp = Config.dirPatterns;
        //     foreach ( p in Gcd.HatchPatterns)
        //     {
        //         Utils.SaveClass2(p, System.IO.Path.Combine(sp, p.Name));
        //     }

        // }

        // public void LoadPrintStyles()
        //     {


        //     string[] s ;         
        //     string sp ;         
        //     string spd ;         
        //     PrintStyle p ;         

        //     spd = Config.dirPrintStyles;
        //     s = Dir(spd);
        //     foreach ( sp in s)
        //     {
        //         p = new PrintStyle;
        //         Utils.LoadClass(p, spd &/ sp);
        //         Gcd.PrintStyles.Add(p, p.Name);
        //     }

        // }

        // public void SavePrintStyles()
        //     {


        //     string[] s ;         
        //     string sp ;         
        //     PrintStyle p ;         

        //     sp = Config.dirPrintStyles;
        //     foreach ( p in Gcd.PrintStyles)
        //     {
        //         Utils.SaveClass(p, sp &/ p.Name);
        //     }

        // }

        public static void InitClasses()
        // {


        //     // Class cClass ;         
        //     // string s ;         
        //     // string sFinishedClasses ;         
        //     // New String[] sSplit ;         

        //     // sFinishedClasses = "LEADER HATCH POLYLINE ENDBLK SEQEND VERTEX POINT RECTANGLE POLYGON ATTDEF ATTRIB LINE LWPOLYLINE CIRCLE ELLIPSE ARC TEXT MTEXT SPLINE SOLID INSERT DIMENSION DIMENSION_LINEAR DIMENSION_DIAMETER DIMENSION_RADIUS DIMENSION_ANG3PT DIMENSION_ALIGNED DIMENSION_ORDINATE LARGE_RADIAL_DIMENSION ARC_DIMENSION VIEWPORT ARC3POINT";

        //     // sFinishedClasses &= " AREA ARRAY BLOCKS BREAK CHAMFER COPY DIVIDE EDIT ERASE EXPLODE FILLET HATCHBUILDER ENTITYBUILDER LAYERS MIRROR MOVE MTEXTBUILDER OFFSET PAN PROTRACTOR ROTATE RULER SCALE SELECTION STRETCH TRIM ZOOME ZOOMW";
        //     // sFinishedClasses &= " MLINE";
        //     //  // smart
        //     // sFinishedClasses &= " SLAB BIMENTITYBUILDER";

        //     // sSplit = Split(sFinishedClasses, " ");

        //     // foreach ( s in sSplit)
        //     // {
        //     //      // intento crearla
        //     //     cClass = Null;
        //     //     cClass = Class.Load("cad" + s);
        //     //     if ( cClass )
        //     //     {
        //     //         Gcd.CCC.add(cClass.AutoCreate(), s);

        //     //         Debug s;
        //     //     }
        //     //     else
        //     //     {
        //     //         Console.WriteLine("WARNING: the Class " + s + " it + "\n"); //s not implemented."
        //     //     }

        //     // }

        // }

         
    {
        // Get all types in the current assembly that implement IToolsBase
        var assembly = Assembly.GetExecutingAssembly();
        var ToolTypes = assembly.GetTypes()
            .Where(type => typeof(IToolsBase).IsAssignableFrom(type) && 
                          !type.IsInterface && 
                          !type.IsAbstract &&
                          type.GetConstructor(Type.EmptyTypes) != null)
            .ToList();
        
        // Create instances and add to dictionary
        foreach (var type in ToolTypes)
        {
            try
            {
                var instance = (IToolsBase)Activator.CreateInstance(type);
                Gcd.Tools[Gb.Mid(type.Name.ToUpper(),4)] = instance;
                Console.WriteLine($"Loaded {type.Name} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {type.Name}: {ex.Message}");
            }
        }
        
        Console.WriteLine($"Total tools loaded: {Gcd.Tools.Count}\n");

         var EntityTypes = assembly.GetTypes()
            .Where(type => typeof(IEntity).IsAssignableFrom(type) && 
                          !type.IsInterface && 
                          !type.IsAbstract &&
                          type.GetConstructor(Type.EmptyTypes) != null)
            .ToList();
        
        // Create instances and add to dictionary
        foreach (var type in EntityTypes)
        {
            try
            {
                var instance = (IEntity)Activator.CreateInstance(type);
                Gcd.CCC[Gb.Mid(type.Name.ToUpper(),4)] = instance;
                Console.WriteLine($"Loaded {type.Name} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {type.Name}: {ex.Message}");
            }
        }
        
        Console.WriteLine($"Total entities loaded: {Gcd.CCC.Count}\n");
    }

        public static int RGB(int r, int g, int b)
        {
            // return (r << 16) | (g << 8) | b;
            return (b << 16) | (g << 8) | r;
        }

        public static void InitColors()
        {

            // Load CAD color into Gambas colors
            // array index is CAD color, returning Gambas int color

            Gcd.gColor.Add(RGB(0, 0, 0));
            Gcd.gColor.Add(RGB(255, 0, 0));
            Gcd.gColor.Add(RGB(255, 255, 0));
            Gcd.gColor.Add(RGB(0, 255, 0));
            Gcd.gColor.Add(RGB(0, 255, 255));
            Gcd.gColor.Add(RGB(0, 0, 255));
            Gcd.gColor.Add(RGB(255, 0, 255));
            Gcd.gColor.Add(RGB(255, 255, 255));
            Gcd.gColor.Add(RGB(128, 128, 128));
            Gcd.gColor.Add(RGB(192, 192, 192));
            Gcd.gColor.Add(RGB(255, 0, 0));
            Gcd.gColor.Add(RGB(255, 127, 127));
            Gcd.gColor.Add(RGB(165, 0, 0));
            Gcd.gColor.Add(RGB(165, 82, 82));
            Gcd.gColor.Add(RGB(127, 0, 0));
            Gcd.gColor.Add(RGB(127, 63, 63));
            Gcd.gColor.Add(RGB(76, 0, 0));
            Gcd.gColor.Add(RGB(76, 38, 38));
            Gcd.gColor.Add(RGB(38, 0, 0));
            Gcd.gColor.Add(RGB(38, 19, 19));
            Gcd.gColor.Add(RGB(255, 63, 0));
            Gcd.gColor.Add(RGB(255, 159, 127));
            Gcd.gColor.Add(RGB(165, 41, 0));
            Gcd.gColor.Add(RGB(165, 103, 82));
            Gcd.gColor.Add(RGB(127, 31, 0));
            Gcd.gColor.Add(RGB(127, 79, 63));
            Gcd.gColor.Add(RGB(76, 19, 0));
            Gcd.gColor.Add(RGB(76, 47, 38));
            Gcd.gColor.Add(RGB(38, 9, 0));
            Gcd.gColor.Add(RGB(38, 23, 19));
            Gcd.gColor.Add(RGB(255, 127, 0));
            Gcd.gColor.Add(RGB(255, 191, 127));
            Gcd.gColor.Add(RGB(165, 82, 0));
            Gcd.gColor.Add(RGB(165, 124, 82));
            Gcd.gColor.Add(RGB(127, 63, 0));
            Gcd.gColor.Add(RGB(127, 95, 63));
            Gcd.gColor.Add(RGB(76, 38, 0));
            Gcd.gColor.Add(RGB(76, 57, 38));
            Gcd.gColor.Add(RGB(38, 19, 0));
            Gcd.gColor.Add(RGB(38, 28, 19));
            Gcd.gColor.Add(RGB(255, 191, 0));
            Gcd.gColor.Add(RGB(255, 223, 127));
            Gcd.gColor.Add(RGB(165, 124, 0));
            Gcd.gColor.Add(RGB(165, 145, 82));
            Gcd.gColor.Add(RGB(127, 95, 0));
            Gcd.gColor.Add(RGB(127, 111, 63));
            Gcd.gColor.Add(RGB(76, 57, 0));
            Gcd.gColor.Add(RGB(76, 66, 38));
            Gcd.gColor.Add(RGB(38, 28, 0));
            Gcd.gColor.Add(RGB(38, 33, 19));
            Gcd.gColor.Add(RGB(255, 255, 0));
            Gcd.gColor.Add(RGB(255, 255, 127));
            Gcd.gColor.Add(RGB(165, 165, 0));
            Gcd.gColor.Add(RGB(165, 165, 82));
            Gcd.gColor.Add(RGB(127, 127, 0));
            Gcd.gColor.Add(RGB(127, 127, 63));
            Gcd.gColor.Add(RGB(76, 76, 0));
            Gcd.gColor.Add(RGB(76, 76, 38));
            Gcd.gColor.Add(RGB(38, 38, 0));
            Gcd.gColor.Add(RGB(38, 38, 19));
            Gcd.gColor.Add(RGB(191, 255, 0));
            Gcd.gColor.Add(RGB(223, 255, 127));
            Gcd.gColor.Add(RGB(124, 165, 0));
            Gcd.gColor.Add(RGB(145, 165, 82));
            Gcd.gColor.Add(RGB(95, 127, 0));
            Gcd.gColor.Add(RGB(111, 127, 63));
            Gcd.gColor.Add(RGB(57, 76, 0));
            Gcd.gColor.Add(RGB(66, 76, 38));
            Gcd.gColor.Add(RGB(28, 38, 0));
            Gcd.gColor.Add(RGB(33, 38, 19));
            Gcd.gColor.Add(RGB(127, 255, 0));
            Gcd.gColor.Add(RGB(191, 255, 127));
            Gcd.gColor.Add(RGB(82, 165, 0));
            Gcd.gColor.Add(RGB(124, 165, 82));
            Gcd.gColor.Add(RGB(63, 127, 0));
            Gcd.gColor.Add(RGB(95, 127, 63));
            Gcd.gColor.Add(RGB(38, 76, 0));
            Gcd.gColor.Add(RGB(57, 76, 38));
            Gcd.gColor.Add(RGB(19, 38, 0));
            Gcd.gColor.Add(RGB(28, 38, 19));
            Gcd.gColor.Add(RGB(63, 255, 0));
            Gcd.gColor.Add(RGB(159, 255, 127));
            Gcd.gColor.Add(RGB(41, 165, 0));
            Gcd.gColor.Add(RGB(103, 165, 82));
            Gcd.gColor.Add(RGB(31, 127, 0));
            Gcd.gColor.Add(RGB(79, 127, 63));
            Gcd.gColor.Add(RGB(19, 76, 0));
            Gcd.gColor.Add(RGB(47, 76, 38));
            Gcd.gColor.Add(RGB(9, 38, 0));
            Gcd.gColor.Add(RGB(23, 38, 19));
            Gcd.gColor.Add(RGB(0, 255, 0));
            Gcd.gColor.Add(RGB(127, 255, 127));
            Gcd.gColor.Add(RGB(0, 165, 0));
            Gcd.gColor.Add(RGB(82, 165, 82));
            Gcd.gColor.Add(RGB(0, 127, 0));
            Gcd.gColor.Add(RGB(63, 127, 63));
            Gcd.gColor.Add(RGB(0, 76, 0));
            Gcd.gColor.Add(RGB(38, 76, 38));
            Gcd.gColor.Add(RGB(0, 38, 0));
            Gcd.gColor.Add(RGB(19, 38, 19));
            Gcd.gColor.Add(RGB(0, 255, 63));
            Gcd.gColor.Add(RGB(127, 255, 159));
            Gcd.gColor.Add(RGB(0, 165, 41));
            Gcd.gColor.Add(RGB(82, 165, 103));
            Gcd.gColor.Add(RGB(0, 127, 31));
            Gcd.gColor.Add(RGB(63, 127, 79));
            Gcd.gColor.Add(RGB(0, 76, 19));
            Gcd.gColor.Add(RGB(38, 76, 47));
            Gcd.gColor.Add(RGB(0, 38, 9));
            Gcd.gColor.Add(RGB(19, 38, 23));
            Gcd.gColor.Add(RGB(0, 255, 127));
            Gcd.gColor.Add(RGB(127, 255, 191));
            Gcd.gColor.Add(RGB(0, 165, 82));
            Gcd.gColor.Add(RGB(82, 165, 124));
            Gcd.gColor.Add(RGB(0, 127, 63));
            Gcd.gColor.Add(RGB(63, 127, 95));
            Gcd.gColor.Add(RGB(0, 76, 38));
            Gcd.gColor.Add(RGB(38, 76, 57));
            Gcd.gColor.Add(RGB(0, 38, 19));
            Gcd.gColor.Add(RGB(19, 38, 28));
            Gcd.gColor.Add(RGB(0, 255, 191));
            Gcd.gColor.Add(RGB(127, 255, 223));
            Gcd.gColor.Add(RGB(0, 165, 124));
            Gcd.gColor.Add(RGB(82, 165, 145));
            Gcd.gColor.Add(RGB(0, 127, 95));
            Gcd.gColor.Add(RGB(63, 127, 111));
            Gcd.gColor.Add(RGB(0, 76, 57));
            Gcd.gColor.Add(RGB(38, 76, 66));
            Gcd.gColor.Add(RGB(0, 38, 28));
            Gcd.gColor.Add(RGB(19, 38, 33));
            Gcd.gColor.Add(RGB(0, 255, 255));
            Gcd.gColor.Add(RGB(127, 255, 255));
            Gcd.gColor.Add(RGB(0, 165, 165));
            Gcd.gColor.Add(RGB(82, 165, 165));
            Gcd.gColor.Add(RGB(0, 127, 127));
            Gcd.gColor.Add(RGB(63, 127, 127));
            Gcd.gColor.Add(RGB(0, 76, 76));
            Gcd.gColor.Add(RGB(38, 76, 76));
            Gcd.gColor.Add(RGB(0, 38, 38));
            Gcd.gColor.Add(RGB(19, 38, 38));
            Gcd.gColor.Add(RGB(0, 191, 255));
            Gcd.gColor.Add(RGB(127, 223, 255));
            Gcd.gColor.Add(RGB(0, 124, 165));
            Gcd.gColor.Add(RGB(82, 145, 165));
            Gcd.gColor.Add(RGB(0, 95, 127));
            Gcd.gColor.Add(RGB(63, 111, 127));
            Gcd.gColor.Add(RGB(0, 57, 76));
            Gcd.gColor.Add(RGB(38, 66, 76));
            Gcd.gColor.Add(RGB(0, 28, 38));
            Gcd.gColor.Add(RGB(19, 33, 38));
            Gcd.gColor.Add(RGB(0, 127, 255));
            Gcd.gColor.Add(RGB(127, 191, 255));
            Gcd.gColor.Add(RGB(0, 82, 165));
            Gcd.gColor.Add(RGB(82, 124, 165));
            Gcd.gColor.Add(RGB(0, 63, 127));
            Gcd.gColor.Add(RGB(63, 95, 127));
            Gcd.gColor.Add(RGB(0, 38, 76));
            Gcd.gColor.Add(RGB(38, 57, 76));
            Gcd.gColor.Add(RGB(0, 19, 38));
            Gcd.gColor.Add(RGB(19, 28, 38));
            Gcd.gColor.Add(RGB(0, 63, 255));
            Gcd.gColor.Add(RGB(127, 159, 255));
            Gcd.gColor.Add(RGB(0, 41, 165));
            Gcd.gColor.Add(RGB(82, 103, 165));
            Gcd.gColor.Add(RGB(0, 31, 127));
            Gcd.gColor.Add(RGB(63, 79, 127));
            Gcd.gColor.Add(RGB(0, 19, 76));
            Gcd.gColor.Add(RGB(38, 47, 76));
            Gcd.gColor.Add(RGB(0, 9, 38));
            Gcd.gColor.Add(RGB(19, 23, 38));
            Gcd.gColor.Add(RGB(0, 0, 255));
            Gcd.gColor.Add(RGB(127, 127, 255));
            Gcd.gColor.Add(RGB(0, 0, 165));
            Gcd.gColor.Add(RGB(82, 82, 165));
            Gcd.gColor.Add(RGB(0, 0, 127));
            Gcd.gColor.Add(RGB(63, 63, 127));
            Gcd.gColor.Add(RGB(0, 0, 76));
            Gcd.gColor.Add(RGB(38, 38, 76));
            Gcd.gColor.Add(RGB(0, 0, 38));
            Gcd.gColor.Add(RGB(19, 19, 38));
            Gcd.gColor.Add(RGB(63, 0, 255));
            Gcd.gColor.Add(RGB(159, 127, 255));
            Gcd.gColor.Add(RGB(41, 0, 165));
            Gcd.gColor.Add(RGB(103, 82, 165));
            Gcd.gColor.Add(RGB(31, 0, 127));
            Gcd.gColor.Add(RGB(79, 63, 127));
            Gcd.gColor.Add(RGB(19, 0, 76));
            Gcd.gColor.Add(RGB(47, 38, 76));
            Gcd.gColor.Add(RGB(9, 0, 38));
            Gcd.gColor.Add(RGB(23, 19, 38));
            Gcd.gColor.Add(RGB(127, 0, 255));
            Gcd.gColor.Add(RGB(191, 127, 255));
            Gcd.gColor.Add(RGB(82, 0, 165));
            Gcd.gColor.Add(RGB(124, 82, 165));
            Gcd.gColor.Add(RGB(63, 0, 127));
            Gcd.gColor.Add(RGB(95, 63, 127));
            Gcd.gColor.Add(RGB(38, 0, 76));
            Gcd.gColor.Add(RGB(57, 38, 76));
            Gcd.gColor.Add(RGB(19, 0, 38));
            Gcd.gColor.Add(RGB(28, 19, 38));
            Gcd.gColor.Add(RGB(191, 0, 255));
            Gcd.gColor.Add(RGB(223, 127, 255));
            Gcd.gColor.Add(RGB(124, 0, 165));
            Gcd.gColor.Add(RGB(145, 82, 165));
            Gcd.gColor.Add(RGB(95, 0, 127));
            Gcd.gColor.Add(RGB(111, 63, 127));
            Gcd.gColor.Add(RGB(57, 0, 76));
            Gcd.gColor.Add(RGB(66, 38, 76));
            Gcd.gColor.Add(RGB(28, 0, 38));
            Gcd.gColor.Add(RGB(33, 19, 38));
            Gcd.gColor.Add(RGB(255, 0, 255));
            Gcd.gColor.Add(RGB(255, 127, 255));
            Gcd.gColor.Add(RGB(165, 0, 165));
            Gcd.gColor.Add(RGB(165, 82, 165));
            Gcd.gColor.Add(RGB(127, 0, 127));
            Gcd.gColor.Add(RGB(127, 63, 127));
            Gcd.gColor.Add(RGB(76, 0, 76));
            Gcd.gColor.Add(RGB(76, 38, 76));
            Gcd.gColor.Add(RGB(38, 0, 38));
            Gcd.gColor.Add(RGB(38, 19, 38));
            Gcd.gColor.Add(RGB(255, 0, 191));
            Gcd.gColor.Add(RGB(255, 127, 223));
            Gcd.gColor.Add(RGB(165, 0, 124));
            Gcd.gColor.Add(RGB(165, 82, 145));
            Gcd.gColor.Add(RGB(127, 0, 95));
            Gcd.gColor.Add(RGB(127, 63, 111));
            Gcd.gColor.Add(RGB(76, 0, 57));
            Gcd.gColor.Add(RGB(76, 38, 66));
            Gcd.gColor.Add(RGB(38, 0, 28));
            Gcd.gColor.Add(RGB(38, 19, 33));
            Gcd.gColor.Add(RGB(255, 0, 127));
            Gcd.gColor.Add(RGB(255, 127, 191));
            Gcd.gColor.Add(RGB(165, 0, 82));
            Gcd.gColor.Add(RGB(165, 82, 124));
            Gcd.gColor.Add(RGB(127, 0, 63));
            Gcd.gColor.Add(RGB(127, 63, 95));
            Gcd.gColor.Add(RGB(76, 0, 38));
            Gcd.gColor.Add(RGB(76, 38, 57));
            Gcd.gColor.Add(RGB(38, 0, 19));
            Gcd.gColor.Add(RGB(38, 19, 28));
            Gcd.gColor.Add(RGB(255, 0, 63));
            Gcd.gColor.Add(RGB(255, 127, 159));
            Gcd.gColor.Add(RGB(165, 0, 41));
            Gcd.gColor.Add(RGB(165, 82, 103));
            Gcd.gColor.Add(RGB(127, 0, 31));
            Gcd.gColor.Add(RGB(127, 63, 79));
            Gcd.gColor.Add(RGB(76, 0, 19));
            Gcd.gColor.Add(RGB(76, 38, 47));
            Gcd.gColor.Add(RGB(38, 0, 9));
            Gcd.gColor.Add(RGB(38, 19, 23));
            Gcd.gColor.Add(RGB(0, 0, 0));
            Gcd.gColor.Add(RGB(51, 51, 51));
            Gcd.gColor.Add(RGB(102, 102, 102));
            Gcd.gColor.Add(RGB(153, 153, 153));
            Gcd.gColor.Add(RGB(204, 204, 204));
            Gcd.gColor.Add(RGB(255, 255, 255));
            Gcd.gColor.Add(RGB(255, 255, 255)); //By Layer
            Gcd.gColor.Add(RGB(255, 255, 255)); //By Block
            Gcd.gColor.Add(RGB(255, 255, 255)); //By Object?

            // corrijo los colores que no se ven contra el fondo
            Gcd.gColor[0] = Config.WhiteAndBlack;
            Gcd.gColor[7] = Config.WhiteAndBlack;
            Gcd.gColor[250] = Config.WhiteAndBlack;
            Gcd.gColor[255] = Config.WhiteAndBlack;
            Gcd.gColor[256] = Config.WhiteAndBlack;
            Gcd.gColor[257] = Config.WhiteAndBlack;
            Gcd.gColor[258] = Config.WhiteAndBlack;

        }

        

 // carga los cursores desde SVG y los coloca en Gcd
        // public static void LoadCursors()
        // {


        //     int c;
        //     string sCursor;

        //     sCursor = dsk.Contrary(System.IO.Path.Combine(Application.Path, "svg", "Cursors", "cursor1.svg"), "#0066b3", Config.ModelBackgroundColor);
        //     Gcd.CursorCross = new Cursor(Image.FromString(sCursor).Stretch(48, 48).Picture, 24, 24);

        //     sCursor = dsk.Contrary(System.IO.Path.Combine(Application.Path, "svg", "Cursors", "cursor2.svg"), "#0066b3", Config.ModelBackgroundColor);
        //     Gcd.CursorSelect = new Cursor(Image.FromString(sCursor).Stretch(48, 48).Picture, 24, 24);

        //     sCursor = dsk.Contrary(System.IO.Path.Combine(Application.Path, "svg", "Cursors", "cursor3.svg"), "#0066b3", Config.ModelBackgroundColor);
        //     Gcd.CursorSelectAdd = new Cursor(Image.FromString(sCursor).Stretch(48, 48).Picture, 24, 24);

        //     sCursor = dsk.Contrary(System.IO.Path.Combine(Application.Path, "svg", "Cursors", "cursor4.svg"), "#0066b3", Config.ModelBackgroundColor);
        //     Gcd.CursorSelectRem = new Cursor(Image.FromString(sCursor).Stretch(48, 48).Picture, 24, 24);

        //     sCursor = dsk.Contrary(System.IO.Path.Combine(Application.Path, "svg", "Cursors", "cursor5.svg"), "#0066b3", Config.ModelBackgroundColor);
        //     Gcd.CursorSelectXchange = new Cursor(Image.FromString(sCursor).Stretch(48, 48).Picture, 24, 24);

        // }

    }
}

using System.Data.Common;
using System.Runtime.CompilerServices;
using Gtk;

namespace Gaucho;

public partial class fMain
{
    public Button CreateButton(string tag, string name, int iconSize = 24, string? tooltip = null)
    {
        var shortname = Gb.Mid(name, 4).ToLower();
        var svgPath1 = Config.dirResources + $"/svg" + $"/Gray" + $"/{shortname}.svg";
        var p = Gb.InStr(tag, "-");
        var tagname = Gb.Mid(tag, p + 1).ToLower();
        var svgPath2 = Config.dirResources + $"/svg" + $"/Gray" + $"/{tagname}.svg";
        var svgPath = File.Exists(svgPath1) ? svgPath1 : svgPath2;

        Console.WriteLine($"Creating button '{name}' with icon from: {svgPath}");
        var image = Image.NewFromFile(svgPath);
        image.SetPixelSize(iconSize);

        var button = Button.New();
        button.SetChild(image);

        if (!string.IsNullOrEmpty(tooltip))
        {
            button.SetTooltipText(tooltip);
        }

        button.SetName(name);
        button.OnClicked += (_, _) => ActionActivate($"cad:{name}");

        return button;
    }

    public void FillGridWithButtons(Gtk.Grid grid, (string tag, string Name, string Tooltip)[] buttons, int cols = 4)
    {
        int i = -1;
        foreach (var (tag, name, tooltip) in buttons)
        {
            i++;

            var button = CreateButton(tag, name, Config.ButtonSize, tooltip);
            grid.Attach(button, i % cols, i / cols, 1, 1);
        }

    }


    private void BuildUI()
    {
        _mainBox = Box.New(Orientation.Vertical, 0);
        _mainBox.SetSpacing(6);
        _mainBox.MarginTop = 12;
        _mainBox.MarginBottom = 12;
        _mainBox.MarginStart = 12;
        _mainBox.MarginEnd = 12;
        _mainBox.SetHexpand(true);
        _mainBox.SetVexpand(true);

        // Top button bar
        var topButtonBar = Box.New(Orientation.Horizontal, 6);
        topButtonBar.AddCssClass("toolbar");
        topButtonBar.SetHexpand(true);

        var newButton = Button.NewWithMnemonic("_New");
        newButton.OnClicked += (_, _) => ActionActivate("new");

        var openButton = Button.NewWithMnemonic("_Open");
        openButton.OnClicked += (_, _) => ActionActivate("open");

        var openHistoryButton = CreateOpenHistoryMenuButton();

        var saveButton = Button.NewWithMnemonic("_Save");
        saveButton.OnClicked += (_, _) => ActionActivate("save");

        var exportButton = Button.NewWithMnemonic("_Export");

        var quickAccessList = CreateTopBarQuickList();

        topButtonBar.Append(CreateButton("prog-new", "new", Config.ButtonSize, Trans.Translate("New document")));
        topButtonBar.Append(CreateButton("prog-open", "open", Config.ButtonSize, Trans.Translate("Open document")));
        topButtonBar.Append(openHistoryButton);
        // topButtonBar.Append(quickAccessList);
        topButtonBar.Append(CreateButton("prog-save", "save", Config.ButtonSize, Trans.Translate("Save document")));
        topButtonBar.Append(CreateButton("prog-export", "export", Config.ButtonSize, Trans.Translate("Export document")));
        topButtonBar.Append(Separator.New(Gtk.Orientation.Vertical));
        topButtonBar.Append(CreateButton("prog-print", "print", Config.ButtonSize, Trans.Translate("Print document")));
        topButtonBar.Append(CreateButton("prog-config", "config", Config.ButtonSize, Trans.Translate("Configure document")));
        topButtonBar.Append(Separator.New(Gtk.Orientation.Vertical));
        topButtonBar.Append(CreateButton("prog-cadZoomW", "cadZoomW", Config.ButtonSize, Trans.Translate("Zoom Window")));
        topButtonBar.Append(CreateButton("prog-cadZoomE", "cadZoomE", Config.ButtonSize, Trans.Translate("Zoom Extents")));
        topButtonBar.Append(Separator.New(Gtk.Orientation.Vertical));
        topButtonBar.Append(CreateButton("prog-undo", "undo", Config.ButtonSize, Trans.Translate("Undo last action")));
        topButtonBar.Append(CreateButton("prog-redo", "redo", Config.ButtonSize, Trans.Translate("Redo last action")));
        topButtonBar.Append(Separator.New(Gtk.Orientation.Vertical));
        topButtonBar.Append(CreateButton("prog-cadLayers", "cadLayers", Config.ButtonSize, Trans.Translate("Layers management")));
        var Layerslist = DropDown.NewFromStrings(new string[] { "Layer 1", "Layer 2", "Layer 3" });
        Layerslist.SetTooltipText("Select Layer");
        topButtonBar.Append(Layerslist);
        topButtonBar.Append(CreateButton("prog-colors", "colors", Config.ButtonSize, Trans.Translate("Select Color")));
        topButtonBar.Append(CreateButton("prog-linetype", "linetype", Config.ButtonSize, Trans.Translate("Select Line Type")));
        topButtonBar.Append(CreateButton("prog-linewt", "linewt", Config.ButtonSize, Trans.Translate("Select Line Weight")));
        topButtonBar.Append(CreateButton("prog-dimstyle", "dimstyle", Config.ButtonSize, Trans.Translate("Select Dimension Style")));
        topButtonBar.Append(Separator.New(Gtk.Orientation.Vertical));
        topButtonBar.Append(CreateButton("prog-about", "about", Config.ButtonSize, Trans.Translate("About")));


        // Left toolbar with sectors
        var sectorFrame = Frame.New("Sectors");
        var sectorScroll = ScrolledWindow.New();
        sectorScroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        var entitiesGrid = Grid.New();
        entitiesGrid.SetColumnSpacing(4);
        entitiesGrid.SetRowSpacing(4);
        entitiesGrid.MarginTop = 12;
        entitiesGrid.MarginBottom = 12;
        entitiesGrid.MarginStart = 12;
        entitiesGrid.MarginEnd = 12;
        entitiesGrid.SetHexpand(true);
        var GridDimensions = Grid.New();
        GridDimensions.SetColumnSpacing(4);
        GridDimensions.SetRowSpacing(4);
        GridDimensions.MarginTop = 12;
        GridDimensions.MarginBottom = 12;
        GridDimensions.MarginStart = 12;
        GridDimensions.MarginEnd = 12;
        GridDimensions.SetHexpand(true);
        // var BoxDimensions = Box.New(Orientation.Vertical, 4);
        var BoxBlocks = Grid.New();
        var BoxTools = Grid.New();
        var BoxInquiry = Grid.New();
        var BoxSnap = Grid.New();


        // Entidades
        var entityButtons = new (string tag, string Name, string Tooltip)[]
            {
            ( "cad/draw-line", "cadLine", Trans.Translate ("Line")),
            ( "cad/draw-ray", "cadRay", Trans.Translate ("Ray")),
            ( "cad/draw-circle", "cadCircle", Trans.Translate ("Circle")),
            ( "cad/draw-arc", "cadArc", Trans.Translate ("Arc")),
            ( "cad/draw-arc", "cadArc3point", Trans.Translate ("Arc 3 point")),
            ( "cad/draw-polyline", "cadLWPolyline", Trans.Translate ("Polyline")),
            ( "cad/draw-ellipse", "cadEllipse", Trans.Translate ("Ellipse")),
            ( "cad/draw-text", "cadMText", Trans.Translate ("Text")),
            ( "cad/alter-attdef", "cadAttdef", Trans.Translate ("Attdef")),
            ( "cad/draw-mtext", "cadMtext", Trans.Translate ("M-Text")),
            ( "cad/draw-dimension", "caddimension", Trans.Translate ("Dimension")),
            ( "cad/draw-leader", "cadLeader", Trans.Translate ("Leader")),
            ( "cad/draw-spline", "cadSPline", Trans.Translate ("Spline")),
            ( "cad/draw-rectangle", "cadRectangle", Trans.Translate ("Rectangle")),
            ( "cad/draw-polygon", "cadPolygon", Trans.Translate ("Polygon")),
            ( "cad/draw-point", "cadPoint", Trans.Translate ("Point")),
            ( "cad/draw-solid", "cadSolid", Trans.Translate ("Solid")),
            ( "cad/draw-pattern", "cadHatch", Trans.Translate ("Pattern"))
            };

        //Dimensiones
        var dimButtons = new (string tag, string Name, string Tooltip)[]
            {
            ( "cad/dim-linear", "cadDimension_Linear",Trans.Translate("Linear dimmension")),
            ( "cad/dim-aligned", "caddimension_aligned",Trans.Translate("Aligned dimmension")),
            ( "cad/dim-ang3pt", "caddimension_ang3pt",Trans.Translate("Angle dimmension")),
            ( "cad/dim-diameter", "caddimension_diameter",Trans.Translate("Diameter dimmension")),
            ( "cad/dim-radius", "caddimension_radius",Trans.Translate("Radius dimmension")),
            ( "cad/dim-ordinate", "caddimension_ordinate",Trans.Translate("Ordinate dimmension")),
            ( "cad/dim-arc", "cadArc_dimension",Trans.Translate("Arc dimmension")),
            ( "cad/dim-linear_h", "cadDimension_Linear&1",Trans.Translate("Horizontal dimmension")),
            ( "cad/dim-linear_v", "cadDimension_Linear&2",Trans.Translate("Vertical dimmension")),
            ( "cad/dim-baseline", "cadDimension_linear&20",Trans.Translate("Baseline dimmension")),
            ( "cad/dim-continue", "cadDimension_linear&10",Trans.Translate("Continue dimmension"))
            };

        // Bloques
        var blockButtons = new (string tag, string Name, string Tooltip)[]
        {
            ("cad/alter-complex", "cadblocks", Trans.Translate("Blocks")),
            ("cad/draw-pattern","cadhatch", Trans.Translate("Pattern")),
            ("cad/set-sheet-add","new_sheet", Trans.Translate(" sheet")),
            ("cad/set-viewport", "cadViewport", Trans.Translate("Viewport"))
        };

        //Tools
        var toolButtons = new (string tag, string Name, string Tooltip)[]
            {
            ( "cad/alter-copy", "cadcopy",Trans.Translate("Copy")),
            ( "cad/alter-move", "cadmove",Trans.Translate("Move")),
            ( "cad/alter-delete", "caderase",Trans.Translate("Delete")),
            ( "cad/alter-rotate", "cadrotate",Trans.Translate("Rotate")),
            ( "cad/alter-stretch", "cadstretch",Trans.Translate("Stretch")),
            ( "cad/alter-scale", "cadscale",Trans.Translate("Scale")),
            ( "cad/alter-fillet", "cadfillet",Trans.Translate("Fillet")),
            ( "cad/alter-chamfer", "cadchamfer",Trans.Translate("Chamfer")),
            ( "cad/alter-explode", "cadExplode",Trans.Translate("Explode")),
            ( "cad/alter-trim", "cadtrim",Trans.Translate("Trim")),
            ( "cad/alter-array", "cadarray",Trans.Translate("Array")),
            ( "cad/alter-break", "cadbreak",Trans.Translate("Break")),
            ( "cad/alter-divide", "caddivIde",Trans.Translate("Divide")),
            ( "cad/alter-mirror", "cadMirror",Trans.Translate("Mirror")),
            ( "cad/alter-offset", "cadoffset",Trans.Translate("Offset"))
            };

        // Info
        var infoButtons = new (string tag, string Name, string Tooltip)[]
            {
            ( "cad/measure-ruler", "cadRuler",Trans.Translate("Distance measure")),
            ( "cad/measure-protractor", "cadProtractor",Trans.Translate("Angle measure")),
            ( "cad/measure-area", "cadArea",Trans.Translate("Area measure")),
            ( "cad/alter-properties", "properties",Trans.Translate("Alter properties"))
            };
        // rigth bar
        // Utils.tbnCreator(pnlToolBarRigth, "tbnNewSelection", "selectnew", Trans.Translate ("New selection"))
        //Utils.tbnCreator(pnlToolBarRigth, "cad/select", "selectnew", Trans.Translate ("New selection"), False)

        // Estos botones estan al pedo, Ctrl agrega y Shift quita de la seleccion
        //Utils.tbnCreator(pnlToolBarRigth, "tbnselectadd", "selectadd", Trans.Translate ("Add to selection"))
        //Utils.tbnCreator(pnlToolBarRigth, "tbnselectrem", "selectrem", Trans.Translate ("Remove from selection"))

        // DEPRE: vuelvo a sentido de seleccion, izq crossing, derecha full
        // Utils.tbnCreator(pnlToolBarRigth, "tbnselectcross", "selectcross", Trans.Translate ("Select crossing entities"), True)

        var topButtons = new (string tag, string Name, string Tooltip)[]
            {
            ( "cad/select", "selectnew",Trans.Translate (" Selection") ),
            ( "cad/select-polyline", "selectpoly",Trans.Translate ("Select with polyline")),
            ( "cad/locks-grid", "grid",Trans.Translate ("Toggle Grid")),
            ( "cad/locks-ortho", "ortho",Trans.Translate ("Toggle Ortho"))
            };



        // Snap points
        var snapButtons = new (string tag, string Name, string Tooltip)[]
            {

            ("cad/snap-apparent_intersection", "snapapparentintersection",Trans.Translate ("Snap apparent intersection")),
            ("cad/snap-center_point", "snapcenter",Trans.Translate ("Snap center")),
            ("cad/snap-end_point", "snapend",Trans.Translate ("Snap end point")),
            ("cad/snap-geometric_center", "snapaparentcenter",Trans.Translate ("Snap apparent center")),
            ("cad/snap-insert", "snapbase",Trans.Translate ("Snap insert point")),
            ("cad/snap-intersection", "snapintersection",Trans.Translate ("Snap intersection")),
            ("cad/snap-mid_point", "snapmid",Trans.Translate ("Snap mid point")),
            ("cad/snap-nearest", "snapnearest",Trans.Translate ("Snap nearest")),
            ("cad/snap-node", "snapnode",Trans.Translate ("Snap node")),
            ("cad/snap-perpendicular", "snapperpendicular", Trans.Translate ("Snap perpendicular")),
            ("cad/snap-quadrant", "snapquadrant", Trans.Translate ("Snap quadrant")),
            ("cad/snap-tangent", "snaptangent", Trans.Translate ("Snap tangent"))
            };

        FillGridWithButtons(entitiesGrid, entityButtons, 4);
        FillGridWithButtons(GridDimensions, dimButtons, 4);
        FillGridWithButtons(BoxBlocks, blockButtons, 4);
        FillGridWithButtons(BoxTools, toolButtons, 4);
        FillGridWithButtons(BoxInquiry, infoButtons, 4);
        FillGridWithButtons(BoxSnap, snapButtons, 4);

        var sectorContent = Box.New(Orientation.Vertical, 12);
        sectorContent.Append(entitiesGrid);
        sectorContent.Append(GridDimensions);
        sectorContent.Append(BoxBlocks);
        sectorContent.Append(BoxTools);
        sectorContent.Append(BoxInquiry);
        sectorContent.Append(BoxSnap);

        sectorScroll.SetChild(sectorContent);
        sectorScroll.SetVexpand(true);
        sectorFrame.SetChild(sectorScroll);
        sectorFrame.SetSizeRequest(200, -1);
        sectorFrame.SetVexpand(true);

        // Center notebook with OpenGL drawing areas
        _viewportNotebook = Notebook.New();
        _viewportNotebook.ShowBorder = true;
        _viewportNotebook.TabPos = PositionType.Top;
        _viewportNotebook.SetHexpand(true);
        _viewportNotebook.SetVexpand(true);

        var glAreaPrimary = CreateViewport("Viewport 1");
        var glAreaSecondary = CreateViewport("Viewport 2");

        _viewportNotebook.AppendPage(glAreaPrimary, Label.New("Viewport 1"));
        _viewportNotebook.AppendPage(glAreaSecondary, Label.New("Viewport 2"));

        glArea = glAreaPrimary;

        // Right property grid
        var propertyFrame = Frame.New("Properties");
        var propertyGrid = Grid.New();
        propertyGrid.MarginTop = 12;
        propertyGrid.MarginBottom = 12;
        propertyGrid.MarginStart = 12;
        propertyGrid.MarginEnd = 12;
        propertyGrid.SetRowSpacing(6);
        propertyGrid.SetColumnSpacing(12);

        void AddPropertyRow(int row, string name, string value)
        {
            var nameLabel = Label.New(name + ":");
            nameLabel.Halign = Align.End;
            var valueEntry = Entry.New();
            valueEntry.SetText(value);
            valueEntry.SetHexpand(true);

            propertyGrid.Attach(nameLabel, 0, row, 1, 1);
            propertyGrid.Attach(valueEntry, 1, row, 1, 1);
        }

        AddPropertyRow(0, "Name", "Untitled");
        AddPropertyRow(1, "Type", "Scene");
        AddPropertyRow(2, "Version", "1.0.0");
        AddPropertyRow(3, "Last Modified", DateTime.Now.ToShortDateString());

        propertyFrame.SetChild(propertyGrid);
        propertyFrame.SetSizeRequest(260, -1);
        propertyFrame.SetVexpand(true);

        // Combine center notebook and property grid
        var centerAndProperties = Paned.New(Orientation.Horizontal);
        centerAndProperties.SetStartChild(_viewportNotebook);
        centerAndProperties.SetResizeStartChild(true);
        centerAndProperties.SetShrinkStartChild(false);
        centerAndProperties.SetEndChild(propertyFrame);
        centerAndProperties.SetResizeEndChild(false);
        centerAndProperties.SetShrinkEndChild(false);
        centerAndProperties.SetHexpand(true);
        centerAndProperties.SetVexpand(true);

        // Combine left toolbar with center area
        var mainPaned = Paned.New(Orientation.Horizontal);
        mainPaned.SetStartChild(sectorFrame);
        mainPaned.SetResizeStartChild(false);
        mainPaned.SetShrinkStartChild(false);
        mainPaned.SetEndChild(centerAndProperties);
        mainPaned.SetResizeEndChild(true);
        mainPaned.SetShrinkEndChild(false);
        mainPaned.SetHexpand(true);
        mainPaned.SetVexpand(true);

        // Status bar
        var statusBar = Box.New(Orientation.Horizontal, 6);
        statusBar.AddCssClass("toolbar");
        statusBar.MarginTop = 6;
        statusBar.SetHexpand(true);
        _statusLabel = Label.New("Ready");
        _statusLabel.Halign = Align.Start;
        _statusLabel.SetHexpand(true);

        statusBar.Append(_statusLabel);

        // Assemble main layout
        _mainBox.Append(topButtonBar);
        _mainBox.Append(mainPaned);
        _mainBox.Append(statusBar);

        mainPaned.SetHalign(Align.Fill);
        mainPaned.SetValign(Align.Fill);
        mainPaned.SetVexpand(true);

        SetChild(_mainBox);
    }

    // fixmme: revisar menus
    
    // public static void InitMenus()
    //     {


    //         // colores
    //         void Menu(fMain)
    //         {

    //             Menu mItem;

    //             int i;
    //             int iColor;

    //             string sMenuPre;
    //             string sMenuPost;
    //             string sMenuSnap;

    //             mColors.Name = "mColores";

    //             //============ByLayer======================
    //             i = 256;
    //             iColor = Gcd.gColor[i];
    //             mItem = new Menu(mColors);
    //             mItem.Text = "ByLayer";
    //             mItem.Action = "Color_256"; // & CStr(i)
    //             mItem.Picture = paintPlus.picCirculito(8, Gcd.gColor[256], Colors.Blue);

    //             //============Byblock======================
    //             i = 257;
    //             mItem = new Menu(mColors);
    //             mItem.Text = "ByBlock";
    //             mItem.Action = "Color_257"; // & CStr(i)
    //             mItem.Picture = paintPlus.picCirculito(8, Gcd.gColor[i], Colors.Red);

    //             //============separator======================
    //             mItem = new Menu(mColors);
    //             mItem.Text = "";
    //             mItem.Action = ""; // & CStr(i)

    //             for (i = 0; i <= 10; i + 1) //Gcd.gColor.Max
    //             {
    //                 mItem = new Menu(mColors);
    //                 mItem.Text = "Color " + CStr(i);
    //                 mItem.Action = "Color_" + CStr(i);
    //                 mItem.Picture = paintPlus.picCirculito(8, Gcd.gColor[i], Colors.Red);

    //             }
    //             //============separator======================
    //             mItem = new Menu(mColors);
    //             mItem.Text = "";
    //             mItem.Action = ""; // & CStr(i)

    //             //============more colors======================
    //             mItem = new Menu(mColors);
    //             mItem.Text = ("more colors...");
    //             mItem.Action = "more_colors"; // & CStr(i)

    //             fMain.mbtcolors.text = mColors.Children[0].Text;
    //             fMain.mbtColors.Picture = mColors.Children[0].Picture;
    //             fMain.mbtColors.Menu = mColors.Name;
    //             fMain.mbtColors.Tag = mColors.Children[0].Tag;
    //             fMain.mbtColors.Action = "Color_256";

    //             // Menu contextual de entidades
    //             // colores
    //             void Menu(fMai)
    //             {


    //                 mEntities.Name = "mEntities";
    //                 //fMain.PopupMenu = "mEntities"
    //                 // Cortar/Copiar/Pegar/Propiedades/Apagar sus layers/Hacer actual ese layer
    //                 //============Cut======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Cut");
    //                 mItem.Action = "mEntities-Cut";
    //                 mItem.Picture = Picture["icon:/32/cut"];
    //                 //============Copy======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Copy");
    //                 mItem.Action = "mEntities-Copy";
    //                 mItem.Picture = Picture["icon:/32/copy"];
    //                 //============Paste======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Paste");
    //                 mItem.Action = "mEntities-Paste";
    //                 mItem.Picture = Picture["icon:/32/paste"];
    //                 //============separator======================
    //                 mItem = new Menu(mColors);
    //                 mItem.Text = "";
    //                 mItem.Action = "";
    //                 //============Agrupar======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Make group");
    //                 mItem.Action = "mEntities-Group";
    //                 mItem.Picture = Picture["icon:/32/paste"];
    //                 //============Desagrupar======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Break group");
    //                 mItem.Action = "mEntities-DeGroup";
    //                 mItem.Picture = Picture["icon:/32/paste"];
    //                 //============separator======================
    //                 mItem = new Menu(mColors);
    //                 mItem.Text = "";
    //                 mItem.Action = ""; // & CStr(i)
    //                                    //============Ocultar layers de todo=====
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("HIde these Layers");
    //                 mItem.Action = "mEntities-HIdeLayers";
    //                 mItem.Picture = Picture["icon:/32/paste"];
    //                 //============Desagrupar======================
    //                 mItem = new Menu(mEntities);
    //                 mItem.Text = ("Paste");
    //                 mItem.Action = "mEntities-Paste";
    //                 mItem.Picture = Picture["icon:/32/paste"];
    //                 //fMain.PopupMenu = "mEntities"

    //                 //===================================================================================================================================
    //                 // Armo el menu contextual de cada ENTIDAD
    //                 Gcd.SnapMode = Config.SnapModeSaved;
    //                 sMenuSnap = ";Snap to...;;<nothing>;_nothing;4;;4;End point;_end;end_point;4;MId point;_mId;mId_point;4;Perpendicular;_per;perpendicular;4";
    //                 sMenuSnap += ";Quadrant;_qua;quadrant;4";
    //                 sMenuSnap += ";Center;_cen;center_point;4";
    //                 sMenuSnap += ";Intersection;_int;intersection;4";
    //                 sMenuSnap += ";Tangent;_tan;tangent;4";
    //                 sMenuSnap += ";Nearest;_nea;nearest;4";
    //                 sMenuSnap += ";Base point;_bas;base_point;4";

    //                 sMenuPost = (";;;Cancel;_CANCEL;");

    //                 String[] stxMenus = [];

    //                 foreach (var vClass in Gcd.CCC)
    //                 {
    //                     Utils.MenuMakerPlus(fmain, vClass.gender, vClass.contextmenu + sMenuSnap + sMenuPost, Gcd.dirResources &/ "svg" &/ Config.IconFamily);
    //                 }

    //             }
    //         }
    //     }




}

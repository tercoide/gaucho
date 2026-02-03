namespace Gaucho;

public class cadSelection : ToolsBase, IToolsBase
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

 // Tool maintained by Terco

 // this is the Main Job, either we are doing this or other job
 // Create Static
public string Gender { get; } = "SELECT";

public static string[] EntityType = [];         

public static Grip GripPoint =new Grip();         
public static Grip GripHovered = new Grip();
    public static bool GripCopying = false;
 //Public GripCopying As Boolean = false

public static double dX = 0.0;         
public static double dY = 0.0;         
 //Public ToolActive As Boolean = false
public static bool PanActive = false;
public static bool GripActive = false;
public static bool RectActive = false;



public static bool ReturnOnFirstSelection = true;

 // mas nuevo
public static int SelectType = 1;
const int SelectTypeSingleAndRect = 1;
const int SelectTypeSingle = 0;
const int SelectTypeRect = 2;
const int SelectTypePoly = 3;
const int SelectTypePoint = 4;

public static int SelectMode = 0;
public const int SelectModeNew = 0;
public const int SelectModeAdd = 1;
public const int SelectModeRem = 2;

public static bool SelectCrossing = false;                 // las entidades puedn estar parcialmente dentro del contorno

public static List<double> SelectionPoly = new();         

 // Funcionamiento:

 // ToolActive: significa que otra Tool (Copy, Move, etc) le pidio a esta que modifique la seleccion de entidas

 // A MouseDown
 // A.1 Izquierdo
 // A.1.1 No estoy sobre ninguna entidad -> Inicio una seleccion rectangular ActionActive = ActionRectActive
 // A.1.2 Estoy sobre una entidad
 // A.1.2.1 No esta seleccionada -> seleccionar
 // A.1.2.2 Esta seleccionada previamente
 // A.1.2.2.1 Estoy sobre un grip -> Inicio edicion por grip ActionActive = ActionGripActive (solo si ToolActive=false)
 // A.1.2.2.2 No estoy sobre un grip -> deseleccionar
 // A.2 Derecho
 // A.2.1 ToolActive = false?
 // A.2.1.1 No tengo ninguna seleccion previa-> Repito el ultimo comando, independientemente de si estoy sobre una entidad o no
 // A.2.1.2 Tengo una seleccion previa -> Menu: Cortar, Copiar, Agrupar, Desagrupar, Llevar al layer actual, etc
 // A.2.2 ToolActive = true? -> Finalizo la seleccion y vuelvo a la Tool
 // A.3 Medio -> Inicio el Paneo ActionActive = ActionPanActive

 // B MouseMove
 // B.1 ActionActive = ActionRectActive: actualizo coordenadas del punto final del Rect
 // B.2 ActionActive = ActionGripActive: modifico la entidad con la nueva posicion del punto
 // B.3 ActionActive = ActionPanActive: mando la coordenada a cadPan
 // B.4 ActionActive = 0: nada

 // C MouseUp
 // C.1 Izquierdo
 // C.1.1 ActionActive = ActionRectActive -> Finalizo la seleccion por recuadro
 // C.1.2 ActionActive = ActionGripActive -> Finalizo la edicion por grips
 // C.1.3 ActionActive = ActionPanActive -> nada
 // C.1.4 ActionActive = 0
 // C.2 Derecho -> nada
 // C.3 Medio -> ActionActive = ActionPanActive -> finalizo el paneo

public bool Start(string ElemToBuild, int _Mode= 2)
    {

     // Modes:
     //       0 = Move, all points in the element must be selected, or click on it.
     //       1 = Stretch, selection may be partial, each element is called to see if the support stretching

    Mode = _Mode;

    Prompt = ("Selected") + " " + (Gcd.Drawing.Sheet.EntitiesSelected.Count).ToString() + " " + ("elements") + " " + ("New/Add(Ctrl)/Remove(Shft)/Previous selection");

     // If Mode = ModeNewSelection Then
     //     fMain.PopupMenu = ""                    // no hay menu contextual
     //     Prompt = "New selection"
     //     If Gcd.clsJobPrevious.gender = cadEntityBuilder.gender Then
     //         If cadEntityBuilder.LastEntity Then
     //             Prompt &= " or <rigth click> to repeat " & cadEntityBuilder.LastEntity.gender
     //         End If
     //     End If
     // Else If Mode = ModeAddEntities Then
     //     Prompt = "Add entities to selection"
     // Else If Mode = ModeRemoveEntities Then
     //     Prompt = "Remove entities from selection"
     //
     // Else
     //     Utils.MenuMaker(fMain, "mToolsBase", ContextMenu)
     //     //Gcd.Drawing.Sheet.GlSheet.PopupMenu = "mToolsBase"  // TODO: ver si damos soporte a menus
     // End If
    Gcd.Drawing.iEntity.Clear();
    PoiChecking = true;
    Gcd.DrawHoveredEntity = true;
    GripPoint = null;
    return true;

}

 // Public Sub DblClick()
 //
 //     Dim k As Single
 //     Dim e As Entity
 //     Dim te As Entity
 //
 //
 //
 //     EntityForEdit = clsMouseTracking.CheckAboveEntity(Gcd.Xreal(Mouse.X), Gcd.Yreal(Mouse.Y))
 //     Return
 //
 //     If Not Gcd.flgSearchingPOI Then
 //         Gcd.Drawing.iEntity = clsMouseTracking.CheckBestPOI(Gcd.Xreal(Mouse.X), Gcd.Yreal(Mouse.Y))
 //     Else    // estoy buscando, pero me movi, asi que me desengancho del POI anterior
 //
 //         Gcd.Drawing.iEntity[0] = Gcd.Xreal(Mouse.X)
 //         Gcd.Drawing.iEntity[1] = Gcd.Yreal(Mouse.Y)
 //         Gcd.Drawing.iEntity[2] = -1                 // POI type
 //
 //     End If
 //
 //     If Gcd.Drawing.iEntity[3] >= 0 Then
 //
 //         //Stop
 //         // I comment the abobe line because its stop my tool also. What is the idea whit stop?
 //         //  aca podes lanzar tu editor de texto u otras propiedades
 //         k = Gcd.Drawing.iEntity[3]
 //         e = Gcd.Drawing.Entities[k]
 //
 //         // Select e.Gender
 //         //   Case "Text"
 //         //     If EditingText = false Then
 //         //       // Copying the entity for undo
 //         //       te = clsEntities.ClonEntity(e)
 //         //       te.Handle = e.Handle
 //         //       ftx = New FText([pnlDrawing.ScreenX + 7, pnlDrawing.ScreenY + pnlDrawing.H - 7], e)
 //         //       ftx.Run()
 //         //       While EditingText = false
 //         //         Wait 0.1
 //         //       Wend
 //         //       Gcd.regen
 //         //       EditingText = false
 //         //     Endif
 //         // End Select
 //     Endif
 //
 // End

public  void MouseDown()
    {


    int i ;         
    Entity e ;         

     //Return
    if ( RectActive ) return;

     //GripPoint = -1
    SelStartX = Mouse.X;
    SelStartY = Mouse.Y;
    SelStartXr = Gcd.Xreal(SelStartX);
    SelStartYr = Gcd.Yreal(SelStartY);

    dX = SelStartXr;
    dY = SelStartYr;

    SelEndX = SelStartX;
    SelEndY = SelStartY;

    SelEndXr = SelStartXr;
    SelEndYr = SelStartYr;

    PoiChecking = false;
    if ( Mouse.Right ) return;
    if ( Gcd.clsJobCallBack != null) return;
    if ( RectActive ) return;

     // A.1 Izquierdo
    if ( Mouse.Left )
    {

            // veo si esta en un grip(solo si ToolActive = false)

            // chequeo si estoy sobre un grip
            if (AllowGripEdit) 
                {
                GripPoint = FindGrip(SelStartXr, SelStartYr);
                }
            else
                {
                GripPoint = null;
            }
        if ( GripPoint != null )
        {

             // creo una entidad al vuelo, clonada de la engripada
            OriginalEntityForEdit = GripPoint.AsociatedEntity;
            EntityForEdit = clsEntities.ClonEntity(GripPoint.AsociatedEntity, false);
            GripPoint.AsociatedEntity = EntityForEdit;
            Gcd.Drawing.Sheet.SkipSearch = GripPoint.AsociatedEntity;

             // rectifico la posicion del punto
            SelStartXr = GripPoint.X;
            SelStartYr = GripPoint.Y;

            return;
        } //Or ModeRectSelection Then
        else if ( Gcd.Drawing.HoveredEntity != null)
        {

             // // A.1.2 Estoy sobre una entidad
             //
             // If Not Gcd.Drawing.Sheet.EntitiesSelected.ContainsKey(Gcd.Drawing.HoveredEntity.id) Then
             //     // A.1.2.1 No esta seleccionada -> seleccionar
             //     Gcd.Drawing.Sheet.EntitiesSelected.add(Gcd.Drawing.HoveredEntity, Gcd.Drawing.HoveredEntity.id)
             //     Gcd.Drawing.Sheet.Grips.Clear
             //     Gcd.CCC[Gcd.Drawing.HoveredEntity.Gender].generategrips(Gcd.Drawing.HoveredEntity)
             //     clsEntities.glGenDrawListSel
             //     fProps.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
             //     Gcd.Redraw
             // Else
             //     // A.1.2.2 Esta seleccionada previamente
             //     // A.1.2.2.2 No estoy sobre un grip -> deseleccionar
             //     Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.id)
             //     fprops.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
             //     clsEntities.glGenDrawListSel
             //     Gcd.Redraw
             // End If

        }

        return; // este return es para evitar clicks simultaneos
    }

    if ( Mouse.Middle )
    {
         // A.3 Medio -> Inicio el Paneo ActionActive = ActionPanActive

        return; // este return es para evitar clicks simultaneos
    }

     //         If Gcd.Drawing.Sheet.Viewport Then
     //
     //             // si el click esta fuera del viewport, lo desestimo
     //             If SelStartXr < Gcd.Drawing.Sheet.Viewport.X0 Or SelStartXr > Gcd.Drawing.Sheet.Viewport.X1 Or SelStartYr < Gcd.Drawing.Sheet.Viewport.Y0 Or SelStartYr > Gcd.Drawing.Sheet.Viewport.Y1 Then
     //                 Gcd.Drawing.Sheet.Viewport = null // Desactivo el viewport
     //             Else
     //
     //                 Gcd.Drawing.GLAreaInUse.Mouse = Mouse.SizeAll
     //                 active = true
     //             End If
     //         Else
     //
     //
     //
     //             If Mode = ModeRectSelection Then
     //                 Active = true
     //
     //             Else
     //
     //
     //                 If Gcd.Drawing.HoveredEntity.selected Then
     //
     //
     //                 Else
     //                     clsEntities.SelectElem(Gcd.Drawing.HoveredEntity)                 //   -> la selecciono
     //
     //                 Endif
     //                 Active = false
     //
     //                 clsEntities.GLGenDrawListSel(0)
     //
     //
     //             End If
     //         End If
     //     End If
     // End If

}

public void MouseUp()
    {


    string s ;         
    string tipo ;         
    // double t = Timer;
    Entity e ;         
    Dictionary<string, Entity> cSel = new Dictionary<string, Entity>();

    Gcd.Drawing.iEntity.Clear();
    Gcd.Drawing.Sheet.SkipSearch = null;
    Gcd.Drawing.LastPoint.Clear();
     // If Gcd.Drawing.Sheet.Viewport Then
     //     //
     //     Gcd.Drawing.GLAreaInUse.Mouse = Mouse.Cross
     //     Gcd.flgNewPosition = true
     //     active = false
     //     Return
     // End If

    tipo = "new";
    if ( Key.Shift || SelectMode == SelectModeRem ) tipo = "rem"; // estos elementos de la seleccion anterior
    if ( Key.Control || SelectMode == SelectModeAdd ) tipo = "add"; // elementos a la seleccion anterior
     //

    if ( Mouse.Left )
    {
         // C.1 Izquierdo

         // determino que hacer con la seleccion
        if ( Gcd.clsJobCallBack !=null && ReturnOnFirstSelection && SelectType == SelectTypePoint )
        {
            Gcd.clsJob = Gcd.clsJobCallBack;
            Gcd.clsJobCallBack = null;
            Gcd.clsJob.Run();
            return;
        }

        PoiChecking = true;
        if ( RectActive )
        {

            if ( SelectType == SelectTypePoly )
            {
                SelectionPoly.Add(Gcd.Xreal(Mouse.X));
                SelectionPoly.Add(Gcd.Yreal(Mouse.Y));
                return; // porque esto requiere un RigthClick para terminar
            }
             // C.1.1 ActionActive = ActionRectActive -> Finalizo la seleccion por recuadro
            RectActive = false;
            Gcd.flgSearchingAllowed = true;
                // corrijo para start<end  <- DEPRE
                // If SelStartX > SelEndX Then
                //     crossing = true <- DEPRE
                //     Swap SelStartX, SelEndX
                // Else
                //     crossing = false  <- DEPRE
                // End If
            
            // fixme: revisar esto
            // if (SelStartX > SelEndX) Gb.Swap(ref SelStartX, ref SelEndX);
            // if ( SelStartY < SelEndY ) Gb.Swap(ref SelStartY, ref SelEndY); // this is FLIPPED

            // if ( SelStartXr > SelEndXr ) Gb.Swap(ref SelStartXr, ref SelEndXr);
            // if ( SelStartYr > SelEndYr ) Gb.Swap(ref SelStartYr, ref SelEndYr);

             // veo si el rectangulo es suficientemente grande como para representar una seleccion por rectangulo
            if ( (SelEndX - SelStartX + (-SelEndY + SelStartY)) > 10 )
            {

                cSel = clsEntities.SelectionSquare(SelStartXr, SelStartYr, SelEndXr, SelEndYr, SelectCrossing);

                 // Else // TODO: ver si tengo que desseleccionar
                 //     clsEntities.DeSelection()

            }
            Gcd.Drawing.Sheet.EntitiesSelected = clsEntities.SelectionCombine(Gcd.Drawing.Sheet.EntitiesSelected, cSel, tipo);

             // determino que hacer con la seleccion
            if ( Gcd.clsJobCallBack != null && ReturnOnFirstSelection )
            {
                Gcd.clsJob = Gcd.clsJobCallBack;
                Gcd.clsJobCallBack = null;
                Gcd.clsJob.Run();
                return;
            }
             // e = Gcd.Drawing.Sheet.EntitiesSelected[Gcd.Drawing.Sheet.EntitiesSelected.Last]
             // If e Then
             //     //Gcd.Drawing.Sheet.Grips.Clear
             //     Gcd.CCC[e.Gender].generategrips(e)
             // Endif

             // fixme: revisar esto
            if ( Gcd.Drawing.Sheet.EntitiesSelected.Count > 0 )
            {
                // fMain.fp.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
            }
            else
            {
                // fMain.fp.FillGeneral(Gcd.Drawing.Sheet);
            }
            // clsEntities.GLGenDrawListSel();

             //Try s = Gcd.clsJobCallBack.gender
            Prompt = ("Selected") + " " + Gcd.Drawing.Sheet.EntitiesSelected.Count.ToString() + " " + ("elements") + " " + ("New/Add(Ctrl)/Remove(Shft)/Previous selection");

             // If Gcd.clsJobCallBack Then
             //     Try Gcd.clsJobCallBack.run()
             //     Return
             // Else
             //
             //     // vamos a darle mas funcionalidad
             //     If Gcd.Drawing.Sheet.EntitiesSelected.Count = 0 Then
             //
             //     Else If Gcd.Drawing.Sheet.EntitiesSelected.Count = 1 Then
             //
             //         //Gcd.Drawing.Sheet.GlSheet.PopupMenu = "mColors"
             //
             //     Else // tenemos varias entidades
             //
             //         //Gcd.Drawing.Sheet.GlSheet.PopupMenu = "mEntities"
             //
             //     End If
             // Endif

        }
        else if ( RectActive == false && GripPoint==null )
        {

             // A.1.2 Estoy sobre una entidad
            if ( Gcd.Drawing.HoveredEntity !=null && AllowSingleSelection )
            {
                if ( tipo == "new" ) Gcd.Drawing.Sheet.EntitiesSelected.Clear();
                if ( base.AllowSingleSelection )
                {

                     // A.1.2.1 No esta seleccionada -> seleccionar
                    if ( ! Gcd.Drawing.Sheet.EntitiesSelected.ContainsKey(Gcd.Drawing.HoveredEntity.id) )
                    {
                         // excepto que estos removiendo
                        if ( tipo != "rem" )
                        {
                            Gcd.Drawing.Sheet.EntitiesSelected.Add(Gcd.Drawing.HoveredEntity.id, Gcd.Drawing.HoveredEntity);
                        }
                    } // esta en la seleccion
                    else
                    {
                        if ( tipo == "rem" )
                        {
                            Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.id);
                        }
                    }
                    if ( Gcd.clsJobCallBack!=null && ReturnOnFirstSelection )
                    {
                        Gcd.clsJob = Gcd.clsJobCallBack;
                        Gcd.clsJobCallBack = null;
                        Gcd.clsJob.Run();
                        return;
                    }
                }
                if ( AllowGripEdit )
                {

                    Gcd.Drawing.Sheet.Grips.Clear();
                    clsEntities.GenGrips(Gcd.Drawing.HoveredEntity);

                     // Else  // TODO: ver que pasa con esto

                     //     // A.1.2.2 Esta seleccionada previamente
                     //     // A.1.2.2.2 No estoy sobre un grip -> deseleccionar
                     //     Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.id)
                     //     fprops.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
                     //     clsEntities.glGenDrawListSel
                     //     Gcd.Redraw
                }
                // fixme: revisar esto
                // fMain.fp.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
                // clsEntities.GLGenDrawListSel();
                Gcd.Redraw();
                Prompt = ("Selected") + " " + (Gcd.Drawing.Sheet.EntitiesSelected.Count.ToString()) + " " + ("elements");

            } // inicio la seleccion por recuadro
            else
            {

                if ( SelectType > 0 ) RectActive = true;
                Gcd.flgSearchingAllowed = false;
                SelStartX = Mouse.X;
                SelStartY = Mouse.Y;
                 // Paso a coordenadas reales
                SelStartXr = Gcd.Xreal(SelStartX);
                SelStartYr = Gcd.Yreal(SelStartY);
                if ( SelectType == SelectTypePoly )
                {
                    SelectionPoly.Add(SelStartXr);
                    SelectionPoly.Add(SelStartYr);
                }
                Gcd.Drawing.Sheet.Grips.Clear();

            }

        }
        else if ( GripPoint!=null )
        {

             // C.1.2 ActionActive = ActionGripActive -> Finalizo la edicion por grips
             // guardo todo

            GripEdit();
            Gcd.Drawing.Sheet.Grips.Clear();
            if ( ! GripCopying )
            {
                EntityForEdit.id = OriginalEntityForEdit.id;
                s = GripPoint.ToolTip + (" in ") + GripPoint.AsociatedEntity.Gender;
                // Gcd.Drawing.uUndo.OpenUndoStage(s, Undo.TypeModify);
                // Gcd.Drawing.uUndo.AddUndoItem(OriginalEntityForEdit);

                Gcd.Drawing.Sheet.Entities.Remove(OriginalEntityForEdit.id);
                Gcd.Drawing.Sheet.EntitiesVisibles.Remove(OriginalEntityForEdit.id);
                Gcd.Drawing.Sheet.EntitiesSelected.Remove(OriginalEntityForEdit.id);
            }
            else
            {
                EntityForEdit.id = Gcd.NewId();
                // Gcd.Drawing.uUndo.OpenUndoStage("Grip copy ", Undo.TypeCreate);
                // Gcd.Drawing.uUndo.AddUndoItem(EntityForEdit);

            }

            // Gcd.Drawing.uUndo.CloseUndoStage();

             //Gcd.CCC[EntityForEdit.Gender].finish(EntityForEdit)
            Gcd.CCC[EntityForEdit.Gender].GenerateGrips(EntityForEdit);
            Gcd.Drawing.Sheet.Entities.Add(EntityForEdit.id, EntityForEdit);
            Gcd.Drawing.Sheet.EntitiesVisibles.Add(EntityForEdit.id, EntityForEdit);
            Gcd.Drawing.Sheet.EntitiesSelected.Add(EntityForEdit.id, EntityForEdit);

            // clsEntities.glGenDrawList(EntityForEdit);
            // clsEntities.glGenDrawListSel();
            // clsEntities.glGenDrawListLAyers(EntityForEdit.pLayer);
            EntityForEdit = null;
            OriginalEntityForEdit = null;
            GripPoint = null;
             //GripCopying = false

        }

        if (  PanActive )
        {
             // C.1.3 ActionActive = ActionPanActive -> nada
        }

        if ( Mode == 0 )
        {
             // C.1.4.1 Estoy sobre una entidad

        }
         // C.1.4 ActionActive = 0

        return; // este return es para evitar clicks simultaneos
    }

     // A.2 Derecho
    if ( Mouse.Right )
    {

        if ( RectActive && SelectType == SelectTypePoly )
        {

             // C.1.1 Finalizo la seleccion por POLY
            RectActive = false;
            SelectionPoly.Add(Gcd.Xreal(Mouse.X));
            SelectionPoly.Add(Gcd.Yreal(Mouse.Y));

            cSel = clsEntities.SelectionPoly(SelectionPoly, SelectCrossing);
            SelectionPoly.Clear();
            Gcd.Drawing.Sheet.EntitiesSelected = clsEntities.SelectionCombine(Gcd.Drawing.Sheet.EntitiesSelected, cSel, tipo);

             // determino que hacer con la seleccion
            if ( Gcd.clsJobCallBack != null && ReturnOnFirstSelection )
            {
                Gcd.clsJob = Gcd.clsJobCallBack;
                Gcd.clsJobCallBack = null;
                Gcd.clsJob.Run();
                return;
            }

            e = Gcd.Drawing.Sheet.EntitiesSelected.Values.LastOrDefault();
            if ( e !=null  )
            {
                 //Gcd.Drawing.Sheet.Grips.Clear
                Gcd.CCC[e.Gender].GenerateGrips(e);
            }
            // fixme: revisar esto
            // fProps.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
            // clsEntities.GLGenDrawListSel();

            Prompt = ("Selected") + " " + Gcd.Drawing.Sheet.EntitiesSelected.Count.ToString() + " " + ("elements");

        }
        else if ( Gcd.clsJobCallBack==null )
        {
             // Si estoy aca es porque:
             // -No estoy en un proceso de seleccion
             // -No estoy aplicando una herramienta
             // -No hay entidades selecionadas

            if ( Gcd.clsJobPrevious != null )
            {
                if ( (Gcd.Drawing.Sheet.EntitiesSelected.Count == 1) && (Gcd.clsJobPrevious.Gender == cadEntityBuilder.Gender) )
                {
                    if ( Gcd.Drawing.LastEntity !=null )
                    {

                        Gcd.clsJob = Gcd.clsJobPrevious;
                        Gcd.clsJob.Start();

                    }

                }
                else
                {
                     //If Gcd.Drawing.Sheet.EntitiesSelected.Count = 0 Then              //Sin entidades seleccionadas podria significar cancelar
                     //
                     //                 Gcd.clsJob = Gcd.clsJobPrevious
                     //                 Gcd.clsJob.Cancel
                     //             Else If Gcd.Drawing.Sheet.EntitiesSelected.Count = 1 Then
                     //                 Gcd.clsJob = Gcd.clsJobPrevious
                     //                 Gcd.clsJob.Start()
                     //
                     //             Else // tenemos varias entidades
                    Gcd.clsJob = Gcd.clsJobPrevious;
                    Gcd.clsJob.Start();

                }
            }
             //     End If
             // Else
             //     // A.2.1.2 Tengo una seleccion previa -> Menu: Cortar, Copiar, Agrupar, Desagrupar, Llevar al layer actual, etc
             //     // Esto salta solo, pero debo configurarlo en algun lado
             //     Gcd.clsJob = Gcd.clsJobCallBack
             //     Gcd.clsJob.run()
             //
             //End If
             // FIXME: ver esta pafrte dudosa
             // Else If Gcd.Drawing.Sheet.EntitiesSelected.Count > 0 Then
             //     // Si estoy aca es porque:
             //     // -No estoy en un proceso de seleccion
             //     // -No estoy aplicando una herramienta
             //     // -Hay entidades selecionadas
             //     Gcd.Drawing.Sheet.EntitiesSelected.Clear
             //     clsEntities.GLGenDrawListSel()
             //     fProps.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)

        }
        else
        {

             // A.2.2 ToolActive = true? -> Finalizo la seleccion y vuelvo a la Tool
            // Gcd.Drawing.Sheet.EntitiesSelectedPrevious.Add(Gcd.Drawing.Sheet.EntitiesSelected.ToDictionary);
            Gcd.clsJob = Gcd.clsJobCallBack;
            Gcd.clsJob.Run();

        }
        return; // este return es para evitar clicks simultaneos
    }

    if ( Mouse.Middle )
    {

         // C.3 Medio -> ActionActive = ActionPanActive -> finalizo el paneo
        return; // este return es para evitar clicks simultaneos
    }

    Gcd.Redraw();

}

public void MouseMove()
    {


    Grip g ;         
    Entity e ;         

    SelEndX = Mouse.X;
    SelEndY = Mouse.Y;
    SelEndXr = Gcd.Xreal(SelEndX);
    SelEndYr = Gcd.Yreal(SelEndY);
    SelEndXr = Gcd.Near(SelEndXr);
    SelEndYr = Gcd.Near(SelEndYr);

     // // yo soy el responsable de chequear POI
     // If Not Gcd.flgSearchingPOI Thenntity

     //End If

     //end If
    if ( RectActive && SelectType != SelectTypeSingle )
    {
            // B.1 ActionActive = ActionRectActive: actualizo coordenadas del punto final del Rect
            // (se hace mas arriba)

            if (SelEndXr <= SelStartXr)
            {
                SelectCrossing = true;
        }
            else
            {
                SelectCrossing = false;
            }
    }
    else if ( GripPoint != null)
    {
         // B.2 ActionActive = ActionGripActive: modifico la entidad con la nueva posicion del punto
        Gcd.Drawing.iEntity = clsMouseTracking.CheckBestPOI(SelEndXr, SelEndYr);

        if ( (Gcd.Drawing.iEntity[2] > 0) )
        {

            SelEndXr = Gcd.Drawing.iEntity[0];
            SelEndYr = Gcd.Drawing.iEntity[1];

             // Else
        } // puedo hacer ortogonal
        else
        {
            if ( Gcd.Orthogonal ) // hablame de operadores logicos
            {

                if ( Gb.Abs(SelEndX - SelStartX) > Gb.Abs(SelEndY - SelStartY) ) // prevalece X
                {
                    SelEndYr = SelStartYr;

                }
                else
                {
                    SelEndXr = SelStartXr;
                }

            }

        } //
        if ( GripCopying )
        {

        }
        else
        {

        }
        GripEdit();
        DrawingAids.txtFrom = GripPoint.ToolTip;

    }
    else if ( Active && PanActive )
    {
         // B.3 ActionActive = ActionPanActive: mando la coordenada a cadPan
         // (ni siquiera deberiamos estar aca, deberiamos estar en cadPan.class)
    }
    else
    {
         // busco algun grip
        GripHovered = FindGrip(SelEndXr, SelEndYr);
         // B.4 ActionActive = 0: nada

        if ( Gcd.Drawing.HoveredEntity != null )
        {
            // if ( Config.ShowEntityInspector ) FInspector.Run(Gcd.Drawing.HoveredEntity);
        }
        else
        {
            // FInspector.Close();
        }
    }

     // Else

     // End If

}

public new void KeyText(string EnteredText)
    {


     // in this case, we try to run the command wich is a class
    Object o ;         
    string RunWith ;         
           

    EnteredText = Gb.Trim(EnteredText).ToUpper();
    if ( EnteredText == "" ) return; // no BS here

    switch ( EnteredText)
    {
        case "_CANCEL":
            Cancel();
            break;
        case "R":
            SelectMode = SelectModeRem;
            break;
        case "A":
            SelectMode = SelectModeAdd;
            break;
        case "N": // seleccion previa
            Gcd.Drawing.Sheet.EntitiesSelected.Clear(); // = Gcd.Drawing.Sheet.EntitiesSelectedPrevious.Copy()
            // fixme: revisar esto
            // clsEntities.GLGenDrawListSel();
            Gcd.Redraw();
            break;
        case "P": // seleccion previa
            Gcd.Drawing.Sheet.EntitiesSelected = new Dictionary<string, Entity>(Gcd.Drawing.Sheet.EntitiesSelectedPrevious);
            // fixme: revisar esto
            // clsEntities.GLGenDrawListSel();
            Gcd.Redraw();
            break;
        case "CENTER":
            Gcd.PanTo(0, 0);
            Gcd.Redraw();
             //Gcd.regen
             break;
        case "REGEN":
            Gcd.Regen();
            break;
        case "REGENALL":
            Gcd.PanToOrigin();
            Gcd.Regen();
            break;
        case "REDRAW":
            Gcd.Redraw();
            break;
        case "STL":
        // fixme: revisar esto
            // clsEntities.ExportSTL();
            break;

        default:
             //o = cadDimension // a test

            //  // Intercepto Alias
            // if ( Config.oAlias.ContainsKey(Lower(EnteredText)) )
            // {
            //     EnteredText = Config.oAlias[Lower(EnteredText)].ToUpper();
            // }

            if ( Gcd.CCC.ContainsKey(EnteredText) )
            {
                // es una entidad lo que queire dibujar
                // o = Gcd.CCC[EnteredText];
            }
            else if (Gcd.Tools.ContainsKey(EnteredText) )
            {

                Gcd.clsJobPrevious = Gcd.clsJob;
                Gcd.clsJob = Gcd.Tools[EnteredText];
                Gcd.clsJob.Start();
                
            }
            else
            {

                DrawingAids.ErrorMessage = "Command not recognized";
                return;
            }

           
            break;

    }
    Gcd.Drawing.iEntity.Clear();
    return;

     // TODO: dejar comentado mientras hagamos debug
     // catch

    DrawingAids.ErrorMessage = "Command not recognized";
     //
     //

}

public void Draw()
    {


     List<double> flxPoly = new List<double>();
    int iColor ;         
// fixme: revisar esto
    // ToolsBase.Draw();

    if ( SelectCrossing )
    {
        iColor = Colors.Red;
    }
    else
    {
        iColor = Colors.Green;
    }
    if ( RectActive )
    {

         // si estamos dentro un viewport no hay nada que dibujar, porque no estamos seleccionando nada
        if ( Gcd.Drawing.Sheet.Viewport != null ) return;
        if ( SelectType == SelectTypeRect || SelectType == SelectTypeSingleAndRect )
        {
            SelectionPoly.Clear(); // aprovecho

            Glx.Rectangle2D(SelStartXr, SelStartYr, SelEndXr - SelStartXr, SelEndYr - SelStartYr, Colors.RGB(224, 220, 207),0,0,0, iColor, 1, Gcd.stiDashedSmall, 2);

            return;
        }
        else if ( SelectType == SelectTypePoly )
        {

            // fMain.PopupMenu = ""; // no hay menu contextual

             // como pude habver cambiado en este momento el modo de seleccion, chequeo
            if ( SelectionPoly.Count == 0 )
            {
                SelectionPoly.Add(SelStartXr);
                SelectionPoly.Add(SelStartYr);
            }
            flxPoly.Clear();
            flxPoly.AddRange(SelectionPoly);
            flxPoly.Add(SelEndXr);
            flxPoly.Add(SelEndYr);
            flxPoly.Add(SelectionPoly[0]);
            flxPoly.Add(SelectionPoly[1]);
            Glx.PolyLines(flxPoly, Colors.Red, 1, Gcd.stiDashedSmall);
        }
        else
        {
            SelectionPoly.Clear();

        }

    }

     // No vamos a dibujar nada relativo a los grips si estamsos selecionando para alguna Tool
    if ( Gcd.clsJobCallBack != null) return;

    DrawingAids.DrawGrips(Gcd.Drawing.Sheet.Grips);

    if ( EntityForEdit != null) Gcd.CCC[EntityForEdit.Gender].Draw(EntityForEdit);
    if ( OriginalEntityForEdit != null)
    {
        if ( GripCopying )
        {
        }
        else
        {
            Gcd.CCC[OriginalEntityForEdit.Gender].DrawShadow(OriginalEntityForEdit);
        }
    }
    if ( GripPoint != null)
    {

        Gcd.CCC[GripPoint.AsociatedEntity.Gender].DrawEditing(GripPoint);
        if ( GripCopying )
        {
        }
        else
        {

            Gcd.CCC[GripPoint.AsociatedEntity.Gender].DrawShadow(EntityForEdit);
        }
        DrawingAids.DrawSnapText();

    }
    else if ( GripHovered != null)
    {

        DrawingAids.Helper.texto = GripHovered.ToolTip;
        DrawingAids.Helper.dX = 15;
        DrawingAids.Helper.dY = 15;
    }
    else
    {
        DrawingAids.Helper.texto = "";

    }

}

public void GripEdit()
    {


    if ( GripPoint != null)
    {
        if ( GripCopying )
        {
             // GripPoint.X = SelEndXr
             // GripPoint.Y = SelEndYr
             // Gcd.CCC[GripPoint.AsociatedEntity.Gender].translate(GripPoint.AsociatedEntity, SelEndXr - SelStartXr, SelEndYr - SelStartYr)

        }

        GripPoint.X = SelEndXr;
        GripPoint.Y = SelEndYr;
        Gcd.CCC[GripPoint.AsociatedEntity.Gender].GripEdit(GripPoint);
        Gcd.CCC[GripPoint.AsociatedEntity.Gender].BuildGeometry(GripPoint.AsociatedEntity);
        Gcd.Redraw();

    }

}

public static Grip FindGrip(double x, double y)
    {


    

    foreach ( var g in Gcd.Drawing.Sheet.Grips)
    {
        if (Puntos.Around(x, y, g.X, g.Y, Gcd.Metros(Config.GripProximityDistance)))
        {

            return g;
        }
         //
    }
    DrawingAids.txtFrom = "";
    return null;

}

public new void KeyDown(int iCode)
    {


    if (  Key.Control )
    {
        GripCopying = true;
        // fMain.GLArea1.Cursor = Gcd.CursorSelectAdd;
        SelectMode = SelectModeAdd;
    }
    else if (  Key.Shift )
    {
        GripCopying = false;
        // fMain.GLArea1.Cursor = Gcd.CursorSelectRem;
        SelectMode = SelectModeRem;
    }
    // else if ( iCode == Key.AltKey )
    // {
    //     GripCopying = false;
    //     fMain.GLArea1.Cursor = Gcd.CursorSelectXchange;
    // }

}

public void KeyUp(int iCode)
    {


    if (  Key.Control )
    {
        GripCopying = false;
        // fMain.GLArea1.Cursor = Gcd.CursorCross;
        SelectMode = SelectModeNew;
    }
    else if (  Key.Shift )
    {
        GripCopying = false;
        SelectMode = SelectModeNew;
        // fMain.GLArea1.Cursor = Gcd.CursorCross;
    }
    // else if ( iCode == Key.Alt )
    // {
    //     GripCopying = false;
    //     SelectMode = SelectModeNew;
    //     // fMain.GLArea1.Cursor = Gcd.CursorCross;
    // }

}

}
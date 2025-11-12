namespace Gaucho;

public class cadSelection : ToolsBase
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
const string Gender = "SELECT";

private string[] EntityType ;         

private Grip GripPoint ;         
private Grip GripHovered ;         
private bool GripCopying = false;
 //Public GripCopying As Boolean = false

private double dX ;         
private double dY ;         
 //Public ToolActive As Boolean = false
private bool PanActive = false;
private bool GripActive = false;
private bool RectActive = false;

 // nuevo, para integrar el resto de las tools con esta, que es la principal
public bool AllowSingleSelection = true;
public bool AllowRectSelection = true;
public bool AllowPolySelection = true;
public bool AllowGripEdit = true;
public bool AllowTextInput = true;

public bool ReturnOnFirstSelection = true;

 // mas nuevo
public int SelectType = 1;
const int SelectTypeSingleAndRect = 1;
const int SelectTypeSingle = 0;
const int SelectTypeRect = 2;
const int SelectTypePoly = 3;
const int SelectTypePoint = 4;

public int SelectMode = 0;
const int SelectModeNew = 0;
const int SelectModeAdd = 1;
const int SelectModeRem = 2;

public bool SelectCrossing = false;                 // las entidades puedn estar parcialmente dentro del contorno

public double[] SelectionPoly ;         

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

public static bool Start(Variant ElemToBuild, int _Mode= 2)
    {

     // Modes:
     //       0 = Move, all points in the element must be selected, or click on it.
     //       1 = Stretch, selection may be partial, each element is called to see if the support stretching

    Mode = _Mode;

    Prompt = ("Selected") + " " + Str$(Gcd.Drawing.Sheet.EntitiesSelected.Count) + " " + ("elements") + " " + ("New/Add(Ctrl)/Remove(Shft)/Previous selection");

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
    Gcd.Drawing.iEntity.Clear;
    PoiChecking = true;
    Gcd.DrawHoveredEntity = true;
    GripPoint = null;

}

 // Public Sub DblClick()
 //
 //     Dim k As Single
 //     Dim e As Entity
 //     Dim te As Entity
 //
 //
 //
 //     EntityForEdit = clsMouseTracking.CheckAboveEntity(Gcd.Xreal(Mouse.x), Gcd.Yreal(Mouse.y))
 //     Return
 //
 //     If Not Gcd.flgSearchingPOI Then
 //         Gcd.Drawing.iEntity = clsMouseTracking.CheckBestPOI(Gcd.Xreal(Mouse.x), Gcd.Yreal(Mouse.Y))
 //     Else    // estoy buscando, pero me movi, asi que me desengancho del POI anterior
 //
 //         Gcd.Drawing.iEntity[0] = Gcd.Xreal(Mouse.x)
 //         Gcd.Drawing.iEntity[1] = Gcd.Yreal(Mouse.y)
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

public static void MouseDown()
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
    SelEndy = SelStartY;

    SelEndXr = SelStartXr;
    SelEndYr = SelStartYr;

    PoiChecking = false;
    if ( Mouse.Right ) return;
    if ( Gcd.clsJobCallBack ) return;
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
        if ( GripPoint )
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
        else if ( Gcd.Drawing.HoveredEntity )
        {

             // // A.1.2 Estoy sobre una entidad
             //
             // If Not Gcd.Drawing.Sheet.EntitiesSelected.ContainsKey(Gcd.Drawing.HoveredEntity.Id) Then
             //     // A.1.2.1 No esta seleccionada -> seleccionar
             //     Gcd.Drawing.Sheet.EntitiesSelected.add(Gcd.Drawing.HoveredEntity, Gcd.Drawing.HoveredEntity.Id)
             //     Gcd.Drawing.Sheet.Grips.Clear
             //     Gcd.CCC[Gcd.Drawing.HoveredEntity.Gender].generategrips(Gcd.Drawing.HoveredEntity)
             //     clsEntities.glGenDrawListSel
             //     fProps.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
             //     Gcd.Redraw
             // Else
             //     // A.1.2.2 Esta seleccionada previamente
             //     // A.1.2.2.2 No estoy sobre un grip -> deseleccionar
             //     Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.Id)
             //     fprops.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
             //     clsEntities.glGenDrawListSel
             //     Gcd.Redraw
             // End If

        }

        return; // este return es para evitar clicks simultaneos
    }

    if ( Mouse.MIddle )
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

public static void MouseUp()
    {


    string s ;         
    string tipo ;         
    double t = Timer;
    Entity e ;         
    Dictionary<string, Entity> cSel =[] ;         

    Gcd.Drawing.iEntity.Clear;
    Gcd.Drawing.Sheet.SkipSearch = null;
    Gcd.Drawing.LastPoint.Clear;
     // If Gcd.Drawing.Sheet.Viewport Then
     //     //
     //     Gcd.Drawing.GLAreaInUse.Mouse = Mouse.Cross
     //     Gcd.flgNewPosition = true
     //     active = false
     //     Return
     // End If

    tipo = "new";
    if ( Mouse.Shift || SelectMode == SelectModeRem ) tipo = "rem"; // estos elementos de la seleccion anterior
    if ( Mouse.Control || SelectMode == SelectModeAdd ) tipo = "add"; // elementos a la seleccion anterior
     //

    if ( Mouse.Left )
    {
         // C.1 Izquierdo

         // determino que hacer con la seleccion
        if ( Gcd.clsJobCallBack && ReturnOnFirstSelection && SelectType == SelectTypePoint )
        {
            Gcd.clsJob = Gcd.clsJobCallBack;
            Gcd.clsJobCallBack = null;
            Gcd.clsJob.run;
            return;
        }

        PoiChecking = true;
        if ( RectActive )
        {

            if ( SelectType == SelectTypePoly )
            {
                SelectionPoly.Add(Gcd.Xreal(Mouse.x));
                SelectionPoly.Add(Gcd.Yreal(Mouse.y));
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

            if (SelStartX > SelEndX) Utils.Swap(ref SelStartX, ref SelEndX);
            if ( SelStartY < SelEndy ) Utils.Swap(ref SelStartY, ref SelEndy); // this is FLIPPED

            if ( SelStartXr > SelEndXr ) Utils.Swap(ref SelStartXr, ref SelEndXr);
            if ( SelStartYr > SelEndYr ) Utils.Swap(ref SelStartYr, ref SelEndYr);

             // veo si el rectangulo es suficientemente grande como para representar una seleccion por rectangulo
            if ( (SelEndX - SelStartX + (-SelEndy + SelStartY)) > 10 )
            {

                cSel = clsEntities.SelectionSquare(SelStartXr, SelStartYr, SelEndXr, SelEndYr, SelectCrossing);

                 // Else // TODO: ver si tengo que desseleccionar
                 //     clsEntities.DeSelection()

            }
            Gcd.Drawing.Sheet.EntitiesSelected = clsEntities.SelectionCombine(Gcd.Drawing.Sheet.EntitiesSelected, cSel, tipo);

             // determino que hacer con la seleccion
            if ( Gcd.clsJobCallBack && ReturnOnFirstSelection )
            {
                Gcd.clsJob = Gcd.clsJobCallBack;
                Gcd.clsJobCallBack = null;
                Gcd.clsJob.run;
                return;
            }
             // e = Gcd.Drawing.Sheet.EntitiesSelected[Gcd.Drawing.Sheet.EntitiesSelected.Last]
             // If e Then
             //     //Gcd.Drawing.Sheet.Grips.Clear
             //     Gcd.CCC[e.Gender].generategrips(e)
             // Endif
            if ( Gcd.Drawing.Sheet.EntitiesSelected.Count > 0 )
            {
                fMain.fp.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
            }
            else
            {
                fMain.fp.FillGeneral(Gcd.Drawing.Sheet);
            }
            clsEntities.GLGenDrawListSel();

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
        else if ( RectActive == false && ! GripPoint )
        {

             // A.1.2 Estoy sobre una entidad
            if ( Gcd.Drawing.HoveredEntity && AllowSingleSelection )
            {
                if ( tipo == "new" ) Gcd.Drawing.Sheet.EntitiesSelected.Clear;
                if ( AllowSingleSelection )
                {

                     // A.1.2.1 No esta seleccionada -> seleccionar
                    if ( ! Gcd.Drawing.Sheet.EntitiesSelected.ContainsKey(Gcd.Drawing.HoveredEntity.Id) )
                    {
                         // excepto que estos removiendo
                        if ( tipo != "rem" )
                        {
                            Gcd.Drawing.Sheet.EntitiesSelected.add(Gcd.Drawing.HoveredEntity, Gcd.Drawing.HoveredEntity.Id);
                        }
                    } // esta en la seleccion
                    else
                    {
                        if ( tipo == "rem" )
                        {
                            Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.Id);
                        }
                    }
                    if ( Gcd.clsJobCallBack && ReturnOnFirstSelection )
                    {
                        Gcd.clsJob = Gcd.clsJobCallBack;
                        Gcd.clsJobCallBack = null;
                        Gcd.clsJob.run;
                        return;
                    }
                }
                if ( AllowGripEdit )
                {

                    Gcd.Drawing.Sheet.Grips.Clear;
                    clsEntities.GenGrips(Gcd.Drawing.HoveredEntity);

                     // Else  // TODO: ver que pasa con esto

                     //     // A.1.2.2 Esta seleccionada previamente
                     //     // A.1.2.2.2 No estoy sobre un grip -> deseleccionar
                     //     Gcd.Drawing.Sheet.EntitiesSelected.Remove(Gcd.Drawing.HoveredEntity.Id)
                     //     fprops.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected)
                     //     clsEntities.glGenDrawListSel
                     //     Gcd.Redraw
                }
                fMain.fp.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
                clsEntities.GLGenDrawListSel();
                Gcd.Redraw;
                Prompt = ("Selected") + " " + (Gcd.Drawing.Sheet.EntitiesSelected.Count.ToString()) + " " + ("elements");

            } // inicio la seleccion por recuadro
            else
            {

                if ( SelectType > 0 ) RectActive = true;
                Gcd.flgSearchingAllowed = false;
                SelStartX = Mouse.x;
                SelStartY = Mouse.Y;
                 // Paso a coordenadas reales
                selStartXr = Gcd.Xreal(SelStartX);
                selStartYr = Gcd.Yreal(SelStartY);
                if ( SelectType == SelectTypePoly )
                {
                    SelectionPoly.Add(SelStartXr);
                    SelectionPoly.Add(SelStartYr);
                }
                Gcd.Drawing.Sheet.Grips.Clear;

            }

        }
        else if ( GripPoint )
        {

             // C.1.2 ActionActive = ActionGripActive -> Finalizo la edicion por grips
             // guardo todo

            GripEdit;
            Gcd.Drawing.Sheet.Grips.Clear;
            if ( ! GripCopying )
            {
                EntityForEdit.id = OriginalEntityForEdit.id;
                s = GripPoint.ToolTip + (" in ") + GripPoint.AsociatedEntity.Gender;
                Gcd.Drawing.uUndo.OpenUndoStage(s, Undo.TypeModify);
                Gcd.Drawing.uUndo.AddUndoItem(OriginalEntityForEdit);

                Gcd.Drawing.Sheet.Entities.Remove(OriginalEntityForEdit.id);
                Gcd.Drawing.Sheet.EntitiesVisibles.Remove(OriginalEntityForEdit.id);
                Gcd.Drawing.Sheet.EntitiesSelected.Remove(OriginalEntityForEdit.id);
            }
            else
            {
                EntityForEdit.id = Gcd.NewId();
                Gcd.Drawing.uUndo.OpenUndoStage("Grip copy ", Undo.TypeCreate);
                Gcd.Drawing.uUndo.AddUndoItem(EntityForEdit);

            }

            Gcd.Drawing.uUndo.CloseUndoStage();

             //Gcd.CCC[EntityForEdit.Gender].finish(EntityForEdit)
            Gcd.CCC[EntityForEdit.Gender].generategrips(EntityForEdit);
            Gcd.Drawing.Sheet.entities.Add(EntityForEdit, EntityForEdit.id);
            Gcd.Drawing.Sheet.EntitiesVisibles.Add(EntityForEdit, EntityForEdit.id);
            Gcd.Drawing.Sheet.EntitiesSelected.Add(EntityForEdit, EntityForEdit.id);

            clsEntities.glGenDrawList(EntityForEdit);
            clsEntities.glGenDrawListSel();
            clsEntities.glGenDrawListLAyers(EntityForEdit.pLayer);
            EntityForEdit = null;
            OriginalEntityForEdit = null;
            GripPoint = null;
             //GripCopying = false

        }

        if ( Mode == PanActive )
        {
             // C.1.3 ActionActive = ActionPanActive -> nada
        }

        if ( Mode == false )
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
            SelectionPoly.Add(Gcd.Xreal(Mouse.x));
            SelectionPoly.Add(Gcd.Yreal(Mouse.y));

            cSel = clsEntities.SelectionPoly(SelectionPoly, SelectCrossing);
            SelectionPoly.Clear;
            Gcd.Drawing.Sheet.EntitiesSelected = clsEntities.SelectionCombine(Gcd.Drawing.Sheet.EntitiesSelected, cSel, tipo);

             // determino que hacer con la seleccion
            if ( Gcd.clsJobCallBack && ReturnOnFirstSelection )
            {
                Gcd.clsJob = Gcd.clsJobCallBack;
                Gcd.clsJobCallBack = null;
                Gcd.clsJob.run;
                return;
            }

            e = Gcd.Drawing.Sheet.EntitiesSelected[Gcd.Drawing.Sheet.EntitiesSelected.Last];
            if ( e )
            {
                 //Gcd.Drawing.Sheet.Grips.Clear
                Gcd.CCC[e.Gender].generategrips(e);
            }
            fProps.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);
            clsEntities.GLGenDrawListSel();

            Prompt = ("Selected") + " " + Gcd.Drawing.Sheet.EntitiesSelected.Count.ToString() + " " + ("elements");

        }
        else if ( ! Gcd.clsJobCallBack )
        {
             // Si estoy aca es porque:
             // -No estoy en un proceso de seleccion
             // -No estoy aplicando una herramienta
             // -No hay entidades selecionadas

            if ( Gcd.clsJobPrevious )
            {
                if ( (Gcd.Drawing.Sheet.EntitiesSelected.Count == 1) && (Gcd.clsJobPrevious.gender == cadEntityBuilder.gender) )
                {
                    if ( cadEntityBuilder.LastEntity )
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
            Gcd.Drawing.Sheet.EntitiesSelectedPrevious = Gcd.Drawing.Sheet.EntitiesSelected.Copy();
            Gcd.clsJob = Gcd.clsJobCallBack;
            Gcd.clsJob.run();

        }
        return; // este return es para evitar clicks simultaneos
    }

    if ( Mouse.MIddle )
    {

         // C.3 Medio -> ActionActive = ActionPanActive -> finalizo el paneo
        return; // este return es para evitar clicks simultaneos
    }

    Gcd.Redraw;

}

public void MouseMove()
    {


    Grip g ;         
    Entity e ;         

    SelEndX = Mouse.X;
    SelEndy = Mouse.Y;
    SelEndXr = Gcd.Xreal(SelEndX);
    SelEndYr = Gcd.Yreal(SelEndy);
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
    else if ( GripPoint )
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

                if ( Abs(SelEndX - SelStartX) > Abs(SelEndY - SelStartY) ) // prevalece X
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
        GripEdit;
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

        if ( Gcd.Drawing.HoveredEntity )
        {
            if ( Config.ShowEntityInspector ) FInspector.Run(Gcd.Drawing.HoveredEntity);
        }
        else
        {
            FInspector.Close();
        }
    }

     // Else

     // End If

}

public void KeyText(string EnteredText)
    {


     // in this case, we try to run the command wich is a class
    Object o ;         
    string RunWith ;         
    Class c ;         

    EnteredText = UCase(Trim(EnteredText));
    if ( EnteredText == "" ) return; // no BS here

    switch ( EnteredText)
    {
        case "_CANCEL":
            Cancel;
        case "R":
            SelectMode = SelectModeRem;
        case "A":
            SelectMode = SelectModeAdd;
        case "N": // seleccion previa
            Gcd.Drawing.Sheet.EntitiesSelected.Clear; // = Gcd.Drawing.Sheet.EntitiesSelectedPrevious.Copy()
            clsEntities.GLGenDrawListSel();
            Gcd.redraw;

        case "P": // seleccion previa
            Gcd.Drawing.Sheet.EntitiesSelected = Gcd.Drawing.Sheet.EntitiesSelectedPrevious.Copy();
            clsEntities.GLGenDrawListSel();
            Gcd.redraw;

        case "CENTER":
            Gcd.PanTo(0, 0);
            Gcd.Redraw;
             //Gcd.regen
        case "REGEN":
            Gcd.regen;
        case "REGENALL":
            Gcd.PanToOrigin;
            Gcd.regen;
        case "REDRAW":
            Gcd.Redraw;
        case "STL":
            clsEntities.ExportSTL;

        default:
             //o = cadDimension // a test

             // Intercepto Alias
            if ( Config.oAlias.ContainsKey(Lower(EnteredText)) )
            {
                EnteredText = Upper(Config.oAlias[Lower(EnteredText)]);
            }

            if ( Gcd.CCC.ContainsKey(EnteredText) )
            {
                o = Gcd.CCC[EnteredText];
            }
            else
            {

                DrawingAIds.ErrorMessage = "Command not recognized";
                return;
            }

             // check if the class needs to be run trough other
            if ( o.usewith == "" ) // its a tool
            {
                Gcd.clsJobPrevious = Gcd.clsJob;
                Gcd.clsJob = o;
                Gcd.clsJob.start;

            } // its propably an eentity
            else
            {
                Gcd.clsJobPrevious = Gcd.clsJob;
                Gcd.clsJob = Gcd.CCC[o.usewith];
                Gcd.clsJob.start(o);

            }

    }
    Gcd.Drawing.iEntity.Clear;
    return;

     // TODO: dejar comentado mientras hagamos debug
     // catch

    DrawingAIds.ErrorMessage = "Command not recognized";
     //
     //

}

public void Draw()
    {


     double[] flxPoly = new double[] {};
    int iColor ;         

    Super.Draw();

    if ( SelectCrossing )
    {
        iColor = Color.Red;
    }
    else
    {
        iColor = Color.Green;
    }
    if ( RectActive )
    {

         // si estamos dentro un viewport no hay nada que dibujar, porque no estamos seleccionando nada
        if ( Gcd.Drawing.Sheet.Viewport ) return;
        if ( SelectType == SelectTypeRect || SelectType == SelectTypeSingleAndRect )
        {
            SelectionPoly.Clear; // aprovecho

            glx.Rectangle2D(SelStartXr, SelStartYr, SelEndXr - SelStartXr, SelEndYr - SelStartYr, Color.RGB(224, 220, 207, 215),0,0,0, iColor, 1, Gcd.stiDashedSmall, 2);

            return;
        }
        else if ( SelectType == SelectTypePoly )
        {

            fMain.PopupMenu = ""; // no hay menu contextual

             // como pude habver cambiado en este momento el modo de seleccion, chequeo
            if ( SelectionPoly.Count == 0 )
            {
                SelectionPoly.Add(SelStartXr);
                SelectionPoly.Add(SelStartYr);
            }
            flxPoly.Clear;
            flxPoly.Insert(SelectionPoly.Copy());
            flxPoly.Add(SelEndXr);
            flxPoly.Add(SelEndYr);
            flxPoly.Add(SelectionPoly[0]);
            flxPoly.Add(SelectionPoly[1]);
            glx.PolyLines(flxPoly, Color.red, 1, Gcd.stiDashedSmall);
        }
        else
        {
            SelectionPoly.Clear;

        }

    }

     // No vamos a dibujar nada relativo a los grips si estamsos selecionando para alguna Tool
    if ( Gcd.clsJobCallBack ) return;

    DrawingAids.DrawGrips(Gcd.Drawing.Sheet.Grips);

    if ( EntityForEdit ) Gcd.CCC[EntityForEdit.Gender].Draw(EntityForEdit);
    if ( OriginalEntityForEdit )
    {
        if ( GripCopying )
        {
        }
        else
        {
            Gcd.CCC[OriginalEntityForEdit.Gender].DrawShadow(OriginalEntityForEdit);
        }
    }
    if ( GripPoint )
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
    else if ( GripHovered )
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


    if ( GripPoint )
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
        Gcd.CCC[GripPoint.AsociatedEntity.Gender].buildgeometry(GripPoint.AsociatedEntity);
        Gcd.redraw;

    }

}

public  Grip FindGrip(double x, double y)
    {


    Grip g;

    foreach ( var g in Gcd.Drawing.Sheet.Grips)
    {
        if (puntos.Around(x, y, g.x, g.y, Gcd.Metros(Config.GripProximityDistance)))
        {

            return g;
        }
         //
    }
    DrawingAids.txtFrom = "";
    return null;

}

public void KeyDown(int iCode)
    {


    if ( iCode == Key.ControlKey )
    {
        GripCopying = true;
        fMain.GLArea1.Cursor = Gcd.CursorSelectAdd;
        SelectMode = SelectModeAdd;
    }
    else if ( iCode == Key.ShiftKey )
    {
        GripCopying = false;
        fMain.GLArea1.Cursor = Gcd.CursorSelectRem;
        SelectMode = SelectModeRem;
    }
    else if ( iCode == Key.AltKey )
    {
        GripCopying = false;
        fMain.GLArea1.Cursor = Gcd.CursorSelectXchange;
    }

}

public void KeyUp(int iCode)
    {


    if ( iCode == Key.ControlKey )
    {
        GripCopying = false;
        fMain.GLArea1.Cursor = Gcd.CursorCross;
        SelectMode = SelectModeNew;
    }
    else if ( iCode == Key.ShiftKey )
    {
        GripCopying = false;
        SelectMode = SelectModeNew;
        fMain.GLArea1.Cursor = Gcd.CursorCross;
    }
    else if ( iCode == Key.AltKey )
    {
        GripCopying = false;
        SelectMode = SelectModeNew;
        fMain.GLArea1.Cursor = Gcd.CursorCross;
    }

}

}
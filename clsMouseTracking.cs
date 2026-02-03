using System.Collections.Generic;
using System.Linq;

namespace Gaucho
{
public static class clsMouseTracking
{
 // Gambas class file

 // Estrategias:
 // Si tengo pre seteados los puntos, lineas y poligonos puedo buscar con ese orden
 // de prelacion. Luego una linea sera detectada antes que un poligono. Ello puede ser
 // muy util en caso de poligonos grandes con lineas dentro que deban ser detectadas
 // antes.

 // La otra estrategia es la actual (jun/21) de generar una lista de entidades visibles
 // de acuerdo a los layers encendidos y las entidades que se ven realemnte en la pantalla.



public static Entity?   LastFound = null;          // ultima entidad encontrada
public static double LastPointToCursorDistance = 0.0;         
public static bool WaitEnabled = true;
public static bool Searching = false;         
public static Dictionary<string, Entity> EntitiesSearchables = new();         
        
public static int SearchingSpeed = 250;    // la cantidad de entidades que busco por 0.1 segundos

    // Recolecto las entidades que estan alrededor del mouse, dentro de una tolerancia

public static void CollectEntitiesAroundMouse(double Xr, double Yr, int iTolerance)
    {


    double x0 = 0.0;         
    double y0 = 0.0;         
    double x1 = 0.0;         
    double y1 = 0.0;         
    Layer? lay = null;         
    Entity? e = null;         
    int i = 0;         
    int tot = 0;         
    Sheet? s = null;         
    string ho = "";         
    Timer? t = null;         
    bool insideX = false;         
    bool insideY = false;         
    double tolerance = Gcd.Metros(iTolerance);
    int iCount =0;         

    x0 = Xr - tolerance;
    y1 = Yr + tolerance;
    x1 = Xr + tolerance;
    y0 = Yr - tolerance;

    EntitiesSearchables.Clear();

     // ahora recorro las entidades visibles y veo cuales estan dentro del rectangulo

    foreach ( var e1 in Gcd.Drawing.Sheet.EntitiesVisibles)
    {
        e = e1.Value;
        iCount++;
        if ( iCount == Config.TrackMaxEntitiesNumber ) break;
        if ( e.pLayer.Visible )
        {
            insideX = false;
            insideY = false;
            if ( (e.Limits[0] <= x1) && (e.Limits[2] >= x0) ) insideX = true;
            if ( (e.Limits[1] <= y1) && (e.Limits[3] >= y0) ) insideY = true;

            if ( insideX && insideY )
            {

                EntitiesSearchables.Add(e.id, e);

            }

        }
    }

}

 // Veo si estoy sobre una entidad y la devuelvo
// public static List<Entity> CheckAboveEntities(double Xr, double Yr, int iTolerance= 12)
//     {


//      List<Entity> fEntities = new List<Entity>();         
//     Entity e ;         
//     // double t = Timer;

//     Searching = true;

//      //CollectEntitiesAroundMouse(Xr, Yr, iTolerance)
//     EntitiesFound.Clear();
//     do {
//         if ( Gcd.flgQuitSearch ) break;
//         EntitiesFound= CheckAboveEntity(Xr, Yr, 12, e);
//         if ( e != null )
//         {
//             if ( fEntities.Count > 0 )
//             {
//                 if ( e == fEntities.First() ) break;
//             }

//             fEntities.Add(e); // agrego la parte
//             if ( e.Container.Parent != null ) fEntities.Add(e.Container.Parent); // y el Insert
//         } else {
//             break;
//         }
//         // if ( WaitEnabled ) WaitCallback();
//     } while ( true );

//     Searching = false;
//     // fMain.benMouseTracking = Timer - t;
//     return fEntities;

// }

public static List<double> CheckBestPOI(double Xr, double Yr, Entity? eExclude = null)
    {


     List<double> CurrentPoint = new List<double>();
     List<double> BestPoint = new List<double>();         
    double d = 0.0;         
    double ShortestDistance = 1e10;         
    //  Entity e ;         

    BestPoint.Add(0);
    BestPoint.Add(0);
    BestPoint.Add(0);
    ShortestDistance = 1e10;

    if ( (Gcd.Drawing.HoveredEntities) == null ) return BestPoint;

    foreach (var e in Gcd.Drawing.HoveredEntities)
    {
        if ( Gcd.flgQuitSearch ) break;
        if ( e == eExclude ) continue;
        if ( e == Gcd.Drawing.Sheet.SkipSearch ) continue;

        if ( e.Gender == "INSERT" ) continue;
        CurrentPoint = CheckPOI(Xr, Yr, e);

         // OBSOLETO, ahora verifico que el punto encontrado este cerca del mouse
         // // veo si el punto cae dentro de la pantalla
         // If Gcd.XPix(CurrentPoint[0]) <= 0 Or Gcd.XPix(CurrentPoint[0]) >= Gcd.Drawing.Sheet.GlSheet.w Then Continue
         // If Gcd.YPix(CurrentPoint[1]) <= 0 Or Gcd.YPix(CurrentPoint[1]) >= Gcd.Drawing.Sheet.GlSheet.h Then Continue

        if ( CurrentPoint[2] > 0 )
        {
            d = Puntos.Distancia(Xr, Yr, CurrentPoint[0], CurrentPoint[1]);

             //el punto no puede estar lejos del mouse
            if ( d > Gcd.Metros(Config.SnapDistancePix) ) continue;

            if ( d < ShortestDistance )
            {
                ShortestDistance = d;
                BestPoint[0] = CurrentPoint[0];
                BestPoint[1] = CurrentPoint[1];
                BestPoint[2] = CurrentPoint[2];
            }

        }
    }

    return BestPoint;

}

 // Veo si estoy sobre una entidad y la devuelvo
public static List<Entity> CheckAboveEntity(double Xr, double Yr, int iTolerance= 12, Entity? SkipEntiy = null)
    {


    int iVisible = 0;         
    bool Found = true;
    Entity? eVisible = null;         
    Entity? ePartFound = null;         
    Entity? eInsertFound = null;         
    Entity? eBounded = null;         
    Entity? ePolyline = null;         
    double tolerance = Gcd.Metros(iTolerance);
    Entity? eFound = null;         

    Gcd.flgSearchingAllowed = false;
    iVisible = 0;
    Found = false;
    eFound = null;
    eBounded = null;
    ePolyline = null;
  List<Entity> EntitiesFound = new List<Entity>(); 
    EntitiesFound.Clear();

    foreach (var eVisible1 in Gcd.Drawing.Sheet.EntitiesVisibles)
    {
        eVisible = eVisible1.Value;
        if ( WaitEnabled )
        {

            if ( iVisible == SearchingSpeed )
            {
                Gb.Wait(0);
                iVisible = 0;
            }
        }
        if ( Gcd.flgQuitSearch ) break;
        iVisible++;
         //If iVisible

         //depre If Not eVisible.Trackable Then Continue

         // vemos si esta entidad no debo tenerla en cuenta
        if ( eVisible == SkipEntiy ) continue;

        if ( eVisible == Gcd.Drawing.Sheet.SkipSearch ) continue;

        if ( Gcd.CCC[eVisible.Gender].OverMe(eVisible, Xr, Yr, tolerance) ) EntitiesFound.Add(eVisible);
         //     //If WaitEnabled Then Wait 0
         //     //Debug eVisible.id, Rnd()
         //
         //     Found = True
         //     If eVisible.Container.Parent Then
         //
         //         ePartFound = eVisible
         //         eInsertFound = eVisible.Container.Parent
         //
         //         //eInsertFound = Gcd.Drawing.Entities[eVisible.HandleOwner]
         //     Else If eVisible.Polygon.Count > 0 Then
         //         eBounded = eVisible
         //     Else If eVisible.PolyLine.Count > 0 Then
         //         ePolyline = eVisible
         //
         //     Else
         //
         //         eFound = eVisible
         //
         //         Break
         //     End If
         // End If

         //utils.DoEvents(0.01000)

    }

    Gcd.flgSearchingAllowed = true;

     // If Found Then                           // devuelvo con orden de prelacion
     //     If eFound Then Return eFound        // primero las lineas
     //     If ePolyline Then Return ePolyline  // luego las Poly
     //     If eBounded Then Return eBounded    // luego Polygonos
     //     Return ePartFound //eInsertFound                 // luego Insertos
     // Else
    return EntitiesFound;
     // End If

}

 // This is called by MouseMove events, so make sure its fast
 // and its called form there
public static List<double> CheckPOI(double Xr, double Yr, Entity e)
    {


    List<double> rData = new List<double>();
    List<double> CurrentPoint = new List<double>();
    Entity? e2 = null;         
    Entity? ePart = null;         
    int indexEntity =0;         
    int i =0;         
    int iInter =0;         
    int i2 =0;         
    int iPoligon =0;         
    int iLine =0;         
    int iPoint =0;         
    double pend1 =0;          // vars para las ecuacionesde las rectas
    double base1 =0;         
    double pend2 =0;         
    double base2 =0;         
    List<double> pNea = new List<double>();
    List<double> puntoB = new List<double>();
    List<double> puntoA = new List<double>();
    List<double> pIntersection = new List<double>();
    List<double> pInter = new List<double>();
    List<double> pInterPoly = new List<double>();
    List<double> pInterPoly2 = new List<double>();         
    double End1X =0;         
    double End1Y =0;         
    double End2X =0;         
    double End2Y =0;         
    double x0 =0;         
    double y0 =0;         
    double x1 =0;         
    double y1 =0;         
     // Distancias  los enganches, para determinar cual es el mas cercano al puntero
    double DistPer = 1e6;
    double DistEnd1 = 1e6;
    double DistEnd2 = 1e6;
    double DistMid = 1e6;
    double DistBase = 1e6;
    double DistTang = 1e6;
    double DistQuad = 1e6;
    double DistCenter = 1e6;
    double DistInter = 1e6;
    double DistNearest = 1e6;
    double Dist = 1e5;   // el menor
    double DistEnd1B = 1e6;         
    double DistEnd2B =1e6;         
    double DistMidB =1e6;         
    double tolerance=0;         
    double d =0;         
    double ShortestDistance =0;   
    double qx =0;         
    double qy =0;         
    double q1 =0;         
    double q2 =0;         
    double q3 =0;         
    double q4 =0; 
    double tx =0;         
    double ty =0;         
    double t1x =0;         
    double t1y =0;         
    double t2x =0;         
    double t2y =0;       

     // If Me.flgSearchingPOI Then Return   // no nesting this
    Gcd.flgSearchingPOI = true;
     // Debug "Iniciando busqueda de POI", Rnd(0, 1000)
    // double t = Timer;

    tolerance = Gcd.Metros(16);
     //Debug "Checking POI"
    rData.AddRange(new double[] {0, 0, 0});
    iPoligon = -1;
    iLine = -1;

     //iPoligon = Puntos.FindPOIPoligon(Xr, Yr, Gcd.CurrDrawing.poiPoligons, Gcd.CurrDrawing.poiPoligonStartIndex, Gcd.CurrDrawing.poiPoligonElements)
     //iLine = Puntos.FindPOILines(Xr, Yr, Gcd.CurrDrawing.poiLines, Gcd.CurrDrawing.Metros(16))
     // rData = Puntos.FindPOI(Xr, Yr, Gcd.CurrDrawing.poiPoints, Gcd.CurrDrawing.Metros(16))
     // i = rData[2]
     // If i >= 0 Then
     //     rData[2] = Gcd.CurrDrawing.poitype[i]
     //     rData.Add(Gcd.CurrDrawing.poiEntities[i])
     //
     // Else
     //     rData.Clear
     //     rData.Insert([0, 0, -1, -1])
     // Endif
     // Me.flgSearchingPOI = False
     // Return rData
     //Debug "total poi check time ", Timer - t, Gcd.CurrDrawing.poiPoligons.Count + Gcd.CurrDrawing.poiLines.Count + Gcd.CurrDrawing.poiPoints.Count, " points parsed"
     //e = Gcd.Drawing.HoveredEntity

    if ( e == null )
    {
        Gcd.flgSearchingPOI = false;
        DrawingAids.txtSnapTo = "";
        rData[0] = Xr;
        rData[1] = Yr;
        rData[2] = 0;

        Gcd.Drawing.eLastEntity = null;

        return rData;
    }
     // If iLine > 0 Then e = Gcd.CurrDrawing.arrEntities[Gcd.CurrDrawing.poiLinesEntities[iLine]]
     // If iPoligon > 0 Then e = Gcd.CurrDrawing.arrEntities[iPoligon]

     //Debug "Encontrada entidad ", e.Gender, " en ", Timer - t

    switch ( e.Gender)
    {

        case "INSERT":
            ShortestDistance = 1e10;
            foreach ( var ePart1 in e.pBlock.entities)
            {
                ePart = ePart1.Value;
                if (Gcd.flgQuitSearch) break;
                if (WaitEnabled) Gb.Wait(0);
                CurrentPoint = CheckPOI(Xr, Yr, ePart);

                if (CurrentPoint[2] > 0)
                {
                    d = Puntos.Distancia(Xr, Yr, CurrentPoint[0], CurrentPoint[1]);

                    if ( d < ShortestDistance )
                    {
                        ShortestDistance = d;
                        rData[0] = CurrentPoint[0];
                        rData[1] = CurrentPoint[1];
                        rData[2] = CurrentPoint[2];
                    }

                }
            }
            break;

        case "LINE":

             // perpendicular
            if ( Gcd.Drawing.LastPoint.Count > 0 ) // tengo un punto anterior
            {
                if ( e.P[2] - e.P[0] != 0 )
                {
                    pend1 = (e.P[3] - e.P[1]) / (e.P[2] - e.P[0]);
                    base1 = e.P[1] - pend1 * e.P[0]; // Y = pend1 X + base1

                     // ecuacion de la recta perpendicular que pasa por el punto anterior
                    if ( pend1 != 0 )
                    {
                        pend2 = -1 / pend1;
                        base2 = Gcd.Drawing.LastPoint[1] - pend2 * Gcd.Drawing.LastPoint[0];

                         // necesito otro punto
                        if ( Gcd.Drawing.LastPoint[0] != 0 )
                        {
                            puntoA.Add(0);
                            puntoA.Add(base2);
                        } else {
                            puntoA.Add(1);
                            puntoA.Add(pend2 + base2);

                        }

                         // determino la interseccion de ambas
                        puntoB = Puntos.lineLineIntersection(Gcd.Drawing.LastPoint, puntoA, new List<double> {e.P[0], e.P[1]}, new List<double> {e.P[2], e.P[3]});
                    } else { // la recta es horizontal

                        puntoB.Add(Gcd.Drawing.LastPoint[0]);
                        puntoB.Add(e.P[1]);

                    }
                } else { // la recta es vertical

                    puntoB.Add(e.P[0]);
                    puntoB.Add(Gcd.Drawing.LastPoint[1]);

                }

                 // veo si el punto b esta entre los extremos de la linea , aunque deberia estarlo!!!
                if ( Puntos.onSegment(e.P[0], e.P[1], puntoB[0], puntoB[1], e.P[2], e.P[3]) )
                {

                     // determino la Distancia al puntero para ofrecerlo como opcion de enganche
                    DistPer = Puntos.Distancia(puntoB[0], puntoB[1], Xr, Yr);

                }
            }
            

             // busco tambien intersecciones
            foreach ( var e21 in Gcd.Drawing.Sheet.EntitiesVisibles)
            {
                e2 = e21.Value;
                if ( Gcd.flgQuitSearch ) break;
                if ( WaitEnabled ) Gb.Wait(0);
                if ( e == e2 ) continue;

                switch ( e2.Gender)
                {

                    case "LINE":
                        if ( Puntos.doIntersect(e.P[0], e.P[1], e.P[2], e.P[3], e2.P[0], e2.P[1], e2.P[2], e2.P[3]) )
                        {
                            pInter = Puntos.lineLineIntersection(new List<double> {e.P[0], e.P[1]}, new List<double> {e.P[2], e.P[3]}, new List<double> {e2.P[0], e2.P[1]}, new List<double> {e2.P[2], e2.P[3]});

                            pIntersection.AddRange(pInter);

                        }
                        break;

                    case "LWPOLYLINE" or "SOLID":

                        pInterPoly = Puntos.LinePolyIntersection3(e.P, e2.P);
                        pIntersection.AddRange(pInterPoly);
                        break;
                    case "CIRCLE" or "ARC" or "SPLINE":
                        pInterPoly2 = Puntos.LinePolyIntersection3(e.P, e2.PolyLine);
                        pIntersection.AddRange(pInterPoly2);
                        break;
                }
            }

            if ( pIntersection.Count > 0 ) // puede no haber intersaeccion
            {
                iInter = Puntos.FindNearest(Xr, Yr, pIntersection);
                DistInter = Puntos.Distancia(Xr, Yr, pIntersection[iInter], pIntersection[iInter + 1]);
            } else {
                DistInter = 10e10;
            }   
            break;
        case "CIRCLE":
            if ( Gcd.Drawing.LastPoint.Count > 0 ) // tengo un punto anterior
            {

                 // buscamos una tangente..

                 // Extraido de StackExchange
                 // #Data Section, change As You need #
                 // Cx, Cy = coordenadas del centro
                double Cx = e.P[0];
                double Cy = e.P[1];
                 
                double r = e.fParam[0];

                double Px = Gcd.Drawing.LastPoint[0];
                double Py = Gcd.Drawing.LastPoint[1];

                double dx = Px - Cx;
                double dy = Py - Cy;
                double dXr = -dy;
                double dYr = dx;
                DistCenter = Math.Sqrt(dx * dx + dy * dy); // y ya que estoy veo el Center

                if ( DistCenter >= r )
                {
                    double rho = r / DistCenter;
                    double ad = rho * rho;
                    double bd = rho * Math.Sqrt(1 - rho * rho);

                     // los puntos tangente
                    t1x = Cx + ad * dx + bd * dXr;
                    t1y = Cy + ad * dy + bd * dYr;
                    t2x = Cx + ad * dx - bd * dXr;
                    t2y = Cy + ad * dy - bd * dYr;

                    if ( (DistCenter / r - 1) < 1E-8 )
                    {
                         //P is on the circumference
                    } else { // determino cual de las dos tangentes es la mas cercana

                         if ( Puntos.Distancia(Xr, Yr, t1x, t1y) > Puntos.Distancia(Xr, Yr, t2x, t2y) )
                        {
                            tx = t2x;
                            ty = t2y;
                        } else {
                            tx = t1x;
                            ty = t1y;

                        }
                        DistTang = Puntos.Distancia(Xr, Yr, tx, ty);
                    }
                } else { // esta dentro del circulo

                    DistTang = 1e6;
                     // No tangent Is Possible
                }
            }

             // busco los quadrants
                   
            q1 = Puntos.Distancia(e.P[0] - e.fParam[0], e.P[1], Xr, Yr);
            q2 = Puntos.Distancia(e.P[0] + e.fParam[0], e.P[1], Xr, Yr);
            q3 = Puntos.Distancia(e.P[0], e.P[1] - e.fParam[0], Xr, Yr);
            q4 = Puntos.Distancia(e.P[0], e.P[1] + e.fParam[0], Xr, Yr);

            DistQuad = 1e10;
            if ( DistQuad > q1 )
            {
                DistQuad = q1;
                qx = e.P[0] - e.fParam[0];
                qy = e.P[1];
            }
            if ( DistQuad > q2 )
            {
                DistQuad = q2;
                qx = e.P[0] + e.fParam[0];
                qy = e.P[1];
            }
            if ( DistQuad > q3 )
            {
                DistQuad = q3;
                qx = e.P[0];
                qy = e.P[1] - e.fParam[0];
            }
            if ( DistQuad > q4 )
            {
                DistQuad = q4;
                qx = e.P[0];
                qy = e.P[1] + e.fParam[0];
            }
            break;

            default:
            break;

    }

     // otros puntos
    switch ( e.Gender)
    {
        case "LINE":

            DistEnd1 = Puntos.Distancia(e.P[0], e.P[1], Xr, Yr);
            End1X = e.P[0];
            End1Y = e.P[1];

            End2X = e.P[2];
            End2Y = e.P[3];
            DistEnd2 = Puntos.Distancia(e.P[2], e.P[3], Xr, Yr);

            DistMid = Puntos.Distancia((e.P[2] + e.P[0]) / 2, (e.P[3] + e.P[1]) / 2, Xr, Yr);
            i = 0;

            pNea = Puntos.NearestToLine(Xr, Yr, e.P[0], e.P[1], e.P[2], e.P[3]);

            DistNearest = Puntos.Distancia(Xr, Yr, pNea[0], pNea[1]);
            break;

        case "TEXT" or "MTEXT":

            DistBase = Puntos.Distancia(e.P[0], e.P[1], Xr, Yr);
            break;

        case "CIRCLE":
            DistCenter = Puntos.Distancia(e.P[0], e.P[1], Xr, Yr);

            pNea = Puntos.NearestToPolyLine(Xr, Yr, e.PolyLine);

            DistNearest = Puntos.Distancia(Xr, Yr, pNea[0], pNea[1]);
            break;

        case "ELLIPSE" or "ARC" or "SPLINE":

            DistCenter = Puntos.Distancia(e.P[0], e.P[1], Xr, Yr);

            if ( e.PolyLine.Count > 0 )
            {
                DistEnd1 = Puntos.Distancia(e.PolyLine[0], e.PolyLine[1], Xr, Yr);
                End1X = e.PolyLine[0];
                End1Y = e.PolyLine[1];
                DistEnd2 = Puntos.Distancia(e.PolyLine[e.PolyLine.Count - 2], e.PolyLine[e.PolyLine.Count - 1], Xr, Yr);
                End2X = e.PolyLine[e.PolyLine.Count - 2];
                End2Y = e.PolyLine[e.PolyLine.Count - 1];

                pNea = Puntos.NearestToPolyLine(Xr, Yr, e.PolyLine);
            }
            DistNearest = Puntos.Distancia(Xr, Yr, pNea[0], pNea[1]);
            break;

        case "LWPOLYLINE" or "SOLID":
             // tengo que ver cual es el mas cercano

            for ( i = 0; i <= e.P.Count - 4; i+=2)
            {

                x0 = e.P[i + 0];
                y0 = e.P[i + 1];
                x1 = e.P[i + 2];
                y1 = e.P[i + 3];
                 // busco en que tramo estoy
                if ( Puntos.doIntersect(Xr - tolerance, Yr, Xr + tolerance, Yr, x0, y0, x1, y1) || Puntos.doIntersect(Xr, Yr - tolerance, Xr, Yr + tolerance, x0, y0, x1, y1) )
                {

                    DistEnd1 = Puntos.Distancia(x0, y0, Xr, Yr);
                     //If DistEnd1 > DistEnd1B Then DistEnd1 = DistEnd1B
                    End1X = e.P[i];
                    End1Y = y0;

                    DistEnd2 = Puntos.Distancia(x1, y1, Xr, Yr);
                     //If DistEnd2 > DistEnd2B Then DistEnd2 = DistEnd2B
                    End2X = x1;
                    End2Y = y1;

                    DistMid = Puntos.Distancia((x1 + x0) / 2, (y1 + y0) / 2, Xr, Yr);
                     //If DistMid > DistMidB Then DistMid = DistMidB

                     // perpendicular
                    if ( Gcd.Drawing.LastPoint.Count > 0 ) // tengo un punto anterior
                    {

                        if ( x1 - x0 != 0 )
                        {
                            pend1 = (y1 - y0) / (x1 - x0);
                            base1 = y1 - pend1 * x0; // Y = pend1 X + base1

                             // ecuacion de la recta perpendicular que pasa por el punto anterior
                            if ( pend1 != 0 )
                            {
                                pend2 = -1 / pend1;
                                base2 = Gcd.Drawing.LastPoint[1] - pend2 * Gcd.Drawing.LastPoint[0];

                                 // necesito otro punto
                                if ( Gcd.Drawing.LastPoint[0] != 0 )
                                {
                                    puntoA.Add(0);
                                    puntoA.Add(base2);
                                } else {
                                    puntoA.Add(1);
                                    puntoA.Add(pend2 + base2);

                                }

                                 // determino la interseccion de ambas
                                puntoB = Puntos.lineLineIntersection(Gcd.Drawing.LastPoint, puntoA, [x0, y0], [x1, y1]);
                            } else { // la recta es horizontal

                                puntoB.Add(Gcd.Drawing.LastPoint[0]);
                                puntoB.Add(y0);

                            } 
                        } else {// la recta es vertical

                            puntoB.Add(x0);
                            puntoB.Add(Gcd.Drawing.LastPoint[1]);

                        }

                         // veo si el punto b esta entre los extremos de la linea , aunque deberia estarlo!!!
                        if ( Puntos.onSegment(x0, y0, puntoB[0], puntoB[1], x1, y1) )
                        {

                             // determino la Distancia al puntero para ofrecerlo como opcion de enganche
                            DistPer = Puntos.Distancia(puntoB[0], puntoB[1], Xr, Yr);

                        }
                    }

                    break;
                }
            }

            pNea = Puntos.NearestToPolyLine(Xr, Yr, e.P);
            DistNearest = Puntos.Distancia(Xr, Yr, pNea[0], pNea[1]);
            break;

            default:
            break;



    }

     // veo cual es la opcion mas cercana al puntero

     // Dist = Min(DistBase, DistCenter, DistEnd1, DistEnd2, DistMid, DistPer, DistQuad) // NO FUNCIONA

    if ( (Gcd.SnapMode & Gcd.poiBasePoint) == Gcd.poiBasePoint)
    {
        if ( Dist > DistBase ) Dist = DistBase;
    }
    if ( (Gcd.SnapMode & Gcd.poiCenter) == Gcd.poiCenter)
    {
        if ( Dist > DistCenter ) Dist = DistCenter;
    }
    if ( (Gcd.SnapMode & Gcd.poiEndPoint) == Gcd.poiEndPoint)
    {
        if ( Dist > DistEnd1 ) Dist = DistEnd1;
        if ( Dist > DistEnd2 ) Dist = DistEnd2;
    }
    if ( (Gcd.SnapMode & Gcd.poiMidPoint) == Gcd.poiMidPoint)
    {
        if ( Dist > DistMid ) Dist = DistMid;
    }
    if ( (Gcd.SnapMode & Gcd.poiPerpendicular) == Gcd.poiPerpendicular)
    {
        if ( Dist > DistPer ) Dist = DistPer;
    }
    if ( (Gcd.SnapMode & Gcd.poiQuadrant) == Gcd.poiQuadrant)
    {
        if ( Dist > DistQuad ) Dist = DistQuad;
    }
    if ( (Gcd.SnapMode & Gcd.poiTangent) == Gcd.poiTangent)
    {
        if ( Dist > DistTang ) Dist = DistTang;
    }
    if ( (Gcd.SnapMode & Gcd.poiIntersection) == Gcd.poiIntersection)
    {
        if ( Dist > DistInter ) Dist = DistInter;
    }

    if ( (Gcd.SnapMode & Gcd.poiNearest) == Gcd.poiNearest)
    {
        if ( Dist > DistNearest ) Dist = DistNearest;
    }

     // ofrezco ese punto
    if ( Dist == DistEnd1 )
    {
        rData[0] = End1X;
        rData[1] = End1Y;
        rData[2] = Gcd.poiEndPoint;
        DrawingAids.txtSnapTo = "EndPoint";
    }

     // ofrezco ese punto
    if ( Dist == DistEnd2 )
    {
        rData[0] = End2X;
        rData[1] = End2Y;
        rData[2] = Gcd.poiEndPoint;
        DrawingAids.txtSnapTo = "EndPoint";
    }

     // ofrezco ese punto
    if ( Dist == DistMid )
    {
        rData[0] = (e.P[i + 2] + e.P[i + 0]) / 2;
        rData[1] = (e.P[i + 3] + e.P[i + 1]) / 2;
        rData[2] = Gcd.poiMidPoint;
        DrawingAids.txtSnapTo = "MidPoint";
    }

     // ofrezco ese punto
    if ( Dist == DistInter )
    {
        rData[0] = pIntersection[iInter];
        rData[1] = pIntersection[iInter + 1];
        rData[2] = Gcd.poiIntersection;
        DrawingAids.txtSnapTo = "Intersection";
    }

     // ofrezco ese punto
    if ( Dist == DistPer )
    {

        rData[0] = puntoB[0];
        rData[1] = puntoB[1];
        rData[2] = Gcd.poiPerpendicular;
        DrawingAids.txtSnapTo = "Perpendicular";
    }

    if ( Dist == DistTang )
    {

        rData[0] = tx;
        rData[1] = ty;
        rData[2] = Gcd.poiTangent;
        DrawingAids.txtSnapTo = "Tangent";
    }

    if ( Dist == DistCenter )
    {

        rData[0] = e.P[0];
        rData[1] = e.P[1];
        rData[2] = Gcd.poiCenter;
        DrawingAids.txtSnapTo = "Center";
    }

    if ( Dist == DistQuad )
    {
        rData[0] = qx;
        rData[1] = qy;
        rData[2] = Gcd.poiQuadrant;
        DrawingAids.txtSnapTo = "Quadrant";
    }

    if ( Dist == DistBase )
    {
        rData[0] = e.P[0];
        rData[1] = e.P[1];
        rData[2] = Gcd.poiBasePoint;
        DrawingAids.txtSnapTo = "BasePoint";
    }

    if ( Dist == DistNearest )
    {
        rData[0] = pNea[0];
        rData[1] = pNea[1];
        rData[2] = Gcd.poiNearest;
        DrawingAids.txtSnapTo = "Nearest";
    }

     // Debug "pois checked"
    Gcd.flgSearchingPOI = false;
    LastPointToCursorDistance = Dist;
    Gcd.Drawing.eLastEntity = e;
    return rData;

}

 // Devuelve el punto en la entidad que esta mas cercano al provisto
public static List<double> GetNearestPoint(Entity e, double Xr, double Yr)
    {


    double x2 = 0.0;         
    double y2 = 0.0;         
    double c = 0.0;         
    double d = 0.0;         
    double dx = 0.0;         
    double dy = 0.0;         
    double m1 = 0.0;         
    double m2 = 0.0;         
    double angle = 0.0;         
    List<double> flxIntersec = new List<double>();         

    switch ( e.Gender)
    {
        case "LINE":
            if ( (e.P[2] - e.P[0]) != 0 )
            {
                 // la pendiente de la recta donde esta la linea es
                m1 = (e.P[3] - e.P[1]) / (e.P[2] - e.P[0]);
                if ( m1 != 0 )
                {
                    m2 = -1 / m1;

                     // la ecuacion de la recta que pasa por Xr,Yr con pendiente m2 es:
                     // Y = m X + c
                    c = Yr - m2 * Xr;
                     // si
                    x2 = 1e10;
                    y2 = m2 * x2 + c;

                     // obtenemos el punto interseccion
                    flxIntersec = Puntos.lineLineIntersection(new List<double> {Xr, Yr}, new List<double> {x2, y2}, new List<double> {e.P[0], e.P[1]}, new List<double> {e.P[2], e.P[3]});

                     // y salimos

                    return flxIntersec;

                } else {// la perpendicular es verical
                    return new List<double> {Xr, e.P[1]};
                }
            } else {// la perpendicular es horizontal
                return new List<double> {e.P[0], Yr};

            }
        default:
            flxIntersec = Puntos.NearestToPolyLine(Xr, Yr, e.PolyLine);
            return flxIntersec;

    }
    }
}   


}

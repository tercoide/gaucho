using Gaucho;
public class clsMouseTracking
{
 // Gambas class file

 // Estrategias:
 // Si tengo pre seteados los puntos, lineas y poligonos puedo buscar con ese orden
 // de prelacion. Luego una linea sera detectada antes que un poligono. Ello puede ser
 // muy util en caso de poligonos grandes con lineas dentro que deban ser detectadas
 // antes.

 // La otra estrategia es la actual (jun/21) de generar una lista de entidades visibles
 // de acuerdo a los layers encendidos y las entidades que se ven realemnte en la pantalla.



public Entity   LastFound ;          // ultima entidad encontrada
public double LastPointToCursorDistance ;         
public bool WaitEnabled = true;
public bool Searching ;         
public Dictionary<string, string> EntitiesSearchables ;         
public Entity[] EntitiesFound ;         
public int SearchingSpeed = 250;    // la cantidad de entidades que busco por 0.1 segundos

    // Recolecto las entidades que estan alrededor del mouse, dentro de una tolerancia

public static void CollectEntitiesAroundMouse(double xr, double Yr, int iTolerance)
    {


    double x0 ;         
    double y0 ;         
    double x1 ;         
    double y1 ;         
    Layer lay ;         
    Entity e ;         
    int i ;         
    int tot ;         
    Sheet s ;         
    string ho ;         
    Timer t ;         
    bool insideX ;         
    bool insideY ;         
    double Tolerance = gcd.Metros(iTolerance);
    int iCount ;         

    x0 = xr - tolerance;
    y1 = yr + tolerance;
    x1 = xr + tolerance;
    y0 = yr - tolerance;

    EntitiesSearchables.Clear;

    foreach ( e in gcd.Drawing.Sheet.EntitiesVisibles)
    {
        Inc iCount;
        if ( iCount == Config.TrackMaxEntitiesNumber ) Break;
        if ( e.pLayer.Visible )
        {
            insideX = false;
            insideY = false;
            if ( (e.Limits[0] <= x1) && (e.Limits[2] >= x0) ) insideX = true;
            if ( (e.Limits[1] <= y1) && (e.Limits[3] >= y0) ) insidey = true;

            if ( insideX && insideY )
            {

                EntitiesSearchables.Add(e, e.id);

            }

        }
    }

}

 // Veo si estoy sobre una entidad y la devuelvo
public static  Entity[] CheckAboveEntities(double xr, double Yr, int iTolerance= 12)
    {


     Entity[] fEntities ;         
    Entity e ;         
    double t = Timer;

    Searching = true;

     //CollectEntitiesAroundMouse(xr, yr, iTolerance)
    EntitiesFound.Clear;
    do {
        if ( gcd.flgQuitSearch ) Break;
        e = CheckAboveEntity(xr, yr,, e);
        if ( e )
        {
            if ( fEntities.Count > 0 )
            {
                if ( e == fEntities.First ) Break;
            }

            fEntities.Add(e); // agrego la parte
            if ( e.Container.Parent ) fEntities.Add(e.Container.Parent); // y el Insert
        } else {
            Break;
        }
        if ( WaitEnabled ) Wait;
    } while ( true );

    Searching = false;
    fMain.benMouseTracking = Timer - t;
    return fEntities;

}

public static double[] CheckBestPOI(double xr, double Yr, Entity eExclude)
    {


     Float[] CurrentPoint ;         
     Float[] BestPoint ;         
    double d ;         
    double ShortestDistance ;         
    Entity e ;         

    BestPoint.Add(0);
    BestPoint.Add(0);
    BestPoint.Add(0);
    ShortestDistance = 1e10;

    if ( IsNull(gcd.Drawing.HoveredEntities) ) return BestPoint;

    foreach (var e in gcd.Drawing.HoveredEntities)
    {
        if ( gcd.flgQuitSearch ) Break;
        if ( e == eExclude ) Continue;
        if ( e == gcd.Drawing.Sheet.SkipSearch ) Continue;

        if ( e.Gender == "INSERT" ) Continue;
        CurrentPoint = CheckPOI(xr, yr, e);

         // OBSOLETO, ahora verifico que el punto encontrado este cerca del mouse
         // // veo si el punto cae dentro de la pantalla
         // If gcd.XPix(CurrentPoint[0]) <= 0 Or gcd.XPix(CurrentPoint[0]) >= gcd.Drawing.Sheet.GlSheet.w Then Continue
         // If gcd.YPix(CurrentPoint[1]) <= 0 Or gcd.YPix(CurrentPoint[1]) >= gcd.Drawing.Sheet.GlSheet.h Then Continue

        if ( CurrentPoint[2] > 0 )
        {
            d = puntos.distancia(xr, yr, CurrentPoint[0], CurrentPoint[1]);

             //el punto no puede estar lejos del mouse
            if ( d > gcd.Metros(Config.SnapDistancePix) ) Continue;

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
public static  Entity[] CheckAboveEntity(double xr, double Yr, int iTolerance= 12, Entity SkipEntiy = null)
    {


    int iVisible ;         
    bool Found = true;
    Entity eVisible ;         
    Entity ePartFound ;         
    Entity eInsertFound ;         
    Entity eBounded ;         
    Entity ePolyline ;         
    double Tolerance = gcd.Metros(iTolerance);
    Entity eFound ;         

    gcd.flgSearchingAllowed = false;
    iVisible = 0;
    Found = false;
    eFound = Null;
    eBounded = Null;
    ePolyline = Null;

    EntitiesFound.Clear;

    foreach (var eVisible in gcd.Drawing.Sheet.EntitiesVisibles)
    {
        if ( WaitEnabled )
        {

            if ( iVisible == SearchingSpeed )
            {
                Gb.Wait(0);
                iVisible = 0;
            }
        }
        if ( gcd.flgQuitSearch ) Break;
        Inc iVisible;
         //If iVisible

         //depre If Not eVisible.Trackable Then Continue

         // vemos si esta entidad no debo tenerla en cuenta
        if ( eVisible == SkipEntiy ) Continue;

        if ( eVisible == gcd.Drawing.Sheet.SkipSearch ) Continue;

        if ( Gcd.CCC[eVisible.gender].overme(eVisible, xr, yr, tolerance) ) EntitiesFound.Add(eVisible);
         //     //If WaitEnabled Then Wait 0
         //     //Debug eVisible.id, Rnd()
         //
         //     Found = True
         //     If eVisible.Container.Parent Then
         //
         //         ePartFound = eVisible
         //         eInsertFound = eVisible.Container.Parent
         //
         //         //eInsertFound = gcd.Drawing.Entities[eVisible.HandleOwner]
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

    gcd.flgSearchingAllowed = true;

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
public static double[] CheckPOI(double xr, double Yr, Entity e)
    {


     Float[] rData ;         
     Float[] CurrentPoint ;         
    Entity e2 ;         
    Entity ePart ;         
    int indexEntity ;         
    int i ;         
    int iInter ;         
    int i2 ;         
    int iPoligon ;         
    int iLine ;         
    int iPoint ;         
    double pend1 ;          // vars para las ecuacionesde las rectas
    double base1 ;         
    double pend2 ;         
    double base2 ;         
     Float[] pNea ;         
     Float[] puntoB ;         
     Float[] puntoA ;         
     Float[] pIntersection ;         
     Float[] pInter ;         
     Float[] pInterPoly ;         
     Float[] pInterPoly2 ;         
    double end1X ;         
    double end1Y ;         
    double end2X ;         
    double end2Y ;         
    double x0 ;         
    double y0 ;         
    double x1 ;         
    double y1 ;         
     // distancias  los enganches, para determinar cual es el mas cercano al puntero
    double DistPer = 1e6;
    double DistEnd1 = 1e6;
    double DistEnd2 = 1e6;
    double DistMId = 1e6;
    double DistBase = 1e6;
    double DistTang = 1e6;
    double DistQuad = 1e6;
    double DistCenter = 1e6;
    double DistInter = 1e6;
    double DistNearest = 1e6;
    double Dist = 1e5;   // el menor
    double DistEnd1B ;         
    double DistEnd2B ;         
    double DistMIdB ;         
    double tolerance ;         
    double d ;         
    double ShortestDistance ;         

     // If Me.flgSearchingPOI Then Return   // no nesting this
    gcd.flgSearchingPOI = true;
     // Debug "Iniciando busqueda de POI", Rnd(0, 1000)
    double t = Timer;

    tolerance = gcd.Metros(16);
     //Debug "Checking POI"
    rdata.Insert([0, 0, 0]);
    iPoligon = -1;
    iLine = -1;

     //iPoligon = puntos.FindPOIPoligon(xr, yr, gcd.CurrDrawing.poiPoligons, gcd.CurrDrawing.poiPoligonStartIndex, gcd.CurrDrawing.poiPoligonElements)
     //iLine = puntos.FindPOILines(xr, yr, gcd.CurrDrawing.poiLines, gcd.CurrDrawing.Metros(16))
     // rdata = puntos.FindPOI(xr, yr, gcd.CurrDrawing.poiPoints, gcd.CurrDrawing.Metros(16))
     // i = rdata[2]
     // If i >= 0 Then
     //     rData[2] = gcd.CurrDrawing.poiType[i]
     //     rData.Add(gcd.CurrDrawing.poiEntities[i])
     //
     // Else
     //     rdata.Clear
     //     rdata.Insert([0, 0, -1, -1])
     // Endif
     // Me.flgSearchingPOI = False
     // Return rdata
     //Debug "total poi check time ", Timer - t, gcd.CurrDrawing.poiPoligons.Count + gcd.CurrDrawing.poiLines.Count + gcd.CurrDrawing.poiPoints.Count, " points parsed"
     //e = gcd.Drawing.HoveredEntity

    if ( ! e )
    {
        gcd.flgSearchingPOI = false;
        DrawingAIds.txtSnapTo = "";
        rData[0] = xr;
        rData[1] = yr;
        rData[2] = 0;

        gcd.Drawing.eLastEntity = Null;

        return rdata;
    }
     // If iLine > 0 Then e = gcd.CurrDrawing.arrEntities[gcd.CurrDrawing.poiLinesEntities[iLine]]
     // If iPoligon > 0 Then e = gcd.CurrDrawing.arrEntities[iPoligon]

     //Debug "Encontrada entidad ", e.Gender, " en ", Timer - t

    switch ( e.Gender)
    {

        case "INSERT":
            Stop;
            ShortestDistance = 1e10;
            foreach ( var ePart in e.pBlock.Entities)
            {
                if (gcd.flgQuitSearch) Break;
                if (WaitEnabled) Gb.Wait(0);
                CurrentPoint = CheckPOI(Xr, Yr, ePart);

                if (CurrentPoint[2] > 0)
                {
                    d = puntos.distancia(xr, yr, CurrentPoint[0], CurrentPoint[1]);

                    if ( d < ShortestDistance )
                    {
                        ShortestDistance = d;
                        rData[0] = CurrentPoint[0];
                        rData[1] = CurrentPoint[1];
                        rData[2] = CurrentPoint[2];
                    }

                }
            }

        case "LINE":

             // perpendicular
            if ( gcd.Drawing.LastPoint.max > 0 ) // tengo un punto anterior
            {

                if ( e.P[2] - e.P[0] != 0 )
                {
                    pend1 = (e.P[3] - e.P[1]) / (e.P[2] - e.P[0]);
                    base1 = e.P[1] - pend1 * e.P[0]; // Y = pend1 X + base1

                     // ecuacion de la recta perpendicular que pasa por el punto anterior
                    if ( pend1 != 0 )
                    {
                        pend2 = -1 / pend1;
                        base2 = gcd.Drawing.LastPoint[1] - pend2 * gcd.Drawing.LastPoint[0];

                         // necesito otro punto
                        if ( gcd.Drawing.LastPoint[0] != 0 )
                        {
                            puntoA.Add(0);
                            puntoA.Add(base2);
                        }
                            puntoA.Add(1);
                            puntoA.Add(pend2 + base2);

                        }

                         // determino la interseccion de ambas
                        puntoB = puntos.lineLineIntersection(gcd.Drawing.LastPoint, puntoA, [e.P[0], e.P[1]], [e.P[2], e.P[3]]);
                    } // la recta es horizontal

                        puntoB.Add(gcd.Drawing.LastPoint[0]);
                        puntoB.Add(e.P[1]);

                    }
                } // la recta es vertical

                    puntoB.Add(e.P[0]);
                    puntoB.Add(gcd.Drawing.LastPoint[1]);

                }

                 // veo si el punto b esta entre los extremos de la linea , aunque deberia estarlo!!!
                if ( puntos.onSegment(e.P[0], e.P[1], puntoB[0], puntoB[1], e.P[2], e.P[3]) )
                {

                     // determino la distancia al puntero para ofrecerlo como opcion de enganche
                    DistPer = puntos.distancia(puntoB[0], puntoB[1], xr, yr);

                }
            }

             // busco tambien intersecciones
            foreach ( var e2 in gcd.Drawing.Sheet.EntitiesVisibles)
            {
                if ( gcd.flgQuitSearch ) Break;
                if ( WaitEnabled ) Wait;
                if ( e == e2 ) Continue;

                switch ( e2.Gender)
                {

                    case "LINE":
                        if ( puntos.doIntersect(e.P[0], e.P[1], e.P[2], e.P[3], e2.P[0], e2.P[1], e2.P[2], e2.P[3]) )
                        {
                            pInter = puntos.lineLineIntersection([e.P[0], e.P[1]], [e.P[2], e.P[3]], [e2.P[0], e2.P[1]], [e2.P[2], e2.P[3]]);

                            pIntersection.Insert(pInter);

                        }

                    case "LWPOLYLINE", "SOLID":

                        pInterPoly = puntos.LinePolyIntersection3(e.p, e2.p);
                        pIntersection.Insert(pInterPoly);
                    case "CIRCLE", "ARC", "SPLINE":
                        pInterPoly2 = puntos.LinePolyIntersection3(e.p, e2.PolyLine);
                        pIntersection.Insert(pInterPoly2);
                }
            }

            if ( pIntersection.Count > 0 ) // puede no haber intersaeccion
            {
                iInter = puntos.FindNearest(xr, yr, pIntersection);
                DistInter = puntos.distancia(xr, yr, pIntersection[iInter], pIntersection[iInter + 1]);
            }
                DistInter = 10e10;
            }

        case "CIRCLE":
            if ( gcd.Drawing.LastPoint.max > 0 ) // tengo un punto anterior
            {

                 // buscamos una tangente..

                 // Extraido de StackExchange
                 // #Data Section, change As You need #
                 // Cx, Cy = coordenadas del centro
                double Cx = e.p[0];
                double Cy = e.p[1];
                double tx ;         
                double ty ;         
                double t1x ;         
                double t1y ;         
                double t2x ;         
                double t2y ;         
                double r = e.fParam[0];

                double Px = gcd.Drawing.LastPoint[0];
                double Py = gcd.Drawing.LastPoint[1];

                double dx = Px - Cx;
                double dy = Py - Cy;
                double dxr = -dy;
                double dyr = dx;
                DistCenter = Sqr(dx ^ 2 + dy ^ 2); // y ya que estoy veo el Center

                if ( DistCenter >= r )
                {
                    Dim rho As double = r / DistCenter;
                    double ad = rho ^ 2;
                    double bd = rho * Sqr(1 - rho ^ 2);

                     // los puntos tangente
                    T1x = Cx + ad * dx + bd * dxr;
                    T1y = Cy + ad * dy + bd * dyr;
                    T2x = Cx + ad * dx - bd * dxr;
                    T2y = Cy + ad * dy - bd * dyr;

                    if ( (DistCenter / r - 1) < 1E-8 )
                    {
                         //P is on the circumference
                    } // determino cual de las dos tangentes es la mas cercana

                        if ( puntos.distancia(xr, yr, t1x, t1y) > puntos.distancia(xr, yr, t2x, t2y) )
                        {
                            Tx = T2x;
                            Ty = T2y;
                        }
                            Tx = T1x;
                            Ty = T1y;

                        }
                        DistTang = puntos.distancia(xr, yr, tx, ty);
                    }
                } // esta dentro del circulo

                    DistTang = 1e6;
                     // No tangent Is Possible
                }
            }

             // busco los quadrants
            double qx ;         
            double qy ;         
            double q1 ;         
            double q2 ;         
            double q3 ;         
            double q4 ;         
            q1 = puntos.distancia(e.P[0] - e.fParam[0], e.P[1], xr, yr);
            q2 = puntos.distancia(e.P[0] + e.fParam[0], e.P[1], xr, yr);
            q3 = puntos.distancia(e.P[0], e.P[1] - e.fParam[0], xr, yr);
            q4 = puntos.distancia(e.P[0], e.P[1] + e.fParam[0], xr, yr);

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

    }

     // otros puntos
    switch ( e.Gender)
    {
        case "LINE":

            DistEnd1 = puntos.distancia(e.P[0], e.P[1], xr, yr);
            end1X = e.P[0];
            end1Y = e.P[1];

            end2X = e.P[2];
            end2Y = e.P[3];
            DistEnd2 = puntos.distancia(e.P[2], e.P[3], xr, yr);

            DistMId = puntos.distancia((e.P[2] + e.P[0]) / 2, (e.P[3] + e.P[1]) / 2, xr, yr);
            i = 0;

            pNea = puntos.NearestToLine(xr, yr, e.P[0], e.P[1], e.P[2], e.P[3]);

            DistNearest = puntos.distancia(xr, yr, pNea[0], pNea[1]);

        case "Text", "MText":

            DistBase = puntos.distancia(e.P[0], e.P[1], xr, yr);

        case "CIRCLE":
            DistCenter = puntos.distancia(e.P[0], e.P[1], xr, yr);

            pNea = puntos.NearestToPolyLine(xr, yr, e.PolyLine);

            DistNearest = puntos.distancia(xr, yr, pNea[0], pNea[1]);

        case "ELLIPSE", "ARC", "SPLINE":

            DistCenter = puntos.distancia(e.P[0], e.P[1], xr, yr);

            if ( e.PolyLine.count > 0 )
            {
                DistEnd1 = puntos.distancia(e.PolyLine[0], e.PolyLine[1], xr, yr);
                end1X = e.PolyLine[0];
                end1Y = e.PolyLine[1];
                DistEnd2 = puntos.distancia(e.PolyLine[e.PolyLine.Max - 1], e.PolyLine[e.PolyLine.max], xr, yr);
                end2X = e.PolyLine[e.PolyLine.Max - 1];
                end2Y = e.PolyLine[e.PolyLine.Max];

                pNea = puntos.NearestToPolyLine(xr, yr, e.PolyLine);
            }
            DistNearest = puntos.distancia(xr, yr, pNea[0], pNea[1]);

        case "LWPOLYLINE", "SOLID":
             // tengo que ver cual es el mas cercano

            for ( i = 0; i <= e.P.count - 4; i + 2)
            {

                x0 = e.P[i + 0];
                y0 = e.P[i + 1];
                x1 = e.P[i + 2];
                y1 = e.P[i + 3];
                 // busco en que tramo estoy
                if ( puntos.doIntersect(xr - tolerance, Yr, Xr + tolerance, Yr, x0, y0, x1, y1) || puntos.doIntersect(xr, Yr - tolerance, Xr, Yr + tolerance, x0, y0, x1, y1) )
                {

                    DistEnd1 = puntos.distancia(x0, y0, xr, yr);
                     //If DistEnd1 > DistEnd1B Then DistEnd1 = DistEnd1B
                    end1X = e.P[i];
                    end1Y = y0;

                    DistEnd2 = puntos.distancia(x1, y1, xr, yr);
                     //If DistEnd2 > DistEnd2B Then DistEnd2 = DistEnd2B
                    end2X = x1;
                    end2Y = y1;

                    DistMId = puntos.distancia((x1 + x0) / 2, (y1 + y0) / 2, xr, yr);
                     //If DistMid > DistMidB Then DistMid = DistMidB

                     // perpendicular
                    if ( gcd.Drawing.LastPoint.max > 0 ) // tengo un punto anterior
                    {

                        if ( x1 - x0 != 0 )
                        {
                            pend1 = (y1 - y0) / (x1 - x0);
                            base1 = y1 - pend1 * x0; // Y = pend1 X + base1

                             // ecuacion de la recta perpendicular que pasa por el punto anterior
                            if ( pend1 != 0 )
                            {
                                pend2 = -1 / pend1;
                                base2 = gcd.Drawing.LastPoint[1] - pend2 * gcd.Drawing.LastPoint[0];

                                 // necesito otro punto
                                if ( gcd.Drawing.LastPoint[0] != 0 )
                                {
                                    puntoA.Add(0);
                                    puntoA.Add(base2);
                                }
                                    puntoA.Add(1);
                                    puntoA.Add(pend2 + base2);

                                }

                                 // determino la interseccion de ambas
                                puntoB = puntos.lineLineIntersection(gcd.Drawing.LastPoint, puntoA, [x0, y0], [x1, y1]);
                            } // la recta es horizontal

                                puntoB.Add(gcd.Drawing.LastPoint[0]);
                                puntoB.Add(y0);

                            }
                        } // la recta es vertical

                            puntoB.Add(x0);
                            puntoB.Add(gcd.Drawing.LastPoint[1]);

                        }

                         // veo si el punto b esta entre los extremos de la linea , aunque deberia estarlo!!!
                        if ( puntos.onSegment(x0, y0, puntoB[0], puntoB[1], x1, y1) )
                        {

                             // determino la distancia al puntero para ofrecerlo como opcion de enganche
                            DistPer = puntos.distancia(puntoB[0], puntoB[1], xr, yr);

                        }
                    }

                    Break;
                }
            }

            pNea = puntos.NearestToPolyLine(xr, yr, e.P);

            DistNearest = puntos.distancia(xr, yr, pNea[0], pNea[1]);

    }

     // veo cual es la opcion mas cercana al puntero

     // dist = Min(DistBase, DistCenter, DistEnd1, DistEnd2, DistMid, DistPer, DistQuad) // NO FUNCIONA

    if ( gcd.SnapMode && gcd.poiBasePoint )
    {
        if ( Dist > DistBase ) dist = DistBase;
    }
    if ( gcd.SnapMode && gcd.poiCenter )
    {
        if ( Dist > DistCenter ) dist = DistCenter;
    }
    if ( gcd.SnapMode && gcd.poiEndPoint )
    {
        if ( Dist > DistEnd1 ) dist = DistEnd1;
        if ( Dist > DistEnd2 ) dist = DistEnd2;
    }
    if ( gcd.SnapMode && gcd.poiMIdPoint )
    {
        if ( Dist > DistMId ) dist = DistMId;
    }
    if ( gcd.SnapMode && gcd.poiPerpendicular )
    {
        if ( Dist > DistPer ) dist = DistPer;
    }
    if ( gcd.SnapMode && gcd.poiQuadrant )
    {
        if ( Dist > DistQuad ) dist = DistQuad;
    }
    if ( gcd.SnapMode && gcd.poiTangent )
    {
        if ( Dist > DistTang ) dist = DistTang;
    }
    if ( gcd.SnapMode && gcd.poiIntersection )
    {
        if ( Dist > DistInter ) dist = DistInter;
    }

    if ( gcd.SnapMode && gcd.poiNearest )
    {
        if ( Dist > DistNearest ) dist = DistNearest;
    }

     // ofrezco ese punto
    if ( dist == DistEnd1 )
    {
        rData[0] = end1X;
        rData[1] = end1Y;
        rData[2] = gcd.poiEndPoint;
        DrawingAIds.txtSnapTo = "EndPoint";
    }

     // ofrezco ese punto
    if ( dist == DistEnd2 )
    {
        rData[0] = end2X;
        rData[1] = end2Y;
        rData[2] = gcd.poiEndPoint;
        DrawingAIds.txtSnapTo = "EndPoint";
    }

     // ofrezco ese punto
    if ( dist == DistMId )
    {
        rData[0] = (e.P[i + 2] + e.P[i + 0]) / 2;
        rData[1] = (e.P[i + 3] + e.P[i + 1]) / 2;
        rData[2] = gcd.poiMIdPoint;
        DrawingAIds.txtSnapTo = "MIdPoint";
    }

     // ofrezco ese punto
    if ( dist == DistInter )
    {
        rData[0] = pIntersection[iInter];
        rData[1] = pIntersection[iInter + 1];
        rData[2] = gcd.poiIntersection;
        DrawingAIds.txtSnapTo = "Intersection";
    }

     // ofrezco ese punto
    if ( dist == DistPer )
    {

        rData[0] = puntoB[0];
        rData[1] = puntoB[1];
        rData[2] = gcd.poiPerpendicular;
        DrawingAIds.txtSnapTo = "Perpendicular";
    }

    if ( dist == DistTang )
    {

        rData[0] = tx;
        rData[1] = ty;
        rData[2] = gcd.poiTangent;
        DrawingAIds.txtSnapTo = "Tangent";
    }

    if ( dist == DistCenter )
    {

        rData[0] = e.p[0];
        rData[1] = e.p[1];
        rData[2] = gcd.poiCenter;
        DrawingAIds.txtSnapTo = "Center";
    }

    if ( dist == DistQuad )
    {
        rData[0] = qx;
        rData[1] = qy;
        rData[2] = gcd.poiQuadrant;
        DrawingAIds.txtSnapTo = "Quadrant";
    }

    if ( dist == DistBase )
    {
        rData[0] = e.P[0];
        rData[1] = e.P[1];
        rData[2] = gcd.poiBasePoint;
        DrawingAIds.txtSnapTo = "BasePoint";
    }

    if ( dist == DistNearest )
    {
        rData[0] = pNea[0];
        rData[1] = pNea[1];
        rData[2] = gcd.poiNearest;
        DrawingAIds.txtSnapTo = "Nearest";
    }

     // Debug "pois checked"
    gcd.flgSearchingPOI = false;
    this.LastPointToCursorDistance = dist;
    gcd.Drawing.eLastEntity = e;
    return rData;

}

 // Devuelve el punto en la entidad que esta mas cercano al provisto
public static double[] GetNearestPoint(Entity e, double xr, double Yr)
    {


    double x2 ;         
    double y2 ;         
    double c ;         
    double d ;         
    double dx ;         
    double dy ;         
    double m1 ;         
    double m2 ;         
    double angle ;         
    double[] flxIntersec ;         

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
                    c = yr - m2 * xr;
                     // si
                    x2 = 1e10;
                    y2 = m2 * x2 + c;

                     // obtenemos el punto interseccion
                    flxIntersec = puntos.lineLineIntersection([xr, yr], [x2, y2], [e.P[0], e.P[1]], [e.P[2], e.P[3]]);

                     // y salimos

                    return flxIntersec;

                } // la perpendicular es verical
                    return [xr, e.P[1]];
                }
            } // la perpendicular es horizontal
                return [e.P[0], yr];

            }

    }

}

}
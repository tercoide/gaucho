//
// Converted and adapted from Gambas "puntos.class" to C#
// This variant uses double[] for public APIs and most internal arrays,
// while using List<double> transiently for operations that need mutability.
// - Gambas "Float" -> C# double
// - Public signatures accept/return double[]
// - Helper conversions between double[] and List<double> are provided
//
// NOTE: This is a direct port, not a complete idiomatic rewrite. Please run tests and adjust tolerances/edge-cases as needed.
//
using Gaucho;
    public struct Punto2d
    {
        public double x;
        public double y;
        
        public Punto2d()
        {
            x = 0.0;
            y = 0.0;
        }
        
        public Punto2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Punto3d
    {
        public double x;
        public double y;
        public double z;
        
        public Punto3d()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
        }
        
        public Punto3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public static class Puntos
    {
        public static int HookSize = 16;
        public static double NumericTolerance = 1e-7;
        public const double Pi2 = 6.28318530717959;

        #region List helpers

        private static List<double> CopyRange(List<double> src, int start, int count)
        {
            if (src == null) return new List<double>();
            if (start < 0) start = 0;
            if (start >= src.Count) return new List<double>();
            int available = Math.Min(count, src.Count - start);
            return src.GetRange(start, available);
        }

        private static void Swap(ref double a, ref double b)
        {
            double tmp = a; a = b; b = tmp;
        }

        private static double FMod(double x, double y)
        {
            if (y == 0) return x;
            return x - Math.Floor(x / y) * y;
        }

        #endregion

        #region Trigonometric helper

        public static double Ang(double x, double y)
        {
            return Math.Atan2(y, x);
        }

        #endregion

        #region Geometry functions (public signatures use double[])

        // Returns center, radius, start angle, Count of an arc defined by 3 given points
        // Returns empty list if cannot compute
        public static List<double> Arc3Point(double px1, double py1, double px2, double py2, double px3, double py3)
        {
            var mx1 = (px1 + px2) / 2;
            var my1 = (py1 + py2) / 2;
            var mx2 = (px3 + px2) / 2;
            var my2 = (py3 + py2) / 2;

            var ang1 = Ang(px2 - px1, py2 - py1);
            var ang2 = Ang(px3 - px2, py3 - py2);

            var ax1 = mx1 - 100 * Math.Sin(ang1);
            var ay1 = my1 + 100 * Math.Cos(ang1);
            var ax2 = mx2 - 100 * Math.Sin(ang2);
            var ay2 = my2 + 100 * Math.Cos(ang2);

            var answer = LineLineIntersection2(mx1, my1, ax1, ay1, mx2, my2, ax2, ay2);
            if (answer.Count == 0) return new List<double>();

            var list = answer;

            // radius
            list.Add(Distancia(px1, py1, list[0], list[1]));

            // start and end angles
            ang1 = Ang(px1 - list[0], py1 - list[1]);
            ang2 = Ang(px3 - list[0], py3 - list[1]);

            var angm = Ang(px2 - list[0], py2 - list[1]);

            if (ang1 < 0) ang1 += Pi2;
            if (ang2 < 0) ang2 += Pi2;
            if (angm < 0) angm += Pi2;

            if (ang1 > ang2) ang2 += Pi2;
            if (angm < ang1 && angm < ang2) angm += Pi2;

            if (!(angm >= ang1 && angm <= ang2))
            {
                Swap(ref ang2, ref ang1);
                if (ang2 < ang1) ang2 += Pi2;
            }

            list.Add(ang1);
            list.Add(ang2 - ang1);
            return list;
        }

        // Polygon triangulation (ear clipping-like). Input/Output as List<double> pairs x,y,x,y...
        public static List<double> PolygonTriangulation(List<double> fPolygon)
        {
            if (fPolygon == null) return new List<double>();
            var fPoly = new List<double>(fPolygon);
            var Triangles = new List<double>();

            if (fPoly.Count <= 6) return fPoly;

            // remove coincident consecutive points
            for (int i = 0; i <= fPoly.Count / 2 - 2; i++)
            {
                if (i >= fPoly.Count / 2 - 2) break;
                if (fPoly[i * 2] == fPoly[(i + 1) * 2] && fPoly[i * 2 + 1] == fPoly[(i + 1) * 2 + 1])
                {
                    fPoly.RemoveRange(i * 2, 2);
                    i--;
                }
            }

            // check first and last
            if (fPoly.Count >= 4)
            {
                if (fPoly[0] == fPoly[fPoly.Count - 2] && fPoly[1] == fPoly[fPoly.Count - 1])
                {
                    fPoly.RemoveRange(fPoly.Count - 2, 2);
                }
            }
            if (fPoly.Count <= 6) return fPoly;

            while (true)
            {
                int iAnterior = fPoly.Count;
                bool removedOne = false;
                int maxIter = Math.Max(1, fPoly.Count / 2 - 1);

                for (int i = 0; i < maxIter; i++)
                {
                    int PivotIzq = i;
                    int PivotDer = i + 1;
                    int TestPoint = ((i + 2) * 2 > fPoly.Count - 1) ? 0 : i + 2;

                    double MIdX = (fPoly[PivotIzq * 2] + fPoly[TestPoint * 2]) / 2;
                    double MIdY = (fPoly[PivotIzq * 2 + 1] + fPoly[TestPoint * 2 + 1]) / 2;

                    if (isInsIde(fPoly, MIdX, MIdY))
                    {
                        var intersec = LinePolyIntersection2(new List<double> { fPoly[PivotIzq * 2], fPoly[PivotIzq * 2 + 1], fPoly[PivotDer * 2], fPoly[PivotDer * 2 + 1] }, fPoly);
                        if (intersec.Count == 0)
                        {
                            Triangles.AddRange(new double[] {
                                fPoly[PivotIzq * 2], fPoly[PivotIzq * 2 + 1],
                                fPoly[PivotDer * 2], fPoly[PivotDer * 2 + 1],
                                fPoly[TestPoint * 2], fPoly[TestPoint * 2 + 1]
                            });
                            fPoly.RemoveRange((i + 1) * 2, 2);
                            removedOne = true;
                            break;
                        }
                    }

                    // test left
                    if (i - 1 < 0) TestPoint = fPoly.Count / 2 - 1; else TestPoint = i - 1;
                    MIdX = (fPoly[PivotDer * 2] + fPoly[TestPoint * 2]) / 2;
                    MIdY = (fPoly[PivotDer * 2 + 1] + fPoly[TestPoint * 2 + 1]) / 2;

                    if (isInsIde(fPoly, MIdX, MIdY))
                    {
                        var intersec = LinePolyIntersection2(new List<double> { fPoly[PivotDer * 2], fPoly[PivotDer * 2 + 1], fPoly[TestPoint * 2], fPoly[TestPoint * 2 + 1] }, fPoly);
                        if (intersec.Count == 0)
                        {
                            Triangles.AddRange(new List<double> {
                                fPoly[PivotIzq * 2], fPoly[PivotIzq * 2 + 1],
                                fPoly[PivotDer * 2], fPoly[PivotDer * 2 + 1],
                                fPoly[TestPoint * 2], fPoly[TestPoint * 2 + 1]
                            });
                            fPoly.RemoveRange(i * 2, 2);
                            removedOne = true;
                            break;
                        }
                    }
                }

                if (!removedOne || iAnterior == fPoly.Count) break;
            }

            if (fPoly.Count == 6) Triangles.AddRange(fPoly);
            return Triangles;
        }

        public static List<double> ArcTriangulation(List<double> center, List<double> fPolygon)
        {
            if (center == null || fPolygon == null) return new List<double>();
            var Triangles = new List<double>();
            for (int i = 0; i <= fPolygon.Count - 4; i += 2)
            {
                Triangles.Add(center[0]); Triangles.Add(center[1]);
                Triangles.Add(fPolygon[i]); Triangles.Add(fPolygon[i + 1]);
                Triangles.Add(fPolygon[i + 2]); Triangles.Add(fPolygon[i + 3]);
            }
            return Triangles;
        }

        // Returns [x,y,index] (index as double) or [xr,yr,-1]
        public static List<double> FindPOI(double xr, double yr, List<double> arr, double hook)
        {
            if (arr == null) return new List<double> { xr, yr, -1 };
            for (int i = 0; i <= arr.Count - 2; i += 2)
            {
                var ax = arr[i]; var ay = arr[i + 1];
                if (((ax - hook) < xr && (ax + hook) > xr) || ((ax - hook) > xr && (ax + hook) < xr))
                {
                    if (((ay - hook) < yr && (ay + hook) > yr) || ((ay - hook) > yr && (ay + hook) < yr))
                    {
                        return new List<double> { ax, ay, i / 2.0 };
                    }
                }
            }
            return new List<double> { xr, yr, -1 };
        }

        public static int FindPOILines(double xreal, double yreal, List<double> arr, double tolerance)
        {
            if (arr == null) return -1;
            for (int i = 0; i <= arr.Count - 4; i += 4)
            {
                if (doIntersect(xreal - tolerance / 2, yreal, xreal + tolerance / 2, yreal, arr[i + 0], arr[i + 1], arr[i + 2], arr[i + 3]) ||
                    doIntersect(xreal, yreal - tolerance / 2, xreal, yreal + tolerance / 2, arr[i + 0], arr[i + 1], arr[i + 2], arr[i + 3]))
                {
                    return i / 4;
                }
            }
            return -1;
        }

        public static int FindNearest(double xreal, double yreal, List<double> arr)
        {
            if (arr == null) return -1;
            int iCloser = -1;
            double dCloser = 1e100;
            for (int i = 0; i <= arr.Count - 2; i += 2)
            {
                double d = Distancia(xreal, yreal, arr[i], arr[i + 1]);
                if (d < dCloser) { iCloser = i; dCloser = d; }
            }
            return iCloser;
        }

        public static List<double> NearestToLine(double xp, double yp, double x1, double y1, double x2, double y2)
        {
            double d1 = Distancia(xp, yp, x1, y1);
            double d2 = Distancia(xp, yp, x2, y2);
            double xm = x1 + (x2 - x1) * d1 / (d1 + d2);
            double ym = y1 + (y2 - y1) * d1 / (d1 + d2);
            return new List<double> { xm, ym };
        }

        public static List<double> NearestToPolyLine(double xp, double yp, List<double> fPoly)
        {
            double d = 1e100;
            double xm = 0, ym = 0;
            if (fPoly == null || fPoly.Count < 4) return new List<double> { xm, ym };
            for (int i = 0; i <= fPoly.Count - 4; i += 2)
            {
                var fret = NearestToLine(xp, yp, fPoly[i], fPoly[i + 1], fPoly[i + 2], fPoly[i + 3]);
                double dm = Distancia(xp, yp, fret[0], fret[1]);
                if (d > dm) { d = dm; xm = fret[0]; ym = fret[1]; }
            }
            return new List<double> { xm, ym };
        }

        public static int FindPOIPoligon(double xr, double yr, List<double> arr, List<int> arrIndexes, List<int> arrNElements)
        {
            if (arrIndexes == null || arrNElements == null) return -1;
            for (int i = 0; i < arrIndexes.Count; i++)
            {
                if (isInsIde(arr, xr, yr, arrIndexes[i], arrNElements[i])) return i;
            }
            return -1;
        }

        public static double Distancia(double x1, double y1, double x2, double y2) => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        public static bool InBetween(double x, double x1, double x2) => (x1 <= x && x2 >= x) || (x1 >= x && x2 <= x);

        public static bool InBetweenNotEqual(double x, double x1, double x2) => (x1 < x && x2 > x) || (x1 > x && x2 < x);

        public static int inPolySegment(List<double> poligon, double px, double py, double tolerance = 0)
        {
            if (poligon == null) return -1;
            if (tolerance == 0) tolerance = NumericTolerance;
            for (int i = 0; i <= poligon.Count - 4; i += 2)
            {
                if (PointToLineDistance(new List<double> { px, py }, poligon.GetRange(i, 4)) <= tolerance) return (i + 2) / 2;
            }
            return -1;
        }

        public static bool Around(double p1x, double p1z, double p2x, double p2z, double prox)
        {
            return InBetween(p1x, p2x - prox, p2x + prox) && InBetween(p1z, p2z - prox, p2z + prox);
        }

        public static bool doIntersect(double p1x, double p1y, double q1x, double q1y, double p2x, double p2y, double q2x, double q2y)
        {
            int o1 = orientation(p1x, p1y, q1x, q1y, p2x, p2y);
            int o2 = orientation(p1x, p1y, q1x, q1y, q2x, q2y);
            int o3 = orientation(p2x, p2y, q2x, q2y, p1x, p1y);
            int o4 = orientation(p2x, p2y, q2x, q2y, q1x, q1y);

            if ((o1 != o2 && o3 != o4)) return true;
            if (o1 == 0 && onSegment(p1x, p1y, p2x, p2y, q1x, q1y)) return true;
            if (o2 == 0 && onSegment(p1x, p1y, q2x, q2y, q1x, q1y)) return true;
            if (o3 == 0 && onSegment(p2x, p2y, p1x, p1y, q2x, q2y)) return true;
            if (o4 == 0 && onSegment(p2x, p2y, q1x, q1y, q2x, q2y)) return true;
            return false;
        }

        public static int orientation(double px, double py, double qx, double qy, double rx, double ry)
        {
            double v = (qy - py) * (rx - qx) - (qx - px) * (ry - qy);
            if (v == 0) return 0;
            if (v > 0) return 2;
            return 1;
        }

        public static bool onSegment(double px, double py, double qx, double qy, double rx, double ry)
        {
            if (qx <= Math.Max(px, rx) && qx >= Math.Min(px, rx) &&
                qy <= Math.Max(py, ry) && qy >= Math.Min(py, ry)) return true;
            return false;
        }

        public static double Sign(List<double> p1, List<double> p2, List<double> p3)
        {
            return (p1[0] - p3[0]) * (p2[1] - p3[1]) - (p2[0] - p3[0]) * (p1[1] - p3[1]);
        }

        public static bool IsPointInTri(List<double> v1, List<double> v2, List<double> v3, List<double> pt)
        {
            bool b1 = Sign(pt, v1, v2) < 0.0;
            bool b2 = Sign(pt, v2, v3) < 0.0;
            bool b3 = Sign(pt, v3, v1) < 0.0;
            return (b1 == b2) && (b2 == b3);
        }

        // Point in polygon using ray casting improved algorithm
        public static bool isInsIde(List<double> fPolygon, double px, double py, int StartIndex = 0, int Vertices = 0)
        {
            if (fPolygon == null) return false;
            if (Vertices == 0) Vertices = fPolygon.Count / 2;
            if (Vertices < 3) return false;

            double extremeX = 1e11;
            double extremeY = py;
            int count = 0;

            for (int i = StartIndex; i <= Vertices * 2 - 1; i += 2)
            {
                int next = i + 2;
                if (i == fPolygon.Count - 2) next = 0;

                if (fPolygon[i] < px && fPolygon[next] < px) continue;
                if (fPolygon[i + 1] < py && fPolygon[next + 1] < py) continue;
                if (fPolygon[i + 1] >= py && fPolygon[next + 1] >= py) continue;

                if (doIntersect(fPolygon[i], fPolygon[i + 1], fPolygon[next], fPolygon[next + 1], px, py, extremeX, extremeY))
                {
                    count++;
                }
            }

            return (count % 2 == 1);
        }

        public static bool[] PointsInsidePoligon(List<double> fPoints, List<double> fPolygon)
        {
            if (fPoints == null) return new bool[0];
            var bInside = new bool[fPoints.Count / 2];
            int i2 = 0;
            for (int i = 0; i <= fPoints.Count - 2; i += 2)
            {
                bInside[i2] = isInsIde(fPolygon, fPoints[i], fPoints[i + 1]);
                i2++;
            }
            return bInside;
        }

        public static bool isInsIdePolygons(List<double> fTest, List<double> fAround)
        {
            if (fTest == null) return true;
            for (int i = 0; i <= fTest.Count - 2; i += 2)
            {
                if (!isInsIde(fAround, fTest[i], fTest[i + 1])) return false;
            }
            return true;
        }

        public static bool Equal(float v1, float v2) => v1 == v2;

        public static void RotatePointsFromBase(double CenterX, double CenterY, double fAngle, List<double> flxPoints)
        {
            if (flxPoints == null) return;
            for (int i = 0; i <= flxPoints.Count - 2; i += 2)
            {
                var n = RotateFromPointWithAngle(CenterX, CenterY, fAngle, flxPoints[i], flxPoints[i + 1]);
                flxPoints[i] = n[0];
                flxPoints[i + 1] = n[1];
            }
        }

        public static List<double> RotateFromPointWithAngle(double CenterX, double CenterZ, double fAngle, double Px, double Pz)
        {
            double s = Math.Sin(fAngle);
            double c = Math.Cos(fAngle);
            Px -= CenterX;
            Pz -= CenterZ;
            double xnew = Px * c - Pz * s;
            double ynew = Px * s + Pz * c;
            Px = xnew + CenterX;
            Pz = ynew + CenterZ;
            return new List<double> { Px, Pz };
        }

        public static List<double> RotateWithSinCos(double Px, double Pz, double S, double C)
        {
            double xnew = Px * C - Pz * S;
            double ynew = Px * S + Pz * C;
            return new List<double> { xnew, ynew };
        }

        public static List<double> RotateFromPointWithSinCos(double CenterX, double CenterZ, double S, double C, double Px, double Pz)
        {
            Px -= CenterX;
            Pz -= CenterZ;
            double xnew = Px * C - Pz * S;
            double ynew = Px * S + Pz * C;
            Px = xnew + CenterX;
            Pz = ynew + CenterZ;
            return new List<double> { Px, Pz };
        }

        public static List<double> lineLineIntersection(List<double> A, List<double> B, List<double> C, List<double> D)
        {
            double a1 = B[1] - A[1];
            double b1 = A[0] - B[0];
            double c1 = a1 * (A[0]) + b1 * (A[1]);

            double a2 = D[1] - C[1];
            double b2 = C[0] - D[0];
            double c2 = a2 * (C[0]) + b2 * (C[1]);

            double determinant = a1 * b2 - a2 * b1;
            if (determinant == 0) return new List<double>();
            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;
            return new List<double> { x, y };
        }

        public static List<double> LineLineIntersection2(double ax, double ay, double bx, double by, double cx, double cy, double dx, double dy)
        {
            double a1 = by - ay;
            double b1 = ax - bx;
            double c1 = a1 * (ax) + b1 * (ay);

            double a2 = dy - cy;
            double b2 = cx - dx;
            double c2 = a2 * (cx) + b2 * (cy);

            if (ax == cx && ay == cy) return new List<double> { ax, ay };
            if (ax == dx && ay == dy) return new List<double> { ax, ay };
            if (bx == cx && by == cy) return new List<double> { bx, by };
            if (bx == dx && by == dy) return new List<double> { bx, by };

            double determinant = a1 * b2 - a2 * b1;
            if (determinant == 0) return new List<double>();
            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;
            return new List<double> { x, y };
        }

        public static float[] LineLineIntersection2_Single(float ax, float ay, float bx, float by, float cx, float cy, float dx, float dy)
        {
            float a1 = by - ay;
            float b1 = ax - bx;
            float c1 = a1 * (ax) + b1 * (ay);
            float a2 = dy - cy;
            float b2 = cx - dx;
            float c2 = a2 * (cx) + b2 * (cy);

            if (ax == cx && ay == cy) return new float[] { ax, ay };
            if (ax == dx && ay == dy) return new float[] { ax, ay };
            if (bx == cx && by == cy) return new float[] { bx, by };
            if (bx == dx && by == dy) return new float[] { bx, by };

            float determinant = a1 * b2 - a2 * b1;
            if (determinant == 0) return new float[0];
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;
            return new float[] { x, y };
        }

        // Line vs closed poly intersections - returns list of intersection points (x,y,x,y,...)
        public static List<double> LinePolyIntersection2(List<double> flxLine, List<double> flxPoly)
        {
            var flxIntersections = new List<double>();
            if (flxLine == null || flxPoly == null) return new List<double>();
            if (flxLine.Count != 4) return new List<double>();

            for (int i = 0; i <= flxPoly.Count - 4; i += 2)
            {
                if ((flxLine[0] == flxPoly[i] && flxLine[1] == flxPoly[i + 1]) ||
                    (flxLine[0] == flxPoly[i + 2] && flxLine[1] == flxPoly[i + 3]) ||
                    (flxLine[2] == flxPoly[i] && flxLine[3] == flxPoly[i + 1]) ||
                    (flxLine[2] == flxPoly[i + 2] && flxLine[3] == flxPoly[i + 3])) continue;

                if (doIntersect(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[i], flxPoly[i + 1], flxPoly[i + 2], flxPoly[i + 3]))
                {
                    var t = LineLineIntersection2(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[i], flxPoly[i + 1], flxPoly[i + 2], flxPoly[i + 3]);
                    if (t.Count > 0) flxIntersections.AddRange(t);
                    return flxIntersections; // original returns after first found
                }
            }

            if ((flxLine[0] == flxPoly[flxPoly.Count - 2] && flxLine[1] == flxPoly[flxPoly.Count - 1]) ||
                (flxLine[0] == flxPoly[0] && flxLine[1] == flxPoly[1]) ||
                (flxLine[2] == flxPoly[flxPoly.Count - 2] && flxLine[3] == flxPoly[flxPoly.Count - 1]) ||
                (flxLine[2] == flxPoly[0] && flxLine[3] == flxPoly[1])) return new List<double>();

            if (doIntersect(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[flxPoly.Count - 2], flxPoly[flxPoly.Count - 1], flxPoly[0], flxPoly[1]))
            {
                var t = LineLineIntersection2(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[flxPoly.Count - 2], flxPoly[flxPoly.Count - 1], flxPoly[0], flxPoly[1]);
                if (t.Count > 0) flxIntersections.AddRange(t);
            }

            return flxIntersections;
        }

        public static List<double> LinePolyIntersection3(List<double> flxLine, List<double> flxPoly)
        {
            var flxIntersections = new List<double>();
            if (flxLine == null || flxPoly == null) return new List<double>();
            if (flxLine.Count != 4) return new List<double>();

            for (int i = 0; i <= flxPoly.Count - 4; i += 2)
            {
                var flxTest = LineLineIntersection2(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[i], flxPoly[i + 1], flxPoly[i + 2], flxPoly[i + 3]);
                if (flxTest.Count > 0)
                {
                    if (onSegment(flxPoly[i], flxPoly[i + 1], flxTest[0], flxTest[1], flxPoly[i + 2], flxPoly[i + 3]))
                        flxIntersections.AddRange(flxTest);
                }
            }
            return flxIntersections;
        }

        public static void SortLineSegments(ref List<double> flxPoly)
        {
            if (flxPoly == null) { flxPoly = new List<double>(); return; }
            int iInicial = 2;
            bool Swaped;
            do
            {
                Swaped = false;
                for (int i = iInicial; i <= flxPoly.Count - 4; i += 2)
                {
                    double d1 = Distancia(flxPoly[iInicial - 2], flxPoly[iInicial - 1], flxPoly[iInicial], flxPoly[iInicial + 1]);
                    double d2 = Distancia(flxPoly[iInicial - 2], flxPoly[iInicial - 1], flxPoly[i + 2], flxPoly[i + 3]);
                    if (d1 > d2)
                {
                    var ddd = flxPoly[iInicial];
                    flxPoly[iInicial] = flxPoly[i + 2];
                    flxPoly[i + 2] = ddd;
                    ddd = flxPoly[iInicial + 1];
                    flxPoly[i + 3] = flxPoly[iInicial + 1];
                    flxPoly[iInicial + 1] = ddd;

                        Swaped = true;
                        i -= 2;
                        if (i < iInicial) i = iInicial;
                    }
                }
                iInicial += 2;
            } while (Swaped);
        }

        public static List<double> SortLineSegments2(List<double> flxPoly)
        {
            if (flxPoly == null) return new List<double>();
            var f = new List<double>();
            var flxLim = Limits(flxPoly);
            double dx = flxLim[2] - flxLim[0];
            double dy = flxLim[3] - flxLim[1];

            if (dx > dy)
            {
                var flxRet = new List<double>();
                var flxRet0 = new List<double>();
                var flxRet2 = new List<double>();
                for (int i = 0; i <= flxPoly.Count - 2; i += 2)
                {
                    flxRet.Add(flxPoly[i]);
                    flxRet0.Add(flxPoly[i]);
                    flxRet2.Add(flxPoly[i + 1]);
                }
                var flxSorted = flxRet.OrderBy(v => v).ToList();
                for (int i = 0; i < flxSorted.Count; i++)
                {
                    for (int i2 = 0; i2 < flxRet0.Count; i2++)
                    {
                        if (flxRet0[i2] == flxSorted[i])
                        {
                            f.Add(flxRet[i2]);
                            f.Add(flxRet2[i2]);
                            flxRet0.RemoveAt(i2);
                            flxRet2.RemoveAt(i2);
                            break;
                        }
                    }
                }
            }
            else
            {
                var flxRet = new List<double>();
                var flxRet0 = new List<double>();
                var flxRet2 = new List<double>();
                for (int i = 0; i <= flxPoly.Count - 2; i += 2)
                {
                    flxRet.Add(flxPoly[i + 1]);
                    flxRet0.Add(flxPoly[i + 1]);
                    flxRet2.Add(flxPoly[i]);
                }
                var flxSorted = flxRet.OrderBy(v => v).ToList();
                for (int i = 0; i < flxSorted.Count; i++)
                {
                    for (int i2 = 0; i2 < flxRet0.Count; i2++)
                    {
                        if (flxRet0[i2] == flxSorted[i])
                        {
                            f.Add(flxRet2[i2]);
                            flxRet0.RemoveAt(i2);
                            flxRet2.RemoveAt(i2);
                            f.Add(flxRet[i]);
                            break;
                        }
                    }
                }
            }
            return f;
        }

        public static List<double> InvertPolyline(List<double> flxPoly)
        {
            if (flxPoly == null) return new List<double>();
            var flxRet = new List<double>();
            for (int i = flxPoly.Count - 2; i >= 0; i -= 2)
            {
                flxRet.Add(flxPoly[i]);
                flxRet.Add(flxPoly[i + 1]);
            }
            return flxRet;
        }

        public static List<double> LineHoleIntersection(List<double> flxLine, List<double> flxPoly)
        {
            var tmpIntersection = new List<double>();
            bool FirstAdded = false, LastAdded = false;
            var tmpIntersection2 = LinePolyIntersection(flxLine, flxPoly);
            if (tmpIntersection2.Count == 0) return new List<double>(flxLine);

            if (Distancia(tmpIntersection2[0], tmpIntersection2[1], flxLine[0], flxLine[1]) > Distancia(tmpIntersection2[tmpIntersection2.Count - 2], tmpIntersection2[tmpIntersection2.Count - 1], flxLine[0], flxLine[1]))
                tmpIntersection = InvertPolyline(tmpIntersection2);
            else
                tmpIntersection = new List<double>(tmpIntersection2);

            if (!((tmpIntersection[0] == flxLine[0]) && (tmpIntersection[1] == flxLine[1])))
            {
                tmpIntersection.InsertRange(0, flxLine.GetRange(0, 2));
                FirstAdded = true;
            }

            if (!((tmpIntersection[tmpIntersection.Count - 2] == flxLine[2]) && (tmpIntersection[tmpIntersection.Count - 1] == flxLine[3])))
            {
                tmpIntersection.AddRange(flxLine.GetRange(2, 2));
                LastAdded = true;
            }

            if (tmpIntersection.Count == 4) return new List<double>();
            if (FirstAdded) return tmpIntersection.GetRange(0, (tmpIntersection.Count / 4) * 4);
            if (LastAdded) return tmpIntersection.GetRange(2, tmpIntersection.Count - 2);
            return new List<double>();
        }

        public static List<double> LinePolyIntersection(List<double> flxLine, List<double> flxPoly)
        {
            if (flxLine == null || flxPoly == null) return new List<double>();
            if (flxLine.Count != 4) return new List<double>();
            if (flxPoly.Count < 6) return flxLine.GetRange(0, 4);

            bool FirstInsIde = isInsIde(flxPoly, flxLine[0], flxLine[1]);
            bool LastInsIde = isInsIde(flxPoly, flxLine[2], flxLine[3]);

            var flxIntersections = new List<double>();
            var flxRta = new List<double>();

            for (int i = 0; i <= flxPoly.Count - 4; i += 2)
            {
                if (doIntersect(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[i], flxPoly[i + 1], flxPoly[i + 2], flxPoly[i + 3]))
                {
                    var flxInter = LineLineIntersection2(flxLine[0], flxLine[1], flxLine[2], flxLine[3], flxPoly[i], flxPoly[i + 1], flxPoly[i + 2], flxPoly[i + 3]);
                    if (flxInter.Count > 0) flxIntersections.AddRange(flxInter);
                }
            }

            flxIntersections = SortLineSegments2(flxIntersections);

            if (flxIntersections.Count > 0)
            {
                if (Distancia(flxLine[0], flxLine[1], flxIntersections[0], flxIntersections[1]) > Distancia(flxLine[0], flxLine[1], flxIntersections[flxIntersections.Count - 2], flxIntersections[flxIntersections.Count - 1]))
                    flxIntersections = InvertPolyline(flxIntersections);

                if (FirstInsIde)
                {
                    if (!EqualPoints(flxLine[0], flxLine[1], flxIntersections[0], flxIntersections[1]))
                    {
                        if (!EqualPoints(flxLine[0], flxLine[1], flxIntersections[flxIntersections.Count - 2], flxIntersections[flxIntersections.Count - 1]))
                            flxRta.InsertRange(0, flxLine.GetRange(0, 2));
                    }
                }

                flxRta.AddRange(flxIntersections);

                if (LastInsIde)
                {
                    if (!EqualPoints(flxLine[2], flxLine[3], flxIntersections[flxIntersections.Count - 2], flxIntersections[flxIntersections.Count - 1]))
                    {
                        if (!EqualPoints(flxLine[2], flxLine[3], flxIntersections[0], flxIntersections[1]))
                            flxRta.AddRange(flxLine.GetRange(2, 2));
                    }
                }
            }
            else
            {
                if (FirstInsIde) flxRta.InsertRange(0, flxLine.GetRange(0, 2));
                if (LastInsIde) flxRta.AddRange(flxLine.GetRange(2, 2));
            }

            // fix duplicates
            if (flxRta.Count % 4 != 0)
            {
                for (int i = 0; i <= flxRta.Count - 4; i += 2)
                {
                    if (i > flxRta.Count - 3) break;
                    if (EqualPoints(flxRta[i], flxRta[i + 1], flxRta[i + 2], flxRta[i + 3]))
                    {
                        flxRta.RemoveRange(i, 2);
                    }
                }
            }
            if (flxRta.Count == 2) return new List<double>();
            return flxRta;
        }

        public static bool EqualPoints(double x1, double y1, double x2, double y2)
        {
            return ((float)x1 == (float)x2) && ((float)y1 == (float)y2);
        }

        public static List<double> PolyPolyIntersection(List<double> flxPolyOpen, List<double> flxPolyClosed)
        {
            var flxRta = new List<double>();
            if (flxPolyOpen == null) return new List<double>();
            for (int i = 0; i <= flxPolyOpen.Count - 4; i += 2)
            {
                var seg = flxPolyOpen.GetRange(i, 4);
                var inter = LinePolyIntersection(seg, flxPolyClosed);
                flxRta.AddRange(inter);
            }
            return flxRta;
        }

        public static bool PointOverLine(List<double> fPoint, List<double> fLine, double fTolerance = 1E-3)
        {
            if (fPoint == null || fLine == null) return false;
            double x = fPoint[0], y = fPoint[1];

            if (fLine[0] <= fLine[2])
            {
                if (x + fTolerance < fLine[0]) return false;
                if (x - fTolerance > fLine[2]) return false;
            }
            else
            {
                if (x + fTolerance < fLine[2]) return false;
                if (x - fTolerance > fLine[0]) return false;
            }

            if (fLine[1] <= fLine[3])
            {
                if (y + fTolerance < fLine[1]) return false;
                if (y - fTolerance > fLine[3]) return false;
            }
            else
            {
                if (y + fTolerance < fLine[3]) return false;
                if (y - fTolerance > fLine[1]) return false;
            }

            return Math.Abs(PointToLineDistance(fPoint, fLine)) < fTolerance;
        }

        public static bool PointOverLine_Int(int x, int y, int x0, int y0, int x1, int y1, int iTolerance = 8)
        {
            if (x0 <= x1)
            {
                if (x + iTolerance < x0) return false;
                if (x - iTolerance > x1) return false;
            }
            else
            {
                if (x + iTolerance < x1) return false;
                if (x - iTolerance > x0) return false;
            }

            if (y0 <= y1)
            {
                if (y + iTolerance < y0) return false;
                if (y - iTolerance > y1) return false;
            }
            else
            {
                if (y + iTolerance < y1) return false;
                if (y - iTolerance > y0) return false;
            }

            return Math.Abs(PointToLineDistance_Single(x, y, x0, y0, x1, y1)) < iTolerance;
        }

        public static bool PointOverPolyLine(List<double> fPoint, List<double> fLine, double fTolerance = 1E-3)
        {
            if (fLine == null) return false;
            for (int i = 0; i <= fLine.Count - 4; i += 2)
            {
                if (PointOverLine(fPoint, new List<double> { fLine[i], fLine[i + 1], fLine[i + 2], fLine[i + 3] }, fTolerance)) return true;
            }
            return false;
        }

        public static double PointToLineDistance(List<double> fPoint, List<double> fLine)
        {
            if (fPoint == null || fLine == null) return 0;
            if (fLine[0] == fLine[2]) return Math.Abs(fLine[0] - fPoint[0]);
            else if (fLine[1] == fLine[3]) return Math.Abs(fLine[1] - fPoint[1]);

            var e = lineLineIntersection(new List<double> { fPoint[0], fPoint[1] }, new List<double> { 1e11, fPoint[1] }, new List<double> { fLine[0], fLine[1] }, new List<double> { fLine[2], fLine[3] });
            double CE = fPoint[0] - e[0];
            double angE = Math.Atan2(fLine[3] - fLine[1], fLine[2] - fLine[0]);
            return CE * Math.Sin(angE);
        }

        public static float PointToLineDistance_Single(float x, float y, float x0, float y0, float x1, float y1)
        {
            if (x0 == x1) return Math.Abs(x0 - x);
            else if (y0 == y1) return Math.Abs(y0 - y);

            var e = LineLineIntersection2_Single(x, y, 1e11f, y, x0, y0, x1, y1);
            float CE = x - e[0];
            float angE = (float)Math.Atan2(y1 - y0, x1 - x0);
            return CE * (float)Math.Sin(angE);
        }

        public static double Angle(List<double> vector1, List<double> vector2)
        {
            double alfa = Ang(vector1[0], vector1[1]);
            double beta = Ang(vector2[0], vector2[1]);
            double gamma = Math.Abs(beta - alfa);
            if (gamma > Math.PI) gamma -= Math.PI;
            return gamma;
        }

        public static double Angle2(List<double> vector1, List<double> vector2)
        {
            double alfa = Ang(vector1[0], vector1[1]);
            double beta = Ang(vector2[0], vector2[1]);
            return beta - alfa;
        }

        public static List<double> ReboundVector(List<double> vector, List<double> wall)
        {
            double alfa = Ang(vector[0], vector[1]);
            double beta = Ang(wall[0], wall[1]);
            double gamma = beta * 2 - alfa;
            double l = Distancia(0, 0, vector[0], vector[1]);
            return new List<double> { Math.Cos(gamma) * l, Math.Sin(gamma) * l };
        }

        public static void Normalize(List<double> v)
        {
            if (v == null) return;
            double l = 0;
            for (int i = 0; i < v.Count; i++) l += v[i] * v[i];
            l = Math.Sqrt(l);
            if (l == 0) return;
            for (int i = 0; i < v.Count; i++) v[i] /= l;
        }

        public static double Dot(List<double> v1, List<double> v2) => v1[0] * v2[0] + v1[1] * v2[1];

        public static void Translate(List<double> points, double dx, double dy)
        {
            if (points == null) return;
            for (int i = 0; i <= points.Count - 2; i += 2)
            {
                points[i] += dx;
                points[i + 1] += dy;
            }
        }

        public static void TranslateConditional(List<double> points, double dx, double dy, List<bool> bCondition)
        {
            if (points == null || bCondition == null) return;
            int i2 = 0;
            for (int i = 0; i <= points.Count - 2; i += 2)
            {
                if ((i2 < bCondition.Count) && (bCondition[i2]))
                {
                    points[i] += dx;
                    points[i + 1] += dy;
                }
                i2++;
            }
        }

        public static void Rotate(List<double> points, double Radians)
        {
            if (points == null) return;
            double s = Math.Sin(Radians), c = Math.Cos(Radians);
            for (int i = 0; i <= points.Count - 2; i += 2)
            {
                double xnew = points[i] * c - points[i + 1] * s;
                double ynew = points[i] * s + points[i + 1] * c;
                points[i] = xnew; points[i + 1] = ynew;
            }
        }

        public static void Scale(List<double> points, double Sx, double Sy)
        {
            if (points == null) return;
            for (int i = 0; i <= points.Count - 2; i += 2)
            {
                points[i] *= Sx; points[i + 1] *= Sy;
            }
        }

        public static bool IsPoligonInsIdeRect(List<double> poligon, double x0, double y0, double x1, double y1, int StartIndex = 0, int TotalElements = 0)
        {
            if (poligon == null) return false;
            TotalElements--;
            if (TotalElements < 0) TotalElements = poligon.Count - 1;
            for (int i = StartIndex; i <= TotalElements; i += 2)
            {
                if (poligon[i] < x0) return false;
                if (poligon[i] > x1) return false;
                if (poligon[i + 1] < y0) return false;
                if (poligon[i + 1] > y1) return false;
            }
            return true;
        }

        public static bool IsPoligonSelfIntersecting(List<double> poligon)
        {
            if (poligon == null) return false;
            if (poligon.Count < 8) return false;
            for (int i = 0; i <= poligon.Count - 2; i += 2)
            {
                double TestX0, TestY0, TestX1, TestY1;
                if (i == poligon.Count - 2)
                {
                    TestX0 = poligon[poligon.Count - 2];
                    TestY0 = poligon[poligon.Count - 1];
                    TestX1 = poligon[0];
                    TestY1 = poligon[1];
                }
                else
                {
                    TestX0 = poligon[i];
                    TestY0 = poligon[i + 1];
                    TestX1 = poligon[i + 2];
                    TestY1 = poligon[i + 3];
                }

                for (int ii = 0; ii <= poligon.Count - 4; ii += 2)
                {
                    if (i == ii) continue;
                    var CrossPoint = LineLineIntersection2(TestX0, TestY0, TestX1, TestY1, poligon[ii], poligon[ii + 1], poligon[ii + 2], poligon[ii + 3]);
                    if (CrossPoint.Count > 0)
                    {
                        if (InBetweenNotEqual(CrossPoint[0], poligon[ii], poligon[ii + 2]) && InBetweenNotEqual(CrossPoint[1], poligon[ii + 1], poligon[ii + 3]) &&
                            InBetweenNotEqual(CrossPoint[0], TestX0, TestX1) && InBetweenNotEqual(CrossPoint[1], TestY0, TestY1))
                            return true;
                    }
                }

                var CrossPointEdge = LineLineIntersection2(TestX0, TestY0, TestX1, TestY1, poligon[poligon.Count - 2], poligon[poligon.Count - 1], poligon[0], poligon[1]);
                if (CrossPointEdge.Count > 0)
                {
                    if (InBetweenNotEqual(CrossPointEdge[0], poligon[poligon.Count - 2], poligon[0]) && InBetweenNotEqual(CrossPointEdge[1], poligon[poligon.Count - 1], poligon[1]) &&
                        InBetweenNotEqual(CrossPointEdge[0], TestX0, TestX1) && InBetweenNotEqual(CrossPointEdge[1], TestY0, TestY1))
                        return true;
                }
            }
            return false;
        }

        public static bool IsPoligonCrossingLine(List<double> poligon, double x0, double y0, double x1, double y1, int StartIndex = 0, int TotalElements = 0)
        {
            TotalElements--;
            if (TotalElements < 0) TotalElements = poligon.Count - 1;
            for (int i = StartIndex; i <= TotalElements - 3; i += 2)
            {
                if (doIntersect(poligon[i], poligon[i + 1], poligon[i + 2], poligon[i + 3], x0, y0, x1, y1)) return true;
            }
            if (doIntersect(poligon[StartIndex], poligon[StartIndex + 1], poligon[TotalElements - 1], poligon[TotalElements], x0, y0, x1, y1)) return true;
            return false;
        }

        public static bool IsPoligonCrossingRect(List<double> poligon, double x0, double y0, double x1, double y1, int StartIndex = 0, int TotalElements = 0)
        {
            if (poligon == null) return false;
            if (poligon.Count == 0) return false;
            if (IsPoligonCrossingLine(poligon, x0, y0, x1, y0, StartIndex, TotalElements)) return true;
            if (IsPoligonCrossingLine(poligon, x0, y1, x1, y1, StartIndex, TotalElements)) return true;
            if (IsPoligonCrossingLine(poligon, x0, y0, x0, y1, StartIndex, TotalElements)) return true;
            if (IsPoligonCrossingLine(poligon, x1, y0, x1, y1, StartIndex, TotalElements)) return true;
            return false;
        }

        public static double PoligonCount(List<double> points)
        {
            if (points == null) return 0;
            double l = 0;
            for (int i = 0; i <= points.Count - 4; i += 2)
                l += Distancia(points[i], points[i + 1], points[i + 2], points[i + 3]);
            return l;
        }

        public static double PoligonArea(List<double> points)
        {
            if (points == null) return 0;
            double area = 0;
            for (int i = 0; i <= points.Count - 4; i += 2)
            {
                area += points[i] * points[i + 3] - points[i + 1] * points[i + 2];
            }
            area += points[points.Count - 2] * points[1] - points[points.Count - 1] * points[0];
            return Math.Abs(area) / 2.0;
        }

        public static List<double> Limits(List<double> points)
        {
            if (points == null || points.Count == 0) return new List<double> { 0, 0, 0, 0 };
            double Xmenor = 1e100, Ymenor = 1e100, Xmayor = -1e100, Ymayor = -1e100;
            for (int i = 0; i <= points.Count - 2; i += 2)
            {
                if (Xmenor > points[i]) Xmenor = points[i];
                if (Ymenor > points[i + 1]) Ymenor = points[i + 1];
                if (Xmayor < points[i]) Xmayor = points[i];
                if (Ymayor < points[i + 1]) Ymayor = points[i + 1];
            }
            return new List<double> { Xmenor, Ymenor, Xmayor, Ymayor };
        }

        public static void LimitsMax(List<double> BasePoints, List<double> NewPoints)
        {
            if (BasePoints == null || NewPoints == null) return;
            if (NewPoints[0] < BasePoints[0]) BasePoints[0] = NewPoints[0];
            if (NewPoints[1] < BasePoints[1]) BasePoints[1] = NewPoints[1];
            if (NewPoints[2] > BasePoints[2]) BasePoints[2] = NewPoints[2];
            if (NewPoints[3] > BasePoints[3]) BasePoints[3] = NewPoints[3];
        }

        // DashedLineStrip (ported). Returns list of segment pairs [x0,y0,x1,y1,...]
        public static List<double> DashedLineStrip(List<double> flxStrip, List<double> flxDash, double fScale = 1, double fOffset = 0, bool bInvertDashes = false, bool bIntercalateLines = false, double fWIdth = 1)
        {
            if (flxStrip == null || flxDash == null) return new List<double>();
            var flxDashes = new List<double>();
            if (bInvertDashes)
            {
                for (int i = flxDash.Count - 1; i >= 0; i--) flxDashes.Add(flxDash[i]);
            }
            else flxDashes.AddRange(flxDash);

            double fDashTotal = 0;
            for (int i = 0; i < flxDashes.Count; i++)
            {
                flxDashes[i] *= fScale;
                fDashTotal += Math.Abs(flxDashes[i]);
            }

            if (fDashTotal == 0) return new List<double>(flxStrip);
            if (flxStrip.Count < 4) return new List<double>(flxStrip);

            fOffset = FMod(fOffset, fDashTotal);

            var flxFirstPass = new List<double>();
            int iDashTrame = 0;
            bool bSkip = false;

            for (int seg = 0; seg <= flxStrip.Count - 4; seg += 2)
            {
                double X0 = flxStrip[seg], Y0 = flxStrip[seg + 1];
                double X1 = flxStrip[seg + 2], Y1 = flxStrip[seg + 3];
                double fTrame = Distancia(X0, Y0, X1, Y1);

                if (bIntercalateLines)
                {
                    if (bSkip)
                    {
                        bSkip = false;
                        fOffset += fTrame;
                        fOffset = FMod(fOffset, fDashTotal);
                        continue;
                    }
                    else bSkip = true;
                }

                double fAngle = Ang(X1 - X0, Y1 - Y0);
                double CosB = Math.Cos(fAngle), SinB = Math.Sin(fAngle);
                double DashBaseX = X0, DashBaseY = Y0;

                while (true)
                {
                    double DashTrame = flxDashes[iDashTrame];
                    bool bSpace = DashTrame < 0;
                    DashTrame = Math.Abs(DashTrame);

                    double DashCut = DashTrame - fOffset;
                    if (DashCut < 0)
                    {
                        fOffset = -DashCut;
                    }
                    else
                    {
                        if (DashCut >= fTrame)
                        {
                            fOffset += fTrame;
                            DashCut = fTrame;
                            fTrame = 0;
                        }
                        else
                        {
                            fTrame -= DashCut;
                            fOffset = 0;
                        }

                        double DashFinalX = DashBaseX + DashCut * CosB;
                        double DashFinalY = DashBaseY + DashCut * SinB;

                        if (!bSpace)
                        {
                            flxFirstPass.Add(DashBaseX);
                            flxFirstPass.Add(DashBaseY);
                            flxFirstPass.Add(DashFinalX);
                            flxFirstPass.Add(DashFinalY);
                        }
                        DashBaseX = DashFinalX;
                        DashBaseY = DashFinalY;
                    }

                    if (fTrame == 0) break;
                    iDashTrame++;
                    if (iDashTrame > flxDashes.Count - 1) iDashTrame = 0;
                }
            }

            return flxFirstPass;
        }

        public static Punto2d ExtendLine2D(Punto2d p0, Punto2d p1, double l)
        {
            var p = new Punto2d();
            double l0 = Distancia(p0.x, p0.y, p1.x, p1.y);
            p.x = p0.x; p.y = p0.y;
            if (p0.x != p1.x) p.x = p0.x + (l0 + l) / l0 * (p1.x - p0.x);
            if (p0.y != p1.y) p.y = p0.y + (l0 + l) / l0 * (p1.y - p0.y);
            return p;
        }

        #endregion
    }

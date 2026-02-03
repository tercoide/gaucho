
// Port of Gambas ToolsBase.class -> C#
//
// Notes:
// - This is a direct behavioral translation, not a full idiomatic rewrite.
// - External globals and types referenced here (Gcd, gl, glx, Mouse, clsEntities, Gcd.Tools["SELECTION"], clsMouseTracking, Paint, Color, DrawingAIds, etc.)
//   must exist in your project. Replace or adapt references to match your codebase.
// - Gambas "Float" -> double. Collections -> List<T> or project-specific collection types.
// - Some methods are left as thin wrappers/stubs (NewParameter, Run, KeyPress/KeyDown/KeyUp) because their behavior depends on other classes.
using OpenTK.Graphics.OpenGL;
using Gaucho;

public class ToolsBase
    {
        public  string Gender { get; } = "TOOLSBASE";

        public const bool IsTool = true;
        public  string USEWITH { get; } = "";

        // Selection pixel coordinates (initialize to 0)
        public  int SelStartX { get; set; } = 0;
        public  int SelStartY { get; set; } = 0;
        public  int SelEndX { get; set; } = 0;
        public  int SelEndY { get; set; } = 0;

        // Selection pan start (Pixels)
        public  int SelStartPanX { get; set; } = 0;
        public  int SelStartPanY { get; set; } = 0;

        // Selection in real coordinates (meters)
        public  double SelStartXr { get; set; } = 0.0;
        public  double SelStartYr { get; set; } = 0.0;
        public  double SelEndXr { get; set; } = 0.0;
        public  double SelEndYr { get; set; } = 0.0;

        // Start/End in real coordinates for other uses
        public  double StartXr { get; set; } = 0.0;
        public  double StartYr { get; set; } = 0.0;
        public  double EndXr { get; set; } = 0.0;
        public  double EndYr { get; set; } = 0.0;

        // Mouse tracking
        public  double LastX { get; set; } = 0.0;
        public  double LastY { get; set; } = 0.0;

        public  int MouseX { get; set; } = 0;
        public  int MouseY { get; set; } = 0;
        public  int MouseButton { get; set; } = 0;
        public  bool MouseFakeClick { get; set; } = false;

        // Parameters for builders/tools (initialize to sensible defaults)
        public  int PointsDone { get; set; } = 0;
        public  int PointsTotal { get; set; } = 0;
        public  string NextParamType { get; set; } = string.Empty;      // "P","F","C","S","M" ...
        public  object NextParamDefault { get; set; } = null;
        public  string NextParam { get; set; } = string.Empty;         // description
        public  string Prompt { get; set; } = string.Empty;

        public  bool Active { get; set; } = false;
        public  bool PoiChecking { get; set; } = false;
        public  bool EntityChecking { get; set; } = false;
        public  int Mode { get; set; } = 0;
        public  Entity? EntityForEdit { get; set; } = null;
        public  Entity? OriginalEntityForEdit { get; set; } = null;
        public static string MenuRightClick { get; set; } = string.Empty;

        // selected indices (was Integer[] in Gambas)
        public  List<int> inxSelected { get; set; } = new List<int>();

        // transforms used for GL drawing (initialized to identity / defaults)
        public  double[] glTranslate { get; set; } = new double[] { 0.0, 0.0, 0.0 }; // dX,dY,dZ
        public  double[] glRotate { get; set; } = new double[] { 0.0, 0.0, 1.0 };    // rX,rY,rZ
        public  double glAngle { get; set; } = 0.0;                                  // deg
        public  double[] glScale { get; set; } = new double[] { 1.0, 1.0, 1.0 };     // sX,sY,sZ

        public  int cursorX { get; set; } = 0;
        public  int cursorY { get; set; } = 0;
        public  bool AllowSingleSelection { get; set; }
        public  bool AllowRectSelection { get; set; }
        public  bool AllowPolySelection { get; set; }
        public  bool AllowGripEdit { get; set; }
        public  bool AllowTextInput { get; set; }
        public  int lastCursorX { get; set; } = 0;
        public  int lastCursorY { get; set; } = 0;
        public  string ContextMenu { get; } = "Finish;_FINISH;;;Cancel;_CANCEL;;";

        // Static ctor kept minimal (arrays already initialized above)
        public ToolsBase()
        {
            // All fields have explicit initial values at declaration.
            // This static constructor is retained for future initialization needs.
            AllowGripEdit = false;
        }

        // Start the tool (optionally with an element to build)
        public bool Start(string ElemToBuild = "", int _mode = 0)
        {
            PointsDone = 0;
            Mode = _mode;
            // TODO: hook context menu into UI if required.
            return true;
        }

        // Called by drawing loop to render tool overlays / selected entities
        public void Draw()
        {
            // Translate by sheet pan, then apply tool transforms and call the selected entities GL list.
            // Assumes 'gl' API exists with PushMatrix/PopMatrix/Translatef/Rotatef/Scalef/CallList methods.
            try
            {
                GL.Translate(Gcd.Drawing.Sheet.PanBaseRealX, Gcd.Drawing.Sheet.PanBaseRealY, 0.0f);
                GL.PushMatrix();
                GL.Translate((float)glTranslate[0], (float)glTranslate[1], (float)glTranslate[2]);
                GL.Rotate((float)glAngle, (float)glRotate[0], (float)glRotate[1], (float)glRotate[2]);
                GL.Scale((float)glScale[0], (float)glScale[1], (float)glScale[2]);
                GL.CallList(Gcd.Drawing.GlListEntitiesSelected);
                GL.PopMatrix();
                GL.Translate(-(float)Gcd.Drawing.Sheet.PanBaseRealX, -(float)Gcd.Drawing.Sheet.PanBaseRealY, 0.0f);
            }
            catch (Exception)
            {
                // swallow to avoid crash when GL not ready; consider logging
            }
        }

        // keyboard hooks (override in specific tools)
        public void KeyPress(int iCode, string sKey) { }
        public  void KeyDown(int iCode) { }
        public  void KeyUp(int iCode) { }

        // Called when user submits text (Enter)
        public void KeyText(string EnteredText)
        {
            if (string.IsNullOrWhiteSpace(EnteredText)) return;

            EnteredText = EnteredText.Trim();
            var upper = EnteredText.ToUpperInvariant();

            // handle tool-global commands
            switch (upper)
            {
                case "_CANCEL":
                    Cancel();
                    return;
                case "_FINISH":
                    Finish();
                    return;
                case "_ADD":
                    Gcd.clsJobPrevious = Gcd.clsJob;
                    Gcd.clsJob = Gcd.Tools["SELECTION"];
                    Gcd.Tools["SELECTION"].Mode = cadSelection.SelectModeAdd;
                    return;
                case "_REM":
                case "_REMOVE":
                    Gcd.clsJobPrevious = Gcd.clsJob;
                    Gcd.clsJob = Gcd.Tools["SELECTION"];
                    Gcd.Tools["SELECTION"].Mode = cadSelection.SelectModeRem;
                    return;
                // Snap commands
                case "_MIDPOINT":
                case "_MID":
                    Gcd.SnapMode = Gcd.poiMidPoint;
                    return;
                case "_ENDPOINT":
                case "_END":
                    Gcd.SnapMode = Gcd.poiEndPoint;
                    return;
                case "_PERPENDICULAR":
                case "_PER":
                    Gcd.SnapMode = Gcd.poiPerpendicular;
                    return;
                case "_NEAREST":
                case "_NEA":
                    Gcd.SnapMode = Gcd.poiNearest;
                    return;
                case "_CENTER":
                case "_CEN":
                    Gcd.SnapMode = Gcd.poiCenter;
                    return;
                case "_INTERSECTION":
                case "_INT":
                    Gcd.SnapMode = Gcd.poiIntersection;
                    return;
                case "_BASEPOINT":
                case "_BAS":
                    Gcd.SnapMode = Gcd.poiBasePoint;
                    return;
                case "_TANGENT":
                case "_TAN":
                    Gcd.SnapMode = Gcd.poiTangent;
                    return;
                case "_QUADRANT":
                case "_QUA":
                    Gcd.SnapMode = Gcd.poiQuadrant;
                    return;
            }

            // If the input looks like a point, send as point parameter; otherwise text parameter.
            if (Gcd.IsPoint(EnteredText))
            {
                string errtxt = ", expected a valid point like 12.4,9.5 or @12.34,10.5";
                EnteredText = EnteredText.Trim();
                bool relative = false;

                if (EnteredText.Contains("@"))
                {
                    relative = true;
                    EnteredText = EnteredText.Replace("@", "");
                }

                var parts = EnteredText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 &&
                    double.TryParse(parts[0].Trim(), out double xt) &&
                    double.TryParse(parts[1].Trim(), out double yt))
                {
                    if (relative)
                    {
                        xt += LastX;
                        yt += LastY;
                    }

                    LastX = xt;
                    LastY = yt;

                    // store last point in drawing
                    Gcd.Drawing.LastPoint[0] = (int)xt;
                    Gcd.Drawing.LastPoint[1] = (int)yt;

                    NewParameter("point", Gcd.Drawing.LastPoint, true);
                }
                else
                {
                    // invalid point - ignore or notify user
                }
            }
            else
            {
                NewParameter("text", EnteredText, true);
            }
        }

        // Called by the tool to accept a new parameter (to be implemented by specific tools / entity handlers)
        public  void NewParameter(string tipo, object vValor, bool Definitive = false)
        {
            // default implementation does nothing.
            // Specific tools should override / provide callbacks to receive parameters.
        }

        // Double click handling: choose viewport under mouse etc.
        public  void DblClick()
        {
            SelStartX = Mouse.X;
            SelStartY = Mouse.Y;
            SelStartXr = Gcd.Xreal(SelStartX);
            SelStartYr = Gcd.Yreal(SelStartY);

            // If a viewport is active and click outside it, deactivate
            if (Gcd.Drawing.Sheet.Viewport != null)
            {
                var v = Gcd.Drawing.Sheet.Viewport;
                if (SelStartXr < v.X0 || SelStartXr > v.X1 || SelStartYr < v.Y0 || SelStartYr > v.Y1)
                {
                    Gcd.Drawing.Sheet.Viewport = null;
                }
            }

            // Find any viewport that contains the clicked point and activate it
            if (Gcd.Drawing.Sheet.Viewports != null)
            {
                foreach (var v1 in Gcd.Drawing.Sheet.Viewports)

                {
                    var v = v1.Value;
                    if (SelStartXr >= v.X0 && SelStartXr <= v.X1 && SelStartYr >= v.Y0 && SelStartYr <= v.Y1)
                    {
                        Gcd.Drawing.Sheet.Viewport = v;
                        break;
                    }
                }
            }
        }

        // Mouse up: handle point selection and parameter submission
        public void MouseUp()
        {
            // Right click -> cancel via NewParameter
            if (Mouse.Right)
            {
                NewParameter("action", "_CANCEL", true);
                return;
            }

            double x = Gcd.Xreal(Mouse.X);
            double y = Gcd.Yreal(Mouse.Y);

            // POI checking (snaps): pick best POI if available
            if (!Gcd.flgSearchingPOI)
            {
                // TODO: Implement mouse tracking functionality
                // var iEntity = clsMouseTracking.CheckBestPOI(x, y);
                // if (iEntity != null && iEntity.Length >= 3 && iEntity[2] > 0)
                // {
                //     x = iEntity[0];
                //     y = iEntity[1];
                // }
            }

            x = Gcd.Near(x);
            y = Gcd.Near(y);

            // orthogonal mode: snap axis depending on which delta is larger
            if (PointsDone > 0 && Gcd.Orthogonal)
            {
                if (Math.Abs(x - LastX) > Math.Abs(y - LastY))
                {
                    y = LastY;
                }
                else
                {
                    x = LastX;
                }
            }

            LastX = x;
            LastY = y;

            NewParameter("point", new double[] { LastX, LastY }, true);

            Gcd.Redraw();
        }

        // Mouse move: update preview parameter with current point
        public  void MouseMove()
        {
            double x = Gcd.Xreal(Mouse.X);
            double y = Gcd.Yreal(Mouse.Y);

            if (!Gcd.flgSearchingPOI)
            {
                var iEntity = clsMouseTracking.CheckBestPOI(x, y);
                if (iEntity != null && iEntity.Count >= 3 && iEntity[2] > 0)
                {
                    x = iEntity[0];
                    y = iEntity[1];
                }
            }

            x = Gcd.Near(x);
            y = Gcd.Near(y);

            if (PointsDone > 0 && Gcd.Orthogonal)
            {
                if (Math.Abs(x - LastX) > Math.Abs(y - LastY))
                {
                    y = LastY;
                }
                else
                {
                    x = LastX;
                }
            }

            NewParameter("point", new double[] { x, y }, false);
        }

        // Mouse down: mostly handled at higher level; handle right clicks to change jobs etc.
        public  void MouseDown()
        {
            if (Mouse.Right)
            {
                if (Gcd.clsJob != null && Gcd.clsJob.Gender == "BUILDER")
                {
                    Gcd.clsJob.KeyText("U");
                }
                else if (Gcd.clsJob != null && Gcd.clsJob.Gender == "SELECT")
                {
                    Gcd.clsJob = Gcd.clsJobPrevious;
                    Gcd.clsJob.Start();
                }
            }
            // Left and middle handled elsewhere in application
            Gcd.Redraw();
        }

        // Mouse wheel for dynamic zoom, keeping mouse-point stationary in world coordinates
        public  void MouseWheel()
        {
            bool outside;
            if (Gcd.Drawing.Sheet.Viewport != null)
            {
                var v = Gcd.Drawing.Sheet.Viewport;
                double xr = Gcd.Xreal(Mouse.X);
                double yr = Gcd.Yreal(Mouse.Y);
                outside = xr < v.X0 || xr > v.X1 || yr < v.Y0 || yr > v.Y1;
            }
            else
            {
                outside = true;
            }

            double px = Gcd.Xreal(Mouse.X);
            double py = Gcd.Yreal(Mouse.Y);
            double factor = (1 + 0.075 * Mouse.Delta);

            if (outside)
            {
                Gcd.Drawing.Sheet.ScaleZoom *= factor;
                double dx = Gcd.Xreal(Mouse.X);
                double dy = Gcd.Yreal(Mouse.Y);

                Gcd.Drawing.Sheet.PanX += Gcd.Pixels(dx - px);
                Gcd.Drawing.Sheet.PanY += Gcd.Pixels(dy - py);

                Gcd.flgNewPosition = true;
            }
            else
            {
                // inside viewport: apply zoom to viewport coordinates
                px -= Gcd.Drawing.Sheet.Viewport.X0;
                py -= Gcd.Drawing.Sheet.Viewport.Y0;

                Gcd.Drawing.Sheet.Viewport.ScaleZoom *= factor;

                double dx = Gcd.Xreal(Mouse.X) - Gcd.Drawing.Sheet.Viewport.X0;
                double dy = Gcd.Yreal(Mouse.Y) - Gcd.Drawing.Sheet.Viewport.Y0;

                Gcd.Drawing.Sheet.Viewport.PanX += Gcd.Pixels(dx - px);
                Gcd.Drawing.Sheet.Viewport.PanY += Gcd.Pixels(dy - py);
            }

            Gcd.Redraw();
        }

        // Finish the tool: cleanup transforms, regenerate lists, reset job to selection
        public  void Finish()
        {
            try
            {
                // gl.DeleteLists(Gcd.Drawing.GlListEntitiesSelected, 1);

                glAngle = 0;
                glTranslate[0] = glTranslate[1] = glTranslate[2] = 0.0;
                glScale[0] = glScale[1] = glScale[2] = 1.0;

                clsEntities.GlGenDrawListSelected();
                clsEntities.GlGenDrawListLayers();
                clsEntities.DeSelection();
                clsEntities.CollectVisibleEntities();

                Gcd.Drawing.RequiresSaving = true;

                Gcd.Drawing.iEntity.Clear();
                Gcd.Drawing.iEntity.AddRange(new double[] { 0, 0, -1, -1 });

                Gcd.clsJobPrevious = null;
                Gcd.clsJobCallBack = null;
                Gcd.clsJob = Gcd.Tools["SELECTION"];
                Gcd.clsJob.AllowSingleSelection = true;
                Gcd.clsJob.Start();

                DrawingAids.CleanTexts();
                Gcd.StepsDone = 0;
                Gcd.DrawOriginals = false;
                Active = false;
                lastCursorX = 0;
                lastCursorY = 0;

                Gcd.Redraw();
            }
            catch (Exception)
            {
                // ignore errors during finish
            }
        }

        // Cancel the tool: revert to selection job
        public  void Cancel()
        {
            Gcd.clsJobPrevious = null;
            Gcd.clsJobCallBack = null;
            clsEntities.DeSelection();
            Gcd.clsJob = Gcd.Tools["SELECTION"];
            Gcd.clsJob.Start();
            DrawingAids.CleanTexts();
            Gcd.Redraw();
        }

        // Callback placeholder
        public  void Run()
        {
            // Implement tool-specific run-loop if needed
        }
    }

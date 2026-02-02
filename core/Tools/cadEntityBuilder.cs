namespace Gaucho;
   // Port of Gambas cadEntityBuilder.class -> C#
    //
    // Notes:
    // - This is a behavioral translation that relies on other converted classes and global objects:
    //   Gcd, clsEntities, DrawingAids, fMain, cadSelection, clsMouseTracking, Puntos, etc.
    // - Gambas Float -> double, Collections -> List<T> / arrays depending on context.
    // - Entity.P is expected to be double[] (x,y pairs). Adjust if your Entity model differs.
    // - Methods such as Gcd.CCC[gender].NewParameter / Finish / BuildGeometry are called as in the original.
    // - Error handling: Gambas Try blocks are translated to try/catch where sensible.
    public class cadEntityBuilder : ToolsBase
    {
        // identity
        public new const string Gender = "BUILDER";

        // Pixel coordinates
        public  int LastMouseDownX = 0;
        public  int LastMouseDownY = 0;
      
        public  bool LastPoint = false;

        public  int LastMouseX = -1;
        public  int LastMouseY = -1;

        // Indicators and parameters
        public  int StepsTotal = 0;
        public  int PointsTotal = 0;
        public  string NextParamType = string.Empty;
        public  string[] ParamHelperList = new string[0];
        public  string[] ParamDefault = new string[0];
        public  string prompt = string.Empty;

        // indices of parameter arrays
        public  int iPoints = 0;
        public  int iFloat = 0;
        public  int iString = 0;

        public  int iCounter = 0;

        // current element being built
        public  Entity elem = null;
        public  bool PoiChecking = true;
        public  bool EntityChecking = false;
        public  Entity LastEntity = null;

        public  double[] XYreal = new double[0];

        public const string ContextMenu = "Cancel;_CANCEL;;";

        private int StepsDone = 0;

        // Start building a new entity or editing an existing type
        public new bool Start(string ElemToBuild = "", int Mode = -1)
        {
            // Reset and prepare builder
            LastPoint = false;
            clsEntities.DeSelection();

            // regenerate selection draw list
            clsEntities.GlGenDrawListSel(false);

            // Determine element to build
            if (ElemToBuild == "")
            {
                if (LastEntity != null)
                {
                    // create a new entity of the same gender as last
                    elem = Gcd.CCC[LastEntity.Gender].NewEntity();
                }
                else
                {
                    DrawingAids.ErrorMessage = "Can't create entity";
                    Gcd.clsJob = Gcd.Tools["SELECTION"];
                    Gcd.clsJobPrevious = Gcd.Tools["SELECTION"];
                    return false;
                }
            }
            else
            {
                if (Gcd.CCC.ContainsKey(ElemToBuild))
                {
                    elem = Gcd.CCC[ElemToBuild].NewEntity();
                    } else 
                    {
                        DrawingAids.ErrorMessage = "Can't create entity";
                        Gcd.clsJob = Gcd.Tools["SELECTION"];
                        Gcd.clsJobPrevious = Gcd.Tools["SELECTION"];
                        return false;
                    
                }
            }

            // reset state
            StepsDone = 0;

            try
            {
                elem.Colour = Gcd.Drawing.CurrColor;
            }
            catch { }

            try
            {
                elem.LineWidth = Gcd.Drawing.CurrLineWt;
                if (elem.LineWidth == 0) elem.LineWidth = 1;
            }
            catch { }

            try
            {
                elem.PaperSpace = !Gcd.Drawing.Sheet.IsModel;
            }
            catch { }

            iPoints = 0;
            iFloat = 0;
            iString = 0;

            // PointsTotal: number of x,y pairs in elem.P
            try
            {
                PointsTotal = (elem.P != null) ? elem.P.Count / 2 : 0;
            }
            catch
            {
                PointsTotal = 0;
            }

            // StepsTotal is Count of ParamType string for this gender
            try
            {
                var paramType = Gcd.CCC[elem.Gender].ParamType ?? string.Empty;
                StepsTotal = paramType.Length;
            }
            catch
            {
                StepsTotal = 0;
            }

            // helper lists and defaults (semicolon separated)
            try
            {
                var helper = Gcd.CCC[elem.Gender].ParamHelper ?? string.Empty;
                ParamHelperList = string.IsNullOrEmpty(helper) ? new string[0] : helper.Split(';');
            }
            catch
            {
                ParamHelperList = new string[0];
            }

            try
            {
                var pdef = Gcd.CCC[elem.Gender].ParamDefault ?? string.Empty;
                ParamDefault = string.IsNullOrEmpty(pdef) ? new string[0] : pdef.Split(';');
            }
            catch
            {
                ParamDefault = new string[0];
            }

            // First expected parameter type
            try
            {
                var paramType = Gcd.CCC[elem.Gender].ParamType ?? string.Empty;
                NextParamType = paramType.Length > Gcd.StepsDone ? paramType[Gcd.StepsDone].ToString().ToUpperInvariant() : "+";
            }
            catch
            {
                NextParamType = "+";
            }

            // if (Mode >= 0)
            // {
            //     try
            //     {
            //         elem.iParam[Gcd.CCC[elem.Gender].iiiMode] = Mode;
            //     }
            //     catch { }
            // }

            if (Gcd.SnapMode != 0)
            {
                try { Gcd.SnapMode = Config.SnapModeSaved; } catch { }
            }

            // // set popup menu for sheet
            // try
            // {
            //     Gcd.Drawing.Sheet.GlSheet.PopupMenu = elem.Gender;
            // }
            // catch { }

            // fMain.KeysAccumulator = string.Empty;
            try { prompt = Gcd.CCC[elem.Gender].Prompt; } catch { prompt = string.Empty; }

            Gcd.DrawHoveredEntity = false;

            return true;
        }

        // Entry from command-line/text input
        public  void KeyText(string EnteredText)
        {
            if (Gb.Trim(EnteredText) == "") return;

            double Xt = 0, yt = 0;
            string sText = null;
            string ErrTxt = string.Empty;
            bool Relative = false;
            bool bResult = false;

            var upper = EnteredText.ToUpperInvariant();

            // handle quick commands / snap modes
            switch (upper)
            {
                case "_CANCEL":
                    Cancel();
                    return;
                case "_MidPOINT":
                case "_Mid":
                    Gcd.SnapMode = Gcd.poiMidPoint;
                    break;
                case "_ENDPOINT":
                case "_END":
                    Gcd.SnapMode = Gcd.poiEndPoint;
                    break;
                case "_PERPENDICULAR":
                case "_PER":
                    Gcd.SnapMode = Gcd.poiPerpendicular;
                    break;
                case "_NEAREST":
                case "_NEA":
                    Gcd.SnapMode = Gcd.poiNearest;
                    break;
                case "_CENTER":
                case "_CEN":
                    Gcd.SnapMode = Gcd.poiCenter;
                    break;
                case "_INTERSECTION":
                case "_INT":
                    Gcd.SnapMode = Gcd.poiIntersection;
                    break;
                case "_BASEPOINT":
                case "_BAS":
                    Gcd.SnapMode = Gcd.poiBasePoint;
                    break;
                case "_TANGENT":
                case "_TAN":
                    Gcd.SnapMode = Gcd.poiTangent;
                    break;
                case "_QUADRANT":
                case "_QUA":
                    Gcd.SnapMode = Gcd.poiQuadrant;
                    break;
            }

            // Determine behavior depending on expected parameter
            if (NextParamType == "P" || NextParamType == "+" || NextParamType == "M")
            {
                ErrTxt = ", expected a valid point like 12.4,9.5 or @12.34,10.5";

                var trimmed = EnteredText.Trim();
                if (trimmed.Length == 0) return;
                var first = trimmed.Substring(0, 1);

                if ("@0123456789.".IndexOf(first) >= 0)
                {
                    if (trimmed.Contains("@"))
                    {
                        Relative = true;
                        trimmed = trimmed.Replace("@", "");
                    }

                    var parts = trimmed.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 &&
                        double.TryParse(parts[0].Trim(), out Xt) &&
                        double.TryParse(parts[1].Trim(), out yt))
                    {
                        if (Relative && elem?.P != null && elem.P.Count > 2)
                        {
                            Xt += LastX;
                            yt += LastY;
                        }

                        LastX = Xt;
                        LastY = yt;

                        try
                        {
                            if (Gcd.CCC[elem.Gender].NewParameter(elem,new List<string> { "point", Xt.ToString(), yt.ToString() }, true))
                            {
                                // save last point to drawing
                                Gcd.Drawing.LastPoint.Clear();
                                Gcd.Drawing.LastPoint.AddRange(new double[] { Xt, yt });
                                AdvanceStep();
                            }
                        }
                        catch
                        {
                            // ignore errors from entity-specific NewParameter
                        }
                    }
                    else
                    {
                        DrawingAids.ErrorMessage = "Bad input" + ErrTxt;
                    }
                }
                else
                {
                    // text input for this parameter
                    try
                    {
                        if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "text",  EnteredText }, true))
                            AdvanceStep();
                    }
                    catch { }
                }
            }
            else if (NextParamType == "T")
            {
                ErrTxt = ", expected text, not a point";
                if (Gb.Trim(EnteredText) == "")
                    sText = ParamDefault.Length > Gcd.StepsDone ? ParamDefault[Gcd.StepsDone] : string.Empty;
                else sText = EnteredText;

                try
                {
                    if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "text", sText }, true))
                        AdvanceStep();
                }
                catch { }
            }
            else if ("FARL".IndexOf(NextParamType) >= 0) // "F", "A", "L", "R"
            {
                ErrTxt = "enter a valid numeric value";
                if (Gb.Trim(EnteredText) == "" || EnteredText == "U")
                {
                    try
                    {
                        var def = ParamDefault.Length > Gcd.StepsDone ? ParamDefault[Gcd.StepsDone] : "0";
                        bResult = Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", def.ToString() }, true);
                    }
                    catch { bResult = false; }
                }
                else
                {
                    try
                    {
                        bResult = Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", Convert.ToDouble(EnteredText).ToString() }, true);
                    }
                    catch { bResult = false; }
                }

                if (bResult) AdvanceStep();
            }
            else if (NextParamType == "C")
            {
                // if (Dialog.SelectColor())
                // {
                //     try
                //     {
                //         elem.fParam[iFloat] = (double)Dialog.Color;
                //         iFloat++;
                //         AdvanceStep();
                //     }
                //     catch { }
                // }
            }
        }

        public  void KeyUp(int iCode)
        {
            if (iCode == Key.ControlKey) Gcd.Orthogonal = false;
        }

        public  void AdvanceStep()
        {
            Gcd.StepsDone++;
            if (Gcd.StepsDone == StepsTotal)
            {
                Finish();
            }
            else
            {
                // prepare next parameter
                try
                {
                    var paramType = Gcd.CCC[elem.Gender].ParamType ?? string.Empty;
                    NextParamType = paramType.Length > Gcd.StepsDone ? paramType[Gcd.StepsDone].ToString().ToUpperInvariant() : "+";
                }
                catch { NextParamType = "+"; }

                if (NextParamType == "+")
                {
                    StepsTotal++;
                    NextParamType = "P";
                }

                if (ParamDefault.Length > Gcd.StepsDone)
                {
                    ParamDefault[Gcd.StepsDone] = ParamDefault[Gcd.StepsDone].Trim();
                }

                // trigger preview
                MouseMove();
            }

            // fMain.KeysAccumulator = string.Empty;
            try { prompt = Gcd.CCC[elem.Gender].Prompt; } catch { }
            Gcd.Redraw();
        }

        // Mouse movement: used for preview and snap logic
        public void MouseMove()
        {
            double X = 0, Y = 0, f = 0;
            int MouseTry = -1000000;

            try { MouseTry = Mouse.X; } catch { MouseTry = -1000000; }

            if (MouseTry >= 0)
            {
                LastMouseX = Mouse.X;
                LastMouseY = Mouse.Y;
            }

            X = Gcd.Xreal(LastMouseX);
            Y = Gcd.Yreal(LastMouseY);

            // POI checking
            if (!Gcd.flgSearchingPOI)
            {
                Gcd.Drawing.iEntity = clsMouseTracking.CheckBestPOI(X, Y);
            }

            X = Gcd.Near(X);
            Y = Gcd.Near(Y);

            // if we are not expecting a parameter that takes mouse input, return
            if ("LAPRM".IndexOf(NextParamType) < 0) return;

            // special case for SPLINE
            if (elem != null && elem.Gender == "SPLINE" && (iPoints != ((elem.P != null) ? elem.P.Count / 2 - 1 : 0)))
            {
                iPoints++;
            }

            if (NextParamType == "A")
            {
                f = Puntos.Ang(Gcd.Xreal(LastMouseX) - LastX, Gcd.Yreal(LastMouseY) - LastY);
                f *= 180.0 / Math.PI;
                Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", f.ToString() });
                Gcd.Redraw();
                return;
            }

            if (NextParamType == "R")
            {
                Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "point", X.ToString(), Y.ToString() });
                Gcd.Redraw();
                return;
            }

            if (NextParamType == "L")
            {
                f = Puntos.Distancia(Gcd.Xreal(LastMouseX), Gcd.Yreal(LastMouseY), LastX, LastY);
                Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", f.ToString() });
                Gcd.Redraw();
                return;
            }

            // If snap engaged, adjust to POI coordinates
            if (Gcd.Drawing.iEntity != null && Gcd.Drawing.iEntity.Count >= 3 && Gcd.Drawing.iEntity[2] > 0)
            {
                X = Gcd.Drawing.iEntity[0];
                Y = Gcd.Drawing.iEntity[1];
            }
            else
            {
                if (Gcd.Orthogonal && LastPoint)
                {
                    if (Math.Abs(X - LastX) > Math.Abs(Y - LastY))
                        Y = LastY;
                    else
                        X = LastX;
                }
            }

            Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "point", X.ToString(), Y.ToString() });
            Gcd.Redraw();
        }

        public  void KeyPress(int iCode, string sKey)
        {
            // left intentionally blank; entity-specific behavior may override
        }

        // MouseUp: finalize point input or numeric parameter
        public  void MouseUp()
        {
            if (Mouse.Right)
            {
                Gcd.clsJob.KeyText("U");
                return;
            }

            DrawingAids.ErrorMessage = string.Empty;
            if ("LAPRM".IndexOf(NextParamType) < 0) return;

            double X = 0, Y = 0, f = 0;

            if (NextParamType == "A")
            {
                f = Puntos.Ang(Gcd.Xreal(Mouse.X) - LastX, Gcd.Yreal(Mouse.Y) - LastY) * 180.0 / Math.PI;
                if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", f.ToString() }, true)) AdvanceStep();
                Gcd.Redraw();
                return;
            }

            if (NextParamType == "R")
            {
                if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "point", Gcd.Xreal(Mouse.X).ToString(), Gcd.Yreal(Mouse.Y).ToString() }, true))
                {
                    Gcd.Drawing.LastPoint.Clear();
                    Gcd.Drawing.LastPoint.AddRange(new double[] { Gcd.Xreal(Mouse.X), Gcd.Yreal(Mouse.Y) });
                    AdvanceStep();
                }
                Gcd.Redraw();
                return;
            }

            if (NextParamType == "L")
            {
                f = Puntos.Distancia(Gcd.Xreal(Mouse.X), Gcd.Yreal(Mouse.Y), LastX, LastY);
                if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "float", f.ToString() }, true)) AdvanceStep();
                Gcd.Redraw();
                return;
            }

            // expecting a point
            X = Gcd.Near(Gcd.Xreal(Mouse.X));
            Y = Gcd.Near(Gcd.Yreal(Mouse.Y));

            if (Gcd.Drawing.iEntity != null && Gcd.Drawing.iEntity.Count >= 3 && Gcd.Drawing.iEntity[2] > 0)
            {
                X = Gcd.Drawing.iEntity[0];
                Y = Gcd.Drawing.iEntity[1];
            }
            else
            {
                if (Gcd.Orthogonal && LastPoint)
                {
                    if (Math.Abs(X - LastX) > Math.Abs(Y - LastY))
                        Y = LastY;
                    else
                        X = LastX;
                }
            }

            LastX = X;
            LastY = Y;
            LastPoint = true;

            if (Gcd.CCC[elem.Gender].NewParameter(elem, new List<string> { "point", X.ToString(), Y.ToString() }, true))
            {
                Gcd.Drawing.LastPoint.Clear();
                Gcd.Drawing.LastPoint.AddRange(new double[] { X, Y });
                AdvanceStep();
            }

            Gcd.Redraw();
        }

        public  void MouseDown()
        {
            // No default behavior here - left for higher-level handling
        }

        // public  void MouseWheel()
        // {
        //     ToolsBase.MouseWheel();
        // }

        public  void DblClick()
        {
            // not used in builder by default
        }

        public  void Draw()
        {
            try
            {
                // ensure geometry built for preview
                clsEntities.BuildGeometry(elem);
                clsEntities.Draw(elem);
                DrawingAids.DrawSnapText();
                iCounter++;
            }
            catch { }
        }

        // Called when finishing creating the entity
        public  bool Finish()
        {
            if (elem == null) return false;

        // assign id if missing
        if (elem.id == "") elem.id = Gcd.NewId();

            try
            {
                if (Gcd.Drawing.Sheet.IsModel)
                {
                    Gcd.Drawing.Sheet.Entities.Add(elem.id,elem);
                    Gcd.Drawing.Sheet.EntitiesVisibles.Add(elem.id,elem);
                }
                else
                {
                    Gcd.Drawing.Sheet.Entities.Add(elem.id,elem);
                }
            }
            catch { /* adapt to your collection API */ }

            elem.Container = Gcd.Drawing.Sheet.Block;

            try
            {
                if (elem.pBlock != null && elem.pBlock.Name == "*D")
                {
                    elem.pBlock.Name += elem.id;
                    elem.pBlock.id = Gcd.NewId();
                    Gcd.Drawing.Blocks.Add(elem.pBlock.id,elem.pBlock);
                }
            }
            catch { }

            Gcd.CCC[elem.Gender].Finish(elem);
            Gcd.CCC[elem.Gender].BuildGeometry(elem);

            try
            {
                Gcd.CCC[elem.Gender].LastMode = elem.iParam[Gcd.CCC[elem.Gender].iiiMode];
            }
            catch { }

            LastEntity = elem;
            if (elem.Gender != null && elem.Gender.StartsWith("DIM")) Gcd.Drawing.LastDimension = elem;

            // // UNDO
            // Gcd.Drawing.uUndo.OpenUndoStage("Draw a " + elem.Gender, Undo.TypeCreate);
            // Gcd.Drawing.uUndo.AddUndoItem(elem);
            // Gcd.Drawing.uUndo.CloseUndoStage();

            Gcd.Drawing.RequiresSaving = true;
            Gcd.DrawHoveredEntity = true;
            Gcd.Drawing.iEntity.Clear();
            Gcd.clsJobPrevious = Gcd.Tools["BUILDER"]; // store current builder as previous job

            // select created entity
            Gcd.Drawing.Sheet.EntitiesSelected.Clear();
            Gcd.Drawing.Sheet.EntitiesSelected.Add(elem.id,elem);
            // fMain.fp.FillProperties(Gcd.Drawing.Sheet.EntitiesSelected);

            Gcd.Drawing.Sheet.Grips.Clear();
            Gcd.CCC[elem.Gender].GenerateGrips(elem);

            Gcd.clsJob = Gcd.Tools["SELECTION"];
            Gcd.clsJob.Start();

            // Gcd.Drawing.Sheet.GlSheet.PopupMenu = string.Empty;
            Gcd.Tools["SELECTION"].PoiChecking = true;
            DrawingAids.CleanTexts();

            // Generate GL lists
            clsEntities.GlGenDrawList(elem);
            // clsEntities.GlGenDrawListLAyers(elem.pLayer);

            Gcd.Redraw();

            return true;
        }

        public  void Cancel()
        {
            elem = null;
            Gcd.clsJobPrevious = Gcd.Tools["BUILDER"];
            Gcd.clsJob = Gcd.Tools["SELECTION"];
            DrawingAids.CleanTexts();
            Gcd.Redraw();
            //try { Gcd.Drawing.Sheet.GlSheet.PopupMenu = string.Empty; } catch { }
        }

        public  void KeyDown(int iCode)
        {
            if (iCode == Key.ControlKey) Gcd.Orthogonal = true;
        }
    }

  
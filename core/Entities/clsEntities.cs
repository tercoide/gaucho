
    // Port of Gambas clsEntities.class -> C#
    // Notes:
    // - This translation preserves behavior and structure but assumes existing types:
    //   Entity, Block, Layer, LineType, Style, Sheet, Drawing, Undo, Glx, gl, paint, puntos, stl, dxf, Gcd, etc.
    // - Gambas "Collection" -> List<T> or Dictionary<string,T> depending on keyed usage.
    // - Gambas "Float[]" -> double[] here (consistent with previous puntos conversion).
    // - Many helper classes and global objects (Gcd, Glx, gl, paint, dxf, stl) are referenced as-is; integrate with your project.
    // - Some Gambas semantics (e.g. IsNull, Try ... Catch-free behavior, dynamic resizing) are approximated.
    //
    // You should compile this together with the rest of the converted code and wire the external dependencies.
    using OpenTK.Graphics.OpenGL4;
    using Gaucho;
    public static class clsEntities
    {
        // Buffer IDs previously an Integer[] Collection in Gambas
        public static List<int> InxBuffersID { get; } = new List<int>();

        public static void GenGrips(Entity e)
        {
            if (e == null) return;
            Gcd.CCC[e.Gender].GenerateGrips(e);
        }

        // Edit properties for a collection of entities
        public static bool EditEntities(string sProperty, string vValue, bool DoRegen = true, Dictionary<string, Entity>? cEntities = null)
        {
            if (cEntities == null) cEntities = Gcd.Drawing.Sheet.EntitiesSelected ?? new Dictionary<string, Entity>();

            // Gcd.Drawing.uUndo.OpenUndoStage(sProperty, Undo.TypeModify);

            foreach (var ep in cEntities)
            {
                var e = ep.Value;
                // Gcd.Drawing.uUndo.AddUndoItem(e);
                switch (sProperty)
                {
                    case "color":
                        e.Colour = Convert.ToInt32(vValue);
                        break;
                    case "linewidth":
                        e.LineWidth = Convert.ToInt32(vValue);
                        break;
                    case "layer":
                        e.pLayer = Gcd.GetLayer(Convert.ToString(vValue));
                        break;
                    case "linetype":
                        e.LType = GetLTypeByName(Convert.ToString(vValue));
                        break;
                    // case "points":
                    //     // expecting double[] or float[] based on your model
                    //     e.P = vValue as double[] ?? vValue as float[] as double[];
                    //     break;
                }
            }

           // Gcd.Drawing.uUndo.CloseUndoStage();
            CollectVisibleEntities();
            if (DoRegen) Gcd.Regen();
            return true;
        }

       

        // public static Block FindBlockById(string blockId)
        // {
        //     if (string.IsNullOrEmpty(blockId)) return null;
        //     foreach (var b in Gcd.Drawing.Blocks)
        //     {
        //         if (b.id == blockId) return b;
        //     }
        //     return null;
        // }

        // public static Block FindBlock(string blockName, IEnumerable<Block> container)
        // {
        //     if (string.IsNullOrEmpty(blockName) || container == null) return null;
        //     foreach (var b in container)
        //     {
        //         if (b.Name== blockName) return b;
        //     }
        //     return null;
        // }

        // public static Style FindStyle(string name)
        // {
        //     if (string.IsNullOrEmpty(name)) return null;
        //     foreach (var s in Gcd.Drawing.oStyles)
        //     {
        //         if (s.Name== name) return s;
        //     }
        //     return null;
        // }

        // public static bool ReconstruirBloques()
        // {
        //     // Rebuild blocks from entities
        //     try
        //     {
        //         // var sw = Stopwatch.StartNew();
        //         Gcd.debugInfo("Reconstruyendo bloques");

        //         var entities = Gcd.Drawing.Sheet.Entities;
        //         var arrBlocks = new List<Block>();

        //         int iStart = 0;
        //         for (int i = 0; i < entities.Count; i++)
        //         {
        //             // Gcd.prompt = $"Loading blocks {((double)i / entities.Count):P2}";
        //             // System.Threading.Thread.Sleep(0); // Wait 0

        //             var ent = entities[i];
        //             if (ent.Gender == "BLOCK")
        //             {
        //                 var newBlock = new Block
        //                 {
        //                     Name = ent.block,
        //                     x = ent.p?[0] ?? 0,
        //                     y = ent.p?.Length > 1 ? ent.p[1] : 0,
        //                     entities = new Dictionary<string, Entity>()
        //                 };

        //                 bool partesEncontradas = false;
        //                 for (int i2 = iStart; i2 < entities.Count; i2++)
        //                 {
        //                     var e2 = entities[i2];
        //                     if (e2.block == newBlock.Name&& e2.Gender != "BLOCK")
        //                     {
        //                         if (e2.block == newBlock.Name&& e2.Gender != "INSERT")
        //                         {
        //                             partesEncontradas = true;
        //                             var o2 = ClonEntity(e2);
        //                             Gcd.CCC[o2.Gender].Finish(o2);
        //                             newBlock.entities.Add(o2);
        //                         }
        //                     }
        //                     else
        //                     {
        //                         if (partesEncontradas)
        //                         {
        //                             partesEncontradas = false;
        //                             iStart = i2;
        //                             break;
        //                         }
        //                     }
        //                 }
        //                 arrBlocks.Add(newBlock);
        //             }
        //         }

        //         Gcd.Drawing.arrBlocks = arrBlocks;
        //         System.Threading.Thread.Sleep(0);
        //         sw.Stop();
        //         Gcd.debugInfo($"Reconstruccion finalizada en {sw.Elapsed.TotalSeconds}s");
        //         return true;
        //     }
        //     catch (Exception ex)
        //     {
        //         Gcd.debugInfo("ReconstruirBloques error: " + ex.Message);
        //         return false;
        //     }
        // }

        // // Generate GL buffers: either for a single entity or for the whole drawing VBOs
        // public static void GlGenBuffers(Entity eEntity = null)
        // {
        //     if (eEntity != null)
        //     {
        //         // Per-entity GL list generation is done in other methods (glGenDrawList).
        //         return;
        //     }

        //     // Whole drawing VBO generation
        //     try
        //     {
        //         if (Gcd.Drawing.Sheet.Entities.Count == 0) return;

        //         Glx.VBOFlush();
        //         foreach (var ep in Gcd.Drawing.Sheet.Entities)
        //         {
        //             var e = ep.Value;
        //             Gcd.CCC[e.Gender].Draw(e);
        //         }

        //         // Generate buffers
        //         while (InxBuffersID.Count < 3) InxBuffersID.Add(0);

        //         int[] ids = new int[3];
        //         Glx.glGenBuffers(3, InxBuffersID.ToArray()); // assume Glx.glGenBuffers fills provided array or uses wrapper
        //         var iError = Glx.glGetError();
        //         if (iError != 0) Gcd.debugInfo("GL Error: " + iError);

        //         Glx.glBindBuffer(Glx.ARRAY_BUFFER, InxBuffersID[0]);
        //         iError = Glx.glGetError();
        //         if (iError != 0) Gcd.debugInfo("GL Error: " + iError);

        //         var bytesTot = (Glx.VBO_vertex.Count + Glx.VBO_normals.Count + Glx.VBO_colors.Count) * sizeof(float);
        //         Glx.glBufferData(Glx.ARRAY_BUFFER, bytesTot, IntPtr.Zero, Glx.STATIC_DRAW);

        //         Glx.glBufferSubData(Glx.ARRAY_BUFFER, 0, Glx.VBO_vertex.Count * sizeof(float), Glx.VBO_vertex.Data);
        //         Glx.glBufferSubData(Glx.ARRAY_BUFFER, Glx.VBO_vertex.Count * sizeof(float), Glx.VBO_normals.Count * sizeof(float), Glx.VBO_normals.Data);
        //         Glx.glBufferSubData(Glx.ARRAY_BUFFER, (Glx.VBO_vertex.Count + Glx.VBO_normals.Count) * sizeof(float), Glx.VBO_colors.Count * sizeof(float), Glx.VBO_colors.Data);

        //         iError = Glx.glGetError();
        //         if (iError != 0) Gcd.debugInfo("GL Error: " + iError);

        //         Glx.glBindBuffer(Glx.ARRAY_BUFFER, 0);
        //     }
        //     catch (Exception ex)
        //     {
        //         Gcd.debugInfo("GlGenBuffers error: " + ex.Message);
        //     }
        // }

        // Build GL display lists for entity(s)
        public static void GlGenDrawList(Entity? eEntity = null)
        {
            if (eEntity != null)
            {
                if (eEntity == null) return;
                Gcd.CCC[eEntity.Gender].Translate(eEntity, -Gcd.Drawing.Sheet.PanBaseRealX, -Gcd.Drawing.Sheet.PanBaseRealY);

                // if (!GL.islist(eEntity.glDrwList)) eEntity.glDrwList = GL.GenLists(1);
                // GL.NewList(eEntity.glDrwList, GL.COMPILE);
                Gcd.CCC[eEntity.Gender].Draw(eEntity);
                

                // if (!GL.islist(eEntity.glDrwListSel)) eEntity.glDrwListSel = GL.GenLists(1);
                // GL.NewList(eEntity.glDrwListSel, GL.COMPILE);
                Gcd.CCC[eEntity.Gender].DrawSelected(eEntity);
                

                // if (!GL.islist(eEntity.glDrwListRemark)) eEntity.glDrwListRemark = GL.GenLists(1);
                // GL.NewList(eEntity.glDrwListRemark, GL.COMPILE);
                Gcd.CCC[eEntity.Gender].DrawRemark(eEntity);
                

                Gcd.CCC[eEntity.Gender].Translate(eEntity, Gcd.Drawing.Sheet.PanBaseRealX, Gcd.Drawing.Sheet.PanBaseRealY);
            }
            else
            {
                // var t = Stopwatch.StartNew();
                foreach (var sp in Gcd.Drawing.Sheets)
                {
                    var s = sp.Value;
                    foreach (var ep in s.Entities)
                    {
                        var e = ep.Value;
                        if (e.Generated && !e.Regenerable) continue;
                        e.Generated = true;
                        e.Regenerable = Gcd.CCC[e.Gender].Regenerable;

                        // if (!GL.islist(e.glDrwList)) e.glDrwList = GL.GenLists(1);
                        // GL.NewList(e.glDrwList, GL.COMPILE);
                        Gcd.CCC[e.Gender].Draw(e);
                        

                        // if (!GL.islist(e.glDrwListSel)) e.glDrwListSel = GL.GenLists(1);
                        // GL.NewList(e.glDrwListSel, GL.COMPILE);
                        Gcd.CCC[e.Gender].DrawSelected(e);
                        

                        // if (!GL.islist(e.glDrwListRemark)) e.glDrwListRemark = GL.GenLists(1);
                        // GL.NewList(e.glDrwListRemark, GL.COMPILE);
                        Gcd.CCC[e.Gender].DrawRemark(e);
                        
                    }
                }

                GlGenDrawListLayers();
            }
        }

        public static void GlGenDrawListSelected()
        {
            foreach (var ep in Gcd.Drawing.Sheet.EntitiesSelected)
            {
                var e = ep.Value;
                GlGenDrawList(e);
            }
        }

        public static void GlGenDrawListSel(bool RegenEntity = false)
        {
            // if (!GL.islist(Gcd.Drawing.GlListEntitiesSelected)) Gcd.Drawing.GlListEntitiesSelected = GL.GenLists(1);
            // GL.NewList(Gcd.Drawing.GlListEntitiesSelected, GL.COMPILE);

            foreach (var ep in Gcd.Drawing.Sheet.EntitiesSelected)
            {
                var e = ep.Value;
                // if (e.glDrwListSel > 0) GL.CallList(e.glDrwListSel);
                // else 
                    Gcd.CCC[e.Gender].DrawSelected(e);
            }

            
        }

        public static void GlGenDrawListAll(bool ExcludeSelected = false)
        {
            // GL.NewList(Gcd.Drawing.Sheet.GlListAllEntities, GL.COMPILE);

            foreach (var ep in Gcd.Drawing.Sheet.Entities)
            {
                var e = ep.Value;
                if (!Gcd.Drawing.Sheet.EntitiesSelected.ContainsKey(e.id))
                {
                    if (e.pLayer.Visible) Gcd.CCC[e.Gender].Draw(e);//GL.CallList(e.glDrwList);
                }
            }

            
        }

        public static void GlGenDrawListLayers(Layer? aLayer = null)
        {
            if (aLayer != null)
            {
                // if (!GL.islist(aLayer.glList)) aLayer.glList = GL.GenLists(1);
                // GL.NewList(aLayer.glList, GL.COMPILE);
                foreach (var ep in Gcd.Drawing.Sheet.Entities)
                {
                    var e = ep.Value;
                    if (e.pLayer == aLayer && !e.PaperSpace)
                    {
                        Gcd.CCC[e.Gender].Draw(e);
                    }
                }
                
            }
            else
            {
                foreach (var ap in Gcd.Drawing.Layers)
                {
                    var a = ap.Value;
                    // if (!GL.islist(a.glList)) a.glList = GL.GenLists(1);
                    // GL.NewList(a.glList, GL.COMPILE);
                    foreach (var sp  in Gcd.Drawing.Sheets)
                    {
                        var s = sp.Value;
                        if (!s.Name.Equals("Model")) continue;
                        // Glx.DrawTriangles(s.model3d.xyzVertex, Color.Red);
                        foreach (var ep in s.Entities)
                        {
                            var e = ep.Value;
                            if (e.pLayer == a)
                            {
                                Gcd.CCC[e.Gender].Draw(e);
                            }
                        }
                    }
                    
                }

                foreach (var sp in Gcd.Drawing.Sheets)
                {
                    var s = sp.Value;
                    if (s.Name== "Model") continue;
                    // if (!GL.islist(s.GlListAllEntities)) s.GlListAllEntities = GL.GenLists(1);
                    // GL.NewList(s.GlListAllEntities, GL.COMPILE);
                    foreach (var ap in Gcd.Drawing.Layers)
                    {
                        var a = ap.Value;
                        foreach (var ep in s.Entities)
                        {
                            var e = ep.Value;
                            if (e.pLayer == a)
                            {
                                Gcd.CCC[e.Gender].Draw(e);
                            }
                        }
                    }
                    
                }
            }
        }

        public static Entity ClonEntity(Entity eOrigen, bool GetNewId = true)
        {
            var e = Gcd.CCC[eOrigen.Gender].ClonEntity(eOrigen, GetNewId);
            if (!GetNewId) e.id = eOrigen.id;
            return e;
        }

        public static Dictionary<string, Entity> ClonElements(Dictionary<string, Entity>? cEntities = null, bool GenerateGlList = true)
        {
            var result = new Dictionary<string, Entity>();
            var cToClone = cEntities ?? Gcd.Drawing.Sheet.EntitiesSelected;
            foreach (var ep in cToClone)
            {
                var e = ep.Value;
                var eClon = ClonEntity(e, true);
                if (GenerateGlList) GlGenDrawList(eClon);
                result.Add(eClon.id, eClon);
            }
            return result;
        }

        public static Block CopyBlock(Block bBase)
        {
            var b = new Block
            {
                Name = bBase.Name,
                Description = bBase.Description,
                Explotability = bBase.Explotability,
                Flags = bBase.Flags,
                InsertionPlace = bBase.InsertionPlace,
                InsertUnits = bBase.InsertUnits,
                Layer = bBase.Layer,
                Scalability = bBase.Scalability,
                x0 = bBase.x0,
                y0 = bBase.y0,
                z0 = bBase.z0,
                entities = new Dictionary<string, Entity>()
            };

            foreach (var ep in bBase.entities)
            {
                var e = ep.Value;
                var clone = ClonEntity(e);
                clone.id = Gcd.NewId();
                b.entities.Add(clone.id, clone);
            }

            return b;
        }

        public static int DeSelection(string elementos = "todo", string accion = "deseleccionar")
        {
            accion = string.IsNullOrEmpty(accion) ? "deseleccionar" : accion.ToLowerInvariant();
            elementos = elementos?.ToLowerInvariant();

            Gcd.Drawing.Sheet.EntitiesSelected.Clear();
            Gcd.Drawing.Sheet.Grips.Clear();

            // Original Gambas had commented logic. Return 0 for now.
            return 0;
        }

        public static int DeleteSelected()
        {
            try
            {
                Gcd.debugInfo("Borrando entidades", true);

                // Gcd.Drawing.uUndo.OpenUndoStage("Delete entities", Undo.TypeDelete);
                var selected = Gcd.Drawing.Sheet.EntitiesSelected.ToList();
                int c = selected.Count;
                if (c == 0) return 0;

                foreach (var ep in selected)
                {
                    var e = ep.Value;
                   // Gcd.Drawing.uUndo.AddUndoItem(e);
                    Gcd.Drawing.Sheet.Entities.Remove(e.id);
                    Gcd.Drawing.Sheet.EntitiesVisibles.Remove(e.id);
                    e.pLayer.flgForRegen = true;
                }

                //Gcd.Drawing.uUndo.CloseUndoStage();

                foreach (var layp in Gcd.Drawing.Layers)
                {
                    var lay = layp.Value;
                    if (lay.Visible && lay.flgForRegen)
                    {
                        GlGenDrawListLayers(lay);
                        lay.flgForRegen = false;
                    }
                }

                CollectVisibleEntities();
                return c;
            }
            catch (Exception ex)
            {
                Gcd.debugInfo("DeleteSelected error: " + ex.Message);
                return 0;
            }
        }

        // public static void DrawPoint(double x, double y, int colour = -1, double largoReal = 0.4)
        // {
        //     if (colour != -1) paint.brush = Paint.Color(colour);
        //     else paint.brush = Paint.Color(Color.Blue);
        //     paint.LineWidth = 1;
        //     paint.MoveTo(x - largoReal / 2, y);
        //     paint.RelLineTo(largoReal, 0);
        //     paint.MoveTo(x, y - largoReal / 2);
        //     paint.RelLineTo(0, -largoReal);
        //     paint.Stroke();
        // }

        public static void SelectElem(Entity eEntity, bool andItsPoints = true)
        {
            if (eEntity == null) return;
            if (andItsPoints && eEntity.Psel != null)
            {
                for (int i = 0; i < eEntity.Psel.Count; i++) eEntity.Psel[i] = true;
            }
            Gcd.Drawing.Sheet.EntitiesSelected.Add(eEntity.id, eEntity);
        }

        public static void DeSelectElem(Entity eEntity, bool andItsPoints = true)
        {
            if (eEntity == null) return;
            if (andItsPoints && eEntity.Psel != null)
            {
                for (int i = 0; i < eEntity.Psel.Count; i++) eEntity.Psel[i] = false;
            }
            Gcd.Drawing.Sheet.EntitiesSelected.Remove(eEntity.id);
        }

        public static void Move(double dX, double dY, Dictionary<string, Entity>? cEntitiesToMove = null, bool OnlyPointSelected = false, bool DoUndo = false)
        {
            if (cEntitiesToMove == null) cEntitiesToMove = Gcd.Drawing.Sheet.EntitiesSelected;
            foreach (var ep in cEntitiesToMove)
            {
                var e = ep.Value;
                // if (DoUndo) Gcd.Drawing.uUndo.AddUndoItem(ClonEntity(e, false));
                Gcd.CCC[e.Gender].Translate(e, dX, dY, OnlyPointSelected);
            }
        }

        public static void Rotate(Entity e, double degAngle) => Gcd.CCC[e.Gender].Rotate(e, degAngle);

        public static void Draw(Entity e, Sheet? s = null) => Gcd.CCC[e.Gender].Draw(e);

        public static void Draw2(Entity e, Sheet s = null) => Gcd.CCC[e.Gender].Draw2(e);

        public static void DrawSelected(Entity e, Sheet s = null) => Gcd.CCC[e.Gender].DrawSelected(e);

        public static void DrawRemark(Entity e, Sheet s = null) => Gcd.CCC[e.Gender].DrawRemark(e);

        public static void Translate(Entity e, double dX, double dY) => Gcd.CCC[e.Gender].Translate(e, dX, dY);

        public static void Scale(Entity e, double sX, double sY) => Gcd.CCC[e.Gender].Scale(e, sX, sY);

        // Selection rectangle. Returns collection (List<object>) of selected entities or parents
        public static Dictionary<string, Entity> SelectionSquare(double X0, double Y0, double X1, double Y1, bool crossing = false)
        {
            var c = new Dictionary<string, Entity>();
            foreach (var ep in Gcd.Drawing.Sheet.EntitiesVisibles)
            {
                var e = ep.Value;

                bool insideX = (e.Limits[0] >= X0) && (e.Limits[2] <= X1);
                bool insideY = (e.Limits[1] >= Y0) && (e.Limits[3] <= Y1);
                bool outsideX = (e.Limits[0] > X1) || (e.Limits[2] < X0);
                bool outsideY = (e.Limits[1] > Y1) || (e.Limits[3] < Y0);

                if (insideX && insideY)
                {
                    if (e.Container?.Parent != null) c.Add(e.Container.Parent.id, e.Container.Parent);
                    else c.Add(e.id, e);
                    e.Psel = Enumerable.Repeat(true, e.Psel.Count).ToList();
                }
                else if (outsideX || outsideY)
                {
                    continue;
                }
                else
                {
                    if (crossing)
                    {
                        if (SelPartial(e, X0, Y0, X1, Y1))
                        {
                            if (e.Container?.Parent != null) c.Add(e.Container.Parent.id, e.Container.Parent);
                            else c.Add(e.id, e);
                        }
                    }
                    else
                    {
                        if (SelFull(e, X0, Y0, X1, Y1))
                        {
                            if (e.Container?.Parent != null) c.Add(e.Container.Parent.id, e.Container.Parent);
                            else c.Add(e.id, e);
                        }
                    }
                }
            }
            return c;
        }

        public static Dictionary<string, Entity> SelectionPoly(List<double> poly, bool crossing = false)
        {
            var c = new Dictionary<string, Entity>();
            foreach (var ep in Gcd.Drawing.Sheet.EntitiesVisibles)
            {
                var e = ep.Value;

                if (crossing)
                {
                    if (Gcd.CCC[e.Gender].SelPartialPoly(e, poly)) c.Add(e.id, e);
                }
                else
                {
                    if (Gcd.CCC[e.Gender].SelFullPoly(e, poly)) c.Add(e.id, e);
                }
            }
            return c;
        }

        // Collect visible entities (sets Gcd.Drawing.Sheet.EntitiesVisibles)
        public static void CollectVisibleEntities(Sheet sSheet = null)
        {
            sSheet ??= Gcd.Drawing.Sheet;

            double x0 = Gcd.Xreal(0);
            double y1 = Gcd.Yreal(0);
            double x1 = Gcd.Xreal(Gcd.ScreenWidth());
            double y0 = Gcd.Yreal(Gcd.ScreenHeight());

            var cNewVisibles = new Dictionary<string, Entity>();
            Collect(sSheet.Entities, x0, x1, y0, y1, cNewVisibles);
            Gcd.Drawing.Sheet.EntitiesVisibles = cNewVisibles;
            Gcd.debugInfo($"Recolectadas las entidades visibles {Gcd.Drawing.Sheet.EntitiesVisibles.Count} de {Gcd.Drawing.Sheet.Entities.Count}", true);
        }

        private static void Collect(Dictionary<string, Entity> cEntities, double x0, double x1, double y0, double y1, Dictionary<string, Entity> cNewVisibles)
        {
            foreach (var ep in cEntities)
            {
                var e = ep.Value;
                if (!e.pLayer.Visible) continue;
                if (e.Gender == "INSERT" && e.pBlock != null)
                {
                    Collect(e.pBlock.entities, x0, x1, y0, y1, cNewVisibles);
                }
                else
                {
                    bool insideX = (e.Limits[0] <= x1) && (e.Limits[2] >= x0);
                    bool insideY = (e.Limits[1] <= y1) && (e.Limits[3] >= y0);
                    if (insideX && insideY) cNewVisibles.Add(e.id,e);
                }
            }
        }

        public static void BuildGeometry(Entity e = null, Sheet s = null)
        {
            if (e != null)
            {
                Gcd.CCC[e.Gender].BuildGeometry(e);
            }
            else
            {
                foreach (var bp in Gcd.Drawing.Blocks)
                {
                    var b = bp.Value;
                    if (b.idAsociatedLayout == "0") continue;
                    foreach (var entp in b.entities)
                    {
                        var ent = entp.Value;
                        Gcd.CCC[ent.Gender].BuildGeometry(ent);
                    }
                }
            }
        }

        // Compute limits of a set of entities (returns [minX, minY, maxX, maxY])
        public static List<double> ComputeLimits(Dictionary<string, Entity> entities = null, bool onlyVisibles = true, bool ignorePoints = true, bool onlyModel = true)
        {
            var newLimits = new List<double> { 1e100, 1e100, -1e100, -1e100 };

            var entGroup = entities ?? Gcd.Drawing.Sheet.Entities;

            if (entGroup == null || !entGroup.Any()) return new List<double> { 0, 0, 0, 0 };

            foreach (var ep in entGroup)
            {
                var e = ep.Value;
                bool isVisible = true;
                if (onlyVisibles)
                {
                    if (e.pLayer == null) isVisible = true;
                    else isVisible = e.pLayer.Visible && e.Visible && !e.pLayer.Frozen;
                }

                if (isVisible)
                {
                    Puntos.LimitsMax(newLimits, e.Limits);
                }
            }

            return newLimits;
        }

        public static bool SelFull(Entity eTesting, double X1real, double Y1real, double X2real, double Y2real)
            => Gcd.CCC[eTesting.Gender].SelFull(eTesting, X1real, Y1real, X2real, Y2real);

        public static bool SelPartial(Entity eTesting, double X1real, double Y1real, double X2real, double Y2real)
            => Gcd.CCC[eTesting.Gender].SelPartial(eTesting, X1real, Y1real, X2real, Y2real);

        public static Entity? DXFImportToEntity(Drawing drw, Dictionary<string, string> c, bool IsDummy = false)
        {
            // This function depends on dxf parsing utilities and entity factories.
            // Keep the logic and delegate to per-entity importers.
            var keys = new List<string>();
            var values = new List<string>();   
            Dxf.DigestColeccion(c, ref keys, ref values);
            var entityType = c.TryGetValue("0", out string? value) ? value : "";
            if (string.IsNullOrEmpty(entityType)) return null;

            var e = Gcd.CCC[entityType].NewEntity();
            e.pLayer = GetLayerByName(c.ContainsKey("8") ? c["8"] : null) ?? Gcd.Drawing.CommonLayer;
            e.id = c.ContainsKey("5") ? c["5"] : Gcd.NewId();

            if (c.ContainsKey("62")) int.TryParse(c["62"], out int colorVal);
            if (c.ContainsKey("370")) e.LineWidth = int.Parse(c["370"]);

            if (c.ContainsKey("67") && c["67"] == "1") e.PaperSpace = true;

            if (c.ContainsKey("6"))
            {
                var LT = GetLTypeByName(c["6"]);
                e.LType = LT ?? drw.LineTypes.Values.FirstOrDefault();
            }
            else
            {
                e.LType = drw.LineTypes.Values.FirstOrDefault();
            }

            if (Gcd.CCC[e.Gender].ImportDXF(e, ref keys, ref values))
            {
                if (c.ContainsKey("210")) e.Extrusion[0] = float.Parse(c["210"]);
                if (c.ContainsKey("220")) e.Extrusion[1] = float.Parse(c["220"]);
                if (c.ContainsKey("230")) e.Extrusion[2] = float.Parse(c["230"]);

                Gcd.CCC[e.Gender].Finish(e);
                return e;
            }

            return null;
        }

        public static LineType GetLTypeByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            foreach (var lt in Gcd.Drawing.LineTypes.Values)
            {
                if (lt.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return lt;
            }
            return null;
        }

        public static LineType GetLTypeByIndex(string name) => GetLTypeByName(name);

        public static Layer GetLayerByid(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            foreach (var layp in Gcd.Drawing.Layers)
            {
                var lay = layp.Value;
                if (lay.id == id) return lay;
            }
            return null;
        }

        public static Layer GetLayerByName(string name)
        {
            if (name == null) return null;
            foreach (var layp in Gcd.Drawing.Layers)
            {
                var lay = layp.Value;
                if (lay.Name== name) return lay;
            }
            return null;
        }

        public static Entity TrimmedEntity(Entity eBase, double x0, double y0, double x1, double y1)
        {
            if (SelFull(eBase, x0, y0, x1, y1)) return eBase;
            if (SelPartial(eBase, x0, y0, x1, y1)) return eBase;
            return null;
        }

        // public static string EntityToJsonString(Entity e)
        // {
        //     var j = new JSONCollection();
        //     j.Add(e.Gender, "Entity");
        //     j.Add(e.p, "Points");
        //     j.Add(e.sParam, "ParamString");
        //     j.Add(e.fParam, "ParamFloat");
        //     j.Add(e.iParam, "ParamInt");
        //     j.Add(e.Colour, "Color");
        //     j.Add(e.LineWidth, "LineWidth");
        //     try { j.Add(e.LineType?.Name, "LineType"); } catch { }
        //     try { j.Add(e.pStyle?.name, "Style"); } catch { }
        //     try { j.Add(e.pDimStyle?.name, "DimStyle"); } catch { }
        //     return JSON.Encode2(j);
        // }

        public static Dictionary<string,Entity> SelectionCombine(Dictionary<string,Entity> cBase, Dictionary<string,Entity> cNew, string sMode)
        {
            var result = new Dictionary<string,Entity>();
            if (sMode == "new")
            {
                if (cNew != null)
                {
                    foreach (var kvp in cNew)
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            else if (sMode == "add")
            {
                if (cBase != null)
                {
                    foreach (var kvp in cBase)
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
                if (cNew != null)
                {
                    foreach (var kvp in cNew)
                    {
                        if (!result.ContainsKey(kvp.Key))
                            result.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            else if (sMode == "rem")
            {
                if (cBase != null)
                {
                    foreach (var kvp in cBase)
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
                if (cNew != null)
                {
                    foreach (var kvp in cNew)
                    {
                        result.Remove(kvp.Key);
                    }
                }
            }
            return result;
        }

        public static void RebuildDimensions(Dictionary <string,Entity> cDims = null)
        {
            var c = cDims ?? Gcd.Drawing.Sheet.Entities;
            foreach (var ep in c)
            {
                var e = ep.Value;
                if (e.Gender != null && e.Gender.StartsWith("DIM"))
                {
                    var sBlockName = e.pBlock?.Name;
                    try
                    {
                        e.pBlock = Gcd.CCC[e.Gender].RebuildBlock(e);
                        if (e.pBlock != null) e.pBlock.Name= sBlockName;
                    }
                    catch { }
                }
            }
        }

        public static Entity FindInsertByName(Dictionary<string, Entity> cEntities, string sName)
        {
            foreach (var ep in cEntities)
            {
                var e = ep.Value;
                if (e.pBlock != null && e.pBlock.Name== sName) return e;
            }
            return null;
        }

        // public static void ExportStl()
        // {
        //     // Currently just calls stl.SaveSTL on model3d
        //     stl.SaveSTL(Gcd.Drawing.Sheet.model3d, Gcd.Drawing.FileName + ".stl");
        // }

        // Additional helpers
        public static bool EqualPoints(double x1, double y1, double x2, double y2)
            => ((float)x1 == (float)x2) && ((float)y1 == (float)y2);
    }

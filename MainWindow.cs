using System.Collections.Generic;
using Gtk;
using HarfBuzz;
using static Gtk.Orientation;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Menu = Gio.Menu;
using System.Security.Cryptography.X509Certificates;

namespace Gaucho;

public partial class fMain : ApplicationWindow
{
    private HeaderBar _headerBar = null!;
    private Box _mainBox = null!;
    private MenuButton _menuButton = null!;
    private PopoverMenu _popover = null!;
    public Label _statusLabel = null!;
    private Notebook _viewportNotebook = null!;
    Shader? shader;

    GLArea glArea = null!;
    int vao = 0;
    int vbo = 0;
    GestureClick mouse_click = null!;

    public string KeysAccumulator = "";

    // Matrix management for pan and zoom
    private Matrix4 projectionMatrix = Matrix4.Identity;
    private Matrix4 viewMatrix = Matrix4.Identity;
    private Matrix4 modelMatrix = Matrix4.Identity;
    
    // Pan and zoom state
    private Vector2 panOffset = Vector2.Zero;
    private float zoomLevel = 1.0f;
    private Vector2 lastMousePos = Vector2.Zero;
    private bool isPanning = false;


    private bool _demoLayersInitialized = false;

    public fMain(Application app) : base()
    {
        Application = app;
        InitializeComponent();
        BuildUI();
        SetEventsControllers();

        Gcd.clsJob = Gcd.Tools["SELECTION"];
        
        // Log application startup
        // Gaucho.DebugWindow.LogInfo("Main window initialized");
        // Gaucho.DebugWindow.LogDebug("Application startup complete");
    }

    private void InitializeComponent()
    {
        Title = "Gaucho";
        SetDefaultSize(800, 600);

        // Create header bar
        _headerBar = HeaderBar.New();
        _headerBar.ShowTitleButtons = true;
        SetTitlebar(_headerBar);

        // Create menu button
        _menuButton = MenuButton.New();
        _menuButton.IconName = "open-menu-symbolic";
        _menuButton.TooltipText = "Application Menu";

        // Create simple menu model
        var menu = Gio.Menu.New();
        menu.Append("About", "app.about");
        menu.Append("Show Debug Console", "app.debug");
        menu.Append("Quit", "app.quit");

        // Create popover menu
        _popover = PopoverMenu.NewFromModel(menu);
        _menuButton.Popover = _popover;

        _headerBar.PackEnd(_menuButton);
    }
    
    private void SetEventsControllers()
    {
         // Add actions to application
        var app = (Application)Application!;
        var aboutAction = Gio.SimpleAction.New("about", null);
        // aboutAction.OnActivate += OnAboutActivated;
        app.AddAction(Gio.SimpleAction.New("about", null));
        
        var debugAction = Gio.SimpleAction.New("debug", null);
        debugAction.OnActivate += OnDebugActivated;
        app.AddAction(debugAction);
        
        var quitAction = Gio.SimpleAction.New("quit", null);
        // quitAction.OnActivate += OnQuitActivated;
        app.AddAction(quitAction);

        // Se agrega un controlador de eventos del mouse
        EventControllerMotion events = EventControllerMotion.New();
        events.OnMotion += OnMouseMotion;
        glArea.AddController(events);

        // Create and connect a click gesture
        mouse_click = GestureClick.New();
        mouse_click.SetButton(0);
        // mouse_click = glArea;

        mouse_click.OnPressed += EventsMouse;
        mouse_click.OnReleased += OnMouseReleased;
        glArea.AddController(mouse_click);

        // Add mouse wheel (scroll) event controller
        var scroll_controller = EventControllerScroll.New(EventControllerScrollFlags.Vertical | EventControllerScrollFlags.Horizontal);
        scroll_controller.OnScroll += OnMouseWheel;
        glArea.AddController(scroll_controller);

        EventControllerKey key_controller = EventControllerKey.New();
        key_controller.OnKeyPressed += on_key_pressed;
        key_controller.OnKeyReleased += on_key_released;
        AddController(key_controller);

        bool on_key_pressed(object o, EventControllerKey.KeyPressedSignalArgs args)
        {
            Gcd.clsJob.KeyDown((int) args.Keycode);
            if (args.Keycode == 65505 || args.Keycode == 65506) // Shift keys
            {
                Key.Shift = true;
                return true; // Event handled
            }
            else if (args.Keycode == 65307) // Escape key
            {
                // Handle Escape key press
                // Console.WriteLine("Escape key pressed");
                return true; // Event handled
            }
            // Console.WriteLine($"Key pressed: {args.Keycode}");
            return true;
        }
        void on_key_released(object o, EventControllerKey.KeyReleasedSignalArgs args)
        {
            Gcd.clsJob.KeyUp((int) args.Keyval);

            if ((int) args.Keyval == Key.Enter)
            {
                
                Gcd.clsJob.KeyText(KeysAccumulator);
                Console.WriteLine($"Key Text: " + KeysAccumulator);

                KeysAccumulator = "";   
            } else 

            if (args.ToString() !="")
            {
                var sKey = Gb.Chr((int) args.Keyval).ToString(); ;
                Gcd.clsJob.KeyPress((int) args.Keyval, sKey);
                KeysAccumulator += sKey;
                
                // Console.WriteLine($"sKey : " + sKey);            

            } 
            

            
            if (args.Keycode == 65505 || args.Keycode == 65506) // Shift keys
            {
                Key.Shift = false;
                return;
            }
            // Console.WriteLine($"Key released: {args.Keycode}");
        }
    }

    private new void Realize()
    {
        // Make context current before calling GL functions

        glArea.MakeCurrent();
        GL.LoadBindings(new NativeBindingsContext());
        // Get OpenGL version
        string glVersion = GL.GetString(StringName.Version);
        // Get GLSL version
        string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

        Console.WriteLine("OpenGL Version: " + glVersion);
        Console.WriteLine("GLSL Version: " + glslVersion);

        shader = new Shader("/home/martin/gaucho/shaders/basic.vert", "/home/martin/gaucho/shaders/basic.frag");

        // Triangle vertices (x, y, z)
        float[] vertices = new float[] {
            0.0f,  0.5f, 0.0f,
           -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f
        };

        // Layer IDs for each vertex (all belong to layer 0)
        int[] layerIds = new int[] { 0, 0, 0 };

        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);

        // Vertex buffer
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        // Layer ID buffer
        int layerVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, layerVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, layerIds.Length * sizeof(int), layerIds, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(4);
        GL.VertexAttribIPointer(4, 1, VertexAttribIntegerType.Int, 0, IntPtr.Zero);

        GL.BindVertexArray(0);
        
        // Initialize a demo layer for the test triangle
        var demoLayer = new Layer { Name = "Test Triangle", Visible = true, Colour = 1 };
        LayerManager.RegisterLayer(demoLayer);
        
        // Initialize demo layers now that OpenGL is ready (only once)
        if (!_demoLayersInitialized)
        {
            InitializeDemoLayers();
            _demoLayersInitialized = true;
        }
        
        // Initialize matrices
        UpdateMatrices();
    }
        

        public bool Resize()
        {
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            GL.Viewport(0, 0, w, h);
            UpdateMatrices();
            return true;
        }

        public bool Draw()
        {

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (shader != null)
            {
                shader.Use();
                
                // Set matrix uniforms
                shader.SetMatrix4("uProjection", projectionMatrix);
                shader.SetMatrix4("uView", viewMatrix);
                shader.SetMatrix4("uModel", modelMatrix);
                
                // Set layer visibility data
                var layerVisibility = LayerManager.GetLayerVisibilityArray();
                shader.SetBoolArray("uLayerVisible", layerVisibility);
                shader.SetInt("uMaxLayers", LayerManager.MAX_LAYERS);
                
                // Set color uniform
                shader.SetVector4("uColor", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                
                GL.BindVertexArray(vao);
                GL.DrawArrays(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 0, 3);
                GL.BindVertexArray(0);
            }
            
            return true;

        }
        
        private void UpdateMatrices()
        {
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            
            if (w == 0 || h == 0) return;
            
            float aspectRatio = (float)w / (float)h;
            
            // Create orthographic projection matrix
            float zoom = 20.0f / zoomLevel;
            projectionMatrix = Matrix4.CreateOrthographic(w, h, -1.0f, 1.0f);
            
            projectionMatrix *= Matrix4.CreateScale(zoom);

             projectionMatrix *= Matrix4.CreateTranslation(-panOffset.X, -panOffset.Y, 0.0f);

            // Create view matrix with pan offset
           // viewMatrix = Matrix4.CreateTranslation(-panOffset.X, -panOffset.Y, 0.0f);
            
            // Model matrix remains identity for now
            modelMatrix = Matrix4.Identity;
        }

    private new void Unrealize()
    {
        glArea.MakeCurrent();
        if (vbo != 0)
        {
            GL.DeleteBuffer(vbo);
            vbo = 0;
        }
        if (vao != 0)
        {
            GL.DeleteVertexArray(vao);
            vao = 0;
        }
    }
        
    private void EventsMouse(GestureClick o, GestureClick.PressedSignalArgs e)
    {
        int b = (int)mouse_click.GetCurrentButton();  // 1 = left, 2 = middle, 3 = right
        float x = (float)e.X;
        float y = (float)e.Y;
        
        lastMousePos = new Vector2(x, y);
        
        // Middle mouse button starts panning
        if (b == 2)
        {
            isPanning = true;
            Console.WriteLine("Started panning");
        }
        
        Console.WriteLine($"Mouse pressed at ({x}, {y}) with button {b} ");
        
        // Also log to debug window
        DebugWindow.LogInfo($"Mouse pressed at ({x}, {y}) with button {b}");
    }
    
    private void OnMouseReleased(GestureClick o, GestureClick.ReleasedSignalArgs e)
    {
        int b = (int)mouse_click.GetCurrentButton();
        
        if (b == 2)
        {
            isPanning = false;
            Console.WriteLine("Stopped panning");
        }
    }
    
    private void OnMouseMotion(EventControllerMotion o, EventControllerMotion.MotionSignalArgs e)
    {
        Vector2 currentMousePos = new Vector2((float)e.X, (float)e.Y);
        
        if (isPanning)
        {
            // Calculate mouse delta in screen coordinates
            Vector2 mouseDelta = currentMousePos - lastMousePos;
            
            // Convert mouse delta to world coordinates
            int w = glArea.GetAllocatedWidth();
            int h = glArea.GetAllocatedHeight();
            
            if (w > 0 && h > 0)
            {
                float aspectRatio = (float)w / (float)h;
                float zoom = 1.0f / zoomLevel;
                
                // Scale mouse movement based on current zoom and viewport size
                Vector2 worldDelta = new Vector2(
                    mouseDelta.X * 2.0f * zoom * aspectRatio / w,
                    -mouseDelta.Y * 2.0f * zoom / h  // Negative Y to match OpenGL coordinate system
                );
                
                panOffset += worldDelta;
                UpdateMatrices();
                
                // Queue a redraw
                glArea.QueueRender();
                
                Console.WriteLine($"Pan offset: {panOffset}");
            }
        }
        
        lastMousePos = currentMousePos;
    }

    private bool OnMouseWheel(EventControllerScroll o, EventControllerScroll.ScrollSignalArgs e)
    {
        // Implement zooming with mouse wheel
        float zoomFactor = 1.1f;
        
        if (e.Dy < 0) // Scroll up - zoom in
        {
            zoomLevel *= zoomFactor;
        }
        else if (e.Dy > 0) // Scroll down - zoom out
        {
            zoomLevel /= zoomFactor;
        }
        
        // Limit zoom levels
        zoomLevel = Math.Max(0.1f, Math.Min(10.0f, zoomLevel));
        
        UpdateMatrices();
        glArea.QueueRender();
        
        Console.WriteLine($"Zoom level: {zoomLevel}");
        
        // Still call original handler if needed
        Mouse.Delta = (int)e.Dy;
        
        if (Gcd.clsJob != null)
        {
            Gcd.clsJob.MouseWheel();
        }
        
        return true; // Event handled
    }

    private void OnDebugActivated(object? sender, EventArgs e)
    {
        DebugWindow.ShowDebugWindow();
        DebugWindow.LogInfo("Debug console opened from menu");
    }

    private IReadOnlyList<string> LoadRecentFiles()
    {
        // TODO: Replace with real MRU storage; keep at most 10 entries.
        return new List<string>
        {
            "Project1.dwg",
            "Project2.dwg",
            "Project3.dwg",
            "Project4.dwg",
            "Project5.dwg",
            "Project6.dwg",
            "Project7.dwg",
            "Project8.dwg",
            "Project9.dwg",
            "Project10.dwg"
        };
    }

    private MenuButton CreateOpenHistoryMenuButton()
    {
        var menuButton = MenuButton.New();
        menuButton.SetIconName("document-open-recent-symbolic");
        menuButton.SetTooltipText("Recently opened files");
        menuButton.AddCssClass("flat");

        var popover = Popover.New();
        popover.AddCssClass("open-history-popover");

        var listBox = ListBox.New();
        listBox.SetSelectionMode(SelectionMode.None);
        listBox.AddCssClass("open-history-list");

        foreach (var file in LoadRecentFiles())
        {
            listBox.Append(CreateOpenHistoryRow(file));
        }

        listBox.OnRowActivated += (_, args) =>
        {
            if (args.Row?.Child is Label rowLabel)
            {
                var selectedFile = rowLabel.GetText();
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    popover.Popdown();
                    ActionActivate($"open:history:{selectedFile}");
                }
            }
        };

        popover.SetChild(listBox);
        menuButton.SetPopover(popover);
        return menuButton;
    }

    private static ListBoxRow CreateOpenHistoryRow(string file)
    {
        var row = ListBoxRow.New();
        row.SetActivatable(true);
        row.SetSelectable(false);

        var label = Label.New(file);
        label.MarginStart = 12;
        label.MarginEnd = 12;
        label.MarginTop = 6;
        label.MarginBottom = 6;
        label.SetHalign(Align.Start);
        label.SetValign(Align.Center);
        label.SetHexpand(true);

        row.SetChild(label);
        return row;
    }

    private ListBox CreateTopBarQuickList()
    {
        var listBox = ListBox.New();
        listBox.SetSelectionMode(SelectionMode.None);
        listBox.AddCssClass("topbar-quicklist");
        listBox.SetHalign(Align.Fill);
        listBox.SetValign(Align.Center);
        listBox.SetHexpand(true);
        listBox.SetVexpand(false);

        foreach (var entry in LoadTopBarEntries())
        {
            listBox.Append(CreateTopBarListRow(entry));
        }

        return listBox;
    }

    private IEnumerable<string> LoadTopBarEntries()
    {
        // Placeholder items; replace with dynamic data as needed.
        return new[]
        {
            "Session Alpha",
            "Session Beta",
            "Session Gamma"
        };
    }

    private ListBoxRow CreateTopBarListRow(string text)
    {
        var row = ListBoxRow.New();
        row.SetActivatable(false);
        row.SetSelectable(false);

        var rowBox = Box.New(Orientation.Horizontal, 6);
        rowBox.MarginStart = 6;
        rowBox.MarginEnd = 6;
        rowBox.MarginTop = 4;
        rowBox.MarginBottom = 4;
        rowBox.SetHalign(Align.Fill);
        rowBox.SetValign(Align.Center);
        rowBox.SetHexpand(true);

        var avatar = Image.NewFromIconName("avatar-default-symbolic");
        avatar.SetPixelSize(24);
        avatar.AddCssClass("avatar-circle");

        var label = Label.New(text);
        label.SetHalign(Align.Start);
        label.SetValign(Align.Center);
        label.SetHexpand(true);

        var key = NormalizeActionKey(text);
        var openButton = CreateIconButton("document-open-symbolic", $"Open {text}", $"quicklist:open:{key}");
        var pinButton = CreateIconButton("emblem-favorite-symbolic", $"Pin {text}", $"quicklist:pin:{key}");
        var closeButton = CreateIconButton("window-close-symbolic", $"Close {text}", $"quicklist:close:{key}");

        rowBox.Append(avatar);
        rowBox.Append(label);
        rowBox.Append(openButton);
        rowBox.Append(pinButton);
        rowBox.Append(closeButton);

        row.SetChild(rowBox);
        return row;
    }

    private Button CreateIconButton(string iconName, string tooltip, string actionId)
    {
        var button = Button.New();
        button.SetIconName(iconName);
        button.SetTooltipText(tooltip);
        button.AddCssClass("flat");
        button.OnClicked += (_, _) => ActionActivate(actionId);
        return button;
    }

    private static string NormalizeActionKey(string text)
    {
        return text.ToLowerInvariant().Replace(' ', '-');
    }

    private GLArea CreateViewport(string name)
    {
        GLArea glArea1 = Gtk.GLArea.New();
        glArea1.Vexpand = true;
        glArea1.Hexpand = true;

        // Enable required capabilities
        glArea1.HasDepthBuffer = true;
        glArea1.HasStencilBuffer = true;
        glArea1.CanFocus = true;

        // Se agrega un controlador de eventos del mouse
        EventControllerMotion events = EventControllerMotion.New();

        // Create and connect a click gesture
        GestureClick mouse_click_2 = GestureClick.New();
        mouse_click_2.SetButton(0);
        // mouse_click = glArea1;

        mouse_click_2.OnPressed += EventsMouse;
        glArea1.AddController(mouse_click_2);

        // Add mouse wheel (scroll) event controller to viewport
        var scroll_controller_viewport = EventControllerScroll.New(EventControllerScrollFlags.Vertical | EventControllerScrollFlags.Horizontal);
        scroll_controller_viewport.OnScroll += OnMouseWheel;
        glArea1.AddController(scroll_controller_viewport);

        glArea1.MarginTop = 6;
        glArea1.MarginBottom = 6;
        glArea1.MarginStart = 6;
        glArea1.MarginEnd = 6;
        glArea1.AddCssClass("gl-viewport");

        // Set minimum size
        glArea1.SetSizeRequest(400, 300);

       glArea1.SetRequiredVersion(3, 3);    // Sin esto estamos limitados a OpenGL 2.1 en Linux (GLX) y 1.1 en Windows (WGL)


        // --- OpenGL / shader demo: draw a simple triangle ---
       
  
        // Realize: create GL objects and upload geometry
        glArea1.OnRealize += (o, e) => Realize();

        // Render: clear and draw triangle
        glArea1.OnRender += (o, e) => Draw();

        glArea1.OnResize += (o, e) => Resize();



        // Cleanup when the glArea1 is unrealized
        glArea1.OnUnrealize += (o, e) => Unrealize();
        return glArea1;
    }
        



}
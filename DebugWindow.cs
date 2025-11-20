using Gtk;
using System;


public class DebugWindow : Window
{
    private ScrolledWindow _scrolledWindow = null!;
    private TextView _txtDebug = null!;
    private TextBuffer _textBuffer = null!;
    private Button _clearButton = null!;
    private Button _saveButton = null!;
    private Box _mainBox = null!;
    private HeaderBar _headerBar = null!;

    public DebugWindow() : base()
    {
        InitializeComponent();
        BuildUI();
    }

    private void InitializeComponent()
    {
        Title = "Debug Log";
        SetDefaultSize(600, 400);
        
        // Make it a utility window (stays on top but can be minimized)
        // Set window properties for floating behavior
        Modal = false;
        Resizable = true;
        
        // Create header bar
        _headerBar = HeaderBar.New();
        _headerBar.ShowTitleButtons = true;
        _headerBar.TitleWidget.SetTooltipText("Debug Console");
        SetTitlebar(_headerBar);
    }

    private void BuildUI()
    {
        // Create main container
        _mainBox = Box.New(Orientation.Vertical, 0);
        
        // Create toolbar with clear and save buttons
        var toolbar = Box.New(Orientation.Horizontal, 6);
        toolbar.MarginTop = 6;
        toolbar.MarginBottom = 6;
        toolbar.MarginStart = 12;
        toolbar.MarginEnd = 12;
        toolbar.SetHalign(Align.Fill);
        
        // Clear button
        _clearButton = Button.NewWithLabel("Clear");
        _clearButton.SetTooltipText("Clear the debug log");
        _clearButton.OnClicked += OnClearClicked;
        _clearButton.AddCssClass("suggested-action");
        
        // Save button
        _saveButton = Button.NewWithLabel("Save Log");
        _saveButton.SetTooltipText("Save debug log to file");
        _saveButton.OnClicked += OnSaveClicked;
        
        // Add buttons to toolbar
        toolbar.Append(_clearButton);
        toolbar.Append(_saveButton);
        
        // Create scrolled window for text view
        _scrolledWindow = ScrolledWindow.New();
        _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        _scrolledWindow.SetVexpand(true);
        _scrolledWindow.SetHexpand(true);
        _scrolledWindow.MarginStart = 12;
        _scrolledWindow.MarginEnd = 12;
        _scrolledWindow.MarginBottom = 12;
        
        // Create text view (this is our txtDebug)
        _txtDebug = TextView.New();
        _txtDebug.SetEditable(false);
        _txtDebug.SetCursorVisible(false);
        _txtDebug.SetWrapMode(WrapMode.Word);
        _txtDebug.AddCssClass("debug-text");
        
        // Create text buffer
        _textBuffer = _txtDebug.GetBuffer();
        
        // Add text view to scrolled window
        _scrolledWindow.SetChild(_txtDebug);
        
        // Add components to main box
        _mainBox.Append(toolbar);
        _mainBox.Append(_scrolledWindow);
        
        // Set main box as window content
        SetChild(_mainBox);
        
        // Apply styling
        ApplyStyling();
        
        // Add some initial content
        AddLogMessage("Debug console initialized", LogLevel.Info);
    }

    private void ApplyStyling()
    {
        var cssProvider = Gtk.CssProvider.New();
        cssProvider.LoadFromData("""
            .debug-text {
                font-family: 'Courier New', 'Liberation Mono', 'DejaVu Sans Mono', monospace;
                font-size: 10pt;
                background-color: #1e1e1e;
                color: #d4d4d4;
            }
            
            .debug-window {
                background-color: #2d2d30;
            }
        """, -1);
        
        var display = Gdk.Display.GetDefault();
        if (display != null)
        {
            Gtk.StyleContext.AddProviderForDisplay(display, cssProvider, 800);
        }
        
        AddCssClass("debug-window");
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        _textBuffer.SetText("", 0);
        AddLogMessage("Debug log cleared", LogLevel.Info);
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            string logContent = GetAllText();
            string fileName = $"debug_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            
            System.IO.File.WriteAllText(filePath, logContent);
            AddLogMessage($"Log saved to: {filePath}", LogLevel.Info);
        }
        catch (Exception ex)
        {
            AddLogMessage($"Error saving log: {ex.Message}", LogLevel.Error);
        }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug
    }

    public void AddLogMessage(string message, LogLevel level = LogLevel.Info)
    {
        if (_textBuffer == null) return;

        string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        string levelStr = level switch
        {
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Debug => "DEBUG",
            _ => "INFO"
        };

        string formattedMessage = $"[{timestamp}] [{levelStr}] {message}\n";
        
        // Get end iterator
        TextIter endIter = _textBuffer.GetEndIter();
        
        // Insert the new message
        _textBuffer.Insert(ref endIter, formattedMessage, -1);
        
        // Auto-scroll to bottom
        var mark = _textBuffer.GetInsert();
        _txtDebug.ScrollToMark(mark, 0.0, false, 0.0, 0.0);
        
        // Limit the buffer size to prevent memory issues (keep last 1000 lines)
        LimitBufferSize();
    }

    private void LimitBufferSize(int maxLines = 1000)
    {
        int lineCount = _textBuffer.GetLineCount();
        if (lineCount > maxLines)
        {
            var startIter = _textBuffer.GetStartIter();
            var deleteEndIter = _textBuffer.GetIterAtLine(lineCount - maxLines);
            _textBuffer.Delete(ref startIter, ref deleteEndIter);
        }
    }

    public void AddInfoLog(string message) => AddLogMessage(message, LogLevel.Info);
    public void AddWarningLog(string message) => AddLogMessage(message, LogLevel.Warning);
    public void AddErrorLog(string message) => AddLogMessage(message, LogLevel.Error);
    public void AddDebugLog(string message) => AddLogMessage(message, LogLevel.Debug);

    public string GetAllText()
    {
        var startIter = _textBuffer.GetStartIter();
        var endIter = _textBuffer.GetEndIter();
        return _textBuffer.GetText(startIter, endIter, false);
    }

    public void ClearLog()
    {
        _textBuffer.SetText("", 0);
    }

    public TextView GetTextView()
    {
        return _txtDebug;
    }

    // Static instance for global access
    private static DebugWindow? _instance;
    
    public static DebugWindow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DebugWindow();
            }
            return _instance;
        }
    }

    public static void ShowDebugWindow()
    {
        Instance.Present();
    }

    public static void HideDebugWindow()
    {
        Instance.SetVisible(false);
    }

    public static void LogInfo(string message) => Instance.AddInfoLog(message);
    public static void LogWarning(string message) => Instance.AddWarningLog(message);
    public static void LogError(string message) => Instance.AddErrorLog(message);
    public static void LogDebug(string message) => Instance.AddDebugLog(message);
}
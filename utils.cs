
using System.Diagnostics;

namespace Gaucho;

    public static class  Colors
    {
        public const int Black = 0;
        public const int White = 1;
        public const int Red = 2;
        public const int Green = 3;
        public const int Blue = 4;
        public const int Yellow = 5;
        public const int Magenta = 6;
        public const int Cyan = 7;
        public const int Gray = 8;
        public const int DarkGray = 9;
        public const int LightRed = 10;
        public const int LightGreen = 11;
        public const int LightBlue = 12;
        public const int LightYellow = 13;
        public const int LightMagenta = 14;
        public const int LightCyan = 15;


        public static int ToRgb(int color)
        {
            return color switch
            {
                Black => 0x000000,
                White => 0xFFFFFF,
                Red => 0xFF0000,
                Green => 0x00FF00,
                Blue => 0x0000FF,
                Yellow => 0xFFFF00,
                Magenta => 0xFF00FF,
                Cyan => 0x00FFFF,
                Gray => 0x808080,
                DarkGray => 0x404040,
                LightRed => 0xFF8080,
                LightGreen => 0x80FF80,
                LightBlue => 0x8080FF,
                LightYellow => 0xFFFF80,
                LightMagenta => 0xFF80FF,
                LightCyan => 0x80FFFF,
                _ => throw new ArgumentOutOfRangeException(nameof(color), "Invalid color value"),
            };
        }

        public static int Desaturate(int color)
        {
            int r = (color >> 16) & 0xFF;
            int g = (color >> 8) & 0xFF;
            int b = color & 0xFF;
            int gray = (r + g + b) / 3;
            return (gray << 16) | (gray << 8) | gray;
        }

        public static int Blend(int color1, int color2, float ratio)
        {
            int r1 = (color1 >> 16) & 0xFF;
            int g1 = (color1 >> 8) & 0xFF;
            int b1 = color1 & 0xFF;

            int r2 = (color2 >> 16) & 0xFF;
            int g2 = (color2 >> 8) & 0xFF;
            int b2 = color2 & 0xFF;

            int r = (int)(r1 * (1 - ratio) + r2 * ratio);
            int g = (int)(g1 * (1 - ratio) + g2 * ratio);
            int b = (int)(b1 * (1 - ratio) + b2 * ratio);

            return (r << 16) | (g << 8) | b;
        }   
        public static int Invert(int color)
        {
            int r = 255 - ((color >> 16) & 0xFF);
            int g = 255 - ((color >> 8) & 0xFF);
            int b = 255 - (color & 0xFF);
            return (r << 16) | (g << 8) | b;
        }   
        
    }


    public static class Utils
    {


        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
   


        public static void Shell(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine("Output: " + output);
            Console.WriteLine("Error: " + error);
        }

        /// <summary>
    /// VB.NET-like DoEvents function - Processes pending UI events and messages
    /// This allows the UI to remain responsive during long-running operations
    /// </summary>
    public static void DoEvents()
    {
        try
        {
            // For GTK4, we use GLib.MainContext to process pending events
            var context = GLib.MainContext.Default();
            while (context.Pending())
            {
                context.Iteration(false);
            }
        }
        catch (Exception ex)
        {
            // Log the error but don't let it break the application
            Console.WriteLine($"DoEvents error: {ex.Message}");
        }
    }

    /// <summary>
    /// VB.NET-like DoEvents function with timeout - Processes pending UI events for a specified duration
    /// </summary>
    /// <param name="maxProcessingTimeMs">Maximum time to spend processing events in milliseconds</param>
    public static void DoEvents(int maxProcessingTimeMs)
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var context = GLib.MainContext.Default();

            while (context.Pending() && stopwatch.ElapsedMilliseconds < maxProcessingTimeMs)
            {
                context.Iteration(false);
            }

            stopwatch.Stop();
        }
        catch (Exception ex)
        {
            // Log the error but don't let it break the application
            Console.WriteLine($"DoEvents error: {ex.Message}");
        }
    }
         #region File Path Helper Functions

        /// <summary>
        /// Returns the file name part after the last '/' in the path.
        /// If no '/' is found, returns the original string.
        /// Mirrors the behavior of the provided VB FileFromPath.
        /// </summary>
        /// <param name="sPath">The file path</param>
        /// <returns>The file name portion</returns>
        public static string? FileFromPath(string? sPath)
        {
            if (string.IsNullOrEmpty(sPath)) return sPath;
            int last = sPath.LastIndexOf('/');
            if (last == -1) return sPath;
            return sPath.Substring(last + 1);
        }

        /// <summary>
        /// Returns the path portion including the trailing '/' up to the last '/'.
        /// If no '/' is found, returns an empty string.
        /// Mirrors the behavior of the provided VB PathFromFile.
        /// </summary>
        /// <param name="sPath">The file path</param>
        /// <returns>The path portion with trailing slash</returns>
        public static string? PathFromFile(string? sPath)
        {
            if (sPath == null) return null;
            int last = sPath.LastIndexOf('/');
            if (last == -1) return string.Empty;
            // include the trailing slash, matching VB's Left(..., p2 - 1) behavior
            return sPath.Substring(0, last + 1);
        }

        /// <summary>
        /// Returns the file name without its extension (the part before the last '.').
        /// If there is no '.' in the file name, returns the file name unchanged.
        /// Mirrors the behavior of the provided VB FileWithoutExtension.
        /// </summary>
        /// <param name="sPath">The file path</param>
        /// <returns>The file name without extension</returns>
        public static string? FileWithoutExtension(string? sPath)
        {
            if (sPath == null) return null;
            string? fileName = FileFromPath(sPath);
            if (fileName == null) return null;
            int idx = fileName.LastIndexOf('.');
            if (idx == -1) return fileName;
            // if '.' is the first character, this returns an empty string (matches VB behavior)
            return fileName.Substring(0, idx);
        }

        #endregion

    }




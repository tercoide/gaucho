
using System.Diagnostics;

namespace Gaucho;

public static class Colors
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
    public static int Gradient(int color1, int color2)
        {
            

            return (Blend(color1, color2, 0.5f))    ;
        }

        /// <summary>
        /// Extracts the alpha component from an ARGB color value
        /// </summary>
        /// <param name="color">The ARGB color value (format: 0xAARRGGBB)</param>
        /// <returns>The alpha component (0-255)</returns>
        public static int GetAlpha(int color)
        {
            return (color >> 24) & 0xFF;
        }

        /// <summary>
        /// Extracts the red component from an RGB or ARGB color value
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The red component (0-255)</returns>
        public static int GetRed(int color)
        {
            return (color >> 16) & 0xFF;
        }

        /// <summary>
        /// Extracts the green component from an RGB or ARGB color value
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The green component (0-255)</returns>
        public static int GetGreen(int color)
        {
            return (color >> 8) & 0xFF;
        }

        /// <summary>
        /// Extracts the blue component from an RGB or ARGB color value
        /// </summary>
        /// <param name="color">The color value</param>
        /// <returns>The blue component (0-255)</returns>
        public static int GetBlue(int color)
        {
            return color & 0xFF;
        }

        /// <summary>
        /// Creates an ARGB color value from individual components
        /// </summary>
        /// <param name="alpha">Alpha component (0-255)</param>
        /// <param name="red">Red component (0-255)</param>
        /// <param name="green">Green component (0-255)</param>
        /// <param name="blue">Blue component (0-255)</param>
        /// <returns>The ARGB color value</returns>
        public static int MakeArgb(int alpha, int red, int green, int blue)
        {
            return ((alpha & 0xFF) << 24) | ((red & 0xFF) << 16) | ((green & 0xFF) << 8) | (blue & 0xFF);
        }

        /// <summary>
        /// Creates an RGB color value from individual components (alpha = 255)
        /// </summary>
        /// <param name="red">Red component (0-255)</param>
        /// <param name="green">Green component (0-255)</param>
        /// <param name="blue">Blue component (0-255)</param>
        /// <returns>The ARGB color value with full opacity</returns>
        public static int RGB(int red, int green, int blue)
        {
            return MakeArgb(255, red, green, blue);
        }

}


public static class Util
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

        public static List<string> SplitComplex(string sToDivIde, string sLargeSeparator,  bool KeepIt = false)
        {
            int i =0;         
            string sResto = "";         
            List<string> sRespuesta = new List<string>();         
            string sTab = "";         

            sTab = sLargeSeparator;
            sResto = sToDivIde;
            do {
                i = Gb.InStr(sResto, sTab);
                if ( i > 0 )
                {

                    sRespuesta.Add(Gb.Mid(sResto, 1, i - 1));
                    sResto = Gb.Mid(sResto, i + sTab.Length);
                } else {
                    sRespuesta.Add(sResto);
                    break;
                }
            } while ( i > 0 );
            return sRespuesta;

        }
         // <b>RAD Extension.</b><br>
 // Returns a string with all its characters converted to ascii or utf-8 when bad codification ocurr.

        public static string RemoveUnicodes(string s)
            {


            int k = 1;
            int q = 0;         
            int r = 0;         

            string uni ="";         
            List<string> stx = new List<string>();         
                     
            string rep ="";         
            int i = 0;         

            while (Gb.InStr(s, "\\U+", k) == 0 || k > s.Length)
            {
                q = Gb.InStr(s, "\\U+", k);
                if ( q > 0 )
                {
                     r++;
                    uni = Gb.Mid(s, q, 7);
                    stx.Add(uni);
                    k = q + ("\\U+").Length;
                }
            }

            foreach ( var u in stx)
            {
                i = Gb.CInt("&h" + Gb.Mid(u, 4, 4) + "&");
                rep = Gb.Chr(i).ToString();
                s = Gb.Replace(s, u, rep);
            }

            return s;

        }

        public static string ProcessTabs(string s,  int lTab = 6)
        {

            // reconstruye la string teniendo en cuenta los tabuladfores
            // "\t" o "^I"

            int i = 0;         
            int iAnterior = 0;         
            string sResto = "";         
            string sRespuesta ="";         
            string sEspacios ="";         
            string sTab ="";         

            if ( Gb.InStr(s, "\t") > 0 ) {sTab = "\t"; } else { sTab = "^I";};
            sResto = s;
            do {
                i = Gb.InStr(sResto, sTab);
                if ( i > 0 )
                {
                    if ( iAnterior < lTab )
                    {
                        sEspacios = new string(' ', lTab - iAnterior);
                        iAnterior = 0;
                    } else {
                        // int nTabs = (i - 1) / lTab;
                        // int posInTab = (i - 1) - (nTabs * lTab);
                        // sEspacios = new string(' ', lTab - posInTab);
                        sEspacios = new string(' ', lTab);
                    }
                    iAnterior = i - 1;
                    sRespuesta += Gb.Mid(sResto, 1, i - 1) + sEspacios;
                    sResto = Gb.Mid(sResto, i + 1);
                } else {
                    sRespuesta += sResto;
                    break;
                }
            } while ( i > 0 );
            return sRespuesta;

        }

        /// <summary>
        /// Adds a value to a dictionary using the specified key. If the key already exists,
        /// it appends an underscore followed by a number (starting from 0) until an unused key is found.
        /// </summary>
        /// <typeparam name="TValue">The type of the dictionary values</typeparam>
        /// <param name="dictionary">The dictionary to add the value to</param>
        /// <param name="originalKey">The desired key for the value</param>
        /// <param name="value">The value to add to the dictionary</param>
        /// <returns>The actual key used to store the value</returns>
        public static string AddWithUniqueKey<TValue>(Dictionary<string, TValue> dictionary, string originalKey, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));
            
            if (originalKey == null)
                throw new ArgumentNullException(nameof(originalKey));

            string keyToUse = originalKey;
            int counter = 0;

            // Keep trying keys until we find one that doesn't exist
            while (dictionary.ContainsKey(keyToUse))
            {
                keyToUse = $"{originalKey}_{counter}";
                counter++;
            }

            dictionary[keyToUse] = value;
            return keyToUse;
        }

        

    }




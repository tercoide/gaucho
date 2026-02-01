using System;
using System.IO;
using System.Linq;
using OpenTK.Windowing.Common;
using System.Diagnostics;
namespace Gaucho
{

    /// <summary>
    /// Gambas-style helper functions for type conversion and string manipulation
    /// </summary>
    /// 
    /// 
    public static class Key
    {
        public static bool Shift = false;
        public static bool Control = false;
        public static bool Alt = false;
    }
    public static class Mouse

    {
        public static int X = 0;
        public static int Y = 0;
        public static int Delta = 0; 
        public static bool Left = false;
        public static bool Right = false;
        public static bool Middle = false;

        public static void Update(int x, int y, bool left, bool right, bool middle)
        {
            X = x;
            Y = y;
            Left = left;
            Right = right;
            Middle = middle;
        }

    }

    public static class Gb
    {
        #region Type Conversion Functions

        /// <summary>
        /// Converts a string to an integer (similar to VB.NET CInt)
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The integer value</returns>
        /// <exception cref="FormatException">Thrown when the string is not a valid integer</exception>
        public static int CInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"'{str}' is not a valid integer.");
            }
        }

        /// <summary>
        /// Converts a string to a double (similar to VB.NET CDbl)
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The double value</returns>
        /// <exception cref="FormatException">Thrown when the string is not a valid double</exception>
        public static double CDbl(string str)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"'{str}' is not a valid double.");
            }
        }

        public static double Ang(double X , double Y)
        {
            double result = Math.Atan2(Y, X);
           
                return result;
           
        }


        /// <summary>
        /// Returns the absolute value of an integer
        /// </summary>
        /// <param name="i">The integer value</param>
        /// <returns>The absolute value</returns>
        public static double Abs(double i)
        {
            return Math.Abs(i);
        }

        #endregion

        #region Math Conversion Functions

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees">The angle in degrees</param>
        /// <returns>The angle in radians</returns>
        public static double Rad(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians">The angle in radians</param>
        /// <returns>The angle in degrees</returns>
        public static double Deg(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        #endregion

        #region VB.NET-like String Functions

        /// <summary>
        /// VB.NET-like Left function - Returns a string containing the leftmost characters from a string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="length">The number of characters to return from the left</param>
        /// <returns>A string containing the leftmost characters</returns>
        public static string Left(string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
                
            if (length <= 0)
                return string.Empty;
                
            if (length >= str.Length)
                return str;
                
            return str.Substring(0, length);
        }

        /// <summary>
        /// VB.NET-like Mid function - Returns a substring from the middle of a string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="start">The starting position (1-based index like VB.NET)</param>
        /// <param name="length">The number of characters to return (optional)</param>
        /// <returns>A substring from the specified position</returns>
        public static string Mid(string str, int start, int? length = null)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
                
            // Convert 1-based index to 0-based
            int zeroBasedStart = start - 1;
            
            if (zeroBasedStart < 0 || zeroBasedStart >= str.Length)
                return string.Empty;
                
            if (length.HasValue)
            {
                if (length.Value <= 0)
                    return string.Empty;
                    
                int actualLength = Math.Min(length.Value, str.Length - zeroBasedStart);
                return str.Substring(zeroBasedStart, actualLength);
            }
            else
            {
                return str.Substring(zeroBasedStart);
            }
        }

        /// <summary>
        /// VB.NET-like LTrim function - Removes leading whitespace characters
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>A string with leading whitespace removed</returns>
        public static string LTrim(string str)
        {
            return str?.TrimStart() ?? string.Empty;
        }

        /// <summary>
        /// VB.NET-like RTrim function - Removes trailing whitespace characters
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>A string with trailing whitespace removed</returns>
        public static string RTrim(string str)
        {
            return str?.TrimEnd() ?? string.Empty;
        }

        /// <summary>
        /// VB.NET-like Trim function - Removes leading and trailing whitespace characters
        /// </summary>
        /// <param name="str">The source string</param>
        /// <returns>A string with leading and trailing whitespace removed</returns>
        public static string Trim(string str)
        {
            return str?.Trim() ?? string.Empty;
        }

        /// <summary>
        /// VB.NET-like Split function - Splits a string into an array using the specified separator
        /// </summary>
        /// <param name="str">The source string to split</param>
        /// <param name="separator">The separator character or string</param>
        /// <param name="removeEmpty">Whether to remove empty entries (default: false)</param>
        /// <returns>An array of strings</returns>
        public static List<string> Split(string str, string separator, bool removeEmpty = false)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();
                
            if (string.IsNullOrEmpty(separator))
                return new List<string> { str };
                
            var options = removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return str.Split(new string[] { separator }, options).ToList();
        }

        /// <summary>
        /// VB.NET-like Split function overload - Splits a string using a single character separator
        /// </summary>
        /// <param name="str">The source string to split</param>
        /// <param name="separator">The separator character</param>
        /// <param name="removeEmpty">Whether to remove empty entries (default: false)</param>
        /// <returns>An array of strings</returns>
        public static List<string> Split(string str, char separator, bool removeEmpty = false)
        {
            if (string.IsNullOrEmpty(str))
                return new List<string>();
                
            var options = removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return str.Split(new char[] { separator }, options).ToList();
        }

        /// <summary>
        /// VB.NET-like Replace function - Replaces all occurrences of a substring with another substring
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="oldValue">The substring to be replaced</param>
        /// <param name="newValue">The substring to replace with</param>
        /// <param name="ignoreCase">Whether to ignore case when searching (default: false)</param>
        /// <returns>A string with all occurrences of oldValue replaced with newValue</returns>
        public static string Replace(string str, string oldValue, string newValue, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(oldValue))
                return str ?? string.Empty;
                
            if (newValue == null)
                newValue = string.Empty;
                
            if (!ignoreCase)
            {
                return str.Replace(oldValue, newValue);
            }
            else
            {
                // Case-insensitive replacement
                var comparison = StringComparison.OrdinalIgnoreCase;
                var result = str;
                int index = 0;
                
                while ((index = result.IndexOf(oldValue, index, comparison)) != -1)
                {
                    result = result.Substring(0, index) + newValue + result.Substring(index + oldValue.Length);
                    index += newValue.Length;
                }
                
                return result;
            }
        }

        /// <summary>
        /// VB.NET-like Replace function overload - Replaces all occurrences of a character with another character
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="oldChar">The character to be replaced</param>
        /// <param name="newChar">The character to replace with</param>
        /// <returns>A string with all occurrences of oldChar replaced with newChar</returns>
        public static string Replace(string str, char oldChar, char newChar)
        {
            if (string.IsNullOrEmpty(str))
                return str ?? string.Empty;
                
            return str.Replace(oldChar, newChar);
        }

        /// <summary>
        /// VB.NET-like InStr function - Returns the position of the first occurrence of a substring within a string
        /// </summary>
        /// <param name="sourceString">The string to search in</param>
        /// <param name="searchString">The string to search for</param>
        /// <param name="startPosition">Optional starting position (1-based index like VB.NET, default: 1)</param>
        /// <param name="compareMethod">Optional comparison method (default: case-sensitive)</param>
        /// <returns>The 1-based position of the first occurrence, or 0 if not found</returns>
        public static int InStr(string sourceString, string searchString, int startPosition = 1, StringComparison compareMethod = StringComparison.Ordinal)
        {
            if (string.IsNullOrEmpty(sourceString) || string.IsNullOrEmpty(searchString))
                return 0;
                
            if (startPosition < 1 || startPosition > sourceString.Length)
                return 0;
                
            // Convert 1-based position to 0-based for C#
            int zeroBasedStart = startPosition - 1;
            
            int result = sourceString.IndexOf(searchString, zeroBasedStart, compareMethod);
            
            // Convert back to 1-based indexing (VB.NET style), return 0 if not found
            return result == -1 ? 0 : result + 1;
        }

        /// <summary>
        /// VB.NET-like InStr function overload - Simplified version with just source and search strings
        /// </summary>
        /// <param name="sourceString">The string to search in</param>
        /// <param name="searchString">The string to search for</param>
        /// <returns>The 1-based position of the first occurrence, or 0 if not found</returns>
        public static int InStr(string sourceString, string searchString)
        {
            return InStr(sourceString, searchString, 1, StringComparison.Ordinal);
        }

        /// <summary>
        /// VB.NET-like InStr function overload - With starting position only
        /// </summary>
        /// <param name="startPosition">The starting position (1-based index)</param>
        /// <param name="sourceString">The string to search in</param>
        /// <param name="searchString">The string to search for</param>
        /// <returns>The 1-based position of the first occurrence, or 0 if not found</returns>
        public static int InStr(int startPosition, string sourceString, string searchString)
        {
            return InStr(sourceString, searchString, startPosition, StringComparison.Ordinal);
        }

        /// <summary>
        /// VB.NET-like InStr function overload - Case-insensitive version
        /// </summary>
        /// <param name="sourceString">The string to search in</param>
        /// <param name="searchString">The string to search for</param>
        /// <param name="ignoreCase">If true, performs case-insensitive comparison</param>
        /// <returns>The 1-based position of the first occurrence, or 0 if not found</returns>
        public static int InStr(string sourceString, string searchString, bool ignoreCase)
        {
            var compareMethod = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return InStr(sourceString, searchString, 1, compareMethod);
        }

        #endregion

       
        #region Bit Manipulation Functions

        /// <summary>
        /// Tests if a specific bit is set in an integer value
        /// </summary>
        /// <param name="value">The integer value to test</param>
        /// <param name="bitPosition">The bit position to test (0-based)</param>
        /// <returns>True if the bit is set, false otherwise</returns>
        public static bool IsBitSet(this int value, int bitPosition)
        {
            return (value & (1 << bitPosition)) != 0;
        }

        /// <summary>
        /// Sets a specific bit in an integer value
        /// </summary>
        /// <param name="value">The integer value to modify</param>
        /// <param name="bitPosition">The bit position to set (0-based)</param>
        /// <returns>The modified integer with the bit set</returns>
        public static int SetBit(this int value, int bitPosition)
        {
            return value | (1 << bitPosition);
        }

        /// <summary>
        /// Clears a specific bit in an integer value
        /// </summary>
        /// <param name="value">The integer value to modify</param>
        /// <param name="bitPosition">The bit position to clear (0-based)</param>
        /// <returns>The modified integer with the bit cleared</returns>
        public static int ClearBit(this int value, int bitPosition)
        {
            return value & ~(1 << bitPosition);
        }

        /// <summary>
        /// Toggles a specific bit in an integer value
        /// </summary>
        /// <param name="value">The integer value to modify</param>
        /// <param name="bitPosition">The bit position to toggle (0-based)</param>
        /// <returns>The modified integer with the bit toggled</returns>
        public static int ToggleBit(this int value, int bitPosition)
        {
            return value ^ (1 << bitPosition);
        }

        /// <summary>
        /// Performs a right bit shift operation (similar to VB.NET Shr function)
        /// </summary>
        /// <param name="value">The integer value to shift</param>
        /// <param name="positions">The number of positions to shift right</param>
        /// <returns>The result of the right shift operation</returns>
        public static int Shr(int value, int positions)
        {
            if (positions < 0)
                throw new ArgumentException("Shift positions cannot be negative", nameof(positions));
                
            // Perform logical right shift (unsigned)
            return (int)((uint)value >> positions);
        }

        /// <summary>
        /// Performs a right bit shift operation on a long value
        /// </summary>
        /// <param name="value">The long value to shift</param>
        /// <param name="positions">The number of positions to shift right</param>
        /// <returns>The result of the right shift operation</returns>
        public static long Shr(long value, int positions)
        {
            if (positions < 0)
                throw new ArgumentException("Shift positions cannot be negative", nameof(positions));
                
            // Perform logical right shift (unsigned)
            return (long)((ulong)value >> positions);
        }

        /// <summary>
        /// Performs a right bit shift operation on a byte value
        /// </summary>
        /// <param name="value">The byte value to shift</param>
        /// <param name="positions">The number of positions to shift right</param>
        /// <returns>The result of the right shift operation</returns>
        public static byte Shr(byte value, int positions)
        {
            if (positions < 0)
                throw new ArgumentException("Shift positions cannot be negative", nameof(positions));
                
            if (positions >= 8)
                return 0; // All bits shifted out
                
            return (byte)(value >> positions);
        }

        #endregion
        public static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        public static double ToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }
        public static void  Wait(int value)
        {
            System.Threading.Thread.Sleep(value);
        }

        #region File System Functions

        /// <summary>
        /// Retrieves all files in a given path with a given filter (similar to VB.NET Dir function)
        /// </summary>
        /// <param name="path">The directory path to search in</param>
        /// <param name="filter">File filter pattern (e.g., "*.txt", "*.cs", "*.*")</param>
        /// <param name="includeSubdirectories">Whether to search subdirectories recursively (default: false)</param>
        /// <returns>An array of file paths matching the filter</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory doesn't exist</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when access to the directory is denied</exception>
        public static string[] Dir(string path, string filter = "*.*", bool includeSubdirectories = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            if (string.IsNullOrEmpty(filter))
                filter = "*.*";

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory '{path}' not found.");

            try
            {
                SearchOption searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                
                // Get files matching the filter
                string[] files = Directory.GetFiles(path, filter, searchOption);
                
                // Return just the file names (without full path) for VB.NET Dir compatibility
                // If you want full paths, change this to return files directly
                return files.Select(file => Path.GetFileName(file)).ToArray();
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
            {
                // Re-throw specific exceptions that callers might want to handle
                throw;
            }
        }

        /// <summary>
        /// Retrieves all files in a given path with a given filter, returning full paths
        /// </summary>
        /// <param name="path">The directory path to search in</param>
        /// <param name="filter">File filter pattern (e.g., "*.txt", "*.cs", "*.*")</param>
        /// <param name="includeSubdirectories">Whether to search subdirectories recursively (default: false)</param>
        /// <returns>An array of full file paths matching the filter</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory doesn't exist</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when access to the directory is denied</exception>
        public static string[] DirFullPath(string path, string filter = "*.*", bool includeSubdirectories = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            if (string.IsNullOrEmpty(filter))
                filter = "*.*";

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory '{path}' not found.");

            try
            {
                SearchOption searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                return Directory.GetFiles(path, filter, searchOption);
            }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException)
            {
                throw;
            }
        }

        #endregion

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

        #region Character/Unicode Functions

        /// <summary>
        /// VB.NET-like Asc function - Returns the UTF-16 code of a character in a string
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="position">The 1-based position of the character (default: 1)</param>
        /// <returns>The UTF-16 code of the character</returns>
        /// <exception cref="ArgumentException">Thrown when the string is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is invalid</exception>
        public static int Asc(string str, int position = 1)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("String cannot be null or empty", nameof(str));

            if (position < 1 || position > str.Length)
                throw new ArgumentOutOfRangeException(nameof(position), 
                    $"Position must be between 1 and {str.Length}");

            // Convert 1-based position to 0-based for C#
            return (int)str[position - 1];
        }

        /// <summary>
        /// Gets the UTF-16 code of a character
        /// </summary>
        /// <param name="character">The character to get the code for</param>
        /// <returns>The UTF-16 code of the character</returns>
        public static int Asc(char character)
        {
            return (int)character;
        }

        /// <summary>
        /// VB.NET-like Chr function - Returns the character corresponding to a UTF-16 code
        /// </summary>
        /// <param name="code">The UTF-16 code</param>
        /// <returns>The character corresponding to the code</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when code is not a valid UTF-16 value</exception>
        public static char Chr(int code)
        {
            if (code < 0 || code > 65535)
                throw new ArgumentOutOfRangeException(nameof(code), 
                    "Code must be between 0 and 65535 for UTF-16");

            return (char)code;
        }

        /// <summary>
        /// Gets the full Unicode code point of a character in a string (supports surrogate pairs)
        /// </summary>
        /// <param name="str">The source string</param>
        /// <param name="position">The 1-based position of the character (default: 1)</param>
        /// <returns>The Unicode code point</returns>
        /// <exception cref="ArgumentException">Thrown when the string is null or empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when position is invalid</exception>
        public static int GetUnicodeCodePoint(string str, int position = 1)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("String cannot be null or empty", nameof(str));

            if (position < 1 || position > str.Length)
                throw new ArgumentOutOfRangeException(nameof(position), 
                    $"Position must be between 1 and {str.Length}");

            // Convert 1-based position to 0-based for C#
            int index = position - 1;

            // Check if this is a high surrogate (start of a surrogate pair)
            if (char.IsHighSurrogate(str[index]) && index + 1 < str.Length && char.IsLowSurrogate(str[index + 1]))
            {
                return char.ConvertToUtf32(str[index], str[index + 1]);
            }

            return (int)str[index];
        }

        #endregion

        #region Array Helper Functions

        /// <summary>
        /// Appends one array to another, works with any type (generic/type-agnostic)
        /// </summary>
        /// <typeparam name="T">The type of elements in the arrays</typeparam>
        /// <param name="original">The original array to append to (can be null)</param>
        /// <param name="toAppend">The array to append to the original</param>
        /// <returns>A new array containing all elements from both arrays</returns>
        public static T[] AppendArray<T>(T[]? original, T[] toAppend)
        {
            if (toAppend == null)
                throw new ArgumentNullException(nameof(toAppend), "Array to append cannot be null");

            if (original == null || original.Length == 0)
                return (T[])toAppend.Clone();

            if (toAppend.Length == 0)
                return (T[])original.Clone();

            var result = new T[original.Length + toAppend.Length];
            Array.Copy(original, 0, result, 0, original.Length);
            Array.Copy(toAppend, 0, result, original.Length, toAppend.Length);
            return result;
        }

        /// <summary>
        /// Appends one double array to another, similar to array concatenation
        /// </summary>
        /// <param name="original">The original array to append to (can be null)</param>
        /// <param name="toAppend">The array to append to the original</param>
        /// <returns>A new array containing all elements from both arrays</returns>
        public static double[] AppendDoubleArray(double[]? original, double[] toAppend)
        {
            if (toAppend == null)
                throw new ArgumentNullException(nameof(toAppend), "Array to append cannot be null");

            if (original == null || original.Length == 0)
                return (double[])toAppend.Clone();

            if (toAppend.Length == 0)
                return (double[])original.Clone();

            var result = new double[original.Length + toAppend.Length];
            Array.Copy(original, 0, result, 0, original.Length);
            Array.Copy(toAppend, 0, result, original.Length, toAppend.Length);
            return result;
        }

        #endregion

    }
}
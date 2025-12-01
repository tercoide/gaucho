
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

// namespace Dear_ImGui_Sample
// {
    // A simple class meant to help create shaders.
    public class Shader
    {
        public readonly int Handle;

        private readonly Dictionary<string, int> _uniformLocations;

        // This is how you create a simple shader.
        // Shaders are written in GLSL, which is a language very similar to C in its semantics.
        // The GLSL source is compiled *at runtime*, so it can optimize itself for the graphics card it's currently being used on.
        // A commented example of GLSL can be found in shader.vert.
        public Shader(string vertPath, string fragPath)
        {
            // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
            // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
            //   The vertex shader won't be too important here, but they'll be more important later.
            // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
            //   The fragment shader is what we'll be using the most here.

            // Load vertex shader and compile
            var shaderSource = File.ReadAllText(vertPath);

            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Now, bind the GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);

            // And then compile
            CompileShader(vertexShader);

            // We do the same for the fragment shader.
            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
            // To do this, create a program...
            Handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            // And then link them together.
            LinkProgram(Handle);

            // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        // A wrapper function that enables the shader program.
        public void Use()
        {
            GL.UseProgram(Handle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public void SetBool(string name, bool data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], Convert.ToInt32(data));
        }

        // Uniform setters
        // Uniforms are variables that can be set by user code, instead of reading them from the VBO.
        // You use VBOs for vertex-related data, and uniforms for almost everything else.

        // Setting a uniform is almost always the exact same, so I'll explain it here once, instead of in every method:
        //     1. Bind the program you want to set the uniform on
        //     2. Get a handle to the location of the uniform with GL.GetUniformLocation.
        //     3. Use the appropriate GL.Uniform* function to set the uniform.

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform float on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="name">The name of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }
    }

    /// <summary>
    /// Enumeration for supported primitive types
    /// </summary>
    public enum PrimitiveType
    {
        Triangles,
        Lines,
        LineStrip
    }

    /// <summary>
    /// Individual VBO data for a specific primitive type
    /// </summary>
    public class VboData
    {
        public int VAO { get; set; }
        public int VertexVBO { get; set; }
        public int ColorVBO { get; set; }
        public int NormalsVBO { get; set; }
        public int TextureVBO { get; set; }
        public int VertexCount { get; set; }
        public float[]? VertexArray { get; set; }
        public float[]? ColorArray { get; set; }
        public float[]? NormalsArray { get; set; }
        public float[]? TextureUVArray { get; set; }

        public VboData()
        {
            VAO = GL.GenVertexArray();
            VertexVBO = GL.GenBuffer();
            ColorVBO = GL.GenBuffer();
            NormalsVBO = GL.GenBuffer();
            TextureVBO = GL.GenBuffer();
            VertexCount = 0;
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VertexVBO);
            GL.DeleteBuffer(ColorVBO);
            GL.DeleteBuffer(NormalsVBO);
            GL.DeleteBuffer(TextureVBO);
        }
    }

    /// <summary>
    /// A VBO container class that holds vertex data arrays for rendering
    /// Contains separate VBOs for each supported primitive type
    /// </summary>
    public class VboContainer : IDisposable
    {
        private static int _nextIndex = 0;
        private bool _disposed = false;

        // Public properties
        public int Index { get; private set; }
        public PrimitiveType CurrentPrimitiveType { get; set; }
        
        // Individual VBOs for each primitive type
        public VboData TrianglesVbo { get; private set; }
        public VboData LinesVbo { get; private set; }
        public VboData LineStripVbo { get; private set; }

        // Dictionary for easy access to VBOs by primitive type
        private Dictionary<PrimitiveType, VboData> _vbosByType;

        /// <summary>
        /// Creates a new VBO container with a unique index and 3 VBOs for each primitive type
        /// </summary>
        /// <param name="defaultPrimitiveType">The default primitive type to use</param>
        public VboContainer(PrimitiveType defaultPrimitiveType = PrimitiveType.Triangles)
        {
            Index = _nextIndex++;
            CurrentPrimitiveType = defaultPrimitiveType;
            
            // Create individual VBOs for each primitive type
            TrianglesVbo = new VboData();
            LinesVbo = new VboData();
            LineStripVbo = new VboData();
            
            // Set up dictionary for easy access
            _vbosByType = new Dictionary<PrimitiveType, VboData>
            {
                { PrimitiveType.Triangles, TrianglesVbo },
                { PrimitiveType.Lines, LinesVbo },
                { PrimitiveType.LineStrip, LineStripVbo }
            };
        }

        /// <summary>
        /// Gets the VBO data for the specified primitive type
        /// </summary>
        /// <param name="primitiveType">The primitive type</param>
        /// <returns>The VBO data for that primitive type</returns>
        public VboData GetVbo(PrimitiveType primitiveType)
        {
            return _vbosByType[primitiveType];
        }

        /// <summary>
        /// Gets the VBO data for the current primitive type
        /// </summary>
        /// <returns>The VBO data for the current primitive type</returns>
        public VboData GetCurrentVbo()
        {
            return _vbosByType[CurrentPrimitiveType];
        }

        /// <summary>
        /// Sets the current primitive type
        /// </summary>
        /// <param name="primitiveType">The primitive type to set as current</param>
        public void SetCurrentPrimitiveType(PrimitiveType primitiveType)
        {
            CurrentPrimitiveType = primitiveType;
        }

        // Legacy properties for backward compatibility - they operate on the current VBO
        public float[]? VertexArray 
        { 
            get => GetCurrentVbo().VertexArray; 
            set => GetCurrentVbo().VertexArray = value; 
        }
        
        public float[]? ColorArray 
        { 
            get => GetCurrentVbo().ColorArray; 
            set => GetCurrentVbo().ColorArray = value; 
        }
        
        public float[]? NormalsArray 
        { 
            get => GetCurrentVbo().NormalsArray; 
            set => GetCurrentVbo().NormalsArray = value; 
        }
        
        public float[]? TextureUVArray 
        { 
            get => GetCurrentVbo().TextureUVArray; 
            set => GetCurrentVbo().TextureUVArray = value; 
        }

        public int VAO => GetCurrentVbo().VAO;
        public int VertexVBO => GetCurrentVbo().VertexVBO;
        public int ColorVBO => GetCurrentVbo().ColorVBO;
        public int NormalsVBO => GetCurrentVbo().NormalsVBO;
        public int TextureVBO => GetCurrentVbo().TextureVBO;
        public int VertexCount => GetCurrentVbo().VertexCount;

        // /// <summary>
        // /// Sets the vertex positions array for the current primitive type
        // /// </summary>
        // /// <param name="vertices">Array of vertex positions (x, y, z for each vertex)</param>
        // public void SetVertices(float[] vertices)
        // {
        //     var currentVbo = GetCurrentVbo();
        //     currentVbo.VertexArray = vertices;
        //     currentVbo.VertexCount = vertices.Length / 3; // Assuming 3 components per vertex (x, y, z)
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, currentVbo.VertexVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        // }

        // /// <summary>
        // /// Sets the vertex positions array for a specific primitive type
        // /// </summary>
        // /// <param name="vertices">Array of vertex positions (x, y, z for each vertex)</param>
        // /// <param name="primitiveType">The primitive type to set vertices for</param>
        // public void SetVertices(float[] vertices, PrimitiveType primitiveType)
        // {
        //     var vbo = GetVbo(primitiveType);
        //     vbo.VertexArray = vertices;
        //     vbo.VertexCount = vertices.Length / 3;
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VertexVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        // }

        // /// <summary>
        // /// Sets the color array for the current primitive type
        // /// </summary>
        // /// <param name="colors">Array of colors (r, g, b, a for each vertex)</param>
        // public void SetColors(float[] colors)
        // {
        //     var currentVbo = GetCurrentVbo();
        //     currentVbo.ColorArray = colors;
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, currentVbo.ColorVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
        // }

        // /// <summary>
        // /// Sets the color array for a specific primitive type
        // /// </summary>
        // /// <param name="colors">Array of colors (r, g, b, a for each vertex)</param>
        // /// <param name="primitiveType">The primitive type to set colors for</param>
        // public void SetColors(float[] colors, PrimitiveType primitiveType)
        // {
        //     var vbo = GetVbo(primitiveType);
        //     vbo.ColorArray = colors;
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ColorVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.StaticDraw);
        // }

        // /// <summary>
        // /// Sets the normals array
        // /// </summary>
        // /// <param name="normals">Array of normal vectors (nx, ny, nz for each vertex)</param>
        // public void SetNormals(float[] normals)
        // {
        //     NormalsArray = normals;
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, NormalsVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * sizeof(float), normals, BufferUsageHint.StaticDraw);
        // }

        // /// <summary>
        // /// Sets the texture UV coordinates array
        // /// </summary>
        // /// <param name="uvs">Array of UV coordinates (u, v for each vertex)</param>
        // public void SetTextureUVs(float[] uvs)
        // {
        //     TextureUVArray = uvs;
            
        //     GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);
        //     GL.BufferData(BufferTarget.ArrayBuffer, uvs.Length * sizeof(float), uvs, BufferUsageHint.StaticDraw);
        // }

        /// <summary>
        /// Configures the vertex attributes and prepares the VAO for rendering
        /// </summary>
        public void SetupVertexAttributes()
        {
            GL.BindVertexArray(VAO);

            // Vertex positions (location 0)
            if (VertexArray != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexVBO);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(0);
            }

            // Colors (location 1)
            if (ColorArray != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, ColorVBO);
                GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(1);
            }

            // Normals (location 2)
            if (NormalsArray != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, NormalsVBO);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(2);
            }

            // Texture UVs (location 3)
            if (TextureUVArray != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, TextureVBO);
                GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(3);
            }

            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Renders the VBO using the current shader for the current primitive type
        /// </summary>
        public void Render()
        {
            var currentVbo = GetCurrentVbo();
            if (currentVbo.VertexCount == 0) return;

            GL.BindVertexArray(currentVbo.VAO);

            OpenTK.Graphics.OpenGL4.PrimitiveType glPrimitiveType = CurrentPrimitiveType switch
            {
                PrimitiveType.Triangles => OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles,
                PrimitiveType.Lines => OpenTK.Graphics.OpenGL4.PrimitiveType.Lines,
                PrimitiveType.LineStrip => OpenTK.Graphics.OpenGL4.PrimitiveType.LineStrip,
                _ => OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles
            };

            GL.DrawArrays(glPrimitiveType, 0, currentVbo.VertexCount);
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Renders a specific primitive type VBO
        /// </summary>
        /// <param name="primitiveType">The primitive type to render</param>
        public void Render(PrimitiveType primitiveType)
        {
            var vbo = GetVbo(primitiveType);
            if (vbo.VertexCount == 0) return;

            GL.BindVertexArray(vbo.VAO);

            OpenTK.Graphics.OpenGL4.PrimitiveType glPrimitiveType = primitiveType switch
            {
                PrimitiveType.Triangles => OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles,
                PrimitiveType.Lines => OpenTK.Graphics.OpenGL4.PrimitiveType.Lines,
                PrimitiveType.LineStrip => OpenTK.Graphics.OpenGL4.PrimitiveType.LineStrip,
                _ => OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles
            };

            GL.DrawArrays(glPrimitiveType, 0, vbo.VertexCount);
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Updates vertex data dynamically for the current primitive type
        /// </summary>
        /// <param name="vertices">New vertex data</param>
        public void UpdateVertices(float[] vertices)
        {
            var currentVbo = GetCurrentVbo();
            currentVbo.VertexArray = vertices;
            currentVbo.VertexCount = vertices.Length / 3;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, currentVbo.VertexVBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, vertices.Length * sizeof(float), vertices);
        }

        /// <summary>
        /// Updates color data dynamically
        /// </summary>
        /// <param name="colors">New color data</param>
        public void UpdateColors(float[] colors)
        {
            ColorArray = colors;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, ColorVBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, colors.Length * sizeof(float), colors);
        }

        /// <summary>
        /// Appends vertex data to the existing vertex array
        /// </summary>
        /// <param name="newVertices">Array of new vertices to append (x, y, z for each vertex)</param>
        public void AppendVertices(float[] newVertices)
        {
            if (newVertices == null || newVertices.Length == 0) return;

            // Combine existing and new vertex data
            var combinedVertices = CombineArrays(VertexArray, newVertices);
            // SetVertices(combinedVertices);
        }

        /// <summary>
        /// Appends color data to the existing color array
        /// </summary>
        /// <param name="newColors">Array of new colors to append (r, g, b, a for each vertex)</param>
        public void AppendColors(float[] newColors)
        {
            if (newColors == null || newColors.Length == 0) return;

            // Combine existing and new color data
            var combinedColors = CombineArrays(ColorArray, newColors);
            // SetColors(combinedColors);
        }

        /// <summary>
        /// Appends normal data to the existing normals array
        /// </summary>
        /// <param name="newNormals">Array of new normals to append (nx, ny, nz for each vertex)</param>
        public void AppendNormals(float[] newNormals)
        {
            if (newNormals == null || newNormals.Length == 0) return;

            // Combine existing and new normal data
            var combinedNormals = CombineArrays(NormalsArray, newNormals);
            // SetNormals(combinedNormals);
        }

        /// <summary>
        /// Appends texture UV data to the existing UV array
        /// </summary>
        /// <param name="newUVs">Array of new UV coordinates to append (u, v for each vertex)</param>
        public void AppendTextureUVs(float[] newUVs)
        {
            if (newUVs == null || newUVs.Length == 0) return;

            // Combine existing and new UV data
            var combinedUVs = CombineArrays(TextureUVArray, newUVs);
            // SetTextureUVs(combinedUVs);
        }

        /// <summary>
        /// Appends complete vertex data (vertices, colors, normals, UVs) in one operation
        /// </summary>
        /// <param name="vertices">New vertices to append</param>
        /// <param name="colors">New colors to append (optional)</param>
        /// <param name="normals">New normals to append (optional)</param>
        /// <param name="uvs">New UV coordinates to append (optional)</param>
        public void AppendVertexData(float[] vertices, float[]? colors = null, float[]? normals = null, float[]? uvs = null, bool SetToGPU=false)
        {
            if (vertices == null || vertices.Length == 0) return;

            // Append all provided data arrays
            AppendVertices(vertices);
            
            if (colors != null && colors.Length > 0)
                AppendColors(colors);
                
            if (normals != null && normals.Length > 0)
                AppendNormals(normals);
                
            if (uvs != null && uvs.Length > 0)
                AppendTextureUVs(uvs);

            // Re-setup vertex attributes after appending all data
            if (SetToGPU) SetupVertexAttributes();
        }

        /// <summary>
        /// Appends another VboContainer's data to this container
        /// </summary>
        /// <param name="other">The VboContainer to append data from</param>
        public void AppendFromVbo(VboContainer other)
        {
            if (other == null) return;

            AppendVertexData(
                other.VertexArray ?? new float[0],
                other.ColorArray,
                other.NormalsArray,
                other.TextureUVArray
            );
        }

        /// <summary>
        /// Helper method to combine two float arrays
        /// </summary>
        /// <param name="existing">Existing array (can be null)</param>
        /// <param name="newData">New data to append</param>
        /// <returns>Combined array</returns>
        private static float[] CombineArrays(float[]? existing, float[] newData)
        {
            if (existing == null || existing.Length == 0)
                return (float[])newData.Clone();

            if (newData == null || newData.Length == 0)
                return (float[])existing.Clone();

            var combined = new float[existing.Length + newData.Length];
            Array.Copy(existing, 0, combined, 0, existing.Length);
            Array.Copy(newData, 0, combined, existing.Length, newData.Length);
            return combined;
        }

        /// <summary>
        /// Clears all vertex data arrays for all primitive types
        /// </summary>
        public void ClearAllData()
        {
            // Clear data for all VBOs
            foreach (var vbo in _vbosByType.Values)
            {
                vbo.VertexArray = null;
                vbo.ColorArray = null;
                vbo.NormalsArray = null;
                vbo.TextureUVArray = null;
                vbo.VertexCount = 0;

                // Clear GPU buffers by setting them to empty
                if (vbo.VAO != 0)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VertexVBO);
                    GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                    
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ColorVBO);
                    GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                    
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.NormalsVBO);
                    GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                    
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.TextureVBO);
                    GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                }
            }
        }

        /// <summary>
        /// Clears vertex data for a specific primitive type
        /// </summary>
        /// <param name="primitiveType">The primitive type to clear data for</param>
        public void ClearData(PrimitiveType primitiveType)
        {
            var vbo = GetVbo(primitiveType);
            vbo.VertexArray = null;
            vbo.ColorArray = null;
            vbo.NormalsArray = null;
            vbo.TextureUVArray = null;
            vbo.VertexCount = 0;

            // Clear GPU buffers
            if (vbo.VAO != 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VertexVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.ColorVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.NormalsVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.TextureVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.StaticDraw);
            }
        }

        /// <summary>
        /// Removes vertices at the specified indices
        /// </summary>
        /// <param name="indicesToRemove">Array of vertex indices to remove</param>
        public void RemoveVertices(int[] indicesToRemove)
        {
            if (indicesToRemove == null || indicesToRemove.Length == 0 || VertexArray == null)
                return;

            // Sort indices in descending order to avoid index shifting issues
            var sortedIndices = indicesToRemove.Distinct().OrderByDescending(x => x).ToArray();

            // Remove from each array for the current VBO
            var currentVbo = GetCurrentVbo();
            
            if (currentVbo.VertexArray != null)
                currentVbo.VertexArray = RemoveElementsFromArray(currentVbo.VertexArray, sortedIndices, 3);
                
            if (currentVbo.ColorArray != null)
                currentVbo.ColorArray = RemoveElementsFromArray(currentVbo.ColorArray, sortedIndices, 4);
                
            if (currentVbo.NormalsArray != null)
                currentVbo.NormalsArray = RemoveElementsFromArray(currentVbo.NormalsArray, sortedIndices, 3);
                
            if (currentVbo.TextureUVArray != null)
                currentVbo.TextureUVArray = RemoveElementsFromArray(currentVbo.TextureUVArray, sortedIndices, 2);

            // Update vertex count and refresh GPU buffers
            currentVbo.VertexCount = currentVbo.VertexArray?.Length / 3 ?? 0;
            
            // if (currentVbo.VertexArray != null) SetVertices(currentVbo.VertexArray);
            // if (currentVbo.ColorArray != null) SetColors(currentVbo.ColorArray);
            // if (currentVbo.NormalsArray != null) SetNormals(currentVbo.NormalsArray);
            // if (currentVbo.TextureUVArray != null) SetTextureUVs(currentVbo.TextureUVArray);
            
            SetupVertexAttributes();
        }

        /// <summary>
        /// Helper method to remove elements from an array based on vertex indices
        /// </summary>
        /// <param name="array">Source array</param>
        /// <param name="vertexIndices">Vertex indices to remove (sorted descending)</param>
        /// <param name="componentsPerVertex">Number of components per vertex</param>
        /// <returns>New array with elements removed</returns>
        private static float[] RemoveElementsFromArray(float[] array, int[] vertexIndices, int componentsPerVertex)
        {
            var result = new List<float>(array);
            
            foreach (int vertexIndex in vertexIndices)
            {
                int startIndex = vertexIndex * componentsPerVertex;
                if (startIndex >= 0 && startIndex < result.Count)
                {
                    // Remove all components for this vertex
                    for (int i = 0; i < componentsPerVertex && startIndex < result.Count; i++)
                    {
                        result.RemoveAt(startIndex);
                    }
                }
            }
            
            return result.ToArray();
        }

        /// <summary>
        /// Gets information about this VBO container
        /// </summary>
        /// <returns>String with VBO information</returns>
        public override string ToString()
        {
            return $"VBO[{Index}] - CurrentType: {CurrentPrimitiveType}, Vertices: {VertexCount}, " +
                   $"HasColors: {ColorArray != null}, HasNormals: {NormalsArray != null}, " +
                   $"HasUVs: {TextureUVArray != null}, " +
                   $"TriangleVerts: {TrianglesVbo.VertexCount}, LineVerts: {LinesVbo.VertexCount}, LineStripVerts: {LineStripVbo.VertexCount}";
        }

        /// <summary>
        /// Disposes of OpenGL resources
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VertexVBO);
            GL.DeleteBuffer(ColorVBO);
            GL.DeleteBuffer(NormalsVBO);
            GL.DeleteBuffer(TextureVBO);

            _disposed = true;
        }

        /// <summary>
        /// Finalizer to ensure OpenGL resources are cleaned up
        /// </summary>
        ~VboContainer()
        {
            // Note: In a real application, you should dispose on the main thread
            // This is just a safety net
            Dispose();
        }
    }

    /// <summary>
    /// Static manager class for VBO containers
    /// </summary>
    public static class VboManager
    {
        private static readonly Dictionary<int, VboContainer> _vbos = new();

        public static VboContainer? CurrentVBO;
        public static PrimitiveType primitive=PrimitiveType.Lines;




        /// <summary>
        /// Creates a new VBO container and adds it to the manager
        /// </summary>
        /// <param name="primitiveType">Type of primitives to draw</param>
        /// <returns>The created VBO container</returns>
        public static VboContainer CreateVbo(PrimitiveType primitiveType = PrimitiveType.Triangles)
        {
            var vbo = new VboContainer(primitiveType);
            _vbos[vbo.Index] = vbo;
            return vbo;
        }

        /// <summary>
        /// Gets a VBO container by its index
        /// </summary>
        /// <param name="index">The VBO index</param>
        /// <returns>The VBO container or null if not found</returns>
        public static VboContainer? GetVbo(int index)
        {
            return _vbos.TryGetValue(index, out var vbo) ? vbo : null;
        }

        /// <summary>
        /// Removes a VBO container from the manager and disposes it
        /// </summary>
        /// <param name="index">The VBO index to remove</param>
        /// <returns>True if the VBO was found and removed</returns>
        public static bool RemoveVbo(int index)
        {
            if (_vbos.TryGetValue(index, out var vbo))
            {
                vbo.Dispose();
                _vbos.Remove(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all VBO containers
        /// </summary>
        /// <returns>Collection of all VBO containers</returns>
        public static IEnumerable<VboContainer> GetAllVbos()
        {
            return _vbos.Values;
        }

        /// <summary>
        /// Clears all VBO containers and disposes them
        /// </summary>
        public static void ClearAll()
        {
            foreach (var vbo in _vbos.Values)
            {
                vbo.Dispose();
            }
            _vbos.Clear();
        }
    }
// }

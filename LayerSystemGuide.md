# Layer Visibility System Usage Guide

The Layer Visibility System in Gaucho allows you to control which layers are rendered using GPU-based filtering.

## Key Features

- **GPU-based filtering**: Uses shader arrays to filter vertices by layer ID
- **Real-time toggling**: Instant visibility changes without geometry regeneration  
- **Scalable**: Supports up to 256 layers
- **User-friendly**: Layer panel with checkboxes and bulk controls

## Usage Examples

### 1. Creating and Registering Layers

```csharp
// Create layers
var constructionLayer = new Layer { Name = "Construction", Visible = true, Colour = 8 };
var dimensionLayer = new Layer { Name = "Dimensions", Visible = true, Colour = 2 };
var textLayer = new Layer { Name = "Text", Visible = false, Colour = 3 };

// Register layers to get IDs (do this after OpenGL is initialized)
int constructionLayerId = LayerManager.RegisterLayer(constructionLayer);
int dimensionLayerId = LayerManager.RegisterLayer(dimensionLayer);  
int textLayerId = LayerManager.RegisterLayer(textLayer);
```

### 2. Creating Geometry with Layer Associations

```csharp
var vboContainer = new VboContainer();

// Example vertices for a line (two points)
double[] lineVertices = { 0.0, 0.0, 0.0, 1.0, 1.0, 0.0 }; // 2 vertices * 3 coords
int[] layerIds = LayerManager.CreateLayerIdArray(constructionLayer, 2); // 2 vertices

// Add geometry with layer information
vboContainer.AppendVertexData(lineVertices, layerIds: layerIds, SetToGPU: true);
```

### 3. Controlling Layer Visibility

```csharp
// By name
LayerManager.SetLayerVisibility("Construction", true);   // Show construction layer
LayerManager.SetLayerVisibility("Text", false);          // Hide text layer

// By ID
LayerManager.SetLayerVisibility(dimensionLayerId, true); // Show dimensions by ID
```

### 4. Rendering with Layer Filtering

```csharp
void RenderWithLayers(Shader shader, VboContainer vbo)
{
    shader.Use();
    
    // Set layer visibility data to shader
    var layerVisibility = LayerManager.GetLayerVisibilityArray();
    shader.SetBoolArray("uLayerVisible", layerVisibility);
    shader.SetInt("uMaxLayers", LayerManager.MAX_LAYERS);
    
    // Set other uniforms (matrices, colors, etc.)
    shader.SetMatrix4("uProjection", projectionMatrix);
    shader.SetVector4("uColor", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
    
    // Render - vertices will be filtered by layer visibility in shader
    vbo.Render();
}
```

## Layer Panel UI

The layer panel provides:
- Individual layer visibility checkboxes
- Show All / Hide All buttons  
- Refresh button to update visibility array
- Automatic redraw triggering when visibility changes

## Shader Implementation

The system uses:
- **Vertex Shader**: Passes layer ID from vertex attribute to fragment shader
- **Fragment Shader**: Uses `uLayerVisible[]` array to discard hidden layer fragments
- **Layer ID Attribute**: Location 4 vertex attribute for per-vertex layer assignment

## Important Notes

- **Timing**: Always register layers AFTER OpenGL is initialized (in or after `Realize()`)
- **Performance**: GPU filtering is more efficient than CPU-side culling
- **Limits**: Currently supports up to 256 layers (configurable in `LayerManager.MAX_LAYERS`)
- **Thread Safety**: LayerManager methods are static and should be called from the UI thread
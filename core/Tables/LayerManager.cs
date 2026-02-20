using System;
using System.Collections.Generic;
using System.Linq;

namespace Gaucho
{
    /// <summary>
    /// Manages layer visibility state and provides arrays for shader usage
    /// </summary>
    public static class LayerManager
    {
        private static readonly Dictionary<int, Layer> _layers = new Dictionary<int, Layer>();
        private static readonly Dictionary<string, int> _layerNameToId = new Dictionary<string, int>();
        private static int _nextLayerId = 0;
        
        // Cached arrays for shader usage
        private static bool[] _layerVisibilityArray = new bool[256];
        private static bool _needsUpdate = true;
        
        public const int MAX_LAYERS = 256;

        /// <summary>
        /// Registers a layer and assigns it a unique ID
        /// </summary>
        /// <param name="layer">The layer to register</param>
        /// <returns>The assigned layer ID</returns>
        public static int RegisterLayer(Layer layer)
        {
            if (layer == null) throw new ArgumentNullException(nameof(layer));
            
            // Check if layer already exists by name
            if (_layerNameToId.ContainsKey(layer.Name))
            {
                return _layerNameToId[layer.Name];
            }
            
            int layerId = _nextLayerId++;
            if (layerId >= MAX_LAYERS)
            {
                throw new InvalidOperationException($"Maximum number of layers ({MAX_LAYERS}) exceeded");
            }
            
            _layers[layerId] = layer;
            _layerNameToId[layer.Name] = layerId;
            _needsUpdate = true;
            
            Console.WriteLine($"Registered layer '{layer.Name}' with ID {layerId}");
            return layerId;
        }
        /// <summary>
        /// Gets the layer ID for a given layer name
        /// </summary>
        /// <param name="layerName">Name of the layer</param>
        /// <returns>Layer ID, or -1 if not found</returns>
        public static int GetLayerId(string layerName)
        {
            return _layerNameToId.TryGetValue(layerName, out int id) ? id : -1;
        }

        /// <summary>
        /// Gets the layer ID for a given layer object
        /// </summary>
        /// <param name="layer">The layer object</param>
        /// <returns>Layer ID, or -1 if not found</returns>
        public static int GetLayerId(Layer layer)
        {
            if (layer == null) return -1;
            return GetLayerId(layer.Name);
        }

        /// <summary>
        /// Gets the layer object by ID
        /// </summary>
        /// <param name="layerId">Layer ID</param>
        /// <returns>Layer object, or null if not found</returns>
        public static Layer? GetLayer(int layerId)
        {
            return _layers.TryGetValue(layerId, out Layer? layer) ? layer : null;
        }

        /// <summary>
        /// Sets the visibility of a layer
        /// </summary>
        /// <param name="layerId">Layer ID</param>
        /// <param name="visible">Visibility state</param>
        public static void SetLayerVisibility(int layerId, bool visible)
        {
            if (_layers.TryGetValue(layerId, out Layer? layer))
            {
                layer.Visible = visible;
                _needsUpdate = true;
                Console.WriteLine($"Layer {layerId} ({layer.Name}) visibility set to {visible}");
            }
        }

        /// <summary>
        /// Sets the visibility of a layer by name
        /// </summary>
        /// <param name="layerName">Layer name</param>
        /// <param name="visible">Visibility state</param>
        public static void SetLayerVisibility(string layerName, bool visible)
        {
            int layerId = GetLayerId(layerName);
            if (layerId >= 0)
            {
                SetLayerVisibility(layerId, visible);
            }
        }

        /// <summary>
        /// Gets the visibility array for shader usage
        /// </summary>
        /// <returns>Boolean array indicating visibility for each layer</returns>
        public static bool[] GetLayerVisibilityArray()
        {
            if (_needsUpdate)
            {
                UpdateVisibilityArray();
                _needsUpdate = false;
            }
            return _layerVisibilityArray;
        }

        /// <summary>
        /// Forces an update of the visibility array
        /// </summary>
        public static void UpdateVisibilityArray()
        {
            // Initialize all layers as invisible
            Array.Fill(_layerVisibilityArray, false);
            
            // Set visibility for registered layers
            foreach (var kvp in _layers)
            {
                int layerId = kvp.Key;
                Layer layer = kvp.Value;
                
                if (layerId < MAX_LAYERS)
                {
                    // Layer is visible if it's marked as visible, not frozen, and not hidden
                    _layerVisibilityArray[layerId] = layer.Visible && !layer.Frozen && !layer.Hidden;
                }
            }
        }

        /// <summary>
        /// Gets all registered layers
        /// </summary>
        /// <returns>Dictionary of layer ID to layer objects</returns>
        public static Dictionary<int, Layer> GetAllLayers()
        {
            return new Dictionary<int, Layer>(_layers);
        }

        /// <summary>
        /// Clears all registered layers
        /// </summary>
        public static void ClearAllLayers()
        {
            _layers.Clear();
            _layerNameToId.Clear();
            _nextLayerId = 0;
            Array.Fill(_layerVisibilityArray, false);
            _needsUpdate = true;
        }

        /// <summary>
        /// Gets the count of registered layers
        /// </summary>
        public static int LayerCount => _layers.Count;

        /// <summary>
        /// Creates a layer ID array for vertices belonging to a specific layer
        /// </summary>
        /// <param name="layer">The layer</param>
        /// <param name="vertexCount">Number of vertices</param>
        /// <returns>Array filled with the layer ID</returns>
        public static int[] CreateLayerIdArray(Layer layer, int vertexCount)
        {
            int layerId = GetLayerId(layer);
            if (layerId < 0)
            {
                layerId = RegisterLayer(layer);
            }
            
            var layerIds = new int[vertexCount];
            Array.Fill(layerIds, layerId);
            return layerIds;
        }

        /// <summary>
        /// Creates a layer ID array for vertices belonging to a specific layer by name
        /// </summary>
        /// <param name="layerName">The layer name</param>
        /// <param name="vertexCount">Number of vertices</param>
        /// <returns>Array filled with the layer ID</returns>
        public static int[] CreateLayerIdArray(string layerName, int vertexCount)
        {
            int layerId = GetLayerId(layerName);
            if (layerId < 0)
            {
                throw new ArgumentException($"Layer '{layerName}' not found. Register it first.");
            }
            
            var layerIds = new int[vertexCount];
            Array.Fill(layerIds, layerId);
            return layerIds;
        }
    }
}
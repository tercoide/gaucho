#version 330 core
flat in int vLayerId;
out vec4 FragColor;

uniform vec4 uColor = vec4(1.0, 0.0, 0.0, 1.0);
uniform int uLayerVisible[256]; // Support up to 256 layers (1 = visible, 0 = hidden)
uniform int uMaxLayers = 256;

void main()
{
    // Check if the layer ID is valid and if the layer is visible
    if (vLayerId >= 0 && vLayerId < uMaxLayers && uLayerVisible[vLayerId] == 1) {
        FragColor = uColor;
    } else {
        discard; // Don't render this fragment
    }
};
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 4) in int aLayerId;

uniform mat4 uModel;
uniform mat4 uView; 
uniform mat4 uProjection;

flat out int vLayerId;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(aPosition, 1.0);
    vLayerId = aLayerId;
};
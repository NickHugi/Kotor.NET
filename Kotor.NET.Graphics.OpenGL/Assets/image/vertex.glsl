#version 300 es
precision highp float;

layout (location = 0) in vec2 inPosition;
layout (location = 2) in vec2 inUV;

uniform vec2 uPosition;
uniform vec2 uSize;
uniform vec2 uScreenSize;

out vec2 uv;

void main()
{
    uv = inUV;
    
    vec2 pos = uPosition + (inPosition * uSize);
    
    vec2 ndc = (pos / uScreenSize) * 2.0 - 1.0;
    ndc.y = -ndc.y;
    
    gl_Position = vec4(ndc, 0.0, 1.0);
}

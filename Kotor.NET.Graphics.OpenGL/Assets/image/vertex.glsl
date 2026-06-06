#version 300 es
precision highp float;

layout (location = 0) in vec2 aPosition;
layout (location = 2) in vec2 aUV;

uniform vec2 uPosition;
uniform vec2 uSize;
uniform vec2 uViewport;

out vec2 vUV;

void main()
{
    vUV = aUV;
    
    vec2 pos = uPosition + (aPosition * uSize);
    
    vec2 ndc = (pos / uViewport) * 2.0 - 1.0;
    ndc.y = -ndc.y;
    
    gl_Position = vec4(ndc, 0.0, 1.0);
}

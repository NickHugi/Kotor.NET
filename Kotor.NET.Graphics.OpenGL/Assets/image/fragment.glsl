#version 300 es
precision highp float;

in vec2 vUV;
out vec4 FragColor;

uniform sampler2D uTexture;

void main()
{
    vec4 diffuseColor = texture(uTexture, vUV);
    FragColor = diffuseColor;
} 

#version 300 es
precision highp float;

in vec2 vTexCoord1;
in vec2 vTexCoord2;

out vec4 FragColor;

uniform sampler2D uTexture1;
uniform sampler2D uTexture2;
uniform vec3 uDffuse;
uniform vec3 uAmbient;


void main()
{
    FragColor = vec4(uAmbient, 1.0f) * texture(uTexture1, vTexCoord1);
} 

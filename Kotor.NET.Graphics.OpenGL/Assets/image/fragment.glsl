#version 300 es
precision highp float;

in vec2 uv;
out vec4 FragColor;

uniform sampler2D texture;
void main()
{
    vec4 diffuseColor = texture(texture, uv);
    FragColor = diffuseColor;
} 

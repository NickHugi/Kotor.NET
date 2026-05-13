#version 300 es
precision highp float;

in vec2 uv;
out vec4 FragColor;

uniform sampler2D texture1;
void main()
{
    vec4 diffuseColor = texture(texture1, uv);
    FragColor = diffuseColor;
} 

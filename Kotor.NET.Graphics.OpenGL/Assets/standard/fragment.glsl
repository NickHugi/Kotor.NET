#version 300 es
precision highp float;

in vec2 texCoord1;
in vec2 texCoord2;

out vec4 FragColor;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform vec3 diffuse;
uniform vec3 ambient;


void main()
{
    FragColor = vec4(texCoord1, 0.0f, 0.0f) + vec4(ambient, 1.0f) * texture(texture1, texCoord1);
    //FragColor = vec4(ambient, 1.0f) * texture(texture1, texCoord1);
} 

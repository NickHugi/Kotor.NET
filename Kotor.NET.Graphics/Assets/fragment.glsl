#version 300 es
precision highp float;

in vec2 texCoord1;
in vec2 texCoord2;

out vec4 FragColor;

uniform sampler2D texSampler1;
uniform sampler2D texSampler2;

void main()
{
    //vec4 diffuseColor = texture(texSampler1, texCoord1);

    FragColor = vec4(1.0f, 0.0f, 0.0f, 1.0f);
} 

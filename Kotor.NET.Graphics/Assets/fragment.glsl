#version 300 es
precision highp float;

in vec2 texCoord1;
in vec2 texCoord2;

out vec4 FragColor;

uniform sampler2D texture1;
//uniform sampler2D texture2;

void main()
{
    vec4 diffuseColor = texture(texture1, texCoord1);
    FragColor = diffuseColor; //mix(diffuseColor, vec4(1.0f, 1.0f, 1.0f, 1.0f), 0.5);

    //FragColor = vec4(texCoord1.x, texCoord1.y, 0.0f, 1.0f);
} 

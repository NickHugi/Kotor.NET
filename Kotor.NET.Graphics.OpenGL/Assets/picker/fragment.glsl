#version 300 es
precision highp float;

uniform uint entityID;

out vec4 FragColor;

vec4 intToColor(uint v)
{
    float r = float((v >> 24) & 0xFFu) / 255.0f;
    float g = float((v >> 16) & 0xFFu) / 255.0f;
    float b = float((v >> 8)  & 0xFFu) / 255.0f;
    float a = float((v)       & 0xFFu) / 255.0f;

    return vec4(r, g, b, a);
}

void main()
{
    FragColor = intToColor(entityID);
}

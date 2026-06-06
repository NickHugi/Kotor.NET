#version 300 es
precision highp float;

in vec2 TexCoord;

uniform sampler2D uTexture;

out vec4 FragColor;

void main()
{
    vec4 color = texture(uTexture, TexCoord);

    if (color.a < 0.01)
        discard;

    FragColor = color;
}

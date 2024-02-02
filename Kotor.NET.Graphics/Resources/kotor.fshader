#version 300 es
precision highp float;

in vec2 diffuse_uv;
in vec2 lightmap_uv;
in vec4 pos;
in vec4 color;

out vec4 FragColor;

uniform sampler2D diffuse;
uniform sampler2D lightmap;
uniform int enableLightmap;

void main()
{
    //vec4 diffuseColor = texture(diffuse, diffuse_uv);
    //vec4 lightmapColor = texture(lightmap, lightmap_uv);

    //if (enableLightmap == 1) {
    //    FragColor = mix(diffuseColor, lightmapColor, 0.5);
    //} else {
    //    FragColor = diffuseColor;
    //}

    FragColor = color;
}

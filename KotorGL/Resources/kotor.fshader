#version 420
in vec2 diffuse_uv;
in vec2 lightmap_uv;

out vec4 FragColor;

layout(binding = 0) uniform sampler2D diffuse;
layout(binding = 1) uniform sampler2D lightmap;
uniform int enableLightmap;

void main()
{
    vec4 diffuseColor = texture(diffuse, diffuse_uv);
    vec4 lightmapColor = texture(lightmap, lightmap_uv);

    if (enableLightmap == 1) {
        FragColor = mix(diffuseColor, lightmapColor, 0.5);
    } else {
        FragColor = diffuseColor;
    }
}

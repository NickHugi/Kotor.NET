#version 300 es
precision highp float;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord1;
layout (location = 3) in vec2 aTexCoord2;

out vec2 texCoord1;
out vec2 texCoord2;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = projection * view * model *  vec4(aPosition, 1.0);
    texCoord1 = vec2(aTexCoord1.x, aTexCoord1.y);
    texCoord2 = vec2(aTexCoord2.x, aTexCoord2.y);
}

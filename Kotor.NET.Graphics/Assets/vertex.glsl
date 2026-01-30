#version 300 es
precision highp float;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord1;
layout (location = 3) in vec2 aTexCoord2;
layout (location = 4) in vec4 boneIds; 
layout (location = 5) in vec4 weights;

out vec2 texCoord1;
out vec2 texCoord2;

uniform mat4 entity;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

const int MAX_BONES = 16;
const int MAX_BONE_INFLUENCE = 4;
uniform mat4 finalBonesMatrices[MAX_BONES];

void main()
{
    vec4 totalPosition = vec4(0.0f);
    for(int i = 0 ; i < MAX_BONE_INFLUENCE; i++)
    {
        if(boneIds[i] == -1.0f) 
            continue;
        if(int(boneIds[i]) >= MAX_BONES) 
        {
            totalPosition = vec4(aPosition, 1.0f);
            break;
        }
        vec4 localPosition = finalBonesMatrices[int(boneIds[i])] * vec4(aPosition,1.0f);
        totalPosition += localPosition * weights[i];
    }

    gl_Position = projection * view * entity * model * vec4(aPosition, 1.0f);

    texCoord1 = vec2(aTexCoord1.x, aTexCoord1.y);
    texCoord2 = vec2(aTexCoord2.x, aTexCoord2.y);
}

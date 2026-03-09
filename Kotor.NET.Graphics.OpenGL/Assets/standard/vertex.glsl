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
out float bug;

uniform mat4 entity;
uniform mat4 mesh;
uniform mat4 view;
uniform mat4 projection;

const int MAX_BONES = 16;
const int MAX_BONE_INFLUENCE = 4;
uniform mat4 finalBonesMatrices[MAX_BONES];

void main()
{
    if (boneIds[0] >= 0.0f && boneIds[0] <= 16.0f)
    {
        vec4 localPos = vec4(aPosition, 1.0f);
        vec4 skinnedPosition = vec4(0.0f);

        for (int i = 0; i < MAX_BONE_INFLUENCE; i++)
        {
            int boneID = int(boneIds[i]);

            if (boneID < 0)
                continue;
            if (boneID >= MAX_BONES)
                continue;
            if (weights[i] == 0.0)
                continue;
                
            skinnedPosition += weights[i] * (finalBonesMatrices[boneID] * mesh * localPos);
        }

        gl_Position = projection * view * skinnedPosition;
    }
    else
    {
        gl_Position = projection * view * entity * mesh * vec4(aPosition, 1.0f);
    }

    texCoord1 = vec2(aTexCoord1.x, aTexCoord1.y);
    texCoord2 = vec2(aTexCoord2.x, aTexCoord2.y);
}

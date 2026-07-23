#version 300 es
precision highp float;

layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord1;
layout (location = 3) in vec2 aTexCoord2;
layout (location = 4) in vec4 aBoneIDs; 
layout (location = 5) in vec4 aWeights;

out vec3 vNormal;
out vec2 vTexCoord1;
out vec2 vTexCoord2;

uniform mat4 uEntity;
uniform mat4 uMesh;
uniform mat4 uView;
uniform mat4 uProjection;

const int MAX_BONES = 16;
const int MAX_BONE_INFLUENCE = 4;
uniform mat4 uFinalBonesMatrices[MAX_BONES];

void main()
{
    vNormal = mat3(transpose(inverse(uEntity * uMesh))) * aNormal;  
    
    if (aBoneIDs[0] >= 0.0f && aBoneIDs[0] <= 16.0f)
    {
        vec4 localPos = vec4(aPosition, 1.0f);
        vec4 skinnedPosition = vec4(0.0f);

        for (int i = 0; i < MAX_BONE_INFLUENCE; i++)
        {
            int boneID = int(aBoneIDs[i]);

            if (boneID < 0)
                continue;
            if (boneID >= MAX_BONES)
                continue;
            if (aWeights[i] == 0.0)
                continue;
                
            skinnedPosition += aWeights[i] * (uFinalBonesMatrices[boneID] * uMesh * localPos);
        }

        gl_Position = uProjection * uView * skinnedPosition;
    }
    else
    {
        gl_Position = uProjection * uView * uEntity * uMesh * vec4(aPosition, 1.0f);
    }

    vTexCoord1 = vec2(aTexCoord1.x, aTexCoord1.y);
    vTexCoord2 = vec2(aTexCoord2.x, aTexCoord2.y);
}

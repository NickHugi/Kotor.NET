#version 300 es
precision highp float;

layout (location = 0) in vec2 aCorner;

uniform mat4 uView;
uniform mat4 uProjection;
uniform vec3 uPosition;
uniform float uSize;
uniform bool uFixedSize;
uniform vec2 uViewport;

out vec2 TexCoord;

void main()
{
    TexCoord = aCorner + vec2(0.5);

    vec3 cameraRight = vec3(uView[0][0], uView[1][0], uView[2][0]);
    vec3 cameraUp = vec3(uView[0][1], uView[1][1], uView[2][1]);

    if (!uFixedSize)
    {
        vec3 worldPosition = uPosition + (cameraRight * aCorner.x * uSize) + (cameraUp * aCorner.y) * uSize;

        gl_Position = uProjection * uView * vec4(worldPosition, 1.0);
    }
    else
    {
        vec4 clipCenter = uProjection * uView * vec4(uPosition, 1.0);
        vec2 ndcOffset = (aCorner * uSize * 2.0) / uViewport;

        gl_Position = clipCenter;
        gl_Position.xy += ndcOffset * clipCenter.w;
    }
}

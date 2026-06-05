#version 300 es
precision highp float;

layout(location=0) in vec3 inQuad;

layout(location=1) in vec3 inStart;
layout(location=2) in vec3 inEnd;
layout(location=3) in vec4 inColor;
layout(location=4) in float inThickness;

uniform mat4 uView;
uniform mat4 uProjection;
uniform vec2 uViewport;

out vec4 outColor;

void main()
{
    vec4 clipStart = uProjection * uView * vec4(inStart, 1.0);
    vec4 clipEnd   = uProjection * uView * vec4(inEnd, 1.0);

    float t = inQuad.x;

    vec4 clipPos = mix(clipStart, clipEnd, t);

    vec2 ndcStart = clipStart.xy / clipStart.w;
    vec2 ndcEnd   = clipEnd.xy / clipEnd.w;

    vec2 dir = normalize(ndcEnd - ndcStart);
    vec2 normal = vec2(-dir.y, dir.x);

    float pixelThickness = inThickness;

    vec2 offset = normal * inQuad.y * pixelThickness;
    offset *= 2.0 / uViewport.y; // pixel -> NDC
    clipPos.xy += offset * clipPos.w;

    gl_Position = clipPos;

    outColor = inColor;
}

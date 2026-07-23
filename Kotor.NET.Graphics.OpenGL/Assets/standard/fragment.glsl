#version 300 es
precision highp float;

in vec3 vNormal;
in vec2 vTexCoord1;
in vec2 vTexCoord2;

out vec4 FragColor;

uniform sampler2D uTexture1;
uniform sampler2D uTexture2;
uniform vec3 uDffuse;
uniform vec3 uAmbient;


void main()
{
    vec3 norm = normalize(vNormal);

    vec3 lightDir = normalize(vec3(0.25f, 0.25f, 0.75f));
	float lightDot = max(dot(norm, lightDir), 0.0f);
    float lightBrightness = (lightDot * 0.3f) + 0.7f;

    vec3 diffuse =
		 lightBrightness
		 * uAmbient
         * texture(uTexture1, vTexCoord1).rgb;

	FragColor = vec4(diffuse, 1.0f);
} 

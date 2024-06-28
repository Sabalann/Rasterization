#version 330 core

in vec2 UV;
in vec3 Normal;
in vec3 FragPos;

uniform sampler2D diffuseTexture;
uniform vec3 ambientColor;
uniform vec3 lightPositions[4]; // assuming you have an array of light positions
uniform vec3 lightColors[4];
uniform float lightIntensities[4];
uniform int numLights;
uniform vec3 viewPos;
uniform float shininess; // Ensure shininess is declared here

out vec4 color;

void main()
{
    // Ambient
    vec3 ambient = ambientColor;
    
    // Diffuse
    vec3 norm = normalize(Normal);
    vec3 diffuse;
    for (int i = 0; i < numLights; i++){
        vec3 lightDir = normalize(lightPositions[i] - FragPos);
        float diff = max(dot(norm, lightDir), 0.0);
        diffuse += diff * lightColors[i] * lightIntensities[i];
    }

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 specular;
    for (int i = 0; i < numLights; i++) {
        vec3 lightDir = normalize(lightPositions[i] - FragPos);
        vec3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess); // Use shininess here
        specular += spec * lightColors[i] * lightIntensities[i];
    }

    // Combine results
    vec3 result = (ambient + diffuse + specular) * texture(diffuseTexture, UV).rgb;
    color = vec4(result, 1.0);
}

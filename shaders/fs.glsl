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
    vec3 lightDir = normalize(lightPositions[0] - FragPos); // Using first light for simplicity
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColors[0] * lightIntensities[0];

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess); // Use shininess here
    vec3 specular = spec * lightColors[0] * lightIntensities[0];

    // Combine results
    vec3 result = (ambient + diffuse + specular) * texture(diffuseTexture, UV).rgb;
    color = vec4(result, 1.0);
}

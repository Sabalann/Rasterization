#version 330
 
// Shader inputs
in vec4 positionWorld;  // fragment position in World Space
in vec4 normalWorld;    // fragment normal in World Space
in vec2 uv;             // fragment uv texture coordinates

// Texture sampler
uniform sampler2D diffuseTexture;

// Outputs to framebuffer
out vec4 FragColor;

// Uniforms for ambient light
uniform vec3 ambientColor;

// Uniforms for multiple light sources
uniform vec3 lightPositions[4];
uniform vec3 lightColors[4];
uniform float lightIntensities[4];
uniform int numLights;

// Uniform for the view position
uniform vec3 viewPos;

void main()
{
    // Ambient component
    vec3 ambient = ambientColor;

    // Initialize final color
    vec3 finalColor = ambient;

    // Convert inputs to vec3
    vec3 FragPos = vec3(positionWorld);
    vec3 Normal = normalize(vec3(normalWorld));

    // Loop through all lights
    for(int i = 0; i < numLights; i++)
    {
        // Calculate diffuse component
        vec3 lightDir = normalize(lightPositions[i] - FragPos);
        float diff = max(dot(Normal, lightDir), 0.0);
        vec3 diffuse = diff * lightColors[i];

        // Calculate specular component
        float specularStrength = 0.5;
        vec3 viewDir = normalize(viewPos - FragPos);
        vec3 reflectDir = reflect(-lightDir, Normal);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
        vec3 specular = specularStrength * spec * lightColors[i];

        // Accumulate contributions
        finalColor += lightIntensities[i] * (diffuse + specular);
    }

    // Sample the texture and combine with the final color
    vec4 texColor = texture(diffuseTexture, uv);
    finalColor *= texColor.rgb;

    FragColor = vec4(finalColor, texColor.a);
}
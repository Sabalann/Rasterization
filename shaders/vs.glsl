#version 330 core

layout(location = 0) in vec3 vertexPositionObject;
layout(location = 1) in vec2 vertexUV;
layout(location = 2) in vec3 vertexNormalObject;

uniform mat4 objectToScreen;
uniform mat4 objectToWorld;

out vec2 UV;
out vec3 Normal;
out vec3 FragPos;

void main()
{
    gl_Position = objectToScreen * vec4(vertexPositionObject, 1.0);
    UV = vertexUV;
    FragPos = vec3(objectToWorld * vec4(vertexPositionObject, 1.0));
    Normal = mat3(transpose(inverse(objectToWorld))) * vertexNormalObject;
}

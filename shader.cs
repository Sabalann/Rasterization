﻿using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Template
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int in_vertexPositionObject;
        public int in_vertexNormalObject;
        public int in_vertexUV;
        public int uniform_objectToScreen;
        public int uniform_objectToWorld;

        public int uniform_ambientColor;
        public int uniform_lightPositions;
        public int uniform_lightColors;
        public int uniform_lightIntensities;
        public int uniform_numLights;
        public int uniform_viewPos;
        public int uniform_diffuseTexture;

        // constructor
        public Shader(string vertexShader, string fragmentShader)
        {
            // compile shaders
            programID = GL.CreateProgram();
            if (!OpenTKApp.isMac) GL.ObjectLabel(ObjectLabelIdentifier.Program, programID, -1, vertexShader + " + " + fragmentShader);
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            string infoLog = GL.GetProgramInfoLog(programID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);

            // get locations of shader parameters
            in_vertexPositionObject = GL.GetAttribLocation(programID, "vertexPositionObject");
            in_vertexNormalObject = GL.GetAttribLocation(programID, "vertexNormalObject");
            in_vertexUV = GL.GetAttribLocation(programID, "vertexUV");
            uniform_objectToScreen = GL.GetUniformLocation(programID, "objectToScreen");
            uniform_objectToWorld = GL.GetUniformLocation(programID, "objectToWorld");

            uniform_ambientColor = GL.GetUniformLocation(programID, "ambientColor");
            uniform_lightPositions = GL.GetUniformLocation(programID, "lightPositions");
            uniform_lightColors = GL.GetUniformLocation(programID, "lightColors");
            uniform_lightIntensities = GL.GetUniformLocation(programID, "lightIntensities");
            uniform_numLights = GL.GetUniformLocation(programID, "numLights");
            uniform_viewPos = GL.GetUniformLocation(programID, "viewPos");
            uniform_diffuseTexture = GL.GetUniformLocation(programID, "diffuseTexture");
        }

        // loading shaders
        void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            ID = GL.CreateShader(type);
            if (!OpenTKApp.isMac) GL.ObjectLabel(ObjectLabelIdentifier.Shader, ID, -1, filename);
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            string infoLog = GL.GetShaderInfoLog(ID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);
        }

        public void SetNumLights(int numLights)
        {
            GL.UseProgram(programID);
            GL.Uniform1(uniform_numLights, numLights);
        }

        public void SetAmbientColor(Vector3 color)
        {
            GL.UseProgram(programID);
            GL.Uniform3(uniform_ambientColor, ref color);
        }

        public void SetLight(int index, Vector3 position, Vector3 color, float intensity)
        {
            int uniform_lightPositionElement = GL.GetUniformLocation(programID, $"lightPositions[{index}]");
            int uniform_lightColorElement = GL.GetUniformLocation(programID, $"lightColors[{index}]");
            int uniform_lightIntensityElement = GL.GetUniformLocation(programID, $"lightIntensities[{index}]");
            GL.UseProgram(programID);
            GL.Uniform3(uniform_lightPositionElement, ref position);
            GL.Uniform3(uniform_lightColorElement, ref color);
            GL.Uniform1(uniform_lightIntensityElement, intensity);
        }
        public void SetViewPos(Vector3 viewPos)
        {
            GL.UseProgram(programID);
            GL.Uniform3(uniform_viewPos, ref viewPos);
        }

        public void SetDiffuseTexture(int textureUnit)
        {
            GL.UseProgram(programID);
            GL.Uniform1(uniform_diffuseTexture, textureUnit);
        }
        public void SetFloat(string name, float value)
        {
            GL.UseProgram(programID);
            GL.Uniform1(GL.GetUniformLocation(programID, name), value);
        }


    }
}

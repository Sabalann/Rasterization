using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;

namespace INFOGR2024TemplateP2
{
    internal class SceneGraphNode
    {
        public Mesh? Mesh;
        public Matrix4 LocalTransform;
        public List<SceneGraphNode> Children;
        public List<Light> Lights { get; set; }
        public float Shininess { get; set; }

        public SceneGraphNode(Mesh mesh)
        {
            Children = new List<SceneGraphNode>();
            LocalTransform = Matrix4.Identity;
            Mesh = mesh;
            Lights = new List<Light>();
            Shininess = 32.0f;
            LocalTransform = Matrix4.Identity;
            Children = new List<SceneGraphNode>();
        }

        public void AddChild(SceneGraphNode child)
        {
            Children.Add(child);
        }

        public void Render(Texture texture, Shader shader, Matrix4 parentTransform)
        {
            Matrix4 globalTransform = LocalTransform * parentTransform;

            shader.SetFloat("shininess", Shininess);

            if (Mesh != null)
            {
                Mesh.Render(shader, globalTransform, LocalTransform, texture);
            }

            foreach (var child in Children)
            {
                child.Render(texture, shader, globalTransform);
            }

        }





        public List<Light> CollectLights()
        {
            List<Light> lights = new List<Light>(Lights);
            foreach (var child in Children)
            {
                lights.AddRange(child.CollectLights());
            }
            return lights;
        }
        public void AddLight(Light light)
        {
            Lights.Add(light);
        }
    }
}

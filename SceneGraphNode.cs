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
        public Mesh Mesh;
        public Matrix4 LocalTransform;
        public List<SceneGraphNode> Children;

        public SceneGraphNode(Mesh mesh)
        {
            Mesh = mesh;
            LocalTransform = Matrix4.Identity;
            Children = new List<SceneGraphNode>();
        }

        public void AddChild(SceneGraphNode child)
        {
            Children.Add(child);
        }

        public void Render(Shader shader, Matrix4 parentTransform, Texture texture)
        {
            Matrix4 globalTransform = LocalTransform * parentTransform;
            if (Mesh != null)
            {
                Mesh.Render(shader, globalTransform, LocalTransform, texture);
            }

            foreach (var child in Children)
            {
                child.Render(shader, globalTransform, texture);
            }
        }
    }
}

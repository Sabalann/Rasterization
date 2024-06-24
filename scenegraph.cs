using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template;

namespace INFOGR2024TemplateP2
{
    class SceneGraph
    {
        public SceneGraphNode Root;

        public SceneGraph()
        {
            Root = new SceneGraphNode(null); // Root node with no mesh
        }

        public void Render(Shader shader, Matrix4 cameraMatrix, Texture texture)
        {
            Root.Render(shader, cameraMatrix, texture);
        }
    }
}

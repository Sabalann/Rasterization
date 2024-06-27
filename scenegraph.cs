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
            Root = new SceneGraphNode(null);
        }

        public void Render(Texture texture, Shader shader, Matrix4 cameraMatrix)
        {
            Root.Render(texture, shader, cameraMatrix);
        }

    }
}

using System.Diagnostics;
using INFOGR2024TemplateP2;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh? teapot, teapotChild, floor;                    // meshes to draw using OpenGL
        float a = 0;                            // teapot rotation angle
        readonly Stopwatch timer = new();       // timer for measuring frame duration
        Shader? shader;                         // shader to use for rendering
        Shader? postproc;                       // shader to use for post processing
        Texture? wood;                          // texture to use for rendering
        RenderTarget? target;                   // intermediate render target
        ScreenQuad? quad;                       // screen filling quad for post processing
        readonly bool useRenderTarget = true;   // required for post processing

        SceneGraph sceneGraph;

        // constructor
        public MyApplication(Surface screen)
        {
            this.screen = screen;
            sceneGraph = new SceneGraph();
        }
        // initialize
        public void Init()
        {
            // load teapot
            teapot = new Mesh("../../../assets/teapot.obj");
            teapotChild = new Mesh("../../../assets/teapot.obj");
            floor = new Mesh("../../../assets/floor.obj");
            // initialize stopwatch
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../../shaders/vs.glsl", "../../../shaders/fs.glsl");
            postproc = new Shader("../../../shaders/vs_post.glsl", "../../../shaders/fs_post.glsl");
            // load a texture
            wood = new Texture("../../../assets/wood.jpg");
            // create the render target
            if (useRenderTarget) target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            // build scene graph
            var rootNode = new SceneGraphNode(null);
            var teapotNode = new SceneGraphNode(teapot);
            var teapotChildNode = new SceneGraphNode(teapotChild);
            var floorNode = new SceneGraphNode(floor);

            float angle90degrees = MathF.PI / 2;
            teapotNode.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            teapotChildNode.LocalTransform = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(new Vector3(10, 0, 2)) * Matrix4.CreateFromAxisAngle(new Vector3(3, 2, 0), a) ;
            floorNode.LocalTransform = Matrix4.CreateScale(4.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);

            rootNode.AddChild(teapotNode);
            teapotNode.AddChild(teapotChildNode);
            rootNode.AddChild(floorNode);

            sceneGraph.Root = rootNode;
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrices
            Matrix4 angle90degrees = Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), MathF.PI / 2);
            Matrix4 worldToCamera = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * angle90degrees;
            Matrix4 cameraToScreen = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), (float)screen.width / screen.height, 0.1f, 1000);

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * MathF.PI) a -= 2 * MathF.PI;

            // update local transform for animated rotation
            var teapotNode = sceneGraph.Root.Children[0];
            teapotNode.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);

            var teapotChildNode = teapotNode.Children[0];
            teapotChildNode.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(10, 0, 2)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), a);


            // clear the screen
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // render scene graph
            if (shader != null && wood != null)
            {
                sceneGraph.Render(shader, worldToCamera * cameraToScreen, wood);
            }
        }
    }
}
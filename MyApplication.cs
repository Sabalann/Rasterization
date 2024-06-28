using System.Diagnostics;
using INFOGR2024TemplateP2;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using INFOGR2024TemplateP2;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh? teapot, teapotChild, floor;       // meshes to draw using OpenGL
        float a = 0;                            // teapot rotation angle
        readonly Stopwatch timer = new();       // timer for measuring frame duration
        Shader? shader;                         // shader to use for rendering
        Shader? postproc;                       // shader to use for post processing
        Texture? wood;                          // texture to use for rendering
        RenderTarget? target;                   // intermediate render target
        ScreenQuad? quad;                       // screen filling quad for post processing
        readonly bool useRenderTarget = true;   // required for post processing


        Vector3 cameraPosition = new Vector3(0, 0, -10); // starting position
        Vector3 cameraRotation = Vector3.Zero; // starting rotation

        float camMoveSpeed = 0.1f; // can be adjusted during run with L to increase and K to decrease
        float camRoatSpeed = 1.0f; // adjust to taste

        SceneGraph sceneGraph;
        SceneGraphNode rootNode, teapotNode, teapot2Node, teapot3Node, teapot4Node, floorNode; // scenegraph nodes


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

            SceneGraphNode teapot1 = new SceneGraphNode(teapot);
            teapot1.LocalTransform = Matrix4.CreateTranslation(2, 0, 0);
            teapot1.Shininess = 64.0f;

            SceneGraphNode teapot2 = new SceneGraphNode(teapot);
            teapot2.LocalTransform = Matrix4.CreateTranslation(-2, 0, 0);
            teapot2.Shininess = 16.0f;


            // scenegraph
            rootNode = new SceneGraphNode(null);
            teapotNode = new SceneGraphNode(teapot);
            teapot2Node = new SceneGraphNode(teapot); // same mesh as parent
            teapot3Node = new SceneGraphNode(teapot);
            teapot4Node = new SceneGraphNode(teapot);


            floorNode = new SceneGraphNode(floor);




            floorNode.LocalTransform = Matrix4.CreateScale(4.0f);

            rootNode.AddChild(teapotNode);
            teapotNode.AddChild(teapot2Node);
            teapot2Node.AddChild(teapot3Node);
            teapot3Node.AddChild(teapot4Node);

            rootNode.AddChild(floorNode);

            sceneGraph.Root = rootNode;

            Light light1 = new Light(new Vector3(10.0f, 10.0f, 10.0f), new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
            //Light light2 = new Light(new Vector3(-10.0f, 5.0f, -5.0f), new Vector3(0.5f, 0.0f, 0.0f), 0.8f); // wasn't really working correctly
            //Light light3 = new Light(new Vector3(-10.0f, 5.0f, -5.0f), new Vector3(0f, 0.0f, 10f), 0.8f);


            rootNode.AddLight(light1);
            //rootNode.AddLight(light2);
            //rootNode.AddLight(light3);


            teapotNode.Shininess = 32.0f;
            floorNode.Shininess = 4.0f;


        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            //screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL(FrameEventArgs args, KeyboardState input)
        {

            HandleInput(input);

            // prepare camera transformation matrices
            Matrix4 translation = Matrix4.CreateTranslation(cameraPosition);
            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(cameraRotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(cameraRotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(cameraRotation.Z));

            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            Matrix4 worldToCamera = rotation * translation;
            Matrix4 cameraToScreen = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), (float)screen.width / screen.height, 0.1f, 1000);


            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * MathF.PI) a -= 2 * MathF.PI;

            // update local transform for animated rotation
            teapotNode.LocalTransform = Matrix4.CreateScale(1f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            teapot2Node.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(20, 0, 2)) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            teapot3Node.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(20, 0, 2)) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            teapot4Node.LocalTransform = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(new Vector3(20, 0, 2)) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);


            List<Light> allLights = rootNode.CollectLights();

            if (shader != null)
            {
                shader.SetNumLights(Math.Min(allLights.Count, 4));
                for (int i = 0; i < Math.Min(allLights.Count, 4); i++)
                {
                    shader.SetLight(i, allLights[i].Position, allLights[i].Color, allLights[i].Intensity);
                }
            }
            rootNode.Render(wood, shader, worldToCamera * cameraToScreen);

        }



        private void HandleInput(KeyboardState input)
        {


            // Translation with WASD & QE keys
            if (input.IsKeyDown(Keys.S))
                cameraPosition.Z -= camMoveSpeed;
            if (input.IsKeyDown(Keys.W))
                cameraPosition.Z += camMoveSpeed;
            if (input.IsKeyDown(Keys.D))
                cameraPosition.X -= camMoveSpeed;
            if (input.IsKeyDown(Keys.A))
                cameraPosition.X += camMoveSpeed;
            if (input.IsKeyDown(Keys.Q))
                cameraPosition.Y -= camMoveSpeed;
            if (input.IsKeyDown(Keys.E))
                cameraPosition.Y += camMoveSpeed;
            if (input.IsKeyDown(Keys.L))
                camMoveSpeed += 0.1f;
            if (input.IsKeyDown(Keys.K))
                camMoveSpeed -= 0.1f;

            // Rotation with arrow keys
            if (input.IsKeyDown(Keys.Up))
                cameraRotation.X -= camRoatSpeed;
            if (input.IsKeyDown(Keys.Down))
                cameraRotation.X += camRoatSpeed;
            if (input.IsKeyDown(Keys.Left))
                cameraRotation.Y -= camRoatSpeed;
            if (input.IsKeyDown(Keys.Right))
                cameraRotation.Y += camRoatSpeed;
        }
    }
}
Team members: (names and student IDs)
* Sabalan Alaeiyeh 2576422
* Armand Ayar 6402704
* Jesse Tabak 1142356

Tick the boxes below for the implemented features. Add a brief note only if necessary, e.g., if it's only partially working, or how to turn it on.

Formalities:
[x] This readme.txt
[ ] Cleaned (no obj/bin folders)
[x] Demonstration scene(s) with all implemented features
[ ] (Optional) Screenshots: make it clear which feature is demonstrated in which screenshot

Minimum requirements implemented:
[x] Camera: position and orientation controls
Controls:	WASD for moving/translation of the camera in the X and Z directions, Q and E for moving the camera in the Y direction,
			Arrow keys for rotation of the camera,
			K and L for adjusting the speed of the movement of the camera.

[+] Model matrix: for each mesh, stored as part of the scene graph
[+] Scene graph data structure: tree hierarchy, no limitation on breadth or depth or size
[+] Rendering: recursive scene graph traversal, correct model matrix concatenation
[+/-] Shading in fragment shader: diffuse, glossy, uniform variable for ambient light color
[+] Point light: at least 1, position/color may be hardcoded

Bonus features implemented:
[+/-] Multiple point lights: at least 4, uniform variables to change position and color at runtime
Multiple point lights are supported but cannot be changed at runtime with control input or something
[ ] Spot lights: position, center direction, opening angle, color
[ ] Environment mapping: cube or sphere mapping, used in background and/or reflections
[ ] Frustum culling: in C# code, using scene graph node bounds, may be conservative
[ ] Bump or normal mapping
[ ] Shadow mapping: render depth map to texture, only hard shadows required, some artifacts allowed
[ ] Vignetting and chromatic aberrations: darker corners, color channels separated more near corners
[ ] Color grading: color cube lookup table
[ ] Blur: separate horizontal and vertical blur passes, variable blur size
[ ] HDR glow: HDR render target, blur in HDR, tone-mapping
[ ] Depth of field: blur size based on distance from camera, some artifacts allowed
[ ] Ambient occlusion: darker in tight corners, implemented as screen-space post process
[ ] ...

Notes:
After we had already started working on the project we realized there were instructions on how we can implement the scenegraph and shading,
that's why our implementation is a bit different from the instructions. The model matrix is stored in each node instead of each mesh. Also
we have added a scenegraphNode class instead of just the scenegraph class, which might be redundant but it made more sense for us to have it like this.
The scenegraph render method basically just calls the scenegraphNode render method.

The scene shows 4 teapots, each one is smaller than the next, which shows the hierarchy of the scene graph. The same rotation matrix is applied
to each one each frame, but since they're childeren of each other, the higher down the hierarchy the bigger the rotation. This shows that there
is no real limit to the depth of the scene graph.

We tried to add multiple lights, but it didn't seem to work correctly. We left the code we had in there but it essentially does very little.
Changing the position and color of the first light should work accordingly however.


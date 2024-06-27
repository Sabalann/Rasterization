using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace INFOGR2024TemplateP2
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Color { get; set; }
        public float Intensity { get; set; }

        public Light(Vector3 position, Vector3 color, float intensity)
        {
            Position = position;
            Color = color;
            Intensity = intensity;
        }
    }
}

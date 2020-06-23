using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityLoader
{
    public class NormalizedColor
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public NormalizedColor(float red, float green, float blue, float alpha = 255)
        {
            R = red; G = green; B = blue; A = alpha;
        }

        public Color ColorToUnity() { return new Color(RedToUnity(), GreenToUnity(), BlueToUnity(), AlphaToUnity()); }
        public float RedToUnity() { return R / 255f; }
        public float GreenToUnity() { return G / 255f; }
        public float BlueToUnity() { return B / 255f; }
        public float AlphaToUnity() { return A / 255f; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace StaticHelpers
{
    public static class StaticHelper
    {
        public static ContentManager Content;
        public static GraphicsDevice Device;
        public static GraphicsDeviceManager DeviceManager;
        public static float GetHeightAt(float worldX, float worldZ, int width, int length, float[,] heights)
        {
            int x, z; // Cell coordinates in the height array 
            float fractionX = 0.0f, fractionZ = 0.0f; // Fractional coordinates within the quad 

            // If the position is off the height field to the left or right side, 
            // we interpolate along the respective border of the height field. 
            if (worldX <= 0.0f)
            {
                x = 0;
                fractionX = 0.0f;
            }
            else if (worldX >= (float)(width - 1))
            {
                x = width - 2;
                fractionX = 1.0f;
            }
            else
            {
                x = (int)worldX;
                fractionX = worldX - x;
            }

            // If the position is off the height field to the top or bottom side, 
            // we interpolate along the respective border of the height field. 
            if (worldZ <= 0.0f)
            {
                z = 0;
                fractionZ = 0.0f;
            }
            else if (worldZ >= (float)(length - 1))
            {
                z = length - 2;
                fractionZ = 1.0f;
            }
            else
            {
                z = (int)worldZ;
                fractionZ = worldZ - z;
            }

            if ((fractionX + fractionZ) < 1.0f)
            { // We're in the upper left triangle 

                return
                (MathHelper.Lerp(heights[x, z], heights[x + 1, z], fractionX) +
                  (heights[x, z + 1] - heights[x, z]) * fractionZ) * 2;

            }
            else
            { // We're in the lower right triangle 

                return
                (MathHelper.Lerp(heights[x, z + 1], heights[x + 1, z + 1], fractionX) +
                  (heights[x + 1, z] - heights[x + 1, z + 1]) * (1.0f - fractionZ)) * 2;

            }
        }

    }
}

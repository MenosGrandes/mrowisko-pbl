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
        public static float [,] heights;
        public static int length;
        public static int width;
        public static float GetHeightAt(float worldX, float worldZ)
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

        public static float TurnToFace(Vector2 position, Vector2 faceThis,
           float currentAngle, float turnSpeed)
        {
            // consider this diagram:
            //         C 
            //        /|
            //      /  |
            //    /    | y
            //  / o    |
            // S--------
            //     x
            // 
            // where S is the position of the spot light, C is the position of the cat,
            // and "o" is the angle that the spot light should be facing in order to 
            // point at the cat. we need to know what o is. using trig, we know that
            //      tan(theta)       = opposite / adjacent
            //      tan(o)           = y / x
            // if we take the arctan of both sides of this equation...
            //      arctan( tan(o) ) = arctan( y / x )
            //      o                = arctan( y / x )
            // so, we can use x and y to find o, our "desiredAngle."
            // x and y are just the differences in position between the two objects.
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            // we'll use the Atan2 function. Atan will calculates the arc tangent of 
            // y / x for us, and has the added benefit that it will use the signs of x
            // and y to determine what cartesian quadrant to put the result in.
            float desiredAngle = (float)Math.Atan2(x, y);//- MathHelper.ToRadians(180);

            // so now we know where we WANT to be facing, and where we ARE facing...
            // if we weren't constrained by turnSpeed, this would be easy: we'd just 
            // return desiredAngle.
            // instead, we have to calculate how much we WANT to turn, and then make
            // sure that's not more than turnSpeed.

            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Returns the angle expressed in radians between -Pi and Pi.
        /// </summary>
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

    }
}

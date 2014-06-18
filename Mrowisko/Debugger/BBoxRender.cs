using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debugger
{
    public static class BBoxRender
    {
        public static BasicEffect boxEffect;
        public static GraphicsDevice device;
        public static void InitializeBBoxDebuger(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            boxEffect = new BasicEffect(graphicsDevice);


        }
        // Initialize an array of indices for the box. 12 lines require 24 indices
        public static short[] bBoxIndices = {
    0, 1, 1, 2, 2, 3, 3, 0, // Front edges
    4, 5, 5, 6, 6, 7, 7, 4, // Back edges
    0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
};

        public static void DrawBBox(List<BoundingBox> boundingBoxes, Matrix Projection, Matrix View,Matrix localWorld)
        {   // Use inside a drawing loop
            foreach (BoundingBox box in boundingBoxes)
            {
                Vector3[] corners = box.GetCorners();
                VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                // Assign the 8 box vertices
                for (int i = 0; i < corners.Length; i++)
                {
                    primitiveList[i] = new VertexPositionColor(corners[i], Color.White);
                }

                /* Set your own effect parameters here */

                boxEffect.World = localWorld;
                boxEffect.View = View;
                boxEffect.Projection = Projection;
                boxEffect.TextureEnabled = false;

                // Draw the box with a LineList
                foreach (EffectPass pass in boxEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserIndexedPrimitives(
                        PrimitiveType.LineList, primitiveList, 0, 8,
                        bBoxIndices, 0, 12);
                }
            }
        }
    }
}
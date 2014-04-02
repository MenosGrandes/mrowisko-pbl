using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame5;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mrowisko
{
    class Layer
    {
        private VertexBuffer treeVertexBuffer;
        private VertexDeclaration treeVertexDeclaration;
        private Effect bbEffect;
        private Texture2D treeTexture;
        private GraphicsDevice device;
        private List<Vector3> treeList;
        private int scale;

        public List<Vector3> TreeList
        {
            get { return treeList; }
            set { treeList = value; }
        }
        public Layer(Texture2D tree, GraphicsDevice device, ContentManager Content, int scale)
        {
            treeTexture = tree;
            this.device = device;
            this.scale = scale;
            this.bbEffect = Content.Load<Effect>("Effect");
             
            


        }

        public void GenerateTreePositions(Texture2D treeMap, VertexMultitextured[] terrainVertices, int terrainWidth, int terrainLength, float[,] heightData)
        {
            Color[] treeMapColors = new Color[treeMap.Width * treeMap.Height];
            treeMap.GetData(treeMapColors);

            int[,] noiseData = new int[treeMap.Width, treeMap.Height];
            for (int x = 0; x < treeMap.Width; x++)
                for (int y = 0; y < treeMap.Height; y++)
                    noiseData[x, y] = treeMapColors[y + x * treeMap.Height].R;


            this.treeList = new List<Vector3>(); Random random = new Random();

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    float terrainHeight = heightData[x, y];
                    if ((terrainHeight > 8) && (terrainHeight < 14))
                    {
                        float flatness = Vector3.Dot(terrainVertices[x + y * terrainWidth].Normal, new Vector3(0, 1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                            float relx = (float)x / (float)terrainWidth;
                            float rely = (float)y / (float)terrainLength;

                            float noiseValueAtCurrentPosition = noiseData[(int)(relx * treeMap.Width), (int)(rely * treeMap.Height)];
                            float treeDensity;
                            if (noiseValueAtCurrentPosition > 200)
                                treeDensity = 5;
                            else if (noiseValueAtCurrentPosition > 150)
                                treeDensity = 4;
                            else if (noiseValueAtCurrentPosition > 100)
                                treeDensity = 3;
                            else
                                treeDensity = 0;

                            for (int currDetail = 0; currDetail < treeDensity; currDetail++)
                            {
                                float rand1 = (float)random.Next(1000) / 1000.0f;
                                float rand2 = (float)random.Next(1000) / 1000.0f;
                                Vector3 treePos = new Vector3((float)x - rand1, 0, -(float)y - rand2);
                                treePos.Y = heightData[x, y];
                                treeList.Add(treePos*scale);
                            }
                        }
                    }
                }
            }

            
        }


        public void CreateBillboardVerticesFromList(List<Vector3> treeList)
        {
            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[treeList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in treeList)
            {
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 * scale, 0 * scale));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1 * scale, 0 * scale));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1 * scale, 1 * scale));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 * scale, 0 * scale));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1 * scale, 1 * scale));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 * scale, 1 * scale));
            }

            VertexDeclaration vertexDeclaration = VertexPositionTexture.VertexDeclaration;

            treeVertexBuffer = new VertexBuffer(device, vertexDeclaration, billboardVertices.Length, BufferUsage.WriteOnly);
            treeVertexBuffer.SetData(billboardVertices);
            treeVertexDeclaration = vertexDeclaration;
        }

        public void DrawBillboards(Matrix currentViewMatrix, Matrix projectionMatrix, Vector3 position)
        {
            bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
            bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
            bbEffect.Parameters["xView"].SetValue(currentViewMatrix);
            bbEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            bbEffect.Parameters["xCamPos"].SetValue(position);
            bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
            bbEffect.Parameters["xBillboardTexture"].SetValue(treeTexture);

            device.BlendState = BlendState.AlphaBlend;
            foreach (EffectPass pass in bbEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(treeVertexBuffer);
                int noVertices = treeVertexBuffer.VertexCount;
                int noTriangles = noVertices / 3;
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, noTriangles);
            }
            device.BlendState = BlendState.Opaque;
        }


    }
}

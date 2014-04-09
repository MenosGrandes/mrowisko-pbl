using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame5;
using KlasyZKamera;

namespace KlasyZMapa
{
    public class Layer
    {
        private VertexBuffer treeVertexBuffer;
        private VertexDeclaration treeVertexDeclaration;
        private Effect bbEffect;
        private Texture2D treeTexture;
        private Model tree;
        private GraphicsDevice device;
        private List<Vector3> treeList;
        List<LoadModel> models;
        private int scale;
        private Vector3 scaleM;
        private ContentManager content;

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
            this.content = Content;
            


        }

        public Layer(Model tree, GraphicsDevice device, ContentManager Content, Vector3 scale)
        {
            this.tree = tree;
            this.device = device;
            this.scaleM = scale;
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


            this.treeList = new List<Vector3>();
            Random random = new Random();

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    float terrainHeight = heightData[x, y];
                    if ((terrainHeight > 5) && (terrainHeight < 20))
                    {
                       
                        float flatness = Vector3.Dot(terrainVertices[x + y * terrainWidth].Normal, new Vector3(0, -1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                           
                            float relx = (float)x / (float)terrainWidth;
                            float rely = (float)y / (float)terrainLength;

                            float noiseValueAtCurrentPosition = noiseData[(int)(relx * treeMap.Width), (int)(rely * treeMap.Height)];
                            float treeDensity;
                            if (noiseValueAtCurrentPosition > 200)
                                treeDensity = 3;
                            else if (noiseValueAtCurrentPosition > 150)
                                treeDensity = 2;
                            else if (noiseValueAtCurrentPosition > 100)
                                treeDensity = 1;
                            else
                                treeDensity = 0;

                            for (int currDetail = 0; currDetail < treeDensity; currDetail++)
                            {
                                float rand1 = (float)random.Next(1000000) / 100000000.0f;
                                float rand2 = (float)random.Next(1000000) / 10000000.0f;
                                Vector3 treePos = new Vector3((float)x - rand1, 0, (float)y - rand2);
                                treePos.Y = heightData[x, y];
                                treeList.Add(treePos*scale);
                            }
                        }
                    }
                }
            }

           
            
        }

        public void CreateModelFromList(List<Vector3> treeList)
        {
            models = new List<LoadModel>();
            Random random = new Random();
            foreach (Vector3 currentV3 in treeList)
            {
                float rand1 = (float)random.Next(360000) / 100.0f;
                models.Add(new LoadModel(tree, currentV3, new Vector3(0, 200, 180), new Vector3(3.0f), this.device));
               

            }
            Console.WriteLine(models.Count);
        }


        public void CreateBillboardVerticesFromList()
        {
            
            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[treeList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in treeList)
            {

               


                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 , 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1 , 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 , 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));


            }


           
            treeVertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, billboardVertices.Length, BufferUsage.WriteOnly);
            treeVertexBuffer.SetData(billboardVertices);
        }

        public void DrawBillboards(Matrix currentViewMatrix, Matrix projectionMatrix, Vector3 position)
        {
            bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
            bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
            bbEffect.Parameters["xView"].SetValue(currentViewMatrix);
            bbEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            bbEffect.Parameters["xCamPos"].SetValue(position);
            bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
            bbEffect.Parameters["scale"].SetValue(this.scale);
            bbEffect.Parameters["xBillboardTexture"].SetValue(treeTexture);

            device.SetVertexBuffer(treeVertexBuffer);

            foreach (EffectPass pass in bbEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
               // device.DrawPrimitives(PrimitiveType.TriangleList, 0, treeVertexBuffer.VertexCount / 3);

            }
        }

        public void DrawModels(FreeCamera camera )
        {
            foreach (LoadModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection);
        }


    }
}

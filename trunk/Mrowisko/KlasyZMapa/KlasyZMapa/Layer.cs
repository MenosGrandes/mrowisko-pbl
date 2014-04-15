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
using DebugManager;
using GameCamera;

namespace Map
{             
                 /// <summary>
                 /// Class responsible for creating a layer of objects at the terrain
                 /// </summary>
    public class Layer
    {
        private VertexBuffer treeVertexBuffer;
        private Effect bbEffect;
        private Texture2D envBilbTexture;
        private Model envModel;
        private GraphicsDevice device;
        private List<Vector3> envBilbList;
        List<LoadModel> models;
        private int scale;
        private Vector3 scaleM;
        private ContentManager content;
         /// <summary>
         /// 
         /// </summary>
        public List<Vector3> TreeList
        {
            get { return envBilbList; }
            set { envBilbList = value; }
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="device"></param>
        /// <param name="Content"></param>
        /// <param name="scale"></param>
        public Layer(Texture2D tree, GraphicsDevice device, ContentManager Content, int scale)
        {
            envBilbTexture = tree;
            this.device = device;
            this.scale = scale;
            this.bbEffect = Content.Load<Effect>("Effect");
            this.content = Content;


        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="envModel"></param>
   /// <param name="device"></param>
   /// <param name="Content"></param>
   /// <param name="scale"></param>
        public Layer(Model envModel, GraphicsDevice device, ContentManager Content, Vector3 scale)
        {
            this.envModel = envModel;
            this.device = device;
            this.scaleM = scale;
            this.bbEffect = Content.Load<Effect>("Effect");



        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="objMap"></param>
   /// <param name="terrainVertices"></param>
   /// <param name="terrainWidth"></param>
   /// <param name="terrainLength"></param>
   /// <param name="heightData"></param>
        public void GenerateObjPositions(Texture2D objMap, VertexMultitextured[] terrainVertices, int terrainWidth, int terrainLength, float[,] heightData)
        {
            Color[] objMapColors = new Color[objMap.Width * objMap.Height];
            objMap.GetData(objMapColors);

            int[,] noiseData = new int[objMap.Width, objMap.Height];
            for (int x = 0; x < objMap.Width; x++)
                for (int y = 0; y < objMap.Height; y++)
                    noiseData[x, y] = objMapColors[y + x * objMap.Height].R;


            this.envBilbList = new List<Vector3>();
            Random random = new Random();

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    float terrainHeight = heightData[x, y];
                    if ((terrainHeight > 7) && (terrainHeight < 14))
                    {
                       
                        float flatness = Vector3.Dot(terrainVertices[x + y * terrainWidth].Normal, new Vector3(0, -1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                           
                            float relx = (float)x / (float)terrainWidth;
                            float rely = (float)y / (float)terrainLength;

                            float noiseValueAtCurrentPosition = noiseData[(int)(relx * objMap.Width), (int)(rely * objMap.Height)];
                            float treeDensity;
                            if (noiseValueAtCurrentPosition > 200)
                                treeDensity = 3;
                            else if (noiseValueAtCurrentPosition > 100)
                                treeDensity = 2;
                            else if (noiseValueAtCurrentPosition > 1)
                                treeDensity = 1;
                            else
                                treeDensity = 0;

                            for (int currDetail = 0; currDetail < treeDensity; currDetail++)
                            {
                                float rand1 = (float)random.Next(1000000) / 10000000.0f;
                                float rand2 = (float)random.Next(1000000) / 10000000.0f;
                                Vector3 treePos = new Vector3((float)x - rand1, 0, (float)y - rand2);
                                treePos.Y = heightData[x, y];
                                envBilbList.Add(treePos*scale);
                            }
                        }
                    }
                }
            }

           
            
        }
  /// <summary>
  /// 
  /// </summary>
  /// <param name="treeList"></param>
        public void CreateModelFromList(List<Vector3> treeList)
        {
            models = new List<LoadModel>();
            Random random = new Random();
            foreach (Vector3 currentV3 in treeList)
            {
                float rand1 = (float)random.Next(360000) / 100.0f;
                models.Add(new LoadModel(envModel, currentV3, new Vector3(0, rand1, 0), scaleM, this.device));
               

            }
           // Console.WriteLine(models.Count);
        }

/// <summary>
/// 
/// </summary>
        public void CreateBillboardVerticesFromList()
        {

            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[envBilbList.Count * 6];
            int i = 0;
            foreach (Vector3 currentV3 in envBilbList)
            {

               


                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0 , 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1 , 1));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 1));

                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));


            }


           
            treeVertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, billboardVertices.Length, BufferUsage.WriteOnly);
            treeVertexBuffer.SetData(billboardVertices);
        }
                                                                      /// <summary>
                                                                      /// 
                                                                      /// </summary>
                                                                      /// <param name="currentViewMatrix"></param>
                                                                      /// <param name="projectionMatrix"></param>
                                                                      /// <param name="position"></param>
        public void DrawBillboards(Matrix currentViewMatrix, Matrix projectionMatrix, Vector3 position)
        {
            bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
            bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
            bbEffect.Parameters["xView"].SetValue(currentViewMatrix);
            bbEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            bbEffect.Parameters["xCamPos"].SetValue(position);
            bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
            bbEffect.Parameters["scale"].SetValue(this.scale);
            bbEffect.Parameters["xBillboardTexture"].SetValue(envBilbTexture);

            device.SetVertexBuffer(treeVertexBuffer);

            foreach (EffectPass pass in bbEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, treeVertexBuffer.VertexCount / 3);

            }
        }
 /// <summary>
 /// 
 /// </summary>
 /// <param name="camera"></param>
        public void DrawModels(FreeCamera camera )
        {
            foreach (LoadModel model in models)
               if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                {
                  
                    BoundingSphereRenderer.Render(model.boundingSphere, device, camera.View, camera.Projection, Color.Pink);
                    model.Draw(camera.View, camera.Projection);
                }

                  
        }


    }
}

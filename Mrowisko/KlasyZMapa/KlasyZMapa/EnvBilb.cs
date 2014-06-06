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
using GameCamera;


namespace Map
{
    public class EnvBilb
    {
        
        private VertexBuffer VertexBuffer;
        private Effect bbEffect;
        private Texture2D envBilbTexture;
        private GraphicsDevice device;
        private List<Vector3> envBilbList;
        private LightsAndShadows.Light light;
    
        private int scale;
        private ContentManager content;
        private Texture2D objMap;
         /// <summary>
         /// 
         /// </summary>
        public List<Vector3> EnvBilbList
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
        public EnvBilb(Texture2D objMap, Texture2D bilboardTexture, GraphicsDevice device, ContentManager Content, int scale, LightsAndShadows.Light light)
        {
            envBilbTexture = bilboardTexture;
            this.device = device;
            this.scale = scale;
            this.objMap = objMap;
            this.bbEffect = Content.Load<Effect>("Effects/Bilboarding");
            this.content = Content;
            this.light = light;
            bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
            bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(0, 1, 0));
            bbEffect.Parameters["xScale"].SetValue((float)this.scale);
            bbEffect.Parameters["xBillboardTexture"].SetValue(envBilbTexture);


        }
   /// <summary>
   /// 
   /// </summary>
   /// <param name="objMap"></param>
   /// <param name="terrainVertices"></param>
   /// <param name="terrainWidth"></param>
   /// <param name="terrainLength"></param>
   /// <param name="heightData"></param>
        public void GenerateObjPositions(VertexMultitextured[] terrainVertices, int terrainWidth, int terrainLength, float[,] heightData)
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
                    if ((terrainHeight > 1) && (terrainHeight <30))
                    {
                       
                        float flatness = Vector3.Dot(terrainVertices[x + y * terrainWidth].Normal, new Vector3(0, -1, 0));
                        float minFlatness = (float)Math.Cos(MathHelper.ToRadians(15));
                        if (flatness > minFlatness)
                        {
                           
                            float relx = (float)x / (float)terrainWidth;
                            float rely = (float)y / (float)terrainLength;

                            float noiseValueAtCurrentPosition = noiseData[(int)(relx * objMap.Width), (int)(rely * objMap.Height)];
                            float density;
                            if (noiseValueAtCurrentPosition > 200)
                                density = 3;
                            else if (noiseValueAtCurrentPosition > 100)
                                density = 2;
                            else if (noiseValueAtCurrentPosition > 1)
                                density = 1;
                            else
                                density = 0;

                            for (int currDetail = 0; currDetail < density; currDetail++)
                            {
                                float rand1 = (float)random.Next(1000000) / 10000000.0f;
                                float rand2 = (float)random.Next(1000000) / 10000000.0f;
                                Vector3 position = new Vector3((float)x - rand1, 0, (float)y - rand2);
                                position.Y = heightData[x, y];
                                envBilbList.Add(position );
                            }
                        }
                    }
                }
            }

           
            
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
            VertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, billboardVertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(billboardVertices);
        }
 /// <summary>
 /// 
/// </summary>
 /// <param name="currentViewMatrix"></param>
 /// <param name="projectionMatrix"></param>
 /// <param name="position"></param>
        public void DrawBillboards(Matrix currentViewMatrix, Matrix projectionMatrix, Vector3 position,float time)
        {
            

            bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
            bbEffect.Parameters["xView"].SetValue(currentViewMatrix);
            bbEffect.Parameters["xProjection"].SetValue(projectionMatrix);
            bbEffect.Parameters["xCamPos"].SetValue(position);
            bbEffect.Parameters["xAmbient"].SetValue(light.lightPosChangeBilb(time));
            device.SetVertexBuffer(VertexBuffer);
            foreach (EffectPass pass in bbEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount / 3);

            }
        }

    }
}

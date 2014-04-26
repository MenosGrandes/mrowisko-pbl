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
    class EnvModel
    {
        private VertexBuffer VertexBuffer;
        private Effect bbEffect;
        private Texture2D envBilbTexture;
        private Model envModel;
        private GraphicsDevice device;
        private List<Vector3> envBilbList;


        private List<LoadModel> models; //powinna być LISTA<LISTA<LOADMODEL>>
        int scale;
        private Vector3 scaleM;
        private ContentManager content;
        /// <summary>
        /// 
        /// </summary>
        public List<Vector3> EnvBilbList
        {
            get { return envBilbList; }
            set { envBilbList = value; }
        }

        public List<LoadModel> Models
        {
            get { return models; }
            set { models = value; }
        }

        Texture2D objMap;
   /// <summary>
   /// 
   /// </summary>
   /// <param name="envModel"></param>
   /// <param name="device"></param>
   /// <param name="Content"></param>
   /// <param name="scale"></param>
        public EnvModel(Texture2D objMap, Model envModel, GraphicsDevice device, ContentManager Content, int scale)
        {
            this.envModel = envModel;
            this.device = device;
            this.scale = scale;
            this.scaleM = new Vector3(scale/30);
            this.objMap = objMap;


        }
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
                    if ((terrainHeight > 7) && (terrainHeight < 14))
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
                                envBilbList.Add(position * scale);
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
        public void CreateModelFromList()
        {
            models = new List<LoadModel>();
            Random random = new Random();
            foreach (Vector3 currentV3 in envBilbList)
            {
                float rand1 = (float)random.Next(360000) / 100.0f;
                models.Add(new LoadModel(envModel, currentV3, new Vector3(0, rand1, 0), scaleM, this.device));


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
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
                billboardVertices[i++] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
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
        /// <param name="camera"></param>
        public void DrawModels(FreeCamera camera)
        {
              int licznik = 0;
            foreach (LoadModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                {
                    model.Draw(camera.View, camera.Projection);
                     licznik++;
                }
            Console.WriteLine(licznik);


        }
    }
}

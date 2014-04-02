﻿using System;
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

    struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TexWeights;
        public static int SizeInBytes = (3 + 3 + 4 + 4) * sizeof(float);

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
 (
     new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
     new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
     new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
     new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1)
 );
    }
    class MapRender
    {
         

       private GraphicsDevice device;

       private int terrainWidth;

       public int TerrainWidth
       {
           get { return terrainWidth; }
           set { terrainWidth = value; }
       }
       private int terrainLength;

       public int TerrainLength
       {
           get { return terrainLength; }
           set { terrainLength = value; }
       }
       private float[,] heightData;

       private VertexBuffer terrainVertexBuffer;
       private IndexBuffer terrainIndexBuffer;

       private VertexMultitextured[] vertices;

       private VertexMultitextured[] Vertices
       {
           get { return vertices; }
           set { vertices = value; }
       }

       private int[] indices;
       private ContentManager Content;
       private Effect effect;
       private Texture2D grassTexture, sandTexture, rockTexture, snowTexture, treeTexture;

       private Layer trees;

      

        public MapRender( GraphicsDevice GraphicsDevice, List<Texture2D>texture, ContentManager Content,int Scale, Texture2D treeMap)
        {


            this.device = GraphicsDevice;
            this.grassTexture = texture[0];
            this.sandTexture = texture[1]; //Content.Load<Texture2D>("sand");
            this.rockTexture = texture[2]; //Content.Load<Texture2D>("rock");
            this.snowTexture = texture[3];//Content.Load<Texture2D>("snow");
            this.treeTexture = texture[5];
            this.Content = Content;
            effect = Content.Load<Effect>("Effect");

            LoadHeightData(texture[4]);
            SetUpvertices(Scale);
            SetUpTerrainIndices();
            CalculateNormals();
            CopyToTerrainBuffers();
            this.trees = new Layer(this.treeTexture, device, Content, Scale);
            trees.GenerateTreePositions(treeMap,this.vertices, this.terrainWidth, this.terrainLength, this.heightData);
            trees.CreateBillboardVerticesFromList(trees.TreeList);
           
        }

        

        private void LoadHeightData(Texture2D heightMap)
        {
            float minimumHeight = float.MaxValue;
            float maximumHeight = float.MinValue;

            terrainWidth = heightMap.Width;
            terrainLength = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainLength];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainLength];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R;
                    if (heightData[x, y] < minimumHeight) minimumHeight = heightData[x, y];
                    if (heightData[x, y] > maximumHeight) maximumHeight = heightData[x, y];
                }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                    heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
        }


        private void SetUpvertices(int Scale)
        {
            vertices = new VertexMultitextured[terrainWidth * terrainLength];

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x * Scale, heightData[x, y] * Scale, -y * Scale);
                    vertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 10.0f;
                    vertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 10.0f;

                    vertices[x + y * terrainWidth].TexWeights.X = MathHelper.Clamp(1.0f - Math.Abs(heightData[x, y] - 0) / 8.0f, 0, 1);
                    vertices[x + y * terrainWidth].TexWeights.Y = MathHelper.Clamp(1.0f - Math.Abs(heightData[x, y] - 12) / 6.0f, 0, 1);
                    vertices[x + y * terrainWidth].TexWeights.Z = MathHelper.Clamp(1.0f - Math.Abs(heightData[x, y] - 20) / 6.0f, 0, 1);
                    vertices[x + y * terrainWidth].TexWeights.W = MathHelper.Clamp(1.0f - Math.Abs(heightData[x, y] - 30) / 6.0f, 0, 1);

                    float total = vertices[x + y * terrainWidth].TexWeights.X;
                    total += vertices[x + y * terrainWidth].TexWeights.Y;
                    total += vertices[x + y * terrainWidth].TexWeights.Z;
                    total += vertices[x + y * terrainWidth].TexWeights.W;

                    vertices[x + y * terrainWidth].TexWeights.X /= total;
                    vertices[x + y * terrainWidth].TexWeights.Y /= total;
                    vertices[x + y * terrainWidth].TexWeights.Z /= total;
                    vertices[x + y * terrainWidth].TexWeights.W /= total;
                }
            }

        }
    
         private void SetUpTerrainIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainLength - 1) *6];
            int counter = 0;
            for (int y = 0; y < terrainLength - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }


      

        private void CopyToTerrainBuffers()
        {
            terrainVertexBuffer = new VertexBuffer(device, VertexMultitextured.VertexDeclaration, vertices.Length,
                BufferUsage.None);


            terrainVertexBuffer.SetData<VertexMultitextured>(vertices);

            terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            terrainIndexBuffer.SetData(indices);
        }



        public void DrawTerrain(Matrix currentViewMatrix, Matrix projectionMatrix, Vector3 position)
        {



            effect.CurrentTechnique = effect.Techniques["MultiTextured"];
            effect.Parameters["xTexture0"].SetValue(sandTexture);
            effect.Parameters["xTexture1"].SetValue(grassTexture);
            effect.Parameters["xTexture2"].SetValue(rockTexture);
            effect.Parameters["xTexture3"].SetValue(snowTexture);
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(currentViewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
           effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(2.4f);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(0.5f, 1, 0.5f));
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.Indices = terrainIndexBuffer;
                device.SetVertexBuffer(terrainVertexBuffer);

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);

            }
             trees.DrawBillboards(currentViewMatrix, projectionMatrix, position);
            
        }
    }


    }

    


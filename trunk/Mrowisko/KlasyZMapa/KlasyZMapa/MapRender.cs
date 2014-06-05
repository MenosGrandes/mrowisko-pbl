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
                /// <summary>
                /// Struct which implementing new VertexType for Multitexturing.
                /// </summary>
    public struct VertexMultitextured : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector4 TextureCoordinate;
        public Vector4 TexWeights;

        public static int SizeInBytes = (3 + 3 + 4 + 4 ) * sizeof(float);

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
    /// <summary>
    /// Class for creating vertices and indices of terrain.
    /// </summary>
    public class MapRender
    {
         

      // private GraphicsDevice device;

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
       public float[,] heightData
       {
           get;
           set;
       }


       //private VertexBuffer terrainVertexBuffer;
       //private IndexBuffer terrainIndexBuffer;

       private VertexMultitextured[] vertices;

       public VertexMultitextured[] Vertices
       {
           get { return vertices; }
           set { vertices = value; }
       }
       public VertexMultitextured this[int index]
       {
           get { return Vertices[index]; }
           set { Vertices[index] = value; }
       }

       public int[] indices;
       //private ContentManager Content;
       //private Effect effect;
       //private Texture2D grassTexture, sandTexture, rockTexture, snowTexture, treeTexture;

      // private Layer trees, ants;



       // public MapRender( GraphicsDevice GraphicsDevice, List<Texture2D>texture, currentViewMatrix Content,int Scale, Model model)
        /// <summary>
        /// Constructor to create vertices and indices from HeighMap.
        /// </summary>
        /// <param name="texture"> Heigh Map image of terrain</param>
        /// <param name="Scale">It's scale.</param>
    public MapRender (Texture2D texture,int Scale)    
    {



            LoadHeightData(texture);
            SetUpvertices(Scale);
            SetUpTerrainIndices();
            CalculateNormals();

            


        }

        
                                        /// <summary>
                                        ///             Loading informations about heigh from texture.
                                        /// </summary>
                                        /// <param name="heightMap">Heigh Map image</param>
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
                for (int y = 0; y < terrainLength ; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R ;
                    if (heightData[x, y] < minimumHeight) minimumHeight = heightData[x, y];
                    if (heightData[x, y] > maximumHeight) maximumHeight = heightData[x, y];

                                  }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 20.0f;

                }
        }

  /// <summary>
  /// Create vertices from heighData and Scale.
  /// Create positions of all points,and gets informations about texture coordinations and weights.
  /// </summary>
  /// <param name="Scale"></param>
        private void SetUpvertices(int Scale)
        {
            vertices = new VertexMultitextured[terrainWidth * terrainLength];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainLength; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x * Scale, heightData[x, y] * Scale, y * Scale);
                    vertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 5.0f;
                    vertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 5.0f;
                 
                  
                   
                }
            }

        }
/// <summary>
///                    Create indices of terrain.
/// </summary>
    
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
 /// <summary>
 /// Callculate normals for all of Vertices.
 /// </summary>
   
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


    }

        
    }

    


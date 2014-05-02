﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///<summary>
/// Namespace of all context related with Map creation
/// </summary>
namespace Map
{
    /// <summary>
    /// Create QuadTree for terrain. It not use LevelOfDetail. It's only for FrustrumCulling.
    /// </summary>
    public class QuadTree
    {

        public LoadModel model;

        public int MinimumDepth = 7;
        public int IndexCount { get; private set; }
        public BasicEffect Effect;
        private QuadNode _rootNode;
        private MapRender _vertices;
        public BufferManager _buffers;
        private Vector3 _position;
        private int _topNodeSize;
        public LightsAndShadows.Shadow shadow;
        LightsAndShadows.Light light;
        private Vector3 _cameraPosition;
        private Vector3 _lastCameraPosition;

        public int[] Indices;

        public Matrix View;
        public Matrix Projection;
        public float x=1.0f, z=1.0f;
        public GraphicsDevice Device;

        public int TopNodeSize { get { return _topNodeSize; } }
        public QuadNode RootNode { get { return _rootNode; } }
        public MapRender Vertices { get { return _vertices; } }
        public Vector3 CameraPosition
        {
            get { return _cameraPosition; }
            set { _cameraPosition = value; }
        }

        public BoundingFrustum ViewFrustrum { get; private set; }
        Effect effect;
        Effect effect2;

        List<Texture2D> textures;

        public bool Cull { get; set; }
        private QuadNode _activeNode;

        List<EnvModel> envModelList = new List<EnvModel>();
        List<EnvBilb> envBilbList = new List<EnvBilb>();
        /// <summary>
        /// Create terrain at <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textures"></param>
        /// <param name="device"></param>
        /// <param name="scale"></param>
        /// <param name="Content"></param>
        /// <param name="camera"></param>
        public QuadTree(Vector3 position, List<Texture2D> textures, GraphicsDevice device, int scale, ContentManager Content, GameCamera.FreeCamera camera)
        {
            shadow = new LightsAndShadows.Shadow();
            light = new LightsAndShadows.Light(0.6f, 0.4f, new Vector3(51300+25600, 400, 51300+25600));

            ViewFrustrum = new BoundingFrustum(camera.View * camera.Projection);
            Model model = Content.Load<Model>("Models/mrowka_01");
            this.model = new LoadModel(model, Vector3.One, Vector3.Up, new Vector3(100), device);
            this.textures = textures;
            effect = Content.Load<Effect>("Effects/MultiTextured");
            effect2 = Content.Load<Effect>("Effects/Shadows");
            Device = device;

            _position = position;
            _topNodeSize = textures[4].Width - 1;

            _vertices = new MapRender(textures[4], scale);
            _buffers = new BufferManager(_vertices.Vertices, device);
            _rootNode = new QuadNode(NodeType.FullNode, _topNodeSize, 1, null, this, 0);


            //Construct an array large enough to hold all of the indices we'll need.
            Indices = _vertices.indices;


            envBilbList.Add(new EnvBilb(textures[6], textures[5], device, Content, scale));
            envModelList.Add(new EnvModel(textures[6], model, device, Content, scale));
            foreach (EnvBilb pass in envBilbList)
            {
                pass.GenerateObjPositions(_vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
                pass.CreateBillboardVerticesFromList();
            }

            foreach (EnvModel pass1 in envModelList)
            {
                pass1.GenerateObjPositions(_vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
                pass1.CreateModelFromList();
            }


            
       effect.CurrentTechnique = effect.Techniques["MultiTextured"];
       effect.Parameters["xTexture0"].SetValue(textures[1]);
       effect.Parameters["xTexture1"].SetValue(textures[0]);
       effect.Parameters["xTexture2"].SetValue(textures[2]);
       effect.Parameters["xTexture3"].SetValue(textures[3]);
       effect.Parameters["xTexture5"].SetValue(textures[7]);
       Matrix worldMatrix = Matrix.Identity;
       effect.Parameters["xWorld"].SetValue(worldMatrix);
       effect.Parameters["xEnableLighting"].SetValue(false);
       effect.Parameters["xAmbient"].SetValue(light.Ambient);
       effect.Parameters["xLightPower"].SetValue(light.LightPower);
       
          

           effect.Parameters["Ground"].SetValue(textures[7]);
   effect.Parameters["GroundText0"].SetValue(textures[8]);
   effect.Parameters["GroundText1"].SetValue(textures[9]);
   effect.Parameters["GroundText2"].SetValue(textures[10]);




        }
        public void Update(GameTime gameTime)
        {

            //Only update if the camera position has changed



           
            // _lastCameraPosition = _cameraPosition;
            IndexCount = 0;

            _rootNode.EnforceMinimumDepth();

            // _activeNode = _rootNode.DeepestNodeWithPoint(CameraPosition);

            // if (_activeNode != null)
            // {
            //     _activeNode.Split();
            //  }

            _rootNode.SetActiveVertices();

            _buffers.UpdateIndexBuffer(Indices, IndexCount);
            _buffers.SwapBuffer();
        }
        public void Draw(GameCamera.FreeCamera camera, float time)
        {
            
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = rasterizerState;

            this.Device.SetRenderTarget(shadow.RenderTarget);
          //  this.Device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            this.CameraPosition = camera.Position;
            this.View = camera.View;
            this.Projection = camera.Projection;
            ViewFrustrum.Matrix = camera.View * camera.Projection;

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;
          //  this.x+=1;

            this.model.Position = light.lightPosChange(time);
            effect.Parameters["xLightPos"].SetValue(this.model.Position);


           shadow.UpdateLightData(0.4f, 0.6f, this.model.Position, camera);
          //  Console.WriteLine("pozycja " + this.model.Position);
          // Console.WriteLine("pozycja mnozenie " + light.lightPosChange(time*100));
           

       effect.Parameters["xView"].SetValue(camera.View);
       effect.Parameters["xProjection"].SetValue(camera.Projection);
             effect.Parameters["xLightsWorldViewProjection"].SetValue(Matrix.Identity * shadow.lightsViewProjectionMatrix);
           effect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
             effect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);
       //effect.Parameters["xTime2"].SetValue(time );
       

           foreach (EffectPass pass in effect.CurrentTechnique.Passes)
           {
               pass.Apply();
             //  foreach (EffectPass pass2 in effect2.CurrentTechnique.Passes)
             //  {
                  
            //       pass2.Apply();
                   if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
                   Console.WriteLine(IndexCount);
            //   }

           }
         

       //    effect.CurrentTechnique = effect.Techniques["ShadowMap"];
          

        //   foreach (EffectPass pass in effect.CurrentTechnique.Passes)
         //  {
        //       pass.Apply();
        //       if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
        //   }

           this.Device.SetRenderTarget(null);
           shadow.ShadowMap = (Texture2D)shadow.RenderTarget;


        //   this.Device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

       //   effect.CurrentTechnique = effect.Techniques["ShadowedScene"];
      //     effect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);

       //    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
      //     {
      //         pass.Apply();
      //     if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
      //     }

           shadow.ShadowMap = null;
          // model.Draw(camera.View, camera.Projection);
            /*
            foreach (EnvModel pass1 in envModelList)
            {
                pass1.DrawModels(camera);
            }
            Device.BlendState = BlendState.AlphaBlend;
             
          */
      //      foreach (EnvBilb pass in envBilbList)
      //      {
       //         pass.DrawBillboards(camera.View, camera.Projection, camera.Position,time/10);
       //     }

           

        }
        internal void UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }



    }


}

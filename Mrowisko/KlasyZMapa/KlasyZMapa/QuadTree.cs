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


        public int MinimumDepth = 7;
        public int IndexCount { get; private set; }
        public BasicEffect Effect;
        private QuadNode _rootNode;
        private MapRender _vertices;
        public BufferManager _buffers;
        private Vector3 _position;
        private int _topNodeSize;

        LightsAndShadows.Light light;
        private Vector3 _cameraPosition;
        private Vector3 _lastCameraPosition;

        public int[] Indices;

        public Matrix View;
        public Matrix Projection;
        public float x = 1.0f, z = 1.0f;
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

        List<Texture2D> textures;

        public bool Cull { get; set; }
        private QuadNode _activeNode;

        public List<EnvBilb> envBilbList = new List<EnvBilb>();
        /// <summary>
        /// Create terrain at <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textures"></param>
        /// <param name="device"></param>
        /// <param name="scale"></param>
        /// <param name="Content"></param>
        /// <param name="camera"></param>
        public QuadTree(Vector2 position, List<Texture2D> textures, GraphicsDevice device, int scale, ContentManager Content, GameCamera.FreeCamera camera)
        {

            light = new LightsAndShadows.Light(0.7f, 0.4f, new Vector3(513, 100, 513));

            ViewFrustrum = new BoundingFrustum(camera.View * camera.Projection);
            this.textures = textures;
            effect = Content.Load<Effect>("Effects/MultiTextured");

            Device = device;

            _topNodeSize = textures[15].Width - 1;

            _vertices = new MapRender(textures[15], scale, position);
            _buffers = new BufferManager(_vertices.Vertices, device);
            _rootNode = new QuadNode(NodeType.FullNode, _topNodeSize, 1, null, this, 0);


            //Construct an array large enough to hold all of the indices we'll need.
            Indices = _vertices.indices;


            envBilbList.Add(new EnvBilb(textures[19], textures[20], device, Content, scale));
            envBilbList.Add(new EnvBilb(textures[17], textures[18], device, Content, scale));

            foreach (EnvBilb pass in envBilbList)
            {
                pass.GenerateObjPositions(_vertices.TerrainLength2, _vertices.TerrainWidth2, _vertices.heightDataToControl);
                pass.CreateBillboardVerticesFromList();
            }






            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(light.Ambient);
            effect.Parameters["xLightPower"].SetValue(light.LightPower);
            #region tekstury Alphy
            effect.Parameters["xTexture0"].SetValue(textures[0]);
            effect.Parameters["xTexture1"].SetValue(textures[1]);
            effect.Parameters["xTexture2"].SetValue(textures[2]);
            effect.Parameters["xTexture3"].SetValue(textures[3]);
            effect.Parameters["xTexture4"].SetValue(textures[4]);
            effect.Parameters["xTexture5"].SetValue(textures[5]);
            effect.Parameters["xTexture6"].SetValue(textures[6]);
            #endregion
            #region Tekstury Terenu
            effect.Parameters["xTexture7"].SetValue(textures[7]);
            effect.Parameters["xTexture8"].SetValue(textures[8]);
            effect.Parameters["xTexture9"].SetValue(textures[9]);
            effect.Parameters["xTexture10"].SetValue(textures[10]);
            effect.Parameters["xTexture11"].SetValue(textures[11]);

            effect.Parameters["xTexture12"].SetValue(textures[12]);
            effect.Parameters["xTexture13"].SetValue(textures[13]);
            effect.Parameters["xTexture14"].SetValue(textures[14]);
            #endregion
            #region Proj
            //  effect.Parameters["xTexture14"].SetValue(textures[26]);
            #endregion
            MinimumDepth = 9;
            _rootNode.EnforceMinimumDepth();
            MinimumDepth = 7;
            QuadNodeController.ustawione = true;
            _rootNode.EnforceMinimumDepth();
        }
        public void Update(GameTime gameTime)
        {






            IndexCount = 0;



            _rootNode.SetActiveVertices();

            _buffers.UpdateIndexBuffer(Indices, IndexCount);
            _buffers.SwapBuffer();
        }
        public void Draw(GameCamera.FreeCamera camera, float time, LightsAndShadows.Shadow shadow, LightsAndShadows.Light light)
        {
            effect.CurrentTechnique = effect.Techniques["MultiTextured"];

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = rasterizerState;

            this.CameraPosition = camera.Position;
            this.View = camera.View;
            this.Projection = camera.Projection;
            ViewFrustrum.Matrix = camera.View * camera.Projection;

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;

            effect.Parameters["xLightPos"].SetValue(light.lightPosChange(time));



            effect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * Matrix.Identity);
            effect.Parameters["xWorldViewProjection"].SetValue(shadow.woldsViewProjection * Matrix.Identity);
            effect.Parameters["shadowTexture"].SetValue(shadow.ShadowMap);
            effect.Parameters["PCFSamples"].SetValue(shadow.PcfSamples);
            effect.Parameters["xView"].SetValue(camera.View);
            effect.Parameters["xProjection"].SetValue(camera.Projection);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
            }






            foreach (EnvBilb pass in envBilbList)
            {
                pass.DrawBillboards(camera.View, camera.Projection, camera.Position);
            }



        }
        internal void UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }
        public void basicDraw()
        {

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;

            if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
        }



    }


}

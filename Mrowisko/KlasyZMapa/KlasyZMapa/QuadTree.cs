using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KlasyZMapa
{
    public class QuadTree
    {
        public int MinimumDepth = 6;
        public int IndexCount { get; set; }
        public BasicEffect Effect;
        private QuadNode _rootNode;
        private MapRender _vertices;
        private BufferManager _buffers;
        private Vector3 _position;
        private int _topNodeSize;

        private Vector3 _cameraPosition;
        private Vector3 _lastCameraPosition;

        public int[] Indices;

        public Matrix View;
        public Matrix Projection;

        public GraphicsDevice Device;

        public int TopNodeSize { get { return _topNodeSize; } }
        public QuadNode RootNode { get { return _rootNode; } }
        public MapRender Vertices { get { return _vertices; } }
        public Vector3 CameraPosition
        {
            get { return _cameraPosition; }
            set { _cameraPosition = value; }
        }

        public BoundingFrustum ViewFrustrum { get; set; }
        Effect effect;
        List<Texture2D> textures;
        private Layer trees;
        private Layer ants;
        public QuadTree(Vector3 position, List<Texture2D> textures, GraphicsDevice device, int scale,ContentManager Content,KlasyZKamera.FreeCamera camera)
        {

            ViewFrustrum = new BoundingFrustum(camera.View * camera.Projection);
            Model model = Content.Load<Model>("mrowka_01");
            this.textures = textures;
            effect = Content.Load<Effect>("Effect");
            Device = device;
            _position = position;
            _topNodeSize = textures[4].Width - 1;

            _vertices = new MapRender (textures[4],scale);
            _buffers = new BufferManager(_vertices.Vertices, device);
            _rootNode = new QuadNode(NodeType.FullNode, _topNodeSize, 1, null, this, 0);


            //Construct an array large enough to hold all of the indices we'll need.
            Indices = _vertices.indices;


            this.trees = new Layer(textures[5], device, Content, scale);
          //  this.ants = new Layer(model, device, Content, new Vector3(.03f));
            trees.GenerateTreePositions(textures[6], _vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
          //  ants.GenerateTreePositions(textures[6], _vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
           // ants.CreateModelFromList(trees.TreeList);
            trees.CreateBillboardVerticesFromList();
           
        }
        public void Update(GameTime gameTime)
        {
            //Only update if the camera position has changed
            IndexCount = 0;

            _rootNode.EnforceMinimumDepth();
            _rootNode.SetActiveVertices();

            _buffers.UpdateIndexBuffer(Indices, IndexCount);
            _buffers.SwapBuffer();
        }
        public void Draw(KlasyZKamera.FreeCamera camera)
        {

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = rasterizerState;


            ViewFrustrum = new BoundingFrustum(camera.View * camera.Projection);

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;

           
            effect.CurrentTechnique = effect.Techniques["MultiTextured"];
            effect.Parameters["xTexture0"].SetValue(textures[1]);
            effect.Parameters["xTexture1"].SetValue(textures[0]);
            effect.Parameters["xTexture2"].SetValue(textures[2]);
            effect.Parameters["xTexture3"].SetValue(textures[3]);
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(camera.View);
            effect.Parameters["xProjection"].SetValue(camera.Projection);
           effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(2.4f);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(0.5f, 1, 0.5f));
             foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);

            }
             trees.DrawBillboards(camera.View, camera.Projection, camera.Position);
            // ants.DrawModels(camera);
    
    

        }
        internal void UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }


    
    }


}

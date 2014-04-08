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

        internal BoundingFrustum ViewFrustrum { get; set; }
        Effect effect;
        List<Texture2D> textures;
        public QuadTree(Vector3 position, List<Texture2D> textures, GraphicsDevice device, int scale,ContentManager Content)
        {

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

           
        }
        public void Update(GameTime gameTime)
        {
            //Only update if the camera position has changed
            if (_cameraPosition == _lastCameraPosition)
                return;


            _lastCameraPosition = _cameraPosition;
            IndexCount = 0;

            _rootNode.EnforceMinimumDepth();
            _rootNode.SetActiveVertices();
            _rootNode.SetActiveVertices();

            _buffers.UpdateIndexBuffer(Indices, IndexCount);
            _buffers.SwapBuffer();
        }
        public void Draw(Matrix currentViewMatrix, Matrix projectionMatrix,Vector3 position)
        {

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = rasterizerState;


            ViewFrustrum = new BoundingFrustum(currentViewMatrix * projectionMatrix);

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;

           
            effect.CurrentTechnique = effect.Techniques["MultiTextured"];
            effect.Parameters["xTexture0"].SetValue(textures[1]);
            effect.Parameters["xTexture1"].SetValue(textures[0]);
            effect.Parameters["xTexture2"].SetValue(textures[2]);
            effect.Parameters["xTexture3"].SetValue(textures[3]);
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

                Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);

            }        
        
    
    

        }
        internal void UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }


    
    }


}

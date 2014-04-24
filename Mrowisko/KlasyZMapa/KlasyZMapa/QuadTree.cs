using Microsoft.Xna.Framework;
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

        public BoundingFrustum ViewFrustrum { get; private set; }
        Effect effect;
 
        List<Texture2D> textures;

        List<EnvModel> envModelList= new List<EnvModel>();
        List<EnvBilb> envBilbList = new List<EnvBilb>();

        public bool Cull { get; set; }
        private QuadNode _activeNode;
        /// <summary>
        /// Create terrain at <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textures"></param>
        /// <param name="device"></param>
        /// <param name="scale"></param>
        /// <param name="Content"></param>
        /// <param name="camera"></param>
        public QuadTree(Vector3 position, List<Texture2D> textures, GraphicsDevice device, int scale,ContentManager Content,GameCamera.FreeCamera camera)
        {

            ViewFrustrum = new BoundingFrustum(camera.View * camera.Projection);
            Model model = Content.Load<Model>("Models/mrowka_01");
            this.textures = textures;
            effect = Content.Load<Effect>("Effects/MultiTextured");
        
            Device = device;

            _position = position;
            _topNodeSize = textures[4].Width - 1;

            _vertices = new MapRender (textures[4],scale);
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
/*
            trees.GenerateObjPositions(textures[6], _vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
            ants.GenerateObjPositions(textures[6], _vertices.Vertices, _vertices.TerrainWidth, _vertices.TerrainLength, _vertices.heightData);
            ants.CreateModelFromList();
            trees.CreateBillboardVerticesFromList();
            */

            
         effect.CurrentTechnique = effect.Techniques["MultiTextured"];
          effect.Parameters["xTexture0"].SetValue(textures[1]);
          effect.Parameters["xTexture1"].SetValue(textures[0]);
          effect.Parameters["xTexture2"].SetValue(textures[2]);
          effect.Parameters["xTexture3"].SetValue(textures[3]);
          effect.Parameters["xTexture5"].SetValue(textures[7]);
          Matrix worldMatrix = Matrix.Identity;
          effect.Parameters["xWorld"].SetValue(worldMatrix);
          effect.Parameters["xEnableLighting"].SetValue(true);
          effect.Parameters["xAmbient"].SetValue(1.0f);
          effect.Parameters["xLightPower"].SetValue(0.6f);
          effect.Parameters["xLightPos"].SetValue(new Vector3(25600, 1000, 25600));

            /*     
          effect.Parameters["Ground"].SetValue(textures[7]);
        effect.Parameters["GroundText0"].SetValue(textures[8]);
        effect.Parameters["GroundText1"].SetValue(textures[9]);
        effect.Parameters["GroundText2"].SetValue(textures[10]);
            */
          _rootNode.EnforceMinimumDepth();
        }
        public void Update(GameTime gameTime)
        {

            //Only update if the camera position has changed


           

           // _lastCameraPosition = _cameraPosition;
            IndexCount = 0;
            _rootNode.SetActiveVertices();

           // ;

                              /*
            _rootNode.Merge();
            _activeNode = _rootNode.DeepestNodeWithPoint(CameraPosition);

            if (_activeNode != null)
            {
                _activeNode.Split();
            }
              */

            _buffers.UpdateIndexBuffer(Indices, IndexCount);
            _buffers.SwapBuffer();
        }
        public void Draw(GameCamera.FreeCamera camera, float time)
        {

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //Device.RasterizerState = rasterizerState;
            this.CameraPosition = camera.Position;
            this.View = camera.View;
            this.Projection = camera.Projection;
            ViewFrustrum.Matrix = camera.View * camera.Projection;

            this.Device.SetVertexBuffer(_buffers.VertexBuffer);
            this.Device.Indices = _buffers.IndexBuffer;


           
          effect.Parameters["xView"].SetValue(camera.View);
          effect.Parameters["xProjection"].SetValue(camera.Projection);
            
          effect.Parameters["xTime2"].SetValue(time*100);
                /*
            effect.CurrentTechnique = effect.Techniques["Terrain"];

          effect.Parameters["View"].SetValue(camera.View);
          effect.Parameters["Projection"].SetValue(camera.Projection);
             *   */
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
               
                pass.Apply();
                 
                if (IndexCount > 0) Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Vertices.Length, 0, IndexCount);
                Console.WriteLine(IndexCount);

            }


            foreach (EnvModel pass1 in envModelList)
            {
                pass1.DrawModels(camera);
            }
            Device.BlendState = BlendState.AlphaBlend;

           foreach (EnvBilb pass in envBilbList)
            {
                pass.DrawBillboards(camera.View, camera.Projection, camera.Position);
            }
            
        }
        internal void   UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }


    
    }


}

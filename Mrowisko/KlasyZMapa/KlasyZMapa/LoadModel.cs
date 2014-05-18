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
using Animations;
namespace Map
{
    /// <summary>
    /// Class responsible of loding and positioning model at the terrain.
    /// </summary>
    public class LoadModel                                                                                      
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Model Model { get; private set; }
        public Boolean Selected;
        public Vector3 playerTarget;
        public ContentManager content;
        public SkinningData skinningData;
        public AnimationPlayer Player;
        public List<ShadowCasterObject> shadowCasters;
       public BoundingSphere[] spheres
        {

            get
            {
                // No need for rotation, as this is a sphere
                List<BoundingSphere> spheres = new List<BoundingSphere>();
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    Matrix worldTransform = Matrix.CreateScale(Scale)* Matrix.CreateTranslation(Position);

                    if (mesh.Name.Contains("BoundingSphere")) {

                    BoundingSphere transformed = mesh.BoundingSphere.Transform(worldTransform);
                    spheres.Add(transformed);
                    }
                }

                return spheres.ToArray();
            }
            set{}
        }
        
        public Matrix baseWorld;

        public Matrix[] modelTransforms;
        private GraphicsDevice graphicsDevice;
        private BoundingSphere boundingSphere;

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) *  Matrix.CreateTranslation(Position);
                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);

                return transformed;
            }
        }
        /// <summary>
        /// Constructor of LoadModel class.
        /// Create <paramref name="Model"/> at <paramref name="Position"/ >  with <paramref name="Scale"/> and <paramref name="Rotation"/>
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="Position"></param>
        /// <param name="Rotation"></param>
        /// <param name="Scale"></param>
        /// <param name="graphicsDevice"></param>
        public LoadModel(Model Model, Vector3 Position, Vector3 Rotation,
        Vector3 Scale, GraphicsDevice graphicsDevice)
        {
            this.baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(
            Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position);

            shadowCasters = new List<ShadowCasterObject>();
            this.Model = Model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.graphicsDevice = graphicsDevice;
            buildBoundingSphere();

            Matrix[] bones = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(bones);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (!mesh.Name.Contains("BoundingSphere"))
                    foreach (ModelMeshPart meshpart in mesh.MeshParts)
                {


                     ShadowCasterObject shad = new ShadowCasterObject(
                                                    //meshpart.VertexDeclaration,
                                                    meshpart.VertexBuffer,
                                                    meshpart.VertexOffset,
                                                    //meshpart.VertexStride,
                                                    meshpart.IndexBuffer,
                                                    //meshpart.BaseVertex,
                                                    meshpart.NumVertices,
                                                    meshpart.StartIndex,
                                                    meshpart.PrimitiveCount,
                                                    bones[mesh.ParentBone.Index]*Matrix.CreateTranslation(Position));
                     this.shadowCasters.Add(shad);
                }
            }
            
        }
        //Animated model constructor
        public LoadModel(Model Model, Vector3 Position, Vector3 Rotation,
        Vector3 Scale, GraphicsDevice GraphicsDevice,
        ContentManager Content)
        {
            this.Model = Model;
            this.graphicsDevice = GraphicsDevice;
            this.content = Content;
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            SkinningData skinningData = Model.Tag as SkinningData;
            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");
            this.skinningData = Model.Tag as SkinningData;
            Player = new AnimationPlayer(skinningData);
            buildBoundingSphere();

        }

        /// <summary>
        /// Method to create BoudingSphere at the model.
        /// This metod gather all Mesh from model creates bounding sphere for each of theme, then combine all and create one big. 
        /// </summary>
        private void buildBoundingSphere()
        {

            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            List<BoundingSphere> spheres = new List<BoundingSphere>();
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (mesh.Name.Contains("BoundingSphere") )
                {
                    BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                spheres.Add(transformed);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);

                }
            }
            
             
            this.boundingSphere = sphere;
            this.spheres = spheres.ToArray();   
        }
        /// <summary>
        /// Method to Draw model 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Projection"></param>
        public void Draw(Matrix View, Matrix Projection)
        {
            Matrix baseWorld = Matrix.CreateScale(Scale)* Matrix.CreateFromYawPitchRoll(
            Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (!mesh.Name.Contains("BoundingSphere"))
                { 
                   Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                   * baseWorld;
                   foreach (ModelMeshPart meshPart in mesh.MeshParts)
                   {
                       BasicEffect effect = (BasicEffect)meshPart.Effect;
                       effect.World = localWorld;
                       effect.View = View;
                       effect.Projection = Projection;
                       effect.EnableDefaultLighting();
                   }

                   mesh.Draw();
                           }
            }
           }
           /// <summary>
           /// Method to Draw AnimatedModel
           /// </summary>
           /// <param name="View"></param>
           /// <param name="Projection"></param>
           /// <param name="CameraPosition"></param>
           public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
           {

               Matrix[] bones = Player.GetSkinTransforms();

               Matrix baseWorld = Matrix.CreateScale(Scale)
               * Matrix.CreateFromYawPitchRoll(
               Rotation.Y, Rotation.X, Rotation.Z)
               * Matrix.CreateTranslation(Position);


               foreach (ModelMesh mesh in Model.Meshes)
               {

                   Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                  * baseWorld;
                   foreach (SkinnedEffect effect in mesh.Effects)
                   {
                       effect.SetBoneTransforms(bones);
                       effect.World = localWorld;
                       effect.View = View;
                       effect.Projection = Projection;

                       effect.EnableDefaultLighting();

                       effect.SpecularColor = new Vector3(0.25f);
                       effect.SpecularPower = 16;
                   }
                   
                mesh.Draw();
            }

        }
        /// <summary>
        /// Update metod for Animations
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // update world
            Matrix world = Matrix.CreateScale(Scale) *
   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
   Matrix.CreateTranslation(Position);
         Player.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

        }

        public void basicDraw()
        {
             foreach (ModelMesh mesh in Model.Meshes)
               {

                   if (!mesh.Name.Contains("BoundingSphere"))
                   mesh.Draw();
               }

        }
        public void UpdateBoundingSpheres()
        {

            }   
        

                         
                

    }
}

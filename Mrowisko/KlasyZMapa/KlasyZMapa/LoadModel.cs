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
        public ContentManager content;
        public SkinningData skinningData;
        public AnimationPlayer Player;
        private Matrix[] modelTransforms;
        private GraphicsDevice graphicsDevice;
        public BoundingSphere boundingSphere;
        float cameraArc = 0;
        float cameraRotation = 0;
        float cameraDistance = 100;
        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale)
                * Matrix.CreateTranslation(Position);
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
            this.Model = Model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            buildBoundingSphere();

            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.graphicsDevice = graphicsDevice;
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

        }

        /// <summary>
        /// Method to create BoudingSphere at the model.
        /// This metod gather all Mesh from model creates bounding sphere for each of theme, then combine all and create one big. 
        /// </summary>
        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }
        /// <summary>
        /// Method to Draw model 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Projection"></param>
        public void Draw(Matrix View, Matrix Projection)
        {
            Console.WriteLine("cos");
            Matrix baseWorld = Matrix.CreateScale(Scale)
            * Matrix.CreateFromYawPitchRoll(
            Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
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

    }
}

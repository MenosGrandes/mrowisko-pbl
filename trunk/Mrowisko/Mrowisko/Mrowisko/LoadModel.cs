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
using KlasyZAnimacja;
namespace WindowsGame5
{
    class LoadModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Model Model { get; private set; }
        public AnimationPlayer Player;
        SkinningData skinningData;
        ContentManager content;
        private Matrix[] modelTransforms;
        private GraphicsDevice graphicsDevice;
        public BoundingSphere boundingSphere;
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
        
         this.skinningData = Model.Tag as SkinningData;
         Player = new AnimationPlayer(skinningData);
         //setNewEffect();
     }
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
        // setNewEffect(); - macie coś alternatywnego
        }

        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
             this.boundingSphere=sphere;
        }
        void setNewEffect()
     {
     //wiem że będziecie to robić na shaderach :D ale tak jest w tutorialu
         foreach(ModelMesh mesh in Model.Meshes)
         {
             foreach(ModelMeshPart part in mesh.MeshParts)
             {
                 SkinnedEffect newEffect = new SkinnedEffect(graphicsDevice);
                 BasicEffect oldEffect = ((BasicEffect)part.Effect);
                 newEffect.EnableDefaultLighting();
                 newEffect.SpecularColor = Color.Black.ToVector3();

                 newEffect.AmbientLightColor = oldEffect.AmbientLightColor;
                 newEffect.DiffuseColor = oldEffect.DiffuseColor;
                 newEffect.Texture = oldEffect.Texture;
                 part.Effect = newEffect;
             }
         }
     }
        public void Draw(Matrix View, Matrix Projection)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
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
                    //effect.EnableDefaultLighting();

                }
                mesh.Draw();
            }
        }
        public void Update (GameTime gameTime)
     {
         Matrix world = Matrix.CreateScale(Scale) *
Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
Matrix.CreateTranslation(Position);
         Player.Update(gameTime.ElapsedGameTime, world);
     }
    }
}

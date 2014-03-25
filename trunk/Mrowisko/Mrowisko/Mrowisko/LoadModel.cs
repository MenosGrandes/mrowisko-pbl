﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame5
{
    class LoadModel
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Model Model { get; private set; }
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
                    effect.EnableDefaultLighting();

                }
                mesh.Draw();
            }
        }

    }
}

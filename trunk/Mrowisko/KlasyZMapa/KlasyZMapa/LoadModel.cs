﻿using System;
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
    ///     [Serializable]

    [Serializable]
    public class LoadModel                                                                                      
    {
        public bool Collide;
        public Vector3 Position { get; set; }
        public Vector3 tempPosition { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        [NonSerialized]
        public Model Model;
        public Boolean Selected;
        public Vector3 playerTarget;
        public ContentManager content;
                    

        public SkinningData skinningData;
        [NonSerialized]
        public AnimationPlayer Player;
        [NonSerialized]
        public LightsAndShadows.Light light;
        public List<ShadowCasterObject> shadowCasters;
        private BoundingSphere[] spheres;

        public BoundingSphere[] Spheres
        {

            get {
                Matrix worldTransform = Matrix.CreateScale(Scale) *Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z)* Matrix.CreateTranslation(Position) ;
                BoundingSphere[] spheres2 =new  BoundingSphere[spheres.Length];
                for (int i = 0; i < spheres2.Length;i++ )
                {
                    spheres2[i] = spheres[i];
                    spheres2[i] = spheres2[i].Transform(worldTransform);
                }
               
               
                return spheres2;
            }
            set { spheres = value; }
            
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
        Vector3 Scale, GraphicsDevice graphicsDevice, LightsAndShadows.Light light)
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
            this.playerTarget = this.Position;
            this.Scale = Scale;
            this.graphicsDevice = graphicsDevice;
            this.light = light;
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
        ContentManager Content, LightsAndShadows.Light light)
        {
            this.Model = Model;
            this.graphicsDevice = GraphicsDevice;
            this.content = Content;
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
            this.light = light;
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
            int d=0;
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            BoundingSphere[] spheres2=new BoundingSphere[3];
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                if (mesh.Name.Contains("BoundingSphere"))
                {
                    
                    spheres2[d] = transformed;
                    
                    d++;
                }
                else {
                    sphere = BoundingSphere.CreateMerged(sphere, transformed);
                }
            }
            
             
            this.boundingSphere = sphere;
            this.spheres=spheres2;
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
                           /*
                       effect.DirectionalLight0.Enabled = true;
                       effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs(-1 * (float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower) + 1), 0.3f, 0.9f);
                       //Console.WriteLine(MathHelper.Clamp((Math.Abs(-1 * (float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower)), 0.3f, 0.9f));// a red light
                       effect.DirectionalLight0.Direction = lightDir;  // coming along the x-axis
                       effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs((float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower) + 1), 0.3f, 0.9f); ; // with green highlights
                       */


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
           public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition, float time)
           {

               Matrix[] bones = Player.GetSkinTransforms();

               Matrix baseWorld = Matrix.CreateScale(Scale)
               * Matrix.CreateFromYawPitchRoll(
               Rotation.Y, Rotation.X, Rotation.Z)
               * Matrix.CreateTranslation(Position);


               foreach (ModelMesh mesh in this.Model.Meshes)
               {

                   Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                  * baseWorld;
                   foreach (SkinnedEffect effect in mesh.Effects)
                   {
                       Vector3 lightDir = Vector3.Normalize((this.Position * this.Rotation) - light.lightPosChange(time));
                       lightDir.X *= -1;
                       lightDir.Z *= -1;
                       effect.SetBoneTransforms(bones);
                       effect.World = localWorld;
                       effect.View = View;
                       effect.Projection = Projection;

                       effect.DirectionalLight0.Enabled = true;
                       effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs(-1*(float)Math.Sin(MathHelper.ToRadians(time-1.58f))/light.LightPower)+1),0.3f,0.9f); // a red light
                       effect.DirectionalLight0.Direction = lightDir;  // coming along the x-axis
                       effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs((float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower) + 1), 0.3f, 0.9f); ; // with green highlights

                       //effect.SpecularColor = new Vector3(0.25f);
                       //effect.SpecularPower = 16;
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
            if (Player != null) { 
         Player.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            }
         if (shadowCasters != null)
         {

             foreach (ModelMesh mesh in Model.Meshes)
             {
                 if (!mesh.Name.Contains("BoundingSphere"))
                     foreach (ModelMeshPart meshpart in mesh.MeshParts)
                     {

                         foreach (ShadowCasterObject sc in shadowCasters)
                         {
                             
                             sc.World = modelTransforms[mesh.ParentBone.Index] * world;
                         }

                     }
             }
         }

        }

        public void basicDraw()
        {
             foreach (ModelMesh mesh in Model.Meshes)
               {

                   if (!mesh.Name.Contains("BoundingSphere"))
                   mesh.Draw();
               }

        }

        

                         
                

    }
}

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
using Map;

namespace Logic
{
    /// <summary>
    /// Class responsible of loding and positioning model at the terrain.
    /// </summary>
    ///     [Serializable]

    [Serializable]
    public class LoadModel
    {
        public bool Hit = false;
        private BoundingBox b_box;
        public BoundingBox B_Box
        {
            get { return b_box; }
            set { b_box = value; }
        }

        public bool Collide;
        public Vector3 Position { get; set; }
        public Vector3 tempPosition { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        [NonSerialized]
        public Model Model;
        public Boolean Selected;
        public Vector3 playerTarget;
                [NonSerialized]
        public ContentManager content;
        public Boolean animationChange = false;//true oznacza ze został wciśniety guzik do zmiany animacji i można ją zmienić
        [NonSerialized]
        public SkinningData skinningData;
        [NonSerialized]
        public AnimationPlayer Player;
        private List<BoundingSphere> spheres;
                        [NonSerialized]
        AnimationClip list = null;//animacja oczekująca na zmiane;
        public Matrix baseWorld;
        public Matrix LocalWorld
        {
            get
            {
                return localWorld;
            }
            set
            {
                localWorld = value;
            }
        }
        private Matrix localWorld;

        public List<BoundingSphere> Spheres
        {

            get
            {
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z) * Matrix.CreateTranslation(Position);
                List<BoundingSphere> spheres2 = new List<BoundingSphere>();
                for (int i = 0; i < spheres.Count; i++)
                {
                    spheres2.Add(spheres[i]);
                    spheres2[i] = spheres2[i].Transform(worldTransform);
                }


                return spheres2;
            }
            set { spheres = value; }

        }


        public Matrix[] modelTransforms;
                        [NonSerialized]
        private GraphicsDevice graphicsDevice;
        private BoundingSphere boundingSphere;

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
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
            Console.WriteLine(Position);
            this.baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(
            Rotation.Y, Rotation.X, Rotation.Z)
            * Matrix.CreateTranslation(Position);

            this.Model = Model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            this.Position = Position;
            this.Rotation = Rotation;
            this.playerTarget = Vector3.Zero;
            this.Scale = Scale;
            this.graphicsDevice = graphicsDevice;
            buildBoundingSphere();


            

        }
        //Animated model constructor
        public LoadModel(Model Model, Vector3 Position, Vector3 Rotation,
        Vector3 Scale, GraphicsDevice GraphicsDevice,
        ContentManager Content)
        {
            this.baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(
           Rotation.Y, Rotation.X, Rotation.Z)
           * Matrix.CreateTranslation(Position);
            Console.WriteLine(Position);
            this.Model = Model;
            this.graphicsDevice = GraphicsDevice;
            this.content = Content;
            this.Position = Position;
            this.playerTarget = Vector3.Zero;
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
            buildBoundingSphereAnimated();
            

        }

        /// <summary>
        /// Method to create BoudingSphere at the model.
        /// This metod gather all Mesh from model creates bounding sphere for each of theme, then combine all and create one big. 
        /// </summary>

        private void buildBoundingSphereAnimated()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            List<BoundingSphere> spheres2 = new List<BoundingSphere>();

            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);

                spheres2.Add(new BoundingSphere(transformed.Center - new Vector3(transformed.Radius / 6, 0, 6 * transformed.Radius / 6), transformed.Radius / 3));
                spheres2.Add(new BoundingSphere(transformed.Center - new Vector3(spheres2[0].Center.X / 2, 0, spheres2[0].Center.Z / 2), transformed.Radius / 3));
                spheres2.Add(new BoundingSphere(transformed.Center - new Vector3(spheres2[1].Center.X / 2, 0, spheres2[1].Center.Z / 2), transformed.Radius / 3));
                spheres2.Add(new BoundingSphere(transformed.Center - new Vector3(spheres2[2].Center.X / 2, 0, spheres2[2].Center.Z / 2), transformed.Radius / 3));


                sphere = BoundingSphere.CreateMerged(sphere, transformed);

            }


            this.boundingSphere = sphere;
            this.spheres = spheres2;
        }
        public void BuildBoundingSphereMaterial()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            List<BoundingSphere> spheres2 = new List<BoundingSphere>();

            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);

                spheres2.Add(new BoundingSphere(transformed.Center, transformed.Radius / 4));
                sphere = BoundingSphere.CreateMerged(sphere, transformed);

            }
            this.boundingSphere = sphere;
            this.spheres = spheres2;
        }

        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            List<BoundingSphere> spheres2 = new List<BoundingSphere>();

            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(modelTransforms[mesh.ParentBone.Index]);
                if (mesh.Name.Contains("BoundingSphere"))
                {

                    spheres2.Add(transformed);

                }
                else
                {
                    sphere = BoundingSphere.CreateMerged(sphere, transformed);
                }
            }


            this.boundingSphere = sphere;
            this.spheres = spheres2;
        }
        /// <summary>
        /// Method to Draw model 
        /// </summary>
        /// <param name="View"></param>
        /// <param name="Projection"></param>
        public void Draw(GameCamera.FreeCamera camera)
        {


            baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(
           Rotation.Y, Rotation.X, Rotation.Z)
           * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (!mesh.Name.Contains("BoundingSphere"))
                {
                    Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                   * baseWorld;
                    this.localWorld = localWorld;
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {

                        BasicEffect effect = (BasicEffect)meshPart.Effect;
                        effect.World = localWorld;
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
                        effect.EnableDefaultLighting();
                        effect.Alpha = 0.9f;
                        if (Hit)
                        {
                            effect.AmbientLightColor = new Vector3(255, 0, 0);
                        }
                        else
                        {
                            effect.AmbientLightColor = new Vector3(0, 0, 0);
                        }
                        /*
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs(-1 * (float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower) + 1), 0.3f, 0.9f);
                    //Console.WriteLine(MathHelper.Clamp((Math.Abs(-1 * (float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower)), 0.3f, 0.9f));// a red light
                    effect.DirectionalLight0.Direction = lightDir;  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f) * MathHelper.Clamp((Math.Abs((float)Math.Sin(MathHelper.ToRadians(time - 1.58f)) / light.LightPower) + 1), 0.3f, 0.9f); ; // with green highlights
                    */
                        effect.Alpha = 0.4f;

                    }

                    mesh.Draw();
                }
            }
        }



        public void DrawOpague(GameCamera.FreeCamera camera, float Alpha, LoadModel model2)
        {


            baseWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(
           Rotation.Y, Rotation.X, Rotation.Z)
           * Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in model2.Model.Meshes)
            {
                if (!mesh.Name.Contains("BoundingSphere"))
                {
                    Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                   * baseWorld;
                    this.localWorld = localWorld;
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {

                        BasicEffect effect = (BasicEffect)meshPart.Effect;
                        effect.World = localWorld;
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
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
        public void Draw(GameCamera.FreeCamera camera, float time)
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
                  
                    effect.SetBoneTransforms(bones);
                    effect.World = localWorld;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

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
            if (list != null && Player.end)
            {
                Console.Out.WriteLine(Player.CurrentClip.Duration.TotalMilliseconds - Player.CurrentTime.TotalMilliseconds);
                Player.StartClip(list);
                list = null;
                animationChange = false;
            }
            // update world
            Matrix world = Matrix.CreateScale(Scale) *
   Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) *
   Matrix.CreateTranslation(Position);

                if (Player != null)
                {
                    Player.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
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


        public void switchAnimation(string nazwa, int howManyTimesPlay = -1)
        {
            if (howManyTimesPlay < 1)
            {
                Player.timesToPlay = -1;
                Player.looped = true;
            }
            else
            {
                Player.timesToPlay = howManyTimesPlay;
                Player.looped = false;
            }
            Player.howManyTimesPlayed = 0;
            if (animationChange == false) animationChange = true;
            //if (animationFlag==true)
            if (Player.CurrentClip == null)
            {
                AnimationClip clip = skinningData.AnimationClips[nazwa];//inne animacje to idle2 i run
                Player.StartClip(clip);
                animationChange = false;
            }
            else
            {
                if (Player.animationFlag)
                {
                    list = skinningData.AnimationClips[nazwa];

                    //AnimationClip clip = skinningData.AnimationClips[nazwa];//inne animacje to idle2 i run
                    // Player.StartClip(clip);
                    // animationChange = false;
                }
            }
            animationChange = false;
        }
        public BoundingBox CreateBoudingBox()
        {

            foreach (ModelMesh mesh in Model.Meshes)
            {
                if (!mesh.Name.Contains("BoundingSphere"))
                {
                    Matrix meshTransform = modelTransforms[mesh.ParentBone.Index];
                    b_box = BuildBoundingBox(mesh, meshTransform);

                    return b_box;
                }
            }
            return new BoundingBox();
        }
        private BoundingBox BuildBoundingBox(ModelMesh mesh, Matrix meshTransform)
        {
            // Create initial variables to hold min and max xyz values for the mesh
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);

            foreach (ModelMeshPart part in mesh.MeshParts)
            {
                // The stride is how big, in bytes, one vertex is in the vertex buffer
                // We have to use this as we do not know the make up of the vertex
                int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                // Find minimum and maximum xyz values for this mesh part
                Vector3 vertPosition = new Vector3();

                for (int i = 0; i < vertexData.Length; i++)
                {
                    vertPosition = vertexData[i].Position;

                    // update our values from this vertex
                    meshMin = Vector3.Min(meshMin, vertPosition);
                    meshMax = Vector3.Max(meshMax, vertPosition);
                }
            }

            // transform by mesh bone matrix
            meshMin = Vector3.Transform(meshMin, meshTransform);
            meshMax = Vector3.Transform(meshMax, meshTransform);

            // Create the bounding box
            BoundingBox box = new BoundingBox(meshMin, meshMax);

            return box;
        }
        public BoundingBox updateBoundingBox()
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), baseWorld);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new BoundingBox(min, max);
        }

    }
}

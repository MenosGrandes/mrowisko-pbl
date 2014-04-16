using Animations;
using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using DebugManager;
namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
       
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        List<Map.LoadModel> models = new List<Map.LoadModel>();
        Camera camera;
        MouseState lastMouseState;
        
        LoadModel anim;
        
         QuadTree quadTree;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            this.IsFixedTimeStep = false;
            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            List<Texture2D> texture = new List<Texture2D>();
            texture.Add(Content.Load<Texture2D>("grass"));
            texture.Add(Content.Load<Texture2D>("sand"));
            texture.Add(Content.Load<Texture2D>("rock"));
            texture.Add(Content.Load<Texture2D>("snow"));
            texture.Add(Content.Load<Texture2D>("terrain"));
            texture.Add(Content.Load<Texture2D>("tree"));
            texture.Add(Content.Load<Texture2D>("treeMap"));

            
            // Create a new SpriteBatch, which can be used to draw textures.
            device = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BoundingSphereRenderer.InitializeGraphics(device, 100);


           // terrain = new KlasyZMapa.MapRender(device, texture, Content, 1, Content.Load<Model>("mrowka_01"));

            camera = new FreeCamera(
                new Vector3(25650,9000,25650),
                MathHelper.ToRadians(0), // Turned around 153 degrees
                MathHelper.ToRadians(-45), // Pitched up 13 degrees
                GraphicsDevice);
            quadTree = new QuadTree(Vector3.Zero,texture,device,100,Content,(FreeCamera)camera);
            quadTree.Cull = true;
            
              
            anim = new LoadModel(
               Content.Load<Model>("mrowka_animowana1"),
               Vector3.Zero,Vector3.Up,
               new Vector3(100), GraphicsDevice, Content);
           AnimationClip clip = anim.skinningData.AnimationClips["idle1"];//inne animacje to idle2 i run
            anim.Player.StartClip(clip);
           lastMouseState = Mouse.GetState();

           models.Add(new LoadModel(Content.Load<Model>("mrowka_01"), Vector3.Zero, Vector3.Up, new Vector3(22.05f), GraphicsDevice));

            

             
        }

        /// <summary>w
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            updateAnt(gameTime);
            camera.Update(gameTime);
            quadTree.View =camera.View;
           quadTree.Projection = camera.Projection;
           quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);
            
            anim.Update(gameTime);
            base.Update(gameTime);
            
        }


        protected override void Draw(GameTime gameTime)
        {
            BoundingSphereRenderer.Render(models[0].boundingSphere, device, camera.View, camera.Projection, Color.Pink);

           // RasterizerState rasterizerState = new RasterizerState();
          //rasterizerState.FillMode = FillMode.WireFrame;
          // GraphicsDevice.RasterizerState = rasterizerState;   
            foreach (LoadModel model in models)
            {   
        if(camera.BoundingVolumeIsInView(model.BoundingSphere))  {
                         model.Draw(camera.View, camera.Projection);

                }

            }
            
           
           //anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
             
            quadTree.Draw( (FreeCamera)camera);
            base.Draw(gameTime);
        }

        void updateAnt(GameTime gameTime)
        {            KeyboardState keyState = Keyboard.GetState();


      

            if (keyState.IsKeyDown(Keys.Up) ) models[0].Position+= Vector3.Forward*100;
            if (keyState.IsKeyDown(Keys.Down)) models[0].Position += Vector3.Backward * 100;
            if (keyState.IsKeyDown(Keys.Left)) models[0].Position += Vector3.Left * 100;
            if (keyState.IsKeyDown(Keys.Right)) models[0].Position += Vector3.Right * 100;
                                   
        }
    }
}


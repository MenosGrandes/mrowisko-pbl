using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        List<LoadModel> models = new List<LoadModel>();
        Camera camera;
        MouseState lastMouseState;
        Mrowisko.MapRender terrain;


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

            base.Initialize();
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


            // Create a new SpriteBatch, which can be used to draw textures.
            device = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            models.Add(new LoadModel(Content.Load<Model>("mrowka_01"), Vector3.Zero, Vector3.Zero, new Vector3(1.0f), GraphicsDevice));
            terrain = new Mrowisko.MapRender( device,texture,  Content,20);
            camera = new FreeCamera(new Vector3(2000, 0, -2000),
                MathHelper.ToRadians(120), // Turned around 153 degrees
                MathHelper.ToRadians(360), // Pitched up 13 degrees
            GraphicsDevice);

            lastMouseState = Mouse.GetState();

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            updateCamera(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
           // RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;   
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (LoadModel model in models)
                if (camera.BoundingVolumeIsInView(model.BoundingSphere))
                    model.Draw(camera.View, camera.Projection);

            terrain.DrawTerrain(camera.View, camera.Projection);
            base.Draw(gameTime);
        }
        //terrain.Draw(camera.View, camera.Projection);
        void updateCamera(GameTime gameTime)
        {
            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

                ((FreeCamera)camera).Rotate(deltaX * .01f, -deltaY * .01f);

            
           
            Vector3 translation = Vector3.Zero;// Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += Vector3.Forward;
            if (keyState.IsKeyDown(Keys.S)) translation += Vector3.Backward;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right;
            // Move 3 units per millisecond, independent of frame rate
            translation *= 3 * (float)gameTime.ElapsedGameTime.
            TotalMilliseconds;
            // Move the camera
            ((FreeCamera)camera).Move(translation);
            // Update the camera
            camera.Update();
            // Update the mouse state
            lastMouseState = mouseState;
        }
    }
}


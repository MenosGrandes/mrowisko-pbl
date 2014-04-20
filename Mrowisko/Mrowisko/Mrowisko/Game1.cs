using Animations;
using GameCamera;
using Map;
using Logic;
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
        List<Map.LoadModel> inter = new List<Map.LoadModel>();
        public Camera camera;
        MouseState lastMouseState;
        Map.Water water;
        LoadModel anim;
        Light light;
        Control control;

         QuadTree quadTree;
                     //FPS COUNTER

         SpriteFont _spr_font;
         int _total_frames = 0;
         float _elapsed_time = 0.0f;
         int _fps = 0;


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

            control = new Control();
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



            device = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _spr_font = Content.Load<SpriteFont>("FPS");// you have on your project

            List<Texture2D> texture = new List<Texture2D>();
            texture.Add(Content.Load<Texture2D>("grass"));
            texture.Add(Content.Load<Texture2D>("sand"));
            texture.Add(Content.Load<Texture2D>("rock"));
            texture.Add(Content.Load<Texture2D>("snow"));
            texture.Add(Content.Load<Texture2D>("terrain"));
            texture.Add(Content.Load<Texture2D>("tree"));
            texture.Add(Content.Load<Texture2D>("treeMap"));

            camera = new FreeCamera(
new Vector3(texture[4].Width*50 / 2, texture[4].Width*50/10, texture[4].Width*50 / 2),
MathHelper.ToRadians(0), // Turned around 153 degrees
MathHelper.ToRadians(-45), // Pitched up 13 degrees
GraphicsDevice);
            light = new Light(this);
          
            quadTree = new QuadTree(Vector3.Zero,texture,device,50,Content,(FreeCamera)camera);
            quadTree.Cull = true;

           water = new Water(device, Content, texture[4].Width, 50);
           

           models.Add(new LoadModel(Content.Load<Model>("mrowka_01"), Vector3.Zero, Vector3.Up, new Vector3(20.05f), GraphicsDevice));

           inter = quadTree.ants.models;
         
                         
            /*
            anim = new LoadModel(
Content.Load<Model>("ludek2"),
Vector3.Zero, Vector3.Up,
new Vector3(100), GraphicsDevice, Content);
            AnimationClip clip = anim.skinningData.AnimationClips["Take 001"];//inne animacje to idle2 i run
            anim.Player.StartClip(clip); */
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


             _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }



            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            updateAnt(gameTime);
           
            quadTree.View =camera.View;
           quadTree.Projection = camera.Projection;
           quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);


            control.Move(); 
            camera.Update(gameTime);
            //anim.Update(gameTime);
            base.Update(gameTime);
            
        }


        protected override void Draw(GameTime gameTime)
        {
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            _total_frames++;


              
            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);


            water.DrawRefractionMap(camera.View);
            water.DrawReflectionMap((FreeCamera)camera);
            water.DrawWater(time, camera.View, camera.Projection, camera.View);   


        if(camera.BoundingVolumeIsInView(models[0].BoundingSphere))  {
            
                        models[0].Draw(camera.View, camera.Projection);
                         

              //  }

            }

        //light.DrawLight(gameTime, this);
             
            quadTree.Draw( (FreeCamera)camera);
           
            //anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                new Vector2(10.0f, 20.0f), Color.Tomato);
            spriteBatch.End();
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


using Animations;
using Debugger;
using GameCamera;
using Logic;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Logic.Building.AntBuildings.Granary;
using Logic.Building.AntBuildings.SeedFarms;
using System;

namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        List<InteractiveModel> IModel = new List<InteractiveModel>();
        Control control;


        GraphicsDeviceManager graphics;
        public GraphicsDevice device;
        float x, y, z;
        SpriteBatch spriteBatch;
        List<LoadModel> models = new List<Map.LoadModel>();
        List<LoadModel> inter = new List<Map.LoadModel>();
        public Camera camera;
        MouseState lastMouseState;
        Water water;
        LoadModel anim;
         QuadTree quadTree;
                     //FPS COUNTER
         int licznik;
         SpriteFont _spr_font;
         int _total_frames = 0;
         float _elapsed_time = 0.0f;
         int _fps = 0;
        
        
        MouseState currentMouseState;
        MouseState LastMouseState_2;
        int f = 0;
        Vector3 playerTarget;
        bool kolizja;
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

                        PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            pp.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            pp.BackBufferHeight = 600;
            pp.BackBufferWidth = 800;

            device = GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _spr_font = Content.Load<SpriteFont>("Fonts/FPS");// you have on your project

            List<Texture2D> texture = new List<Texture2D>();
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass"));
            texture.Add(Content.Load<Texture2D>("Textures/Ground/sand"));
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));
            texture.Add(Content.Load<Texture2D>("Textures/Ground/snow"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/terrain"));
            texture.Add(Content.Load<Texture2D>("Textures/Bilboard/tree"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/treeMap"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/colorHeigh"));
            texture.Add(Content.Load<Texture2D>("Textures/lisc"));
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass"));
            texture.Add(Content.Load<Texture2D>("Textures/muszle"));
            texture.Add(Content.Load<Texture2D>("Textures/select2"));

                    camera = new FreeCamera(
        new Vector3(texture[4].Width * 1 / 2, texture[4].Width * 1 / 10, texture[4].Width * 1 / 2),
        MathHelper.ToRadians(0), // Turned around 153 degrees
        MathHelper.ToRadians(-45), // Pitched up 13 degrees
        GraphicsDevice);

                    quadTree = new QuadTree(Vector3.Zero, texture, device, 1, Content, (FreeCamera)camera);
            quadTree.Cull = true;
            water = new Water(device, Content, texture[4].Width, 1);
           

           models.Add(new LoadModel(Content.Load<Model>("Models/mrowka_01"), Vector3.Zero, Vector3.Up, new Vector3(0.5f), GraphicsDevice));
     
            //inter = quadTree.ants.Models;

          // GraphicsDevice.BlendState = BlendState.AlphaBlend;
          // GraphicsDevice.BlendFactor = Color.Yellow;


            //animacja CHYBA dzia³a (nie wiem jak zrobiæ ¿eby by³o j¹ widaæ)
            //na starszych wersjach repozytorium dzia³a bez problemu (pliki x)
            //plik xml jest potrzebny ¿eby dzia³a³o prze³¹czanie, nie wiem czemu ale jak jest w folderze models to nie dzia³a 
           anim = new LoadModel(
Content.Load<Model>("animacja"),
Vector3.Zero, Vector3.Up,
new Vector3(1), GraphicsDevice, Content);
           AnimationClip clip = anim.skinningData.AnimationClips["run"];//inne animacje to idle2 i run
           anim.Player.StartClip(clip);
            lastMouseState = Mouse.GetState();

            BoundingSphereRenderer.InitializeGraphics(device, 33);
            AntGranary gr = new AntGranary(Content, new LoadModel(
Content.Load<Model>("Models/domek"),
new Vector3(300, 40, 300), Vector3.Zero,
new Vector3(1), GraphicsDevice), 10, 10, 10, 10, 10);


            HyacyntFarm hf = new HyacyntFarm(Content, new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(100,15,100), Vector3.Up,
new Vector3(1), GraphicsDevice), 10, 10, 10, 10);
            IModel.Add(hf);
            IModel.Add(gr);

            control = new Control(texture[11]);


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

            kolizja = false;


            currentMouseState = Mouse.GetState();





            KeyboardState keyState = Keyboard.GetState();
             _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
            if (keyState.IsKeyDown(Keys.C) && !keyState.IsKeyDown(Keys.C))
            {
                quadTree.Cull = !quadTree.Cull;
            }

            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
          
           
            quadTree.View =camera.View;
           quadTree.Projection = camera.Projection;
            quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);


            control.View = camera.View;
            control.Projection = camera.Projection;
            control.models = models;
            control.device = device;
            control.Update(gameTime);

             foreach(LoadModel model in models)
             {
                      foreach(LoadModel model2 in models)
                     {    if(model2==model)
                     { break; }
                         if(model.BoundingSphere.Intersects(model2.BoundingSphere))
                         {
                             kolizja = true;
                         }
                     }
              }
             
            camera.Update(gameTime);
            anim.Update(gameTime);
            base.Update(gameTime);
            
        }


        protected override void Draw(GameTime gameTime)
        {
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;

            licznik = 0;
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;

            _total_frames++;






            water.DrawRefractionMap((FreeCamera)camera,quadTree);

          water.DrawReflectionMap((FreeCamera)camera,quadTree);
          

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer , Color.Black, 1.0f, 0);
            water.sky.DrawSkyDome((FreeCamera)camera);
            quadTree.Draw((FreeCamera)camera, time);

            water.DrawWater(time, (FreeCamera)camera);









            anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
            foreach (LoadModel model in models) { 
     if(camera.BoundingVolumeIsInView(models[0].BoundingSphere))  {
            
                     model.Draw(camera.View, camera.Projection);
                     BoundingSphereRenderer.Render(model.BoundingSphere, device, camera.View, camera.Projection,
                        (Matrix.CreateScale(model.Scale) * Matrix.CreateTranslation(model.Position)), new Color(0.3f, 0.4f, 0.2f));
                     licznik ++;

       }
            } 
      anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
      BoundingSphereRenderer.Render(anim.BoundingSphere, device, camera.View, camera.Projection,
                       (Matrix.CreateScale(1) * Matrix.CreateTranslation(anim.Position)), new Color(0.3f, 0.4f, 0.2f));
          
           
           foreach(InteractiveModel model in IModel)
           {
               if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
               {
                   model.Draw(camera.View, camera.Projection);
                   BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection,
                      (Matrix.CreateScale(10) * Matrix.CreateTranslation(model.Model.Position)), Color.Yellow);
    
               
               }
               
           }
             


            
            spriteBatch.Begin();
     /*       spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                new Vector2(10.0f, 20.0f), Color.Tomato);
                spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik),
                new Vector2(10.0f, 50.0f), Color.Tomato);
            spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", kolizja),
              new Vector2(10.0f, 80.0f), Color.Pink); */
            control.Draw(spriteBatch);

            spriteBatch.End();
               
            base.Draw(gameTime);
        }

       public void updateAnt(GameTime gameTime)
        {

            // Check if the player has reached the target, if not, move towards it. 

            float Speed = (float)30;
            if (models[0].Position.X > playerTarget.X)
            {
                models[0].Position += Vector3.Left * Speed;
            }
            if (models[0].Position.X < playerTarget.X)
            {
                models[0].Position += Vector3.Right * Speed;
            }

            if (models[0].Position.Z > playerTarget.Z)
            {
                models[0].Position += Vector3.Forward * Speed;
            }
            if (models[0].Position.Z < playerTarget.Z)
            {
                models[0].Position += Vector3.Backward * Speed;
            }



            
            /*KeyboardState keyState = Keyboard.GetState();
            MouseState MS = Mouse.GetState();
            Vector2 mouse_pos = new Vector2(MS.X, MS.Y);
            Vector3 mouse3d2 = CalculateMouse3DPosition();
            float Speed = (float)4;
            if (MS.LeftButton == ButtonState.Pressed)
            {

                if (mouse3d2.X > models[0].Position.X)
                {
                    while (mouse3d2.X > models[0].Position.X)
                    {
                        models[0].Position += Vector3.Right * Speed;
                    }

                }

                if (mouse3d2.X < models[0].Position.X)
                {
                    while (mouse3d2.X < models[0].Position.X)
                    {
                        models[0].Position += Vector3.Left * Speed;
                    }

                }


                if (mouse3d2.Z > models[0].Position.Z)
                {

                    while (mouse3d2.Z > models[0].Position.Z)
                    {
                        models[0].Position += Vector3.Backward * Speed;
                    }
                }

                if (mouse3d2.Z < models[0].Position.Z)
                {
                    while (mouse3d2.Z < models[0].Position.Z)
                    {
                        models[0].Position += Vector3.Forward * Speed;
                    }
                }


                if (keyState.IsKeyDown(Keys.Up)) models[0].Position += Vector3.Forward * 100;
                if (keyState.IsKeyDown(Keys.Down)) models[0].Position += Vector3.Backward * 100;
                if (keyState.IsKeyDown(Keys.Left)) models[0].Position += Vector3.Left * 100;
                if (keyState.IsKeyDown(Keys.Right)) models[0].Position += Vector3.Right * 100;

            }
        
             */ 
        }


        private void CalculateMouse3DPosition()
        {
            Plane GroundPlane = new Plane(0, 1, 0, 0); // x - lewo prawo Z- gora dol
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
            Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = device.Viewport.Unproject(nearsource,
                camera.Projection, camera.View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                camera.Projection, camera.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
           
            float? position = pickRay.Intersects(models[0].BoundingSphere);
              /*
            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0,0,0);
          
              */


           // Console.WriteLine(position);
            
            
        }
    
    
    }
}


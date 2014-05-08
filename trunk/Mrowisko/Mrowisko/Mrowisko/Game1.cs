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
using Logic.Units.Ants;
using Logic.Meterials;
using Logic.Meterials.MaterialCluster;

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
        List<InteractiveModel> models = new List<InteractiveModel>();
        List<InteractiveModel> inter = new List<InteractiveModel>();
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
            quadTree.shadow.RenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, true, device.DisplayMode.Format, DepthFormat.Depth24Stencil8);
            water = new Water(device, Content, texture[4].Width, 1);
           

           models.Add(new AntPeasant(10,10,10,10,10,10, new LoadModel(Content.Load<Model>("Models/mrowka_01"), Vector3.Zero, Vector3.Up, new Vector3(0.5f), GraphicsDevice),101 ));
           models.Add(new Log(new LoadModel(Content.Load<Model>("Models/stone2"), new Vector3(-150,14,-150), Vector3.Up, new Vector3(1), GraphicsDevice),3000));

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
            AntGranary gr = new AntGranary( new LoadModel(
Content.Load<Model>("Models/domek"),
new Vector3(300, 13, 300), Vector3.Zero,
new Vector3(1), GraphicsDevice), 10, 10, 10, 10, 10);


            HyacyntFarm hf = new HyacyntFarm( new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(100,15,100), Vector3.Up,
new Vector3(1), GraphicsDevice), 10, 10, 10, 10);
            IModel.Add(hf);
            IModel.Add(gr);

            control = new Control(texture[11]);
            inter.Add(models[0]);
            models.Add(gr);
            

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
            if (keyState.IsKeyDown(Keys.C) )
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rasterizerState;
            }

            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            foreach (InteractiveModel model in models)
            {
               // Console.WriteLine(model.GetType().Name);
                foreach (InteractiveModel model2 in models)
                {
                    if (model2 == model)
                    { break; }
                    if (model.Model.BoundingSphere.Intersects(model2.Model.BoundingSphere))
                    {

                          if(model.GetType().Name=="Log" && model2.GetType().Name=="AntPeasant")
                          {
                                                          
                              model2.gaterMaterial((Material)model);

                             
                               

                         
                          }
                        if(model.GetType().Name=="AntGranary" && model2.GetType().Name=="AntPeasant" &&((AntPeasant)model2).Capacity>0)
                        {

                            Player.addWood(((AntPeasant)model2).releaseMaterial());
                           
                        }
                        
                        //kolizja = true;
                        //model.Model.Scale -= new Vector3(0.001f, 0.001f, 0.001f);
                        //player.addWood();
                        //if (model.Scale.X < 0)
                        //{
                        //    model.Position = new Vector3(1000, 10000, 10000);
                        //}
                    }
                }

            }
           
            quadTree.View =camera.View;
           quadTree.Projection = camera.Projection;
            quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);


            control.View = camera.View;
            control.Projection = camera.Projection;
            control.models = inter;
            control.device = device;
            control.Update(gameTime);
            anim.Update(gameTime);
            camera.Update(gameTime);
           

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
            foreach (InteractiveModel model in models) { 
     if(camera.BoundingVolumeIsInView(model.Model.BoundingSphere))  {

         model.Draw(camera.View, camera.Projection);
                     BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection,
                         new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f));
                     licznik ++;

       }
            } 
      anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
      BoundingSphereRenderer.Render(anim.BoundingSphere, device, camera.View, camera.Projection,
                       new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f));
          
           
           foreach(InteractiveModel model in IModel)
           {
               if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
               {
                   model.Draw(camera.View, camera.Projection);
                 
    
               
               }
               
           }
             


            
            spriteBatch.Begin();
           spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                new Vector2(10.0f, 20.0f), Color.Tomato);
            Vector2 pos = new Vector2(graphics.PreferredBackBufferWidth - (graphics.PreferredBackBufferWidth / 10), 0);
            spriteBatch.Draw(quadTree.shadow.RenderTarget, pos, null, Color.White, 0f, Vector2.Zero, .1F, SpriteEffects.None, 0f);
               
                spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik),new Vector2(10.0f, 50.0f), Color.Tomato);

            spriteBatch.DrawString(_spr_font, string.Format("kolizja? ={0}", kolizja),new Vector2(10.0f, 80.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("Drewno mrowki={0}", ((AntPeasant)models[0]).Capacity), new Vector2(10.0f, 110.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("Drewno graczas={0}",Player.wood ), new Vector2(10.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("Drewno w klodzie={0}", ((Log)models[1]).ClusterSize), new Vector2(10.0f, 180.0f), Color.Pink); 

            control.Draw(spriteBatch);

            spriteBatch.End();
               
            base.Draw(gameTime);
        }

    


       
    
    
    }
}


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
using Logic.Building;
using Logic.Building.AntBuildings;
using SimpleStaticHelpers;
using Logic.EnviroModel;

namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private readonly Form1 Form;
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
        Vector3 S = new Vector3(250.0f, 0.0f, 250.0f);
        Vector3 pozycja = new Vector3(250.0f, 0.0f, 250.0f);
        int promien = 100;

        public Game1(Form1 form)
        {
            this.Form = form;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Form.ViewportSize.Width,
                PreferredBackBufferHeight = Form.ViewportSize.Height
            };
            graphics.PreparingDeviceSettings+=graphics_PreparingDeviceSettings;
            System.Windows.Forms.Control.FromHandle(Window.Handle).VisibleChanged += MainGame_VisibleChanged;
            
            Content.RootDirectory = "Content";

        }
        void MainGame_VisibleChanged(object sender, System.EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(Window.Handle).Visible)
                System.Windows.Forms.Control.FromHandle(Window.Handle).Visible = false;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = Form.CanvasHandle;
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
            Mouse.WindowHandle = this.Form.Handle;
            
            this.IsFixedTimeStep = false;
            base.Initialize();
            this.IsMouseVisible = true;
            base.Initialize();
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
new Vector3(texture[4].Width * 1 / 2, texture[4].Width * 1 / 3, texture[4].Width * 1 / 2),
MathHelper.ToRadians(0), // Turned around 153 degrees
MathHelper.ToRadians(-45), // Pitched up 13 degrees
GraphicsDevice);

            quadTree = new QuadTree(Vector3.Zero, texture, device, 1, Content, (FreeCamera)camera);
            quadTree.Cull = true;
            quadTree.shadow.RenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, true, device.DisplayMode.Format, DepthFormat.Depth24Stencil8);
            water = new Water(device, Content, texture[4].Width, 1);

            lastMouseState = Mouse.GetState();

            BoundingSphereRenderer.InitializeGraphics(device, 33);
         
            CreatorController.content = Content;
            CreatorController.device = device;
            CreatorController.models.AddRange(models);
            

           // CreatorController.models.Add(m);
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

            CreatorController.View = camera.View;
            CreatorController.Projection = camera.Projection;
            currentMouseState = Mouse.GetState();
            CreatorController.mouseState = currentMouseState;
            CreatorController.CalculateMouse3DPosition();
         //   Console.WriteLine(CreatorController.MousePosition);
           // Console.WriteLine(CreatorController.mouseState);



            KeyboardState keyState = Keyboard.GetState();
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Second has passed



            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;

            }

            if (keyState.IsKeyDown(Keys.C))
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.FillMode = FillMode.WireFrame;
                GraphicsDevice.RasterizerState = rasterizerState;
            }


            quadTree.View = camera.View;
            quadTree.Projection = camera.Projection;
            quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);



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






            water.DrawRefractionMap((FreeCamera)camera, quadTree);

            water.DrawReflectionMap((FreeCamera)camera, quadTree);


            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            water.sky.DrawSkyDome((FreeCamera)camera);
            quadTree.Draw((FreeCamera)camera, time);

            water.DrawWater(time, (FreeCamera)camera);




           foreach(InteractiveModel model in CreatorController.models)
           {
               if(((FreeCamera)camera).BoundingVolumeIsInView(model.Model.BoundingSphere))
               model.Model.Draw(((FreeCamera)camera).View, ((FreeCamera)camera).Projection);
               if(model.selected==true)
               {
                   BoundingSphereRenderer.Render(model.Model.BoundingSphere, CreatorController.device, camera.View, camera.Projection, Color.Red,Color.Beige,Color.Gainsboro);
               }
           }



                 


            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                 new Vector2(10.0f, 20.0f), Color.Tomato);
            /*
            spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik), new Vector2(10.0f, 50.0f), Color.Tomato);

            spriteBatch.DrawString(_spr_font, string.Format("kolizja? ={0}", kolizja), new Vector2(10.0f, 80.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("D m={0}", ((AntPeasant)models[0]).wood2), new Vector2(10.0f, 110.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("K m={0}", ((AntPeasant)models[0]).rock2), new Vector2(140.0f, 110.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("D g={0}", Player.wood), new Vector2(10.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.stone), new Vector2(130.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("h g={0}", Player.hyacynt), new Vector2(240.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("d g={0}", Player.dicentra), new Vector2(350.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("heli g={0}", Player.chelidonium), new Vector2(550.0f, 140.0f), Color.Pink);

            spriteBatch.DrawString(_spr_font, string.Format("Drewno w klodzie={0}", ((Log)models[1]).ClusterSize), new Vector2(10.0f, 180.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("Kamien w skale={0}", ((Rock)models[2]).ClusterSize), new Vector2(10.0f, 220.0f), Color.Pink);
            
            spriteBatch.DrawString(_spr_font, string.Format("rot={0}", models[0].Model.Rotation), new Vector2(10.0f, 260.0f), Color.Pink); 
            spriteBatch.DrawString(_spr_font, string.Format("Kamien w skale={0}", ((Rock)models[2]).ClusterSize), new Vector2(10.0f, 220.0f), Color.Pink);
          
            control.Draw(spriteBatch);
             */
            spriteBatch.End();

            base.Draw(gameTime);
        }

      


    }
}


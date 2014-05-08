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
        public LightsAndShadows.Shadow shadow;
        public LightsAndShadows.Light light;
        Effect hiDefShadowEffect;

        Matrix world = Matrix.CreateTranslation(Vector3.Zero);
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
            light = new LightsAndShadows.Light(0.7f, 0.4f, new Vector3(513, 100, 513));
            shadow = new LightsAndShadows.Shadow();
                        PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            pp.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            pp.BackBufferHeight = 600;
            pp.BackBufferWidth = 800;
           
           
            hiDefShadowEffect = Content.Load<Effect>("Effects/Shadows");
            device = GraphicsDevice;
            shadow.RenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, DepthFormat.Depth24Stencil8);
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

            device.SetRenderTarget(shadow.RenderTarget);
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
           
            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            
            licznik = 0;
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;

            _total_frames++;

          
            shadow.UpdateLightData(0.6f, light.lightPosChange(time), (FreeCamera)camera);
            shadow.setShadowMap();
            device.SetRenderTarget(null);
            PopulateShadowEffect("ShadowMap");
            
            foreach (EffectPass pass in hiDefShadowEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
               
                quadTree.basicDraw();
                
             
            }
            
          
           

            device.SetRenderTarget(null);
           
           // PopulateShadowEffect(false);


            
            /*
            water.DrawRefractionMap((FreeCamera)camera,quadTree);

          water.DrawReflectionMap((FreeCamera)camera,quadTree);
          

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer , Color.Black, 1.0f, 0);
            water.sky.DrawSkyDome((FreeCamera)camera);
            quadTree.Draw((FreeCamera)camera, time);
           
            water.DrawWater(time, (FreeCamera)camera);






            anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
            foreach (LoadModel model in models)
            {
                if (camera.BoundingVolumeIsInView(models[0].BoundingSphere))
                {

                    model.Draw(camera.View, camera.Projection);
                    BoundingSphereRenderer.Render(model.BoundingSphere, device, camera.View, camera.Projection,
                       (Matrix.CreateScale(model.Scale) * Matrix.CreateTranslation(model.Position)), new Color(0.3f, 0.4f, 0.2f));
                    licznik++;

                }
            }
            anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);
            BoundingSphereRenderer.Render(anim.BoundingSphere, device, camera.View, camera.Projection,
                             (Matrix.CreateScale(1) * Matrix.CreateTranslation(anim.Position)), new Color(0.3f, 0.4f, 0.2f));


            foreach (InteractiveModel model in IModel)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    model.Draw(camera.View, camera.Projection);
              

                }

            }



            
            */
            
            spriteBatch.Begin();
           spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                new Vector2(10.0f, 20.0f), Color.Tomato);
            Vector2 pos = new Vector2(graphics.PreferredBackBufferWidth - (graphics.PreferredBackBufferWidth / 10), 0);
            spriteBatch.Draw(shadow.RenderTarget, pos, null, Color.White, 0f, Vector2.Zero, .1F, SpriteEffects.None, 0f);
               
                spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik),new Vector2(10.0f, 50.0f), Color.Tomato);

            spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", kolizja),new Vector2(10.0f, 80.0f), Color.Pink); 
            control.Draw(spriteBatch);

            spriteBatch.End();
               
            base.Draw(gameTime);
        }


        private void PopulateShadowEffect(string techniqueName)
        {
            
            hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques[techniqueName];
            hiDefShadowEffect.Parameters["xView"].SetValue(camera.View);
            hiDefShadowEffect.Parameters["xProjection"].SetValue(camera.Projection);
            hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(Matrix.Identity * shadow.lightsViewProjectionMatrix);
            hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
            hiDefShadowEffect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);
           
        }
        private void DrawModel(Model model, Matrix wMatrix, string technique, LightsAndShadows.Light light, LightsAndShadows.Shadow shadow, float time)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            int i = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques[technique];
                    currentEffect.Parameters["xWorldViewProjection"].SetValue(worldMatrix * camera.View * camera.Projection);
                    //currentEffect.Parameters["xTexture"].SetValue(textures[i++]);
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xLightPos"].SetValue(light.lightPosChange(time));
                    currentEffect.Parameters["xLightPower"].SetValue(light.LightPower);
                    currentEffect.Parameters["xAmbient"].SetValue(light.Ambient);
                    currentEffect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * shadow.lightsViewProjectionMatrix);
                }
                mesh.Draw();
            }
        }
    
    }
}


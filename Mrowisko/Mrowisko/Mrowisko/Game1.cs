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
using Logic.Meterials.MaterialCluster;
using Logic.Units.Ants;
using Logic.Building.AntBuildings;
using Logic.Meterials;
using Logic.Building;

namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        List<InteractiveModel> models = new List<InteractiveModel>();
        List<InteractiveModel> inter = new List<InteractiveModel>(); 
        List<InteractiveModel> IModel = new List<InteractiveModel>();

        Control control;
        GraphicsDeviceManager graphics;
        public GraphicsDevice device;
        float x, y, z;
        SpriteBatch spriteBatch;
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
            light = new LightsAndShadows.Light(0.7f, 0.4f, new Vector3(2048, 1200, 2048));
            shadow = new LightsAndShadows.Shadow();
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            pp.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            pp.BackBufferHeight = 600;
            pp.BackBufferWidth = 800;


            hiDefShadowEffect = Content.Load<Effect>("Effects/Shadows");
            device = GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            shadow.RenderTarget = new RenderTarget2D(device, 4096, 4096, false, pp.BackBufferFormat, DepthFormat.Depth24Stencil8);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _spr_font = Content.Load<SpriteFont>("Fonts/FPS");// you have on your project

            #region Tekstury
            List<Texture2D> texture = new List<Texture2D>();
            //alphy do terenu
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_grass")); // alpha pomiedzy trawa 0 i trawa1
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_grass1")); // alpha pomiedzy 1 2
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_sand"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_forest_cover"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_stone")); // alpha pomiedzy 4 5
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_grass1")); // alpha pomiedzy 5 6
            texture.Add(Content.Load<Texture2D>("HeighMaps/texturemap_snow")); // alpha pomiedzy ciemne7 i jasne 8
            //0 - 6

            //tekstury terenu
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass1")); //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass1"));  // to mo縩a wyci规
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass1")); // to mo縩a wyci规
            texture.Add(Content.Load<Texture2D>("Textures/Ground/sand"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/forest_cover")); //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  // to mo縩a wyci规
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  // to mo縩a wyci规
            //7 - 14
            //heighmapa terenu
            texture.Add(Content.Load<Texture2D>("HeighMaps/terrain"));
            //15
            //tekstura od zaznaczenia
            texture.Add(Content.Load<Texture2D>("Textures/select2"));
            //16
            //mapy rozmieszczenia obiektow i bilboardow (w zaleznosci ile czego bedzie to sobie pododajesz)
            texture.Add(Content.Load<Texture2D>("HeighMaps/grassMap"));
            texture.Add(Content.Load<Texture2D>("Textures/Bilboard/tree"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/iguyMap"));
            texture.Add(Content.Load<Texture2D>("Textures/Bilboard/igua"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/szuwaryMap"));
            texture.Add(Content.Load<Texture2D>("Textures/Bilboard/szuwary"));
            //17 - 22

            //mapy rozmieszczenia obiektow (w zaleznosci ile czego bedzie to sobie pododajesz)
            texture.Add(Content.Load<Texture2D>("HeighMaps/koamienMap"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/trawaMap"));
            texture.Add(Content.Load<Texture2D>("HeighMaps/kzak1Map"));
            //23 - 25
            #endregion

            camera = new FreeCamera(
new Vector3(texture[4].Width * 1 / 2, texture[4].Width * 1 / 10, texture[4].Width * 1 / 2),
MathHelper.ToRadians(0), // Turned around 153 degrees
MathHelper.ToRadians(-45), // Pitched up 13 degrees
GraphicsDevice);

            quadTree = new QuadTree(Vector3.Zero, texture, device, 1, Content, (FreeCamera)camera);
            quadTree.Cull = true;

            water = new Water(device, Content, texture[4].Width, 1);


            models.Add(new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(Content.Load<Model>("Models/mrowka_01"), new Vector3(0, 0, 0), new Vector3(0, 6, 0), new Vector3(0.5f), GraphicsDevice, light), 10000, 10));
           models.Add(new Log(new LoadModel(Content.Load<Model>("Models/stone2"), new Vector3(-150, 14, -150), Vector3.Up, new Vector3(1), GraphicsDevice, light), 3000));
        models.Add(new Rock(new LoadModel(Content.Load<Model>("Models/stone2"), new Vector3(-450, 14, -150), Vector3.Up, new Vector3(1), GraphicsDevice,light), 5000));
           // models.Add(new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(Content.Load<Model>("Models/mrowka_01"), new Vector3(250.0f, 0.0f, 250.0f), Vector3.Up, new Vector3(0.5f), GraphicsDevice, light), 10000, 10));      //

           // models.Add(new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(Content.Load<Model>("Models/mrowka_01"), new Vector3(300, 12, 300), Vector3.Up, new Vector3(0.5f), GraphicsDevice, light), 10000, 10));
            // models.Add(new Log(new LoadModel(Content.Load<Model>("Models/stone2"), new Vector3(-150, 14, -150), Vector3.Up, new Vector3(1), GraphicsDevice), 610));
            ///  models.Add(new Rock(new LoadModel(Content.Load<Model>("Models/stone2"), new Vector3(-450, 14, -150), Vector3.Up, new Vector3(1), GraphicsDevice), 5000));

            IModel.Add(new BuildingPlace(new LoadModel(Content.Load<Model>("Models/BuildingPlace"),new Vector3(100,15,100),Vector3.Zero,new Vector3(1),device,light),0,0,0,0));
            models.Add(IModel[0]);
            /*
            anim = new LoadModel(
 Content.Load<Model>("grasshopper"),
 Vector3.Zero, Vector3.Up,
 new Vector3(1), GraphicsDevice, Content, light);
            AnimationClip clip = anim.skinningData.AnimationClips["Jump"];//inne animacje to idle2 i run
            anim.Player.StartClip(clip);
            lastMouseState = Mouse.GetState();

            BoundingSphereRenderer.InitializeGraphics(device, 33);
            AntGranary gr = new AntGranary(new LoadModel(
Content.Load<Model>("Models/domek"),
new Vector3(600, 13, 300), Vector3.Zero,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 10, 10);


            HyacyntFarm hf = new HyacyntFarm(new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(100, 15, 100), Vector3.Up,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 5000, 10);

            DicentraFarm df = new DicentraFarm(new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(200, 15, 100), Vector3.Up,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 5000, 30);


            ChelidoniumFarm hef = new ChelidoniumFarm(new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(300, 15, 100), Vector3.Up,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 5000, 30);
             *             IModel.Add(hf);
            IModel.Add(gr);
            IModel.Add(df);
            IModel.Add(hef);
             *             inter.Add(models[0]);

                  */


            control = new Control(texture[11], quadTree);
            //inter.Add(models[1]);

            //inter.Add(models[3]);
            /*    
                for (int i = 4; i < 16;i++ )
                {
                    inter.Add(models[i]);

                } 
                //models.Add(gr);
                 */


         //   models.Add(gr);

            StaticHelpers.StaticHelper.Content = Content;
            StaticHelpers.StaticHelper.Device = device;
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
            if (IModel[0].GetType() == typeof(BuildingPlace)) { 
            if (keyState.IsKeyDown(Keys.J))
            {
                Vector3 Pos = IModel[0].Model.Position;
                models.Remove(IModel[0]);
                IModel.Remove(IModel[0]);
                IModel.Add( new AntGranary(new LoadModel(
                    Content.Load<Model>("Models/domek"),
                    Pos, Vector3.Zero,
                    new Vector3(1), GraphicsDevice, light), 10, 10, 10, 10, 10));
                models.Add(IModel[0]);

            }
            if (keyState.IsKeyDown(Keys.K))
            {
                Vector3 Pos = IModel[0].Model.Position;
                models.Remove(IModel[0]);
                IModel.Remove(IModel[0]);
                IModel.Add(new HyacyntFarm(new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(100, 15, 100), Vector3.Up,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 5000, 10));
                models.Add(IModel[0]);
            }
            if (keyState.IsKeyDown(Keys.L))
            {
                Vector3 Pos = IModel[0].Model.Position;
                models.Remove(IModel[0]);
                IModel.Remove(IModel[0]);
                IModel.Add(new ChelidoniumFarm(new LoadModel(
Content.Load<Model>("Models/domek2"),
new Vector3(300, 15, 100), Vector3.Up,
new Vector3(1), GraphicsDevice, light), 10, 10, 10, 5000, 30));
                models.Add(IModel[0]);
            }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (InteractiveModel model in models)
            {
                model.Update(gameTime);
                model.Model.Update(gameTime);

                foreach (InteractiveModel model2 in models)
                {
                    if (model2 == model)
                    { break; }
                    if (model.Model.BoundingSphere.Intersects(model2.Model.BoundingSphere))
                    {

                        //model2.Model.Position -= new Vector3(model2.Model.BoundingSphere.Radius, 0, model2.Model.BoundingSphere.Radius);
                        model2.Model.Position = model2.Model.tempPosition;
                        model.Model.Position = model.Model.tempPosition;

                        if (model.GetType().BaseType == typeof(Material) && model2.GetType() == typeof(AntPeasant) || model2.GetType().BaseType == typeof(Material) && model.GetType() == typeof(AntPeasant))
                        {
                           
                            if (model2.GetType() == typeof(AntPeasant))
                            {
                             
                                if (((AntPeasant)model2).gaterTime < ((AntPeasant)model2).elapsedTime)
                                {
                                    model2.gaterMaterial((Material)model);
                                }
                            }
                            else if (model2.GetType().BaseType == typeof(Material))
                            {


                                if (((AntPeasant)model).gaterTime < ((AntPeasant)model).elapsedTime)
                                {
                                    model.gaterMaterial((Material)model2);
                                }

                            }


                        }
                        if (model2.GetType().BaseType == typeof(Ant) && model.GetType().BaseType == typeof(Ant))
                        {
                        }

                        if (model.GetType().Name == "AntGranary" && model2.GetType().Name == "AntPeasant" && ((AntPeasant)model2).Capacity > 0)
                        {
                            //  Console.WriteLine(((AntPeasant)model2).Capacity);
                            Player.addMaterial(((AntPeasant)model2).releaseMaterial());

                        }

                    }
                }

            }
                 
                foreach (Building model in IModel)
                {
                  
                        model.Update(gameTime);
                        model.Model.Update(gameTime);


                        if (model.GetType().BaseType == typeof(SeedFarm))
                        {
                            if (((SeedFarm)model).timeElapsed > ((SeedFarm)model).CropTime)
                            {
                                Player.addMaterial(model.addCrop());
                                ((SeedFarm)model).timeElapsed = 0;
                            }
                        }

                    
                }              
            quadTree.View = camera.View;
            quadTree.Projection = camera.Projection;
            quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);


            control.View = camera.View;
            control.Projection = camera.Projection;
            control.models = models;
            control.device = device;
            control.Update(gameTime);
            


            camera.Update(gameTime);
          //  anim.Update(gameTime);
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
            device.SetRenderTarget(shadow.RenderTarget);
            PopulateShadowEffect("ShadowMap");

            /*  foreach (EffectPass pass in hiDefShadowEffect.CurrentTechnique.Passes)
              {

                  pass.Apply();
                  quadTree.basicDraw();
              }*/

            foreach (InteractiveModel model in models)
            {
                hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques["Technique1"];
                //hiDefShadowEffect.Parameters["Model"].SetValue(true);
                hiDefShadowEffect.Parameters["LightView"].SetValue(shadow.lightsView);
                hiDefShadowEffect.Parameters["LightProjection"].SetValue(shadow.lightsProjection);
                // hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * (Matrix.Identity* model.Model.baseWorld));
                //hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
                //hiDefShadowEffect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);


                foreach (EffectPass pass in hiDefShadowEffect.CurrentTechnique.Passes)
                {
                    // pass.Apply();
                    // if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                    //  {


                    foreach (ShadowCasterObject shadowCaster in model.Model.shadowCasters)
                    {
                        hiDefShadowEffect.Parameters["World"].SetValue(shadowCaster.World);
                        pass.Apply();
                        device.SetVertexBuffer(shadowCaster.VertexBuffer);
                        device.Indices = shadowCaster.IndexBuffer;
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, shadowCaster.StreamOffset, 0, shadowCaster.VerticesCount, shadowCaster.StartIndex, shadowCaster.PrimitiveCount);
                    }
                }
            }
            shadow.setShadowMap();
            device.SetRenderTarget(null);

            /*
                hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques["ShadowedScene"];
                            
                hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * Matrix.Identity);
                hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(shadow.woldsViewProjection * Matrix.Identity);
                hiDefShadowEffect.Parameters["shadowTexture"].SetValue(shadow.ShadowMap);
                foreach (EffectPass pass in hiDefShadowEffect.CurrentTechnique.Passes)
                {

                    pass.Apply();
                    quadTree.basicDraw();
                }
            
                foreach (InteractiveModel model in IModel)
                {
                    hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques["ShadowedScene"];
                    //hiDefShadowEffect.Parameters["Model"].SetValue(true);
                    hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * Matrix.Identity);
                    
                    // hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * (Matrix.Identity* model.Model.baseWorld));
                    //hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
                    //hiDefShadowEffect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);


                    foreach (EffectPass pass in hiDefShadowEffect.CurrentTechnique.Passes)
                    {
                        // pass.Apply();
                        // if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                        //  {


                        foreach (ShadowCasterObject shadowCaster in model.Model.shadowCasters)
                        {
                            hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(shadow.woldsViewProjection * Matrix.Identity * shadowCaster.World);
                           
                            pass.Apply();
                            device.SetVertexBuffer(shadowCaster.VertexBuffer);
                            device.Indices = shadowCaster.IndexBuffer;
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, shadowCaster.StreamOffset, 0, shadowCaster.VerticesCount, shadowCaster.StartIndex, shadowCaster.PrimitiveCount);
                        }
                    }
                }
        */

            water.DrawRefractionMap((FreeCamera)camera, quadTree);

            water.DrawReflectionMap((FreeCamera)camera, quadTree);


            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            water.sky.DrawSkyDome((FreeCamera)camera);


            quadTree.Draw((FreeCamera)camera, time, shadow, light);

            water.DrawWater(time, (FreeCamera)camera);







         //   anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position, time);
            foreach (InteractiveModel model in models)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {

                    model.Draw(camera.View, camera.Projection);
                    //   BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection,
                    //    new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f), new Color(0.3f, 0.4f, 0.2f));
                      BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));
                    licznik++;

                }
            }
             
            foreach (InteractiveModel model in IModel)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    // BoundingSphereRenderer.Render(model.Model.spheres, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));
                    BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection, new Color(0.1f, 0.1f, 0.1f), new Color(0.1f, 0.1f, 0.1f), new Color(0.1f, 0.1f, 0.1f));

                    model.Draw(camera.View, camera.Projection);



                }

            }
            








            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                 new Vector2(10.0f, 20.0f), Color.Tomato);


            spriteBatch.DrawString(_spr_font, string.Format("kolizja? ={0}", kolizja), new Vector2(10.0f, 80.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("D m={0}", ((AntPeasant)models[0]).wood2), new Vector2(10.0f, 110.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("K m={0}", ((AntPeasant)models[0]).rock2), new Vector2(140.0f, 110.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("D g={0}", Player.wood), new Vector2(10.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.stone), new Vector2(130.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("h g={0}", Player.hyacynt), new Vector2(240.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("d g={0}", Player.dicentra), new Vector2(350.0f, 140.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("heli g={0}", Player.chelidonium), new Vector2(550.0f, 140.0f), Color.Pink);
            /*
          spriteBatch.DrawString(_spr_font, string.Format("Drewno w klodzie={0}", ((Log)models[1]).ClusterSize), new Vector2(10.0f, 180.0f), Color.Pink);
          spriteBatch.DrawString(_spr_font, string.Format("Kamien w skale={0}", ((Rock)models[2]).ClusterSize), new Vector2(10.0f, 220.0f), Color.Pink);
           
        spriteBatch.DrawString(_spr_font, string.Format("iloscMrowek={0}", models.Count), new Vector2(10.0f, 220.0f), Color.Pink);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik), new Vector2(10.0f, 50.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Yaw), new Vector2(10.0f, 150.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Pitch), new Vector2(10.0f, 250.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Position), new Vector2(10.0f, 350.0f), Color.Tomato);
       */
            //spriteBatch.DrawString(_spr_font, string.Format("obrot ={0}", models[0].Model.Rotation), new Vector2(10.0f, 80.0f), Color.Pink);
           
            control.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void PopulateShadowEffect(string techniqueName)
        {

            hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques["ShadowMap"];

            hiDefShadowEffect.Parameters["xView"].SetValue(camera.View);
            hiDefShadowEffect.Parameters["xProjection"].SetValue(camera.Projection);
            hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * Matrix.Identity);
            hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(shadow.woldsViewProjection);
            hiDefShadowEffect.Parameters["Model"].SetValue(false);
            //hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);

        }




    }
}


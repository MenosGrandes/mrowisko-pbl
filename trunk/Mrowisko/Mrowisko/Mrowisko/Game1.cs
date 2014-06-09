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
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using Controlers;
using Controlers.CursorEnum;
using Logic.Triggers;
using Logic.Player;
using Microsoft.Xna.Framework.Audio;
using SoundController;

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
        List<InteractiveModel> Enemys = new List<InteractiveModel>();
        List<LaserTrigger> timeTriggers = new List<LaserTrigger>();
        List<Curve3D> curvesForLaser = new List<Curve3D>();
        List<List<PointInTime>> pointsForLasers = new List<List<PointInTime>>();
        Logic.Control control;
        ControlEnemy e= new ControlEnemy();
        InteractiveModel spider;
        //HUD.LifeBar hp = new HUD.LifeBar(5.0f);


        //-----------
        float liczba = 0;
        //-----------
        GraphicsDeviceManager graphics;
        public GraphicsDevice device;
        float x, y, z;
        SpriteBatch spriteBatch;
        public Camera camera;
        MouseState lastMouseState;
        Water water;
        LoadModel mrowka, krolowa, pajak, konik, silacz;
        QuadTree quadTree;
        //FPS COUNTER
        int licznik;
        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        float time = 0;

        MouseState currentMouseState;
        MouseState LastMouseState_2;
        int f = 0;
        Vector3 playerTarget;
        bool kolizja;
        public LightsAndShadows.Shadow shadow;
        public LightsAndShadows.Light light;
        Effect hiDefShadowEffect;

        Matrix world = Matrix.CreateTranslation(Vector3.Zero);

        Particles.ParticleSystem explosionParticles;
        Particles.ParticleSystem explosionSmokeParticles;
        Particles.ParticleSystem projectileTrailParticles;
        Particles.ParticleSystem smokePlumeParticles;
        Particles.ParticleSystem fireParticles;

        List<Particles.Projectile> projectiles = new List<Particles.Projectile>();

        TimeSpan timeToNextProjectile = TimeSpan.Zero;
        Random random = new Random();
        private Effect animHiDefShadowEffect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Construct our particle system components.
           // explosionParticles = new Particles.ParticleSystems.ExplosionParticleSystem(this, Content);
           // explosionSmokeParticles = new Particles.ParticleSystems.ExplosionSmokeParticleSystem(this, Content);
            //projectileTrailParticles = new Particles.ParticleSystems.ProjectileTrailParticleSystem(this, Content);
            //smokePlumeParticles = new Particles.ParticleSystems.SmokePlumeParticleSystem(this, Content);
            //fireParticles = new Particles.ParticleSystems.FireParticleSystem(this, Content);
            
            //fireParticles.Interval = 2.5f;

            // Set the draw order so the explosions and fire
            // will appear over the top of the smoke.
         //   smokePlumeParticles.DrawOrder = 100;
          //  explosionSmokeParticles.DrawOrder = 200;
           // projectileTrailParticles.DrawOrder = 300;
            //explosionParticles.DrawOrder = 400;
           // fireParticles.DrawOrder = 500;

            // Register the particle system components.
            //Components.Add(explosionParticles);
            //Components.Add(explosionSmokeParticles);
            //Components.Add(projectileTrailParticles);
            //Components.Add(smokePlumeParticles);
            //Components.Add(fireParticles);

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
            #region KURSORY i okno
            StaticHelpers.StaticHelper.DeviceManager = graphics;
            WindowController.window = (Form)Form.FromHandle(this.Window.Handle);
            MouseCursorController.LoadCustomCursor(@"Content/Cursors/TronNormal.ani");
            MouseCursorController.LoadCustomCursor(@"Content/Cursors/TronBusy.ani");
            MouseCursorController.LoadCustomCursor(@"Content/Cursors/TronAlternate.ani");
            MouseCursorController.LoadCustomCursor(@"Content/Cursors/gam1232.ani");

            MouseCursorController.stage = Controlers.CursorEnum.CursorStage.Normal;
            #endregion
            
            #region Light Shadow
            light = new LightsAndShadows.Light(0.7f, 0.4f, new Vector3(2048, 1200, 2048));
            shadow = new LightsAndShadows.Shadow();
            LoadModelsFromFile._light = light;
            #endregion
            #region PresentationParameters
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            pp.DepthStencilFormat = DepthFormat.Depth24Stencil8;

            #endregion
            #region PointsForLaser
            pointsForLasers.Add(new List<PointInTime>());
            pointsForLasers[0].Add(new PointInTime(new Vector3(75, 40, 450), 0)) ;
            pointsForLasers[0].Add(new PointInTime(new Vector3(30, 40, 360), 2000)) ;
            pointsForLasers[0].Add(new PointInTime(new Vector3(120, 40, 300), 4000)) ;
            pointsForLasers[0].Add(new PointInTime(new Vector3(30, 40, 240), 6000) );
            pointsForLasers[0].Add(new PointInTime(new Vector3(120, 40, 180), 8000) );

            pointsForLasers[0].Add(new PointInTime(new Vector3(750, 40, 120), 10000)) ;
            pointsForLasers[0].Add(new PointInTime(new Vector3(60, 40, 60), 12000)) ;


            #endregion
            #region Curve
            curvesForLaser.Add(new Curve3D(pointsForLasers[0]));
            #endregion
            hiDefShadowEffect = Content.Load<Effect>("Effects/Shadows");
            animHiDefShadowEffect = Content.Load<Effect>("Effects/AnimatedShadow");
            device = GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            shadow.RenderTarget = new RenderTarget2D(device, 4096, 4096, false, pp.BackBufferFormat, DepthFormat.Depth24Stencil8);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _spr_font = Content.Load<SpriteFont>("Fonts/FPS");// you have on your project


            StaticHelpers.StaticHelper.Content = Content;
            StaticHelpers.StaticHelper.Device = device;
            
            #region loadFromFile
             
            Controlers.LoadModelsFromFile.Load();
            foreach (InteractiveModel i in LoadModelsFromFile.listOfAllInteractiveModelsFromFile)
            {
               // Console.WriteLine(models.GetType().BaseType.Name);
                if(i.GetType().BaseType==typeof(Building) ||i.GetType().BaseType==typeof(Material))
                {
                    IModel.Add(i);
                }else
                {
                    models.Add(i);
                }
            }
            #endregion   
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
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass0")); //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass"));  // to mo縩a wyci规
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass1")); // to mo縩a wyci规
            texture.Add(Content.Load<Texture2D>("Textures/Ground/sand"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/forest_cover")); //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/dirt"));  // to mo縩a wyci规
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
new Vector3(texture[4].Width * 1 / 2, texture[4].Width * 1 / 20, texture[4].Width * 1 / 2),
MathHelper.ToRadians(0), // Turned around 153 degrees
MathHelper.ToRadians(-45), // Pitched up 13 degrees
GraphicsDevice);

            quadTree = new QuadTree(Vector3.Zero, texture, device, 1, Content, (FreeCamera)camera);
            quadTree.Cull = true;

            water = new Water(device, Content, texture[4].Width, 1);




            control = new Logic.Control(texture[11], quadTree);
          

            //e.Ant = models[0];
           // e.Enemy = spider;
          //  e.Ants = Enemys;
         

            
           
            WindowController.setWindowSize(1366, 768, false);
            //models.Add(new AntPeasant(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/mrowka_01"), Vector3.Zero, Vector3.Zero, new Vector3(0.3f), StaticHelpers.StaticHelper.Device, light)));
           // models.Add(new TownCenter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/domek"), Vector3.Zero, Vector3.Zero, new Vector3(0.23f), StaticHelpers.StaticHelper.Device, light)));
            models.Add(new AntSpitter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/domek"), Vector3.Zero, Vector3.Zero, new Vector3(0.23f), StaticHelpers.StaticHelper.Device, light)));
            List<String> aa = new List<string>();
            aa.Add("s1");
            aa.Add("s2");
            SoundController.SoundController.content = Content;
           SoundController.SoundController.Initialize(aa);
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
                if (timeTriggers.Count<1)
                {
                    //UpdateFire();
                    //UpdateSmokePlume();
                    //UpdateExplosions(gameTime);
                    //UpdateProjectiles(gameTime);
                }




            KeyboardState keyState = Keyboard.GetState();
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
           
            
           // time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
          //  IModel[1].Model.Position=curee.GetPointOnCurve(time);

            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
/*
            #endregion wstepneBudowanie
            * */
            #region zmiany rozdzielczosci
            if(keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1))
            {
               // WindowController.setWindowSize(1366, 768, false);
                models[8].Attack(models[3]);
            }
            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2))
            {
                WindowController.setWindowSize(400, 250, false);
            }
            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3))
            {
                WindowController.setWindowSize(700, 432, false);
            }
            #endregion
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();
            #region timeTrigger

            for (int i = 0; i < timeTriggers.Count;i++ )
            {

                timeTriggers[i].Update(gameTime);
                if(timeTriggers[i].used==true)
                {
                    timeTriggers.Remove(timeTriggers[i]);
                }

            }
            #endregion
                foreach (InteractiveModel model in models)
                {
                    model.Update(gameTime);
                    model.Model.Update(gameTime);

                    if (control.selectedObject != null)
                    {
                        if (control.selectedObject.GetType().BaseType == typeof(Material))
                        {

                            model.setGaterMaterial((Material)control.selectedObject);
                        }
                    }
                    if (control.selectedObjectMouseOnlyMove != null)
                    {
                       // Console.WriteLine(control.selectedObjectMouseOnlyMove);
                        if (currentMouseState.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            if (control.selectedObjectMouseOnlyMove.GetType().BaseType == typeof(Material))
                            {
                                // window.Cursor = cursors[1];
                                MouseCursorController.stage = CursorStage.Gater;
                            }

                        }
                        else
                        {
                            MouseCursorController.stage = CursorStage.Normal;
                        }
                        if(control.selectedObjectMouseOnlyMove.GetType().BaseType==typeof(Building))
                        {
                            MouseCursorController.stage = CursorStage.Go;

                        }
                    }
                    else
                    {
                        MouseCursorController.stage = CursorStage.Normal;
                    }
                    foreach (InteractiveModel model2 in models)
                    {
                        model.Intersect(model2);
                    }
                }
             
            
                 
                foreach (InteractiveModel model in IModel)
                {
                  
                        model.Update(gameTime);
                        model.Model.Update(gameTime);


                        if (model.GetType().BaseType == typeof(SeedFarm))
                        {
                            if (((SeedFarm)model).timeElapsed > ((SeedFarm)model).CropTime)
                            {
                                Logic.Player.Player.addMaterial(model.addCrop());
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
            control.Models_Colision =IModel;


          //  e.gameTime = gameTime;
          //  e.Update();

            MouseCursorController.Update();
            camera.Update(gameTime);
           

          //  anim.Update(gameTime);
            base.Update(gameTime);

        }


        protected override void Draw(GameTime gameTime)
        {
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;

            //explosionParticles.SetCamera(camera.View, camera.Projection);
            //explosionSmokeParticles.SetCamera(camera.View, camera.Projection);
    //        projectileTrailParticles.SetCamera(camera.View, camera.Projection);
      //      smokePlumeParticles.SetCamera(camera.View, camera.Projection);
        //    fireParticles.SetCamera(camera.View, camera.Projection);





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
             //PopulateShadowEffect("ShadowMap");
             animHiDefShadowEffect.CurrentTechnique = animHiDefShadowEffect.Techniques["Technique1"];


             foreach (InteractiveModel model in models)
             {
                 hiDefShadowEffect.CurrentTechnique = hiDefShadowEffect.Techniques["Technique1"];
                 //hiDefShadowEffect.Parameters["Model"].SetValue(true);
                 hiDefShadowEffect.Parameters["LightView"].SetValue(shadow.lightsView);
                 hiDefShadowEffect.Parameters["LightProjection"].SetValue(shadow.lightsProjection);
                 animHiDefShadowEffect.Parameters["LightView"].SetValue(shadow.lightsView);
                 animHiDefShadowEffect.Parameters["LightProjection"].SetValue(shadow.lightsProjection);
                 // hiDefShadowEffect.Parameters["xLightsWorldViewProjection"].SetValue(shadow.lightsViewProjectionMatrix * (Matrix.Identity* model.Model.baseWorld));
                 //hiDefShadowEffect.Parameters["xWorldViewProjection"].SetValue(Matrix.Identity * camera.View * camera.Projection);
                 //hiDefShadowEffect.Parameters["xShadowMap"].SetValue(shadow.ShadowMap);

                 if (model.Model.Player == null)
                 {
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
                 else
                 {
                    
                     foreach (EffectPass pass in animHiDefShadowEffect.CurrentTechnique.Passes)
                     {
                         // pass.Apply();
                         // if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                         //  {


                         foreach (ShadowCasterObject shadowCaster in model.Model.shadowCasters)
                         {
                            
                             animHiDefShadowEffect.Parameters["World"].SetValue(shadowCaster.World);
                             animHiDefShadowEffect.Parameters["Bones"].SetValue(model.Model.Player.GetSkinTransforms());
                             pass.Apply();
                             device.SetVertexBuffer(shadowCaster.VertexBuffer);
                             device.Indices = shadowCaster.IndexBuffer;
                             device.DrawIndexedPrimitives(PrimitiveType.TriangleList, shadowCaster.StreamOffset, 0, shadowCaster.VerticesCount, shadowCaster.StartIndex, shadowCaster.PrimitiveCount);
                         }
                     }
                 }
             }
             shadow.setShadowMap();   
            device.SetRenderTarget(null);
          
            #region Odbicie i rozproszenie wody
            water.DrawRefractionMap((FreeCamera)camera, time, shadow, light,quadTree);

            water.DrawReflectionMap((FreeCamera)camera, time, shadow, light, quadTree);
            #endregion



            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            water.sky.DrawSkyDome((FreeCamera)camera);

            quadTree.Draw((FreeCamera)camera, time, shadow, light);

            water.DrawWater(time, (FreeCamera)camera);





            foreach (InteractiveModel model in models)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    if (model.Model.Player==null)
                    { model.Draw((FreeCamera)camera); }
                    else
                    { model.Draw((FreeCamera)camera, time); }
                   

                      BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection,
                       Color.Green, Color.Aquamarine, Color.White);
                      BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, Color.Black, Color.Yellow, Color.Red   );
                    licznik++;
                }
            }
             
            foreach (InteractiveModel model in IModel)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    // BoundingSphereRenderer.Render(model.Model.spheres, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));
                    BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));

                   // model.Draw(((camera)));
                    model.Draw((FreeCamera)camera);


                }

            }

           // spider.Draw((FreeCamera)camera);
            if (control.selectedObjectMouseOnlyMove != null)
            {
               // Console.WriteLine(control.selectedObjectMouseOnlyMove);
                switch (control.selectedObjectMouseOnlyMove.GetType().BaseType.Name)
                {
                    case "Material":
                        SoundController.SoundController.Play(SoundEnum.SelectedMaterial);
                        break;
                }
            }



            spriteBatch.Begin();
            spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
                 new Vector2(10.0f, 20.0f), Color.Tomato);

            spriteBatch.DrawString(_spr_font, string.Format("D g={0}", (control.selectedObjectMouseOnlyMove)), new Vector2(10.0f, 140.0f), Color.Pink);

           /* 
            spriteBatch.DrawString(_spr_font, string.Format("D g={0}", ((FreeCamera)camera).Position), new Vector2(10.0f, 140.0f), Color.Pink);
             spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.stone), new Vector2(130.0f, 240.0f), Color.Pink);
             spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.wood), new Vector2(230.0f, 240.0f), Color.Pink);

            spriteBatch.DrawString(_spr_font, string.Format("h g={0}", Player.hyacynt), new Vector2(340.0f, 240.0f), Color.Pink);
             spriteBatch.DrawString(_spr_font, string.Format("d g={0}", Player.dicentra), new Vector2(450.0f, 240.0f), Color.Pink);
             spriteBatch.DrawString(_spr_font, string.Format("heli g={0}", Player.chelidonium), new Vector2(550.0f, 240.0f), Color.Pink);
             
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

        /// <summary>
        /// Helper for updating the explosions effect.
        /// </summary>
        void UpdateExplosions(GameTime gameTime)
        {
            timeToNextProjectile -= gameTime.ElapsedGameTime;

            if (timeToNextProjectile <= TimeSpan.Zero)
            {
                // Create a new projectile once per second. The real work of moving
                // and creating particles is handled inside the Projectile class.
                projectiles.Add(new Particles.Projectile(explosionParticles,
                                               explosionSmokeParticles,
                                               projectileTrailParticles));

                timeToNextProjectile += TimeSpan.FromSeconds(1);
            }
        }


        /// <summary>
        /// Helper for updating the list of active projectiles.
        /// </summary>
        void UpdateProjectiles(GameTime gameTime)
        {
            int i = 0;

            while (i < projectiles.Count)
            {
                if (!projectiles[i].Update(gameTime))
                {
                    // Remove projectiles at the end of their life.
                    projectiles.RemoveAt(i);
                }
                else
                {
                    // Advance to the next projectile.
                    i++;
                }
            }
        }


        /// <summary>
        /// Helper for updating the smoke plume effect.
        /// </summary>                                                                               \
        /// ] ,l-p
        void UpdateSmokePlume()
        {
            // This is trivial: we just create one new smoke particle per frame.
          //  smokePlumeParticles.AddParticle(Vector3.Zero, Vector3.Zero);
        }


        /// <summary>
        /// Helper for updating the fire effect.
        /// </summary>
        void UpdateFire()
        {
            const int fireParticlesPerFrame = 50;

            // Create a number of fire particles, randomly positioned around a circle.
            for (int i = 0; i < fireParticlesPerFrame; i++)
            {
                //fireParticles.AddParticle(RandomPointOnCircle(), Vector3.One);
            }

            // Create one smoke particle per frmae, too.
         //   smokePlumeParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);
        }


        /// <summary>
        /// Helper used by the UpdateFire method. Chooses a random location
        /// around a circle, at which a fire particle will be created.
        /// </summary>
        Vector3 RandomPointOnCircle()
        {
            const float radius = 10;
            const float height = 10;

            double angle = random.NextDouble() * Math.PI * 2;
            double angle2 = random.NextDouble() * Math.PI;

            float x = (float)Math.Cos(angle)*(float)Math.Sin(angle2);
            float y = (float)Math.Sin(angle)*(float)Math.Sin(angle2);
            float z = (float)Math.Cos(angle2);

            return new Vector3(models[6].Model.Position.X + x * radius, models[6].Model.Position.Y - (y * radius + height), models[6].Model.Position.Z + z * radius);
        }

       


    }
}


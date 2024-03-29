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
using GUI;
using System.IO;
using Logic.Units.Allies;
using Logic.EnviroModel;
using System.Diagnostics;
using Logic.PathFinderManagerNamespace;
using Logic.PathFinderNamespace;
using Logic.Units.Predators;
using Logic.Building.AllieBuilding;
namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public bool startGame = false;
        public bool showGrid = false;
        List<InteractiveModel> models = new List<InteractiveModel>();
        List<InteractiveModel> inter = new List<InteractiveModel>();
        List<InteractiveModel> IModel = new List<InteractiveModel>();
        List<InteractiveModel> Enemys = new List<InteractiveModel>();
        List<LaserTrigger> timeTriggers = new List<LaserTrigger>();
        List<Curve3D> curvesForLaser = new List<Curve3D>();
        List<List<PointInTime>> pointsForLasers = new List<List<PointInTime>>();
        Logic.Control control;
        InteractiveModel spider;
        MainGUI gui;

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
        List<QuadTree> quadTree = new List<QuadTree>();

        MapRender mapR;
        //FPS COUNTER
        int licznik=0;
        bool endGame = false;
        public Trigger theEnd;
        public Texture2D endGamePicture;
        public List<Texture2D> intro;
        public Texture2D _intro;
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
        Stream stream;
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


        Texture2D circleTexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Construct our particle system components.
            explosionParticles = new Particles.ParticleSystems.ExplosionParticleSystem(this, Content);
            explosionSmokeParticles = new Particles.ParticleSystems.ExplosionSmokeParticleSystem(this, Content);
            projectileTrailParticles = new Particles.ParticleSystems.ProjectileTrailParticleSystem(this, Content);
            smokePlumeParticles = new Particles.ParticleSystems.SmokePlumeParticleSystem(this, Content);
            fireParticles = new Particles.ParticleSystems.FireParticleSystem(this, Content);

            fireParticles.Interval = 3f;
            projectileTrailParticles.Interval = 2.5f;
            smokePlumeParticles.Interval = 3f;

            // Set the draw order so the explosions and fire
            // will appear over the top of the smoke.
            smokePlumeParticles.DrawOrder = 100;
            explosionSmokeParticles.DrawOrder = 200;
            projectileTrailParticles.DrawOrder = 300;
            explosionParticles.DrawOrder = 400;
            fireParticles.DrawOrder = 500;

            // Register the particle system components.
            Components.Add(explosionParticles);
            Components.Add(explosionSmokeParticles);
            Components.Add(projectileTrailParticles);
            Components.Add(smokePlumeParticles);
            Components.Add(fireParticles);

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
            light = new LightsAndShadows.Light(0.7f, 0.4f, new Vector3(-2800, 4000, -2800));
            shadow = new LightsAndShadows.Shadow();
            LoadModelsFromFile._light = light;
            #endregion
            #region PresentationParameters
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            pp.DepthStencilFormat = DepthFormat.Depth24Stencil8;

            #endregion
            #region PointsForLaser
            pointsForLasers.Add(new List<PointInTime>());
            pointsForLasers[0].Add(new PointInTime(new Vector3(100, 40, 100), 0));
            pointsForLasers[0].Add(new PointInTime(new Vector3(1600, 40, 300), 20000));
            pointsForLasers[0].Add(new PointInTime(new Vector3(3000, 40, 260), 40000));
            pointsForLasers[0].Add(new PointInTime(new Vector3(1300, 40, 900), 60000));
            pointsForLasers[0].Add(new PointInTime(new Vector3(400, 40, 1500), 80000));
            pointsForLasers[0].Add(new PointInTime(new Vector3(1299, 40, 1900), 100000));
            pointsForLasers[0].Add(new PointInTime(new Vector3(100, 40, 3000), 120000));



            #endregion
            #region Curve
            curvesForLaser.Add(new Curve3D(pointsForLasers[0],CurveLoopType.Oscillate));
            #endregion
            #region Laser

            #endregion

            
           
           
            hiDefShadowEffect = Content.Load<Effect>("Effects/Shadows");
            animHiDefShadowEffect = Content.Load<Effect>("Effects/AnimatedShadow");
            device = GraphicsDevice;
            device.DepthStencilState = DepthStencilState.Default;
            shadow.RenderTarget = new RenderTarget2D(device, 4096, 4096, false, pp.BackBufferFormat, DepthFormat.Depth24Stencil8);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            StaticHelpers.StaticHelper._spr_font = Content.Load<SpriteFont>("Fonts/FPS");// you have on your project
            

            StaticHelpers.StaticHelper.Content = Content;
            StaticHelpers.StaticHelper.Device = device;
            endGamePicture = Content.Load<Texture2D>("Textures/t�oko�cowe");
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
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass"));  // to mo�na wyci��
            texture.Add(Content.Load<Texture2D>("Textures/Ground/grass1")); // to mo�na wyci��
            texture.Add(Content.Load<Texture2D>("Textures/Ground/sand"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/forest_cover")); //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  //
            texture.Add(Content.Load<Texture2D>("Textures/Ground/dirt"));  // to mo�na wyci��
            texture.Add(Content.Load<Texture2D>("Textures/Ground/rock"));  // to mo�na wyci��
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
            texture.Add(Content.Load<Texture2D>("Textures/circle"));
            #endregion



            camera = new FreeCamera(
new Vector3(texture[4].Width * 1 / 2, texture[4].Width * 1 / 20, texture[4].Width * 1 / 2),
MathHelper.ToRadians(0), // Turned around 153 degrees
MathHelper.ToRadians(-45), // Pitched up 13 degrees
GraphicsDevice);

            quadTree.Add(new QuadTree(Vector2.Zero, texture, device, 3, Content, (FreeCamera)camera));



            water = new Water(device, Content, texture[4].Width, 3);
            foreach (QuadTree q in quadTree)
            {
                q.Cull = true;
            }
            StaticHelpers.StaticHelper.heights = quadTree[0].Vertices.heightDataToControl;
            StaticHelpers.StaticHelper.width = (int)Math.Sqrt(quadTree[0].Vertices.heightDataToControl.Length);
            StaticHelpers.StaticHelper.length = (int)Math.Sqrt(quadTree[0].Vertices.heightDataToControl.Length);

            PathFinderManager.PathFinderManagerInitialize(128);


            #region loadFromFile

            Controlers.LoadModelsFromFile.Load();
            foreach (InteractiveModel i in LoadModelsFromFile.listOfAllInteractiveModelsFromFile)
            {

                // Console.WriteLine(models.GetType().BaseType.Name);
                if (i.GetType().BaseType == typeof(Building) || i.GetType().BaseType == typeof(Material) || i.GetType().BaseType == typeof(EnviroModels) || i.GetType().BaseType==typeof(AllieBuilding))
                {

                    IModel.Add(i);
                }
                else
                {
                    if (i.GetType() == typeof(Beetle))
                    {
                        ((Beetle)i).Ants = models;
                        ((Beetle)i).removeMyself();
                    }
                    else if (i.GetType() == typeof(Spider))
                    {
                        ((Spider)i).Ants = models;
                        ((Spider)i).removeMyself();

                    }
                    models.Add(i);
                }
            }
            #endregion
            PathFinderManager.blockAllNodes(IModel);
            //for (int i = 0; i < PathFinderManager.GridSize; i += 1)
            //{
            //    for (int J = 0; J < PathFinderManager.GridSize; J += 1)
            //    {
            //        if (PathFinderManager.tileList[i, J].walkable == false || PathFinderManager.tileList[i, J].haveMineral == true || PathFinderManager.tileList[i, J].haveBuilding == true)
            //        {
            //            inter.Add(new InteractiveModel(new LoadModel(Content.Load<Model>("Models/log2"),new Vector3(PathFinderManager.tileList[i,J].centerPosition.X,StaticHelpers.StaticHelper.GetHeightAt(PathFinderManager.tileList[i,J].centerPosition.X,PathFinderManager.tileList[i,J].centerPosition.Y),PathFinderManager.tileList[i,J].centerPosition.Y),Vector3.Zero,new Vector3(1f,0.3f,1f),device,light)));
            //        }
            //    }
            //}



            /////////////// nie wiem czy to powinno by� czy nie wi�c zakomentowa�em tylko
            //
            //            mapR = new MapRender(texture[15], 3);
            //            Console.WriteLine(GraphicsDevice.Viewport.Bounds);

            //e.Ant = models[0];
            // e.Enemy = spider;
            //  e.Ants = Enemys;




            WindowController.setWindowSize(1366, 768, false);
            //models.Add(new AntPeasant(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/mrowka_01"), Vector3.Zero, Vector3.Zero, new Vector3(0.3f), StaticHelpers.StaticHelper.Device, light)));
            // models.Add(new TownCenter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/domek"), Vector3.Zero, Vector3.Zero, new Vector3(0.23f), StaticHelpers.StaticHelper.Device, light)));
            ////models.Add(new Queen(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/grasshopper"), new Vector3(300,40,300), Vector3.Zero, new Vector3(0.23f), StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content,light)));
            ////models[models.Count - 1].Model.switchAnimation("Idle");

            List<String> aa = new List<string>();
            aa.Add("s1");
            aa.Add("s2");
            aa.Add("Gater");
            SoundController.SoundController.content = Content;
            SoundController.SoundController.Initialize(aa);

            List<String> PlayList = new List<string>();
            PlayList.Add("Kwai");
            PlayList.Add("MarketGarden");
            PlayList.Add("Escape");
            SoundController.SoundController.InitializeBackground(PlayList);








            // models[models.Count - 1].Model.switchAnimation("Idle");

            // models.Add(new Cancer(new LoadModel(Content.Load<Model>("Models/crab"), new Vector3(0, 40, 0), new Vector3(0), new Vector3(0.4f), GraphicsDevice,  light), models));



              models.Add(new Cancer(new LoadModel(Content.Load<Model>("Models/crab"), new Vector3(1375, 40, 2500), new Vector3(0), new Vector3(0.4f), GraphicsDevice, Content, light), models));

              models[models.Count - 1].Model.switchAnimation("Idle");

             


             models.Add(new SunDew(new LoadModel(Content.Load<Model>("Models/rosiczka"), new Vector3(1574, 40, 2100), new Vector3(0), new Vector3(0.8f), GraphicsDevice, Content, light), models));
            models[models.Count - 1].Model.switchAnimation("Idle");
            models.Add(new SunDew(new LoadModel(Content.Load<Model>("Models/rosiczka"), new Vector3(2400, 40, 805), new Vector3(0), new Vector3(0.8f), GraphicsDevice, Content, light), models));
            models[models.Count - 1].Model.switchAnimation("Idle");
            models.Add(new SunDew(new LoadModel(Content.Load<Model>("Models/rosiczka"), new Vector3(1700, 40, 1111), new Vector3(0), new Vector3(0.8f), GraphicsDevice, Content, light), models));
            models[models.Count - 1].Model.switchAnimation("Idle");
            // models.Add(new Spider(new LoadModel(Content.Load<Model>("Models/spider"), new Vector3(250, 40, 250), new Vector3(0), new Vector3(0.4f), GraphicsDevice, Content, light), models));
            // models[models.Count - 1].Model.switchAnimation("Idle");




            ////models.Add(new Grasshopper(new LoadModel(Content.Load<Model>("Models/grasshopper"), new Vector3(50, 40, 50), new Vector3(0), new Vector3(0.4f), GraphicsDevice, Content, light), models));
            ////models[models.Count - 1].Model.switchAnimation("Idle");


            // models.Add(new StrongAnt(new LoadModel(Content.Load<Model>("Models/strongAnt"),new Vector3(120,40,20),new Vector3(0),new Vector3(0.4f),GraphicsDevice,Content,light)));
            //// models[models.Count - 1].Model.switchAnimation("Idle");



            // models.Add(new AntPeasant(new LoadModel(Content.Load<Model>("Models/ant"), new Vector3(100, 40, 10), new Vector3(0), new Vector3(0.4f), GraphicsDevice, Content, light)));

           

           // Console.WriteLine("QuadNode: " + QuadNodeController.QuadNodeList.Count);

            BBoxRender.InitializeBBoxDebuger(device);

           // IModel.Add(new BeetleBuilding(new LoadModel(Content.Load<Model>("Models/h4"), new Vector3(700, 60, 900), new Vector3(0), new Vector3(0.4f), GraphicsDevice, light)));
          //  IModel.Add(new GrassHopperBuilding(new LoadModel(Content.Load<Model>("Models/h3"), new Vector3(900, 40, 1100), new Vector3(0), new Vector3(0.4f), GraphicsDevice, light)));





            Console.WriteLine(QuadNodeController.QuadNodeList2.Count);

            #region trigger end game
            theEnd = new Trigger((new LoadModel(Content.Load<Model>("Models/endNode"), new Vector3(1375, 40, 2500), new Vector3(0), new Vector3(4f), GraphicsDevice, light)));
            IModel.Add(theEnd);
            #endregion

            IModel.Add(new Laser((new LoadModel(Content.Load<Model>("Models/laser"), new Vector3(0, 40, 0), new Vector3(0), new Vector3(2f), GraphicsDevice, light)), curvesForLaser[0]));
            timeTriggers.Add(new LaserTrigger((Laser)IModel[IModel.Count - 1], 100));

            control = new Logic.Control(texture[11], quadTree[0]);
            gui = new MainGUI(StaticHelpers.StaticHelper.Content, control,models);



            control.Models_Colision = IModel;

            Player.addMaterial(new Wood(),200);
            Player.addMaterial(new Stone(), 200);
            Player.addMaterial(new Hyacynt(), 200);
            Player.addMaterial(new Dicentra(), 200);
            Player.addMaterial(new Chelidonium(), 200);


            miniMap = new MiniMap(models);
            foreach(InteractiveModel m in IModel) {
                if (m is Laser || m is GrassHopperBuilding || m is BeetleBuilding)
                    miniMap.addObjects(m);
                }
            intro = new List<Texture2D>();
            intro.Add( Content.Load<Texture2D>("Textures/1"));
            intro.Add(Content.Load<Texture2D>("Textures/2"));
            intro.Add(Content.Load<Texture2D>("Textures/3"));
            intro.Add(Content.Load<Texture2D>("Textures/4"));

            _intro = intro[0];
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

            currentMouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            if(startGame==true)   {
            if (StaticHelpers.StaticHelper.pause == false)
            {
                if (Microsoft.Xna.Framework.Media.MediaPlayer.State.Equals(Microsoft.Xna.Framework.Media.MediaState.Stopped))
                {
                    if (SoundController.SoundController.playqueue == 0)
                    {
                        Microsoft.Xna.Framework.Media.MediaPlayer.Play(SoundController.SoundController.BackgroundSongs[SoundController.SoundController.playqueue]);
                        Microsoft.Xna.Framework.Media.MediaPlayer.Volume = 0.01f;
                        SoundController.SoundController.playqueue = 1;
                    }
                    else if (SoundController.SoundController.playqueue == 1)
                    {
                        Microsoft.Xna.Framework.Media.MediaPlayer.Play(SoundController.SoundController.BackgroundSongs[SoundController.SoundController.playqueue]);
                        Microsoft.Xna.Framework.Media.MediaPlayer.Volume = 0.01f;
                        SoundController.SoundController.playqueue = 2;
                    }
                    else if (SoundController.SoundController.playqueue == 2)
                    {
                        Microsoft.Xna.Framework.Media.MediaPlayer.Play(SoundController.SoundController.BackgroundSongs[SoundController.SoundController.playqueue]);
                        Microsoft.Xna.Framework.Media.MediaPlayer.Volume = 0.01f;
                        SoundController.SoundController.playqueue = 0;
                        //etc. etc. etc.;
                    }
                }

                if (this.endGame)
                {
                    if(keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) )
                    {
                        this.Exit();
                    }
                }


                kolizja = false;


                if (timeTriggers.Count<1)
                {
                    UpdateFire();
                  //  UpdateExplosions(gameTime);

                   // UpdateProjectiles(gameTime);

                }




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
                    
                #region timeTrigger

                for (int i = 0; i < timeTriggers.Count; i++)
                {

                    timeTriggers[i].Update(gameTime);
                    //Console.WriteLine(timeTriggers[i].laser.Model.Position);
                    if (timeTriggers[i].used == true)
                    {
                        timeTriggers.Remove(timeTriggers[i]);
                    }

                }
                #endregion
                for (int i = 0; i < models.Count; i++)
                {
                    models[i].Update(gameTime);
                    models[i].Model.Update(gameTime);

                    if (models[i].GetType() == typeof(Queen) && models[i].Model.BoundingSphere.Intersects(theEnd.Model.BoundingSphere))
                    {
                        endGame = true;
                    }
                    //if (models[i].Model.Position.Y < 40)
                    //{
                    //    Console.WriteLine("WODA");
                    //}

                    if (models[i].attacking)
                    {
                        models[i].Attack(gameTime);
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
                        if (control.selectedObjectMouseOnlyMove.GetType().BaseType == typeof(Building))
                        {
                            MouseCursorController.stage = CursorStage.Go;

                        }
                    }
                    else
                    {
                        MouseCursorController.stage = CursorStage.Normal;
                    }
                    //foreach (InteractiveModel model2 in models)
                    //for (int j = 0; j < models.Count;j++ )
                    //{
                    //    models[i].Intersect(models[j]);
                    //}
                    // if(models[i].Hp<=0 && models[i].GetType().BaseType.BaseType==typeof(Unit))


                    for (int kj = 0; kj < IModel.Count; kj++)
                    {
                        models[i].Intersect(IModel[kj]);

                        IModel[kj].Intersect(models[i]);
                        
                    }

                    if (models[i] is AntSpitter)
                    {
                        foreach (InteractiveModel m in models)
                        {  if(Vector2.Distance(new Vector2(m.Model.Position.X,m.Model.Position.Z),new Vector2(models[i].Model.Position.X,models[i].Model.Position.Z))>200 && !m.GetType().IsSubclassOf(typeof(Predator)) )
                        {
                            continue;
                        }
                            models[i].Intersect(m);
                        }
                        foreach (Vector4 pos in models[i].spitPos())
                        {
                            // if()
                            Vector3 tmpPos = new Vector3(pos.X, pos.Y, pos.Z);
                            if (pos.W == 0)
                                UpdateSpit(tmpPos, false);
                            else
                                UpdateSpit(tmpPos, true);

                        }
                    }



                    if (models[i].Hp <= 0 && models[i] is Unit)
                    {


                        if (models[i].GetType() == typeof(SunDew))
                        {
                            models[i].Model.switchAnimation("Relax", 1);
                        }
                        else
                        {
                            models[i].Model.switchAnimation("Death", 1);
                        }
                        
                        foreach (InteractiveModel unit in control.SelectedModels)
                        {
                            if (models[i] == unit && models[i].Model.Player.end)
                                control.SelectedModels.Remove((Unit)unit);
                            break;
                        }
                        //models[i] = null;
                        models.RemoveAt(i);

                    }


                }



                foreach (InteractiveModel model in IModel)
                {

                    model.Update(gameTime);
                    model.Model.Update(gameTime);
                    if (model.raisingBuilding)
                    {
                        UpdateSmokePlumeBuilding(model);
                    }

                   

                }





                control.View = camera.View;
                control.Projection = camera.Projection;
                control.cameraYaw = ((FreeCamera)camera).Yaw;
                control.cameraPitch = ((FreeCamera)camera).Pitch;
                control.models = models;
                control.device = device;

                control.Update(gameTime);

                foreach (Unit unit in control.SelectedModels)
                {

                    //  unit.obstaclesOnRoad(control.filtrObstacles(models));
                    //unit.obstaclesOnRoad(IModel);



                }



                //  e.gameTime = gameTime;
                //  e.Update();

                MouseCursorController.Update();




            }
            if (MainGUI.exit)
            {
                this.Exit();
            }
            miniMap.UpdateMinimap(gameTime,new Vector2(((FreeCamera)camera).Position.X,(((FreeCamera)camera).Position.Z)));
            gui.Update(gameTime);
            foreach (QuadTree tree in quadTree)
            {
                tree.View = camera.View;
                tree.Projection = camera.Projection;
                tree.CameraPosition = ((FreeCamera)camera).Position;
                tree.Update(gameTime);
            }
            camera.Update(gameTime);
        }
                 if(licznik==4)
                {
                    startGame = true;
                }
                 else if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && lasKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))
                     {

                         
                         _intro = intro[licznik];
                         licznik++;
                     }
                 lasKeyState = keyState;
            base.Update(gameTime);

        }


        protected override void Draw(GameTime gameTime)
        {
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;
            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            explosionParticles.SetCamera((FreeCamera)camera);
            explosionSmokeParticles.SetCamera((FreeCamera)camera);
            projectileTrailParticles.SetCamera((FreeCamera)camera);
            smokePlumeParticles.SetCamera((FreeCamera)camera);
            fireParticles.SetCamera((FreeCamera)camera);






            device.SetRenderTarget(shadow.RenderTarget);
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            //device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);



            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;

            _total_frames++;


            shadow.UpdateLightData(0.6f, light.lightPosChange(time), (FreeCamera)camera);
            shadow.setShadowMap();
            device.SetRenderTarget(shadow.RenderTarget);


            animHiDefShadowEffect.CurrentTechnique = animHiDefShadowEffect.Techniques["Technique1"];





            #region shadow
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
            #endregion shadow

            //  device.SetRenderTarget()


            #region Odbicie i rozproszenie wody
            water.DrawRefractionMap((FreeCamera)camera, time, shadow, light, quadTree[0]);

            water.DrawReflectionMap((FreeCamera)camera, time, shadow, light, quadTree[0]);
            #endregion



            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            water.sky.DrawSkyDome((FreeCamera)camera);

            foreach (QuadTree tree in quadTree)
            {
                tree.Draw(((FreeCamera)camera), time, shadow, light);
            }

            water.DrawWater(time, (FreeCamera)camera);


            //foreach (Node n in PathFinderManager.tileList)
            //{
            //    if (camera.BoundingVolumeIsInView(n.Box))
            //    {
            //        BBoxRender.DrawBBox(n.Box, camera.Projection, camera.View, Matrix.Identity, Color.BlueViolet);
            //    }
            //}




            //foreach (InteractiveModel q in inter)
            //{
            //    if (camera.BoundingVolumeIsInView(q.Model.BoundingSphere))
            //    {
            //        BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, Color.Black, Color.Yellow, Color.Red);
            //        BBoxRender.DrawBBox(q.Bounds, camera.Projection, camera.View, Matrix.Identity, Color.BlueViolet);
            //        q.Draw((FreeCamera)camera);
            //    }
            //}

            foreach (InteractiveModel model in models)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    if (model.Model.Player == null)
                    { model.Draw((FreeCamera)camera); }
                    else
                    { model.Draw((FreeCamera)camera, time); }

                    // BBoxRender.DrawBBox(model.Model.B_Box, camera.Projection, camera.View, Matrix.Identity,Color.Black);

                 //   BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection,
                 //    Color.Green, Color.Aquamarine, Color.White);
                    //   BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, Color.Black, Color.Yellow, Color.Red   );
                }
            }

            foreach (InteractiveModel model in IModel)
            {
                if (camera.BoundingVolumeIsInView(model.Model.BoundingSphere))
                {
                    if (model.GetType()==typeof(Trigger)   )
                    {
                        continue;
                    }
                  //  BoundingSphereRenderer.Render(model.Model.BoundingSphere, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));
                  //  BoundingSphereRenderer.Render(model.Model.Spheres, device, camera.View, camera.Projection, new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f), new Color(0.9f, 0.9f, 0.9f));

                    //BBoxRender.DrawBBox(model.Model.boundingBoxes, camera.Projection, camera.View, Matrix.Identity);
                    model.Draw((FreeCamera)camera);


                }

            }

            // spider.Draw((FreeCamera)camera);
            if (control.selectedObjectMouseOnlyMove != null)
            {
                control.selectedObjectMouseOnlyMove.DrawSelected((FreeCamera)camera);

                // Console.WriteLine(control.selectedObjectMouseOnlyMove);
                switch (control.selectedObjectMouseOnlyMove.GetType().BaseType.Name)
                {
                    case "Material":
                        // SoundController.SoundController.Play(SoundEnum.SelectedMaterial);
                        break;

                }
            }

            foreach (InteractiveModel selected in control.SelectedModels)
            {

                selected.DrawSelected((FreeCamera)camera);
                selected.DrawSelectedCircle((FreeCamera)camera);
                if (selected.GetType() == typeof(AntPeasant))
                {
                    if (control.selectedObject != null)
                    {
                        if (control.selectedObject.GetType().BaseType == typeof(Material))
                        {
                            selected.setGaterMaterial((Material)control.selectedObject);
                        }

                    }
                    else
                    {
                        selected.setGaterMaterial(null);
                    }
                }
            }

            if (control.selectedObjectMouseOnlyMove != null)
            {
                control.selectedObjectMouseOnlyMove.DrawSelected((FreeCamera)camera);
            }
            if (startGame == true) {
                spriteBatch.Begin();

            MouseState current_mouse = Mouse.GetState();
            Vector2 pos = new Vector2(current_mouse.X, current_mouse.Y);
            spriteBatch.DrawString(StaticHelpers.StaticHelper._spr_font, string.Format(" {0}", pos), new Vector2(100.0f, 100.0f), Color.Pink);

            /*
            spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.stone), new Vector2(130.0f, 240.0f), Color.Pink);
            spriteBatch.DrawString(_spr_font, string.Format("K g={0}", Player.wood), new Vector2(230.0f, 240.0f), Color.Pink);

              spriteBatch.DrawString(_spr_font, string.Format("h g={0}", Player.hyacynt), new Vector2(340.0f, 240.0f), Color.Pink);
               spriteBatch.DrawString(_spr_font, string.Format("d g={0}", Player.dicentra), new Vector2(450.0f, 240.0f), Color.Pink);
               spriteBatch.DrawString(_spr_font, string.Format("heli g={0}", Player.chelidonium), new Vector2(550.0f, 240.0f), Color.Pink);*/
            /* 
          spriteBatch.DrawString(_spr_font, string.Format("Drewno w klodzie={0}", ((Log)models[1]).ClusterSize), new Vector2(10.0f, 180.0f), Color.Pink);
          spriteBatch.DrawString(_spr_font, string.Format("Kamien w skale={0}", ((Rock)models[2]).ClusterSize), new Vector2(10.0f, 220.0f), Color.Pink);
           
        spriteBatch.DrawString(_spr_font, string.Format("iloscMrowek={0}", models.Count), new Vector2(10.0f, 220.0f), Color.Pink);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", licznik), new Vector2(10.0f, 50.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Yaw), new Vector2(10.0f, 150.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Pitch), new Vector2(10.0f, 250.0f), Color.Tomato);
        spriteBatch.DrawString(_spr_font, string.Format("Widac mrowke? ={0}", ((FreeCamera)camera).Position), new Vector2(10.0f, 350.0f), Color.Tomato);
          
         spriteBatch.DrawString(_spr_font, string.Format("mouse3d ={0}", control.mouse3d2), new Vector2(10.0f, 80.0f), Color.Pink);
         spriteBatch.DrawString(_spr_font, string.Format("position3d ={0}", control.position3d), new Vector2(10.0f, 120.0f), Color.Pink);
        */
            control.Draw(spriteBatch, (FreeCamera)camera);
            gui.Draw(spriteBatch);
            miniMap.Draw(spriteBatch);

            if(this.endGame)
            {
                       
                        spriteBatch.Draw(endGamePicture, new Rectangle(0, 0, 1366, 768), Color.White);
                 
            }


            spriteBatch.End();

            foreach(InteractiveModel model in models)
            {
                if (model.GetType() == typeof(Beetle))
                {
                    model.DrawOpaque((FreeCamera)camera, 0.1f, ((Beetle)model).sfereModel.Model);
                }         
               

            }

            foreach (InteractiveModel model in IModel)
            {
                if(model.GetType() == typeof(Laser) )
                {
                    model.DrawOpaque((FreeCamera)camera, 0.4f, model.Model);
                }
            }
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_intro, new Rectangle(0, 0, 1366, 768), Color.White);
                spriteBatch.End();
            }


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

        void UpdateSpit(Vector3 pos, bool explode)
        {
            int amountPerFrame = 30;
            if (explode)
            {
                for (int i = 0; i < amountPerFrame; i++)
                {
                    explosionParticles.AddParticle(pos, Vector3.Zero);
                }
                for (int i = 0; i < amountPerFrame / 2; i++)
                {
                    smokePlumeParticles.AddParticle(pos, Vector3.Zero);
                }
            }
            else
            {
                for (int i = 0; i < amountPerFrame + 15; i++)
                {
                    projectileTrailParticles.AddParticle(pos + new Vector3(0, 5, 0), new Vector3(1, 0, 1));
                }
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
        void UpdateSmokePlume(Vector3 pos)
        {
            // This is trivial: we just create one new smoke particle per frame.
            smokePlumeParticles.AddParticle(pos, Vector3.Zero);
        }

        void UpdateSmokePlumeBuilding(InteractiveModel building)
        {
            // This is trivial: we just create one new smoke particle per frame.
            smokePlumeParticles.AddParticle(RandomPointOnCircleBuilding(building), Vector3.Zero);
        }


        /// <summary>
        /// Helper for updating the fire effect.
        /// </summary>
        void UpdateFire()
        {
            const int fireParticlesPerFrame = 10;

            // Create a number of fire particles, randomly positioned around a circle.
            for (int i = 0; i < fireParticlesPerFrame / 4; i++)
            {
                fireParticles.AddParticle(RandomPointOnCircle(), Vector3.One);
            }
            for (int i = 0; i < fireParticlesPerFrame / 6; i++)
            {
                // Create one smoke particle per frmae, too.
                smokePlumeParticles.AddParticle(RandomPointOnCircle() + new Vector3(0, 20, 0), Vector3.Zero);
            }
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

            float x = (float)Math.Cos(angle) * (float)Math.Sin(angle2);
            float y = (float)Math.Sin(angle) * (float)Math.Sin(angle2);
            float z = (float)Math.Cos(angle2);


            return new Vector3(IModel[IModel.Count - 1].Model.Position.X + x * radius, IModel[IModel.Count - 1].Model.Position.Y - (y * radius + height), IModel[IModel.Count - 1].Model.Position.Z + z * radius);
        }

        Vector3 RandomPointOnCircleBuilding(InteractiveModel building)
        {


            double angle = random.NextDouble() * Math.PI * 2;


            float x = (float)Math.Sin(angle);
            float z = (float)Math.Cos(angle);


            return new Vector3(building.Model.Position.X + x * building.Model.BoundingSphere.Radius, building.Model.Position.Y, building.Model.Position.Z + z * building.Model.BoundingSphere.Radius);
        }








        public PathFinder pf { get; set; }

        public float _elapsed_time2 { get; set; }

        public MiniMap miniMap { get; set; }

        public KeyboardState lasKeyState { get; set; }
    }
}


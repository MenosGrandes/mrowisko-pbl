//using Animations;
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
using Animations;
namespace AntHill
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
       
        GraphicsDeviceManager graphics;
        public GraphicsDevice device;
        float x, y, z;
        SpriteBatch spriteBatch;
        List<Map.LoadModel> models = new List<Map.LoadModel>();
        List<Map.LoadModel> inter = new List<Map.LoadModel>();
        public Camera camera;
        MouseState lastMouseState;
        Map.Water water;
        LoadModel anim;
         QuadTree quadTree;
                     //FPS COUNTER

         SpriteFont _spr_font;
         int _total_frames = 0;
         float _elapsed_time = 0.0f;
         int _fps = 0;
        
        
        MouseState currentMouseState;
        MouseState LastMouseState_2;
        int f = 0;
        Vector3 playerTarget;

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
            texture.Add(Content.Load<Texture2D>("Textures/kamien"));
            texture.Add(Content.Load<Texture2D>("Textures/muszle"));

                    camera = new FreeCamera(
        new Vector3(texture[4].Width * 100 / 2, texture[4].Width * 100 / 10, texture[4].Width * 100 / 2),
        MathHelper.ToRadians(0), // Turned around 153 degrees
        MathHelper.ToRadians(-45), // Pitched up 13 degrees
        GraphicsDevice);

                    quadTree = new QuadTree(Vector3.Zero, texture, device, 100, Content, (FreeCamera)camera);
            quadTree.Cull = true;

            water = new Water(device, Content, texture[4].Width, 100);
           

           models.Add(new LoadModel(Content.Load<Model>("Models/mrowka_01"), Vector3.Zero, Vector3.Up, new Vector3(20.05f), GraphicsDevice));

           //inter = quadTree.ants.Models;

          // GraphicsDevice.BlendState = BlendState.AlphaBlend;
          // GraphicsDevice.BlendFactor = Color.Yellow;


            //animacja CHYBA dzia³a (nie wiem jak zrobiæ ¿eby by³o j¹ widaæ)
            //na starszych wersjach repozytorium dzia³a bez problemu (pliki x)
            //plik xml jest potrzebny ¿eby dzia³a³o prze³¹czanie, nie wiem czemu ale jak jest w folderze models to nie dzia³a 
           anim = new LoadModel(
Content.Load<Model>("animacja"),
Vector3.Zero, Vector3.Up,
new Vector3(100), GraphicsDevice, Content);
           AnimationClip clip = anim.skinningData.AnimationClips["run"];//inne animacje to idle2 i run
           anim.Player.StartClip(clip);
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
            float pozycja_X_lewo = models[0].Position.X - 800;
            float pozycja_X_prawo = models[0].Position.X + 800;
            float pozycja_Z_gora = models[0].Position.Z + 400;
            float pozycja_Z_dol = models[0].Position.Z - 800;


            currentMouseState = Mouse.GetState();
           // LastMouseState_2 = Mouse.GetState();
            Vector3 mouse3d2 = CalculateMouse3DPosition();
            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                if ((mouse3d2.X > pozycja_X_lewo && mouse3d2.X < pozycja_X_prawo) && (mouse3d2.Z > pozycja_Z_dol && mouse3d2.Z < pozycja_Z_gora))
                {
                    f = 1;
                }
                else
                {
                    f = 0;
                }

            }
            //Console.WriteLine(f);
            if (currentMouseState.LeftButton == ButtonState.Pressed && f==1)
            {
                // This will give the player a target to go to. 
                playerTarget.X = mouse3d2.X;
                playerTarget.Z = mouse3d2.Z;
            }
            updateAnt(gameTime);




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
            if (keyState.IsKeyDown(Keys.X))
                x = x + 0.5f;
            if (keyState.IsKeyDown(Keys.Y))
                y = y + 0.5f;
            if (keyState.IsKeyDown(Keys.Z))
                z = z + 0.5f;
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
          
           
            quadTree.View =camera.View;
           quadTree.Projection = camera.Projection;
            quadTree.CameraPosition = ((FreeCamera)camera).Position;
            quadTree.Update(gameTime);
          

             
            camera.Update(gameTime);
            anim.Update(gameTime);
            base.Update(gameTime);
            
        }


        protected override void Draw(GameTime gameTime)
        {
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.FillMode = FillMode.WireFrame;
            //GraphicsDevice.RasterizerState = rasterizerState;


            float time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.CullCounterClockwiseFace;

            _total_frames++;






         //   water.DrawRefractionMap(camera.View);

        //   water.DrawReflectionMap((FreeCamera)camera);
            anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer , Color.Black, 1.0f, 0);
         //   water.DrawWater(time, (FreeCamera)camera);


           //device.DepthStencilState = DepthStencilState.Default;
                                       quadTree.Draw((FreeCamera)camera, time);
                                     //  device.DepthStencilState = DepthStencilState.None;
      

         
      

          

        if(camera.BoundingVolumeIsInView(models[0].BoundingSphere))  {
            
                        models[0].Draw(camera.View, camera.Projection);
                         


          }


             
     
            


 
           
            

            anim.Draw(camera.View, camera.Projection, ((FreeCamera)camera).Position);

         //   spriteBatch.Begin();
        //    spriteBatch.DrawString(_spr_font, string.Format("FPS={0}", _fps),
         //       new Vector2(10.0f, 20.0f), Color.Tomato);
         //   spriteBatch.End();

            
            base.Draw(gameTime);
        }

        void updateAnt(GameTime gameTime)
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


        private Vector3 CalculateMouse3DPosition()
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
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0,0,0);
          
            
        }
    
    
    }
}


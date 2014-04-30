using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Map;
using GameCamera;


namespace Logic
{
    public class Control
    {
        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;
        public List<Map.LoadModel> models;
        Vector3 playerTarget;
        MouseState currentMouseState;
        int f = 0;

        public void Update(GameTime gameTime)
        {
            //pozycja mrowki
            float pozycja_X_lewo = models[0].Position.X - 800;
            float pozycja_X_prawo = models[0].Position.X + 800;
            float pozycja_Z_gora = models[0].Position.Z + 400;
            float pozycja_Z_dol = models[0].Position.Z - 800;


            currentMouseState = Mouse.GetState();
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
            if (currentMouseState.LeftButton == ButtonState.Pressed && f == 1)
            {
                // This will give the player a target to go to. 
                playerTarget.X = mouse3d2.X;
                playerTarget.Z = mouse3d2.Z;
            }


            updateAnt(gameTime);
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
                this.Projection, this.View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                this.Projection, this.View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            float? position = pickRay.Intersects(GroundPlane);

            if (position != null)
                return pickRay.Position + pickRay.Direction * position.Value;
            else
                return new Vector3(0, 0, 0);


        }


    }
}

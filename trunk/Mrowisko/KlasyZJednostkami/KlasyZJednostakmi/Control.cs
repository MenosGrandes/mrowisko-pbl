using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;


namespace Logic
{
    public class Control
    {
        public Matrix View;
        public Matrix Projection;
        public GraphicsDevice device;
        public List<Map.LoadModel> models;

        public void Update(GameTime gameTime)
        {
            updateAnt(models);
        }

        public void updateAnt(List<Map.LoadModel> models)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState MS = Mouse.GetState();
            Vector2 mouse_pos = new Vector2(MS.X, MS.Y);
            Vector3 mouse3d2 = CalculateMouse3DPosition();
            float Speed = (float)0.002;
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
                Projection, View, Matrix.Identity);

            Vector3 farPoint = device.Viewport.Unproject(farsource,
                Projection, View, Matrix.Identity);

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

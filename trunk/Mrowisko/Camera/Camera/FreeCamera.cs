using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCamera
{
    public class FreeCamera : Camera
    {

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }
        private Vector3 translation;
        private MouseState lastMouseState;
        
        public FreeCamera(Vector3 Position, float Yaw, float Pitch,
        GraphicsDevice graphicsDevice): base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            translation = Vector3.Zero;
        }
        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }
        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }
        public override void Update(GameTime gameTime)
        {
            int scale = 111;
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();




            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            //((FreeCamera)camera).Rotate(deltaX * .01f, -deltaY * .01f);

            Vector3 translation = Vector3.Zero;// Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation += new Vector3(0, -1, 1) * (Pitch) * scale;
            if (keyState.IsKeyDown(Keys.S)) translation += new Vector3(0, 1, -1) * (Pitch) * scale;
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left * (Pitch * -1) * scale;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right * (Pitch * -1) * scale;
            if (keyState.IsKeyDown(Keys.Q)) Rotate(.01f,0);
            if (keyState.IsKeyDown(Keys.E)) Rotate(-0.01f, 0);
            if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
            {
                translation += new Vector3(0, -1, 0) * MathHelper.ToRadians(135.0f)*-1*scale/10;
               
            }
            else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
            {
                translation += new Vector3(0, 1, 0) * MathHelper.ToRadians(135.0f) * -1*scale/10;

            }

            // Move 3 units per millisecond, independent of frame rate
            translation *= 0.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // Move the camera
            Move(translation);






            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;
            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;
            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);

            lastMouseState = mouseState;
        }
    }
}

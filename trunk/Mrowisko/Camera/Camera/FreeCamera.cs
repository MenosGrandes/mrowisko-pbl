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
        public bool Zoom;
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; private set; }
        private Vector3 translation;
        private MouseState lastMouseState;
        public Matrix reflectionViewMatrix { get; set; }
 
        public FreeCamera(Vector3 Position, float Yaw, float Pitch,
        GraphicsDevice graphicsDevice): base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            translation = Vector3.Zero;
            
            Move(Position, 1.0f,Matrix.Identity);
            Zoom = false;
        }
        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;// MathHelper.Lerp((float)MathHelper.ToRadians(this.Yaw), YawChange, 0.9f);
            this.Pitch += PitchChange;
        }
        public void Move(Vector3 Translation,float time,Matrix rotation)
        {
            Translation = Vector3.Lerp(Vector3.Transform(Translation, rotation), Translation, 0.1f);

            this.translation += Translation*time;
            

            Position = Vector3.Lerp(Position,translation, 0.1f);
         
        }
        public override void Update(GameTime gameTime)
        {
            int scale = 11;
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();





            Vector3 translation = Vector3.Zero;// Determine in which direction to move the camera
            float rotatate = 0;

            if (keyState.IsKeyDown(Keys.W)) { translation += new Vector3(0, (float)Math.Sin(-45), 1) * MathHelper.ToRadians(Pitch) * scale ;   Zoom = false; }
            if (keyState.IsKeyDown(Keys.S)) { translation += new Vector3(0, (float)Math.Sin(45), -1) * MathHelper.ToRadians(Pitch) * scale; Zoom = false; }
            if (keyState.IsKeyDown(Keys.A)) translation += Vector3.Left * (MathHelper.ToRadians(Pitch) * -1) * scale ;
            if (keyState.IsKeyDown(Keys.D)) translation += Vector3.Right * (MathHelper.ToRadians(Pitch) * -1) * scale ;
            if (keyState.IsKeyDown(Keys.Q)) rotatate += MathHelper.ToRadians(0.05f);
            if (keyState.IsKeyDown(Keys.E)) rotatate -=MathHelper.ToRadians(0.05f);
            if (mouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
            {
               translation+= Vector3.Lerp(translation, new Vector3(0, -1, 0) * MathHelper.ToRadians(135.0f)*-1*scale,0.3f);
               Zoom = true;
            }
            else if (mouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
            {
                translation += Vector3.Lerp(translation, new Vector3(0, 1, 0) * MathHelper.ToRadians(135.0f) * -1 * scale, 0.3f);
                Zoom = true;
            }
            // Move 3 units per millisecond, independent of frame rate
           // translation *= 0.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            rotatate *= 0.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // Move the camera
                

            Rotate( rotatate,0);
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            Move(translation, (float)gameTime.ElapsedGameTime.TotalMilliseconds, rotation);





     // Camera.Position = Target - Camera.Forward * Zoom;
    //  Camera.View = Matrix.CreateLookAt( Camera.Position, Target, Camera.Up);
            // Calculate the rotation matrix
            // Offset the position and reset the translation    
          //  Position += translation;
            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;
            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);


            Vector3 reflCameraPosition = Position;
            reflCameraPosition.Y = -Position.Y +6 * 2;
            Vector3 reflTargetPos = Target;
            reflTargetPos.Y = -Target.Y + 6 * 2;

            Vector3 cameraRight = Vector3.Transform(new Vector3(1, 0, 0), rotation);
            Vector3 invUpVector = Vector3.Cross(cameraRight, reflTargetPos - reflCameraPosition);

            reflectionViewMatrix = Matrix.CreateLookAt(reflCameraPosition, reflTargetPos, invUpVector);
                      
            lastMouseState = mouseState;
        }

    }
}

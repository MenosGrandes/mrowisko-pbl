using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace SimpleStaticHelpers
{
   public static class CreatorController
    {
       public static List<InteractiveModel> models=new List<InteractiveModel>();
       public static ContentManager content;
       public static GraphicsDevice device;
       public static MouseState mouseState;
       public static Matrix View;
       public static Matrix Projection;
       public static Vector3 MousePosition;
       public static void CalculateMouse3DPosition()
       {
           Plane GroundPlane = new Plane(0, 1, 0, 0); // x - lewo prawo Z- gora dol
           int mouseX =    mouseState.X;
           int mouseY =    mouseState.Y;
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
           {
               MousePosition = pickRay.Position + pickRay.Direction * position.Value;
               MousePosition.Y = 30f;
           }
           else
               MousePosition = new Vector3(0, 0, 0);


       }
    }
}

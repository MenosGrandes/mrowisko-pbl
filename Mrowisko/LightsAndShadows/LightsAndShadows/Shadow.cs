using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightsAndShadows
{
    public class Shadow
    {
        
        public Matrix lightsViewProjectionMatrix;
        public Shadow()
        {

        }

        public void UpdateLightData(float ambientPower, float lightPower, Vector3 lightPos, GameCamera.FreeCamera camera)
        {
            //ambientPower = 0.2f;

            //lightPos = new Vector3(-18, 5, -2);
            //lightPower = 1.0f;

            Matrix lightsView = Matrix.CreateLookAt(lightPos, camera.Target, new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1.333333f, 0.1f, 100000f);

            lightsViewProjectionMatrix = lightsView * lightsProjection;
        }
    }
}

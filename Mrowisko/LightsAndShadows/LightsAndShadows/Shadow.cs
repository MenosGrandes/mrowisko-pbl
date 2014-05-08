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
        RenderTarget2D renderTarget;

        public RenderTarget2D RenderTarget
        {
            get { return renderTarget; }
            set { renderTarget = value; }
        }
        Texture2D shadowMap;

        public Texture2D ShadowMap
        {
            get { return shadowMap; }
            set { shadowMap = value; }
        }
        public Matrix lightsViewProjectionMatrix;
        public Matrix woldsViewProjection;
        public Shadow()
        {
            this.lightsViewProjectionMatrix = Matrix.Identity;
        }

        public void UpdateLightData(float lightPower, Vector3 lightPos, GameCamera.FreeCamera camera)
        {
            //ambientPower = 0.2f;

            //lightPos = new Vector3(-18, 5, -2);
            //lightPower = 1.0f;

            Matrix lightsView = Matrix.CreateLookAt(lightPos, camera.Target, new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1.333333f, 100f, 1000f);

            lightsViewProjectionMatrix = Matrix.Identity * lightsView * lightsProjection;
            woldsViewProjection = Matrix.Identity * camera.View * camera.Projection;
        }

        public void setShadowMap()
        {
            this.ShadowMap = (Texture2D)this.RenderTarget;
        }
        public void ShadowToText()
        {

        }
    }
}

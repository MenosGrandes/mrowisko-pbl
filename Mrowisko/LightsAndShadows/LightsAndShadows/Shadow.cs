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

        Vector2[] pcfSamples;
        public Vector2[] PcfSamples
        {
            get { return pcfSamples; }
            set { pcfSamples = value; }
        }
        public Matrix lightsViewProjectionMatrix;
        public Matrix woldsViewProjection;
        public Matrix lightsView;
        public Matrix lightsProjection;
        public Shadow()
        {
            this.lightsViewProjectionMatrix = Matrix.Identity;
            float texelSize = 2.0f / 2048.0f;

            pcfSamples = new Vector2[17];
            pcfSamples[0] = new Vector2(0.0f, 0.0f);
            pcfSamples[1] = new Vector2(-texelSize, 0.0f);
            pcfSamples[2] = new Vector2(texelSize, 0.0f);

            pcfSamples[3] = new Vector2(-texelSize/2, 0.0f);
            pcfSamples[4] = new Vector2(texelSize/2, 0.0f);

            pcfSamples[5] = new Vector2(0.0f, -texelSize);
            pcfSamples[6] = new Vector2(-texelSize, -texelSize);
            pcfSamples[7] = new Vector2(texelSize, -texelSize);

            pcfSamples[8] = new Vector2(0.0f, -texelSize/2);
            pcfSamples[9] = new Vector2(-texelSize/2, -texelSize/2);
            pcfSamples[10] = new Vector2(texelSize / 2, -texelSize / 2);

            pcfSamples[11] = new Vector2(0.0f, texelSize);
            pcfSamples[12] = new Vector2(-texelSize, texelSize);
            pcfSamples[13] = new Vector2(texelSize, texelSize);

            pcfSamples[14] = new Vector2(0.0f, texelSize/2);
            pcfSamples[15] = new Vector2(-texelSize/2, texelSize/2);
            pcfSamples[16] = new Vector2(texelSize/2, texelSize/2);
        }

        public void UpdateLightData(float lightPower, Vector3 lightPos, GameCamera.FreeCamera camera)
        {
            //ambientPower = 0.2f;

            //lightPos = new Vector3(-18, 5, -2);
            //lightPower = 1.0f;

             lightsView = Matrix.CreateLookAt(lightPos, /*camera.Target*/ new Vector3(2048,-1000,2048), new Vector3(0, 1, 0));
             lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi/9, 1.333333f, 1f, 50000f);

            lightsViewProjectionMatrix = lightsView * lightsProjection;
            woldsViewProjection = camera.View * camera.Projection;
        }

        public void setShadowMap()
        {
            this.ShadowMap = (Texture2D)this.RenderTarget;
        }
  
    }
}

﻿using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class Water
    {
        float waterHeight, terrainWidth, terrainLength;
        int Scale;
        Effect effect;
        GraphicsDevice device;
        private RenderTarget2D refractionRenderTarget;
        public RenderTarget2D reflectionRenderTarget { get; set; }

        private Texture2D reflectionMap;
        private Texture2D refractionMap;
        private Texture2D waterBumpMap;
        private Vector3 windDirection = new Vector3(0, 0, 1);
        public VertexBuffer waterVertexBuffer { get; set; }
        public Plane waterPlane;
       public SkyDome sky;
        public Water(GraphicsDevice device, ContentManager Content, float terrainLength, int scale)
        {
            this.effect = Content.Load<Effect>("Effects/MultiTextured");
            this.waterBumpMap = Content.Load<Texture2D>("Textures/Water/waterbump");
            this.Scale = scale;
            this.device = device;
            this.waterHeight = 6;
            this.terrainLength = terrainLength * Scale;
            this.terrainWidth = this.terrainLength;


            PresentationParameters pp = device.PresentationParameters;


            refractionRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

            reflectionRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            SetUpWaterVertices();
            sky = new SkyDome(device, Content);
        }
        private void SetUpWaterVertices()
        {
            VertexPositionTexture[] waterVertices = new VertexPositionTexture[6];

            waterVertices[0] = new VertexPositionTexture(new Vector3(0, waterHeight, 0), new Vector2(0, 1));
            waterVertices[1] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, terrainWidth), new Vector2(1, 0));
            waterVertices[2] = new VertexPositionTexture(new Vector3(0, waterHeight, terrainWidth), new Vector2(0, 0));

            waterVertices[3] = new VertexPositionTexture(new Vector3(0, waterHeight, 0), new Vector2(0, 1));
            waterVertices[4] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, 0), new Vector2(1, 1));
            waterVertices[5] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, terrainLength), new Vector2(1, 0));
            waterVertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, waterVertices.Count(), BufferUsage.WriteOnly);


            waterVertexBuffer.SetData(waterVertices);

        }
        private Plane CreatePlane(float height, Vector3 planeNormalDirection, Matrix currentViewMatrix, bool clipSide)
        {
            planeNormalDirection.Normalize();
            Vector4 planeCoeffs = new Vector4(planeNormalDirection, height);
            if (clipSide) planeCoeffs *= -1;
            /*
            Matrix worldViewProjection = currentViewMatrix * projectionMatrix;
            Matrix inverseWorldViewProjection = Matrix.Invert(worldViewProjection);
            inverseWorldViewProjection = Matrix.Transpose(inverseWorldViewProjection);
              */
            Plane finalPlane = new Plane(planeCoeffs);
            return finalPlane;
        }

        public void DrawRefractionMap(GameCamera.FreeCamera camera, float time, LightsAndShadows.Shadow shadow, LightsAndShadows.Light light,QuadTree tree)
        {
            waterPlane = CreatePlane(waterHeight + 1.5f, new Vector3(0, -1, 0), camera.View, false);

            //refractionPlane = CreatePlane(30.0045F, new Vector3(0, -1, 0), viewMatrix, false);
            effect.Parameters["ClipPlane0"].SetValue(new Vector4(waterPlane.Normal, waterPlane.D));
            effect.Parameters["Clipping"].SetValue(true);    // Allows the geometry to be clipped for the purpose of creating a refraction map
            device.SetRenderTarget(refractionRenderTarget);
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
         //   tree.Draw(camera, time, shadow, light);
            device.SetRenderTarget(null);
            effect.Parameters["Clipping"].SetValue(false);   // Make sure you turn it back off so the whole scene doesnt keep rendering as clipped
            refractionMap = refractionRenderTarget;

        }

        public void DrawReflectionMap(GameCamera.FreeCamera camera, float time, LightsAndShadows.Shadow shadow, LightsAndShadows.Light light, QuadTree tree)
        {
            Plane reflectionPlane = CreatePlane(waterHeight - 0.5f, new Vector3(0, -1, 0), camera.reflectionViewMatrix, true);
            //device.ClipPlanes[0].Plane = reflectionPlane;
            //device.ClipPlanes[0].IsEnabled = true;
            //device.SetRenderTarget(0, reflectionRenderTarget);

            effect.Parameters["ClipPlane0"].SetValue(new Vector4(reflectionPlane.Normal, reflectionPlane.D));
            //effect.Parameters["ClipPlane0"].SetValue(new Vector4(-reflectionPlane.Normal, -reflectionPlane.D));

            effect.Parameters["Clipping"].SetValue(true);    // Allows the geometry to be clipped for the purpose of creating a refraction map
            device.SetRenderTarget(reflectionRenderTarget);


            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            sky.DrawSkyDome(camera);
           // tree.Draw(camera, time, shadow, light);

            //device.ClipPlanes[0].IsEnabled = false;
            effect.Parameters["Clipping"].SetValue(false);

            //device.SetRenderTarget(0, null);
            device.SetRenderTarget(null);

            reflectionMap = reflectionRenderTarget;
            //System.IO.Stream ss = System.IO.File.OpenWrite("C:\Test\Reflection.jpg");
            //reflectionRenderTarget.SaveAsJpeg(ss, 500, 500);
            //ss.Close();
        }

        public void DrawWater(float time, FreeCamera camera)
        {


            effect.CurrentTechnique = effect.Techniques["Water"];
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(camera.View);
            effect.Parameters["xReflectionView"].SetValue(camera.reflectionViewMatrix);
            effect.Parameters["xProjection"].SetValue(camera.Projection);
            effect.Parameters["xCamPos"].SetValue(camera.Position);
            effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            effect.Parameters["xRefractionMap"].SetValue(refractionMap);
            effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
            effect.Parameters["xWaveLength"].SetValue(0.1f);
            effect.Parameters["xWaveHeight"].SetValue(0.3f);
            effect.Parameters["xTime"].SetValue(time);
            effect.Parameters["xWindForce"].SetValue(0.002f);
            effect.Parameters["xWindDirection"].SetValue(windDirection);


            effect.CurrentTechnique.Passes[0].Apply();


            device.SetVertexBuffer(waterVertexBuffer);

            device.DrawPrimitives(PrimitiveType.TriangleList, 0, waterVertexBuffer.VertexCount / 3);
        }



    }
}

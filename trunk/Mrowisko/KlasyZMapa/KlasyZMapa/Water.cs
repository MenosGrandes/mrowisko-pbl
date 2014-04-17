using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    class Water
    {  /*
        Effect effect;
        const float waterHeight = 5.0f;
        RenderTarget2D refractionRenderTarget;
        Texture2D refractionMap;

        RenderTarget2D reflectionRenderTarget;
        Texture2D reflectionMap;

        GraphicsDevice device;
        VertexBuffer waterVertexBuffer;
        Matrix reflectionViewMatrix;
        
        void InitializeWater()
        {

            PresentationParameters pp = device.PresentationParameters;
            refractionRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            reflectionRenderTarget = new RenderTarget2D(device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);

        
        }

        private Plane CreatePlane(float height, Vector3 planeNormalDirection, Matrix currentViewMatrix, bool clipSide)
        {
            planeNormalDirection.Normalize();
            Vector4 planeCoeffs = new Vector4(planeNormalDirection, height);
            if (clipSide) planeCoeffs *= -1;
            Plane finalPlane = new Plane(planeCoeffs);
            return finalPlane;
        }
        private void DrawRefractionMap()
        {
            Plane refractionPlane = CreatePlane(waterHeight + 1.5f, new Vector3(0, -1, 0), viewMatrix, false);
            effect.Parameters["ClipPlane0"].SetValue(new Vector4(refractionPlane.Normal, refractionPlane.D));
            effect.Parameters["Clipping"].SetValue(true);    // Allows the geometry to be clipped for the purpose of creating a refraction map
            device.SetRenderTarget(refractionRenderTarget);
            device.
        * (ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            device.SetRenderTarget(null);
            effect.Parameters["Clipping"].SetValue(false);   // Make sure you turn it back off so the whole scene doesnt keep rendering as clipped
            refractionMap = refractionRenderTarget;

        }

        private void DrawReflectionMap()
        {
            Plane reflectionPlane = CreatePlane(waterHeight - 0.5f, new Vector3(0, -1, 0), reflectionViewMatrix, true);

            effect.Parameters["ClipPlane0"].SetValue(new Vector4(reflectionPlane.Normal, reflectionPlane.D));

            effect.Parameters["Clipping"].SetValue(true);    // Allows the geometry to be clipped for the purpose of creating a refraction map
            device.SetRenderTarget(reflectionRenderTarget);


           
           
            effect.Parameters["Clipping"].SetValue(false);
            device.SetRenderTarget(null);

            reflectionMap = reflectionRenderTarget;

        }

        private void DrawWater(float time)
        {
            effect.CurrentTechnique = effect.Techniques["Water"];
            Matrix worldMatrix = Matrix.Identity;
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xReflectionView"].SetValue(reflectionViewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xReflectionMap"].SetValue(reflectionMap);
            effect.Parameters["xRefractionMap"].SetValue(refractionMap);
            effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
            effect.Parameters["xWaterBumpMap"].SetValue(waterBumpMap);
            effect.Parameters["xWaveLength"].SetValue(0.1f);
            effect.Parameters["xWaveHeight"].SetValue(0.3f);

            effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(waterVertexBuffer);

            device.DrawPrimitives(PrimitiveType.TriangleList, 0, waterVertexBuffer.VertexCount / 3);
        }
        private void SetUpWaterVertices()
        {
            VertexPositionTexture[] waterVertices = new VertexPositionTexture[6];

            waterVertices[0] = new VertexPositionTexture(new Vector3(0, waterHeight, 0), new Vector2(0, 1));
            waterVertices[2] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, -terrainLength), new Vector2(1, 0));
            waterVertices[1] = new VertexPositionTexture(new Vector3(0, waterHeight, -terrainLength), new Vector2(0, 0));

            waterVertices[3] = new VertexPositionTexture(new Vector3(0, waterHeight, 0), new Vector2(0, 1));
            waterVertices[5] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, 0), new Vector2(1, 1));
            waterVertices[4] = new VertexPositionTexture(new Vector3(terrainWidth, waterHeight, -terrainLength), new Vector2(1, 0));

            waterVertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, waterVertices.Count(), BufferUsage.WriteOnly);
            waterVertexBuffer.SetData(waterVertices);
        }
              */
    }
}

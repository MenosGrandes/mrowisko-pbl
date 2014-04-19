using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameCamera;

namespace AntHill
{
   public class Light 
    {

        private RenderTarget2D colorRT; //color and specular intensity
        private RenderTarget2D normalRT; //normals + specular power
        private RenderTarget2D depthRT; //depth
        private RenderTarget2D lightRT; //lighting

        private Effect clearBufferEffect;
        private Effect directionalLightEffect;

        private Effect pointLightEffect;
        private Model sphereModel; //point ligt volume

        private Effect finalCombineEffect;

        private SpriteBatch spriteBatch;

        private Vector2 halfPixel;

        private GameCamera.FreeCamera camera;

        public Light(Game1 game)
         {
             camera=(FreeCamera)game.camera;
      
            halfPixel = new Vector2()
            {
                X = 0.5f / (float)game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Y = 0.5f / (float)game.GraphicsDevice.PresentationParameters.BackBufferHeight
                
            };

            int backbufferWidth = game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int backbufferHeight = game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            colorRT = new RenderTarget2D(game.GraphicsDevice, backbufferWidth, backbufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            normalRT = new RenderTarget2D(game.GraphicsDevice, backbufferWidth, backbufferHeight, false, SurfaceFormat.Color, DepthFormat.None); 
            depthRT = new RenderTarget2D(game.GraphicsDevice, backbufferWidth, backbufferHeight, false, SurfaceFormat.Single, DepthFormat.None);
            lightRT = new RenderTarget2D(game.GraphicsDevice, backbufferWidth, backbufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
      

            clearBufferEffect = game.Content.Load<Effect>("ClearGBuffer");
            directionalLightEffect = game.Content.Load<Effect>("DirectionaLight");
            finalCombineEffect = game.Content.Load<Effect>("CombineFinal");
            pointLightEffect = game.Content.Load<Effect>("PointLight");
           // sphereModel = Game.Content.Load<Model>("sphere");
           
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            
        }



        private void SetGBuffer(Game game)
        {
            game.GraphicsDevice.SetRenderTargets(colorRT, normalRT, depthRT);
        }

        private void ResolveGBuffer(Game game)
        {
            game.GraphicsDevice.SetRenderTargets(null);
        }

        private void ClearGBuffer()
        {
            clearBufferEffect.Techniques[0].Passes[0].Apply();
           // quadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        private void DrawDirectionalLight(Vector3 lightDirection, Color color, GameCamera.FreeCamera camera)
        {
            directionalLightEffect.Parameters["colorMap"].SetValue(colorRT);
            directionalLightEffect.Parameters["normalMap"].SetValue(normalRT);
            directionalLightEffect.Parameters["depthMap"].SetValue(depthRT);

            directionalLightEffect.Parameters["lightDirection"].SetValue(lightDirection);
            directionalLightEffect.Parameters["Color"].SetValue(color.ToVector3());

            directionalLightEffect.Parameters["cameraPosition"].SetValue(camera.Position);
            directionalLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(camera.View * camera.Projection));

            directionalLightEffect.Parameters["halfPixel"].SetValue(halfPixel);

            directionalLightEffect.Techniques[0].Passes[0].Apply();
           // quadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        private void DrawPointLight(Vector3 lightPosition, Color color, float lightRadius, float lightIntensity, Game game)
        {
            //set the G-Buffer parameters
            pointLightEffect.Parameters["colorMap"].SetValue(colorRT);
            pointLightEffect.Parameters["normalMap"].SetValue(normalRT);
            pointLightEffect.Parameters["depthMap"].SetValue(depthRT);

            //compute the light world matrix
            //scale according to light radius, and translate it to light position
            Matrix sphereWorldMatrix = Matrix.CreateScale(lightRadius) * Matrix.CreateTranslation(lightPosition);
            pointLightEffect.Parameters["World"].SetValue(sphereWorldMatrix);
            pointLightEffect.Parameters["View"].SetValue(camera.View);
            pointLightEffect.Parameters["Projection"].SetValue(camera.Projection);
            //light position
            pointLightEffect.Parameters["lightPosition"].SetValue(lightPosition);

            //set the color, radius and Intensity
            pointLightEffect.Parameters["Color"].SetValue(color.ToVector3());
            pointLightEffect.Parameters["lightRadius"].SetValue(lightRadius);
            pointLightEffect.Parameters["lightIntensity"].SetValue(lightIntensity);

            //parameters for specular computations
            pointLightEffect.Parameters["cameraPosition"].SetValue(camera.Position);
            pointLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(camera.View * camera.Projection));
            //size of a halfpixel, for texture coordinates alignment
            pointLightEffect.Parameters["halfPixel"].SetValue(halfPixel);
            //calculate the distance between the camera and light center
            float cameraToCenter = Vector3.Distance(camera.Position, lightPosition);
            //if we are inside the light volume, draw the sphere's inside face
            if (cameraToCenter < lightRadius)
                game.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            else
                game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            game.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            pointLightEffect.Techniques[0].Passes[0].Apply();
          /* foreach (ModelMesh mesh in sphereModel.Meshes)
           {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    game.GraphicsDevice.Indices = meshPart.IndexBuffer;
                    game.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                    game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
           }*/
            
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void DrawLight(GameTime gameTime, Game game)
        {
            SetGBuffer(game);
            ClearGBuffer();
            ResolveGBuffer(game);
            DrawLights(gameTime, game);
        }

        private void DrawLights(GameTime gameTime, Game game)
        {
            game.GraphicsDevice.SetRenderTarget(lightRT);
            game.GraphicsDevice.Clear(Color.Transparent);
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            Color[] colors = new Color[10];
            colors[0] = Color.Red; colors[1] = Color.Blue;
            colors[2] = Color.IndianRed; colors[3] = Color.CornflowerBlue;
            colors[4] = Color.Gold; colors[5] = Color.Green;
            colors[6] = Color.Crimson; colors[7] = Color.SkyBlue;
            colors[8] = Color.Red; colors[9] = Color.ForestGreen;
            float angle = (float)gameTime.TotalGameTime.TotalSeconds;
            int n = 15;

            for (int i = 0; i < n; i++)
            {
                Vector3 pos = new Vector3((float)Math.Sin(i * MathHelper.TwoPi / n + angle), 0.30f, (float)Math.Cos(i * MathHelper.TwoPi / n + angle));
                DrawPointLight(pos * 40, colors[i % 10], 15, 2, game);
                pos = new Vector3((float)Math.Cos((i + 5) * MathHelper.TwoPi / n - angle), 0.30f, (float)Math.Sin((i + 5) * MathHelper.TwoPi / n - angle));
                DrawPointLight(pos * 20, colors[i % 10], 20, 1, game);
                pos = new Vector3((float)Math.Cos(i * MathHelper.TwoPi / n + angle), 0.10f, (float)Math.Sin(i * MathHelper.TwoPi / n + angle));
                DrawPointLight(pos * 75, colors[i % 10], 45, 2, game);
                pos = new Vector3((float)Math.Cos(i * MathHelper.TwoPi / n + angle), -0.3f, (float)Math.Sin(i * MathHelper.TwoPi / n + angle));
                DrawPointLight(pos * 20, colors[i % 10], 20, 2, game);
            }

            DrawPointLight(new Vector3(0, (float)Math.Sin(angle * 0.8) * 40, 0), Color.Red, 30, 5, game);
            DrawPointLight(new Vector3(0, 25, 0), Color.White, 30, 1, game);
            DrawPointLight(new Vector3(0, 0, 70), Color.Wheat, 55 + 10 * (float)Math.Sin(5 * angle), 3, game);

            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            game.GraphicsDevice.SetRenderTarget(null);

            //Combine everything
            finalCombineEffect.Parameters["colorMap"].SetValue(colorRT);
            finalCombineEffect.Parameters["lightMap"].SetValue(lightRT);
            finalCombineEffect.Parameters["halfPixel"].SetValue(halfPixel);

            finalCombineEffect.Techniques[0].Passes[0].Apply();

        }





    }
}

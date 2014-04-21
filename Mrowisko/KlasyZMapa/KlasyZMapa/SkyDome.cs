using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public class SkyDome
    {
        Texture2D cloudMap;
        Model skyDome;
        Effect effect;
        GraphicsDevice device;

        public SkyDome(GraphicsDevice device, ContentManager Content)
        {

            this.device = device;
            this.effect = Content.Load<Effect>("Effects/TexturedEffect");
            skyDome = Content.Load<Model>("Models/SkyDome/dome");
            
            cloudMap = Content.Load<Texture2D>("Models/SkyDome/cloudMap");
            skyDome.Meshes[0].MeshParts[0].Effect = effect.Clone();

        }



        public void DrawSkyDome(FreeCamera camera)
        {
            device.DepthStencilState = DepthStencilState.None;
            Matrix[] modelTransforms = new Matrix[skyDome.Bones.Count];
            skyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Matrix wMatrix = Matrix.CreateFromYawPitchRoll(camera.Yaw,camera.Pitch,0)*Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(1000*100) * Matrix.CreateTranslation(camera.Position);

            foreach (ModelMesh mesh in skyDome.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(camera.View);
                    currentEffect.Parameters["xProjection"].SetValue(camera.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                  
                }
                mesh.Draw();
            }
            device.BlendState = BlendState.Opaque;
           device.DepthStencilState = DepthStencilState.Default;
        }
    }
}

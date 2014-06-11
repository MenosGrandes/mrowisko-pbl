using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameCamera;

namespace HUD
{
    public class Circle
    {
     
        private float scale;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

     

        private VertexBuffer VertexBuffer;
        private Effect bbEffect;
       
        public Circle()
        {
            this.bbEffect = StaticHelpers.StaticHelper.Content.Load<Effect>("Effects/HUD2");
            this.scale = 8;
            

            bbEffect.CurrentTechnique = bbEffect.Techniques["CylBillboard"];
            bbEffect.Parameters["xAllowedRotDir"].SetValue(new Vector3(1, 0, 0));
            

        }

        public void update(Texture2D bilboardTexture)
        {
            bbEffect.Parameters["xBillboardTexture"].SetValue(bilboardTexture);
        }

        public void CreateBillboardVerticesFromList(Vector3 currentV3)
        {

            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[6];


            billboardVertices[0] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
            billboardVertices[1] = new VertexPositionTexture(currentV3, new Vector2(1, 1));
            billboardVertices[2] = new VertexPositionTexture(currentV3, new Vector2(0, 1));

            billboardVertices[3] = new VertexPositionTexture(currentV3, new Vector2(0, 0));
            billboardVertices[4] = new VertexPositionTexture(currentV3, new Vector2(1, 0));
            billboardVertices[5] = new VertexPositionTexture(currentV3, new Vector2(1, 1));


            VertexBuffer = new VertexBuffer(StaticHelpers.StaticHelper.Device, VertexPositionTexture.VertexDeclaration, billboardVertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(billboardVertices);
        }



        public void healthDraw(FreeCamera camera)
        {
            bbEffect.Parameters["xScale"].SetValue(this.scale);
            bbEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
            bbEffect.Parameters["xView"].SetValue(camera.View);
            bbEffect.Parameters["xProjection"].SetValue(camera.Projection);
            bbEffect.Parameters["xCamPos"].SetValue(camera.Position);
            bbEffect.Parameters["xAmbient"].SetValue(0);
            StaticHelpers.StaticHelper.Device.SetVertexBuffer(VertexBuffer);
            foreach (EffectPass pass in bbEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                StaticHelpers.StaticHelper.Device.DrawPrimitives(PrimitiveType.TriangleList, 0, VertexBuffer.VertexCount / 3);

            }
        }
    }
}

using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Logic.EnviroModel
{
    [Serializable]
    public class Cone: EnviroModels
    {
        public Cone(LoadModel model)
            : base(model)
        {
            selectable = false;
            this.hp = 350;
           // LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
           // circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            //LifeBar.LifeLength = model.Scale.X * 100;
        }
        public Cone()
            : base()
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
       
        }
    }
}


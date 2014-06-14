using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Predators
{
    [Serializable]
    public class Spider:Predator
    {
         public Spider():base()
       { }
       public Spider(LoadModel model):base(model)
       {
           LifeBar.LifeLength = model.Scale.X * 100;

           LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));

       }
       public Spider(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model,float atackInterval)
           : base(hp, armor, strength, range, cost, buildingTime, model,atackInterval)
       { }
       public override void DrawSelected(GameCamera.FreeCamera camera)
       {
           base.DrawSelected(camera);
       }
    }
}

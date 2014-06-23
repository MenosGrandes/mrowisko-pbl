using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    [Serializable]
    public class Spider:Predator
    {
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private int range = 120;
        private int snared_max = 3;
        private int snared = 0;
        private float time = 0.0f;
        private float attack_speed = 2.0f;
        private int damage = 30;
         public Spider():base()
       { }
       public Spider(LoadModel model):base(model)
       {
           LifeBar.LifeLength = model.Scale.X * 100;
           selectable = false;
           LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));

       }
       public Spider(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model,float atackInterval)
           : base(hp, armor, strength, range, cost, buildingTime, model,atackInterval)
       { selectable = false; }
       
         public Spider(LoadModel model, List<InteractiveModel> ants)
            : base(model)
        {
            selectable = false;
            this.Ants = ants;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.Hp = 100;
        }
        
        
        public override void DrawSelected(GameCamera.FreeCamera camera)
       {
           base.DrawSelected(camera);
       }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for(int i=0;i<Ants.Count;i++)
            {
                float spr = (float)Math.Sqrt(Math.Pow(Ants[i].Model.Position.X - this.Model.Position.X, 2.0) + (float)Math.Pow(Ants[i].Model.Position.Z - this.Model.Position.Z, 2.0));
               // Console.WriteLine(spr +" "+ Ants[i].GetType());
                if (spr <= range && this != Ants[i] && snared < snared_max && Ants[i].snr==false)
                {
                    if (Ants[i] is Unit && !(Ants[i] is Predator))
                    {
                        Ants[i].snr = true;
                        snared++;
                        Console.WriteLine("unieruchomienie " + Ants[i].GetType());
                    }
                }

            }

            for (int j = 0; j < Ants.Count; j++)
            {
                if (Ants[j].snr == true && !(Ants[j] is Predator))
                 {
                     if (!(this.Model.BoundingSphere.Intersects(Ants[j].Model.BoundingSphere)))
                      this.reachTarget(gameTime, this, Ants[j].Model.Position);
                     else
                     {
                         time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                         if (time > 2.0f)
                         {
                             Ants[j].Hp -= damage;
                             time = 0;
                         }
                     }
                     break;
                 }
            }
        
        }

    }
}

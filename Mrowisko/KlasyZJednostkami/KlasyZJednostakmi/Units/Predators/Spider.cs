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
        [NonSerialized]
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        [NonSerialized]
        private int range = 250;
        [NonSerialized]
        private int snared_max = 3;
        [NonSerialized]
        private int snared = 0;
        [NonSerialized]
        private float time = 0.0f;
        [NonSerialized]
        private float attack_speed = 2.0f;
        [NonSerialized]
        private int damage = 30;
        [NonSerialized]
        private Vector3 home;
         public Spider():base()
       { }
       public Spider(LoadModel model):base(model)
       {
           LifeBar.LifeLength = model.Scale.X * 100;
           selectable = false;
           LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
           this.Hp = 100;
           this.modelHeight = 26;
           this.MaxHp = this.Hp;
           this.home = this.model.Position;

       }
       public Spider(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model,float atackInterval)
           : base(hp, armor, strength, range, cost, buildingTime, model,atackInterval)
       { selectable = true; this.modelHeight = 26; this.MaxHp = this.Hp; }
       
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
            this.MaxHp = this.Hp;
            this.modelHeight = 26;
            this.home = this.model.Position;
        }

         public void removeMyself()
         { 
            foreach(InteractiveModel model in Ants)
            {
                  if(model==this)
                  {
                      Ants.Remove(model);
                      return;
                  }
            }
         }
        public override void DrawSelected(GameCamera.FreeCamera camera)
       {
           base.DrawSelected(camera);
       }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int counter = 0;
            
          

            for(int i=0;i<Ants.Count;i++)
            {

                float spr = Vector2.Distance(new Vector2(Ants[i].Model.Position.X, Ants[i].Model.Position.Z), new Vector2(this.Model.Position.X, this.Model.Position.Z));
               // Console.WriteLine(spr +" "+ Ants[i].GetType());
                if (spr <= range && snared < snared_max && Ants[i].Model.snr==false && !(Ants[i] is Predator))
                {
                    
                        Ants[i].Model.snr = true;
                        Ants[i].Model.spiderTarget = true;
                        snared++;
                        
                        //Console.WriteLine("unieruchomienie " + Ants[i].GetType());
                    
                }

            }
            
            for (int j = 0; j < Ants.Count; j++)
            {

                if (Ants[j].Model.spiderTarget)
                {
                    counter++;
                }

                if ((Ants[j].Model.snr == true && !(Ants[j] is Predator) )|| Ants[j].Model.spiderTarget)
                 {
                     if (!(this.Model.BoundingSphere.Intersects(Ants[j].Model.BoundingSphere)))
                     {
                         this.model.switchAnimation("Walk");
                         this.reachTargetAutonomus(gameTime, Ants[j].Model.Position);
                         this.model.Rotation = new Vector3(this.model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(this.model.Position.X,this.model.Position.Z), new Vector2(Ants[j].Model.Position.X,Ants[j].Model.Position.Z),this.model.Rotation.Y, 1.05f), model.Rotation.Z);
                     }
                     else if (this.Model.BoundingSphere.Intersects(Ants[j].Model.BoundingSphere))
                     {
                         time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                         this.model.switchAnimation("Atack");
                         if (time > 2.0f)
                         {
                             Ants[j].Hp -= damage;
                             Ants[j].hasBeenHit = true;
                             ((Unit)Ants[j]).LifeBar.LifeLength -= ((Unit)Ants[j]).LifeBar.LifeLength * (((float)damage) / (float)Ants[j].Hp);
                             time = 0;
                         }
                     }
                    
                     break;
                 }
               
            }

            if (counter==0 && Vector2.Distance(new Vector2(this.Model.Position.X, this.Model.Position.Z), new Vector2(this.home.X, this.home.Z)) > this.model.BoundingSphere.Radius)
            {
                this.model.switchAnimation("Walk");
                this.reachTargetAutonomus(gameTime, this.home);
                this.model.Rotation = new Vector3(this.model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(this.model.Position.X, this.model.Position.Z), new Vector2(this.home.X, this.home.Z), this.model.Rotation.Y, 1.05f), model.Rotation.Z);

            }
            else if(counter==0 && Vector2.Distance(new Vector2(this.Model.Position.X, this.Model.Position.Z), new Vector2(this.home.X, this.home.Z)) <= this.model.BoundingSphere.Radius)
            {
                this.model.switchAnimation("Idle");
            }
        
        }

    }
}

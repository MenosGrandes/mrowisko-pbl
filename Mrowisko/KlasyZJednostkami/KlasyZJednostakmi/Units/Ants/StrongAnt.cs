using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Ants
{
        [Serializable]
    public class StrongAnt:Ant
    {
   
        public StrongAnt()
            : base()
        {
            this.armor = 50; 
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;
        }
        public StrongAnt(LoadModel model)
            : base(model)
        {
            this.armor = 50; 
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            hp = 100;
            range = 1000;
            atackInterval = 10;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;
        }
        public StrongAnt(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            this.armor = 50; 
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;

        }
        public override void Update(GameTime time)
        {
            base.Update(time);

        }
        public override void Attack()
        {
            this.model.switchAnimation("Atack");
            //if (range <= Math.Abs(model.Position.X - a.Model.Position.Y) + Math.Abs(model.Position.X - a.Model.Position.Y))
            if (elapsedTime >= atackInterval)
            {
                this.target.hasBeenHit = true;
                this.target.Hp -=  (int)this.strength;
                ((Unit)this.target).LifeBar.LifeLength -= ((Unit)this.target).LifeBar.LifeLength * ((100 * 1) / this.target.Hp);
              //  bullets.Add(new SpitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), target.Model.Position));
                elapsedTime = 0;
            }
        }

 
  
      

        
      /*  public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
           // foreach (SpitMissle sm in bullets)
           // {
            //    sm.Draw(camera);
            //}
        }
        */

        public override void DrawSelected(GameCamera.FreeCamera camera)
        { 
            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3 (0, 1, 0) * model.Scale * 50);
            LifeBar.healthDraw(camera);

        }

        public override void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {
            circle.CreateBillboardVerticesFromList(model.Position + new Vector3(-2, 0.1f, -0.1f) * model.Scale * 50);
            circle.healthDraw(camera);
        }
        public override string ToString()
        {
            return this.GetType().Name + " " + armor;
        }

      
            public override void Intersect(InteractiveModel interactive)
            {
                if (this == interactive)
                { return; }

                 foreach(BoundingSphere sphere in interactive.Model.Spheres)
                 { 
                    if (this.model.BoundingSphere.Intersects(sphere))
                    {

                        Hit(interactive);
                        Console.WriteLine(interactive.ToString());
                        
                    }
                 }
                
            }
            public void Hit(InteractiveModel b)
            {
                if(b.GetType().IsSubclassOf(typeof(Predator)))
                { 
                b.Hp -= 1;
                Console.WriteLine("Siłuje się!");
                ((Unit)b).LifeBar.LifeLength -= 1;
                b.hasBeenHit = true;
                b.Model.Hit = true;
                SoundController.SoundController.Play(SoundController.SoundEnum.RangeHit);
                }
                else if (b.GetType()==typeof(EnviroModel.Cone) || b.GetType()==typeof(EnviroModel.Cone1))
                {
                    if (b.Hp > 0)
                    {
                        b.Hp -= 1;
                        Console.WriteLine("Niszcze szyszke!");
                        ((EnviroModel.EnviroModels)b).LifeBar.LifeLength -= 1;
                        b.hasBeenHit = true;
                        b.Model.Hit = true;
                        b.Model.Rotation -= new Vector3(0, 0.01f, 0);
                        b.Model.Position -= new Vector3(0.1f, 0, 0);
                    }
                   }
            }
            public override void Draw(GameCamera.FreeCamera camera)
            {
                model.Draw(camera);
            }
        }
     
    }


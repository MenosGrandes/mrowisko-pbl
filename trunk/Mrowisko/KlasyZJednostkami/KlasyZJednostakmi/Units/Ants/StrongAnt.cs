﻿using Microsoft.Xna.Framework;
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
            private float time = 0.0f;
   
        public StrongAnt()
            : base()
        {
            this.armor = 50; 
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight =0;
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
            this.modelHeight = 0;
            this.MaxHp = 100;
            this.Model.switchAnimation("Idle");

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
            this.MaxHp = 100;

        }
        public override void Update(GameTime time)
        {
            base.Update(time);

        }
        public override void Attack(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.model.switchAnimation("Atack");
            //if (range <= Math.Abs(model.Position.X - a.Model.Position.Y) + Math.Abs(model.Position.X - a.Model.Position.Y))
            if (time > 2.0f)
            {

                if (this.target.Hp < 0)
                {
                    this.target = null;
                    this.attacking = false;
                    if (this.ImMoving)
                        this.model.switchAnimation("Walk");
                    else
                    {
                        this.model.switchAnimation("Idle");
                    }
                }
                else
                {
                    this.target.Hp -= (int)this.strength;
                    ((Unit)this.target).LifeBar.LifeLength -= ((Unit)this.target).LifeBar.LifeLength * (this.strength / this.target.MaxHp);
                }
              //  bullets.Add(new SpitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), target.Model.Position));
                time = 0;
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

      
            public override void Intersect(InteractiveModel interactive)
            {
                if (this == interactive)
                { return; }

                 
                    if (this.model.BoundingSphere.Intersects(interactive.Model.BoundingSphere))
                    {

                        Hit(interactive);
                        
                    }
                 
                
            }
            public void Hit(InteractiveModel b)
            {
                
                
                if (b.GetType()==typeof(EnviroModel.Cone) || b.GetType()==typeof(EnviroModel.Cone1))
                {
                    if (b.Hp > 0)
                    {
                        b.Hp -= 1;
                        Console.WriteLine("Niszcze szyszke!");
                        ((EnviroModel.EnviroModels)b).LifeBar.LifeLength -= 1;
                        b.hasBeenHit = true;
                        b.Model.Hit = true;
                        b.Model.Rotation -= new Vector3(0, 0.1f, 0);
                       
                        b.Model.Position -= new Vector3(0, 0, 1f);


                    }
                   }
            }
            public override void Draw(GameCamera.FreeCamera camera)
            {
                model.Draw(camera);
            }
            public override string ToString()
            {
                return GetType().Name + " " + hp;
            }
        }
     
    }

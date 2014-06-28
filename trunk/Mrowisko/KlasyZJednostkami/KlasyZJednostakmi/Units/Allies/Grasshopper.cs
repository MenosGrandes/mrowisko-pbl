using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Logic.Units.Ants;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Units.Allies
{    [Serializable]
    public class GrassHopper:Allie
   {

        private float Scope;
        private float ArmorBuffValue;
        private float time = 0.0f;
        public Curve3D jumpPath;
        public List<PointInTime> pointsForJump = new List<PointInTime>();

        public GrassHopper(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float ArmorBuff)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.ArmorBuffValue = ArmorBuff;
        hp = 80;
        this.MaxHp = 80;
        this.modelHeight = 28;
        this.strength = 5;
    }
        public GrassHopper(LoadModel model):base(model)
        {
            Scope = 100;
            ArmorBuffValue = 100;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            hp = 80;
            this.MaxHp = 80;
            this.modelHeight = 28;
            this.strength = 5;
        }
        public GrassHopper()
            : base()
        {

        }
       public override void Update(GameTime time)
        {
            base.Update(time);
           if(Jumping)
           {
               time2 += (float)time.ElapsedGameTime.TotalMilliseconds;

               model.Position = jumpPath.GetPointOnCurve(time2);
               if(new Vector2( model.Position.X,model.Position.Z) == new Vector2( pointsForJump.Last().point.X,pointsForJump.Last().point.Z))
               {
                   Jumping = false;
                   pointsForJump.Clear();
                   jumpPath.removePoints();
                   myNode = getMyNode();
                   ImMoving = false;
                   destination = myNode.centerPosition;
               }
           }
           else
           {
               time2 = 0;
           }
        }
        public override string ToString()
        {
            return this.GetType().Name + base.model.Selected;
        }
        public override void DrawSelected(GameCamera.FreeCamera camera)
        {
            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3(0, 1, 0) * model.Scale * 50);
            LifeBar.healthDraw(camera);

        }

        public override void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {
            circle.CreateBillboardVerticesFromList(model.Position + new Vector3(-2, 0.1f, -0.1f) * model.Scale * 50);
            circle.healthDraw(camera);
        }

        public override void Attack(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.model.switchAnimation("Atack");
            this.hasBeenHit = true;
            //if (range <= Math.Abs(model.Position.X - a.Model.Position.Y) + Math.Abs(model.Position.X - a.Model.Position.Y))
            if (time > 2.0f)
            {

                if (this.target.Hp < 0)
                {
                    this.target = null;
                    this.attacking = false;
                    if(this.ImMoving)
                    this.model.switchAnimation("Walk");
                    else
                    {
                        this.model.switchAnimation("Idle");
                    }
                }
                else
                {
                    this.target.Hp -= (int)this.strength;
                    ((Unit)this.target).LifeBar.LifeLength -= ((Unit)this.target).LifeBar.LifeLength * ((this.strength) / this.target.MaxHp);
                }
                //  bullets.Add(new SpitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), target.Model.Position));
                time = 0;
            }
        }


        public float time2 { get; set; }
   }

}

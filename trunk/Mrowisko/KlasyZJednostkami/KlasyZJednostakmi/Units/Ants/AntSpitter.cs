using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Ants
{
    [Serializable]
    public class AntSpitter : Ant
    {
        private List<SpitMissle> bullets = new List<SpitMissle>();

        public List<SpitMissle> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }
        public AntSpitter()
            : base()
        {
            this.armor = 20; bullets = new List<SpitMissle>();
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;
        }
        public AntSpitter(LoadModel model)
            : base(model)
        {
            this.armor = 20; bullets = new List<SpitMissle>();
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            hp = 100;
            range = 1000;
            atackInterval = 60;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;
        }
        public AntSpitter(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            this.armor = 20; bullets = new List<SpitMissle>();
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

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(time);
                if (bullets[i].hit==true)
                {
                    bullets.RemoveAt(i);
                }
            }


        }
        public override void Attack()
        {
            //if (range <= Math.Abs(model.Position.X - a.Model.Position.Y) + Math.Abs(model.Position.X - a.Model.Position.Y))
            if (elapsedTime >= atackInterval)
            {
                bullets.Add(new SpitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), target.Model.Position));
                elapsedTime = 0;
            }
        }

        public override bool spitter()
        {
            return true;
        }

        public override List<Vector4> spitPos()
        {
            List<Vector4> bulletsPos = new List<Vector4>();
            foreach(SpitMissle bullet in bullets)
            {
                if(!bullet.hit)
                bulletsPos.Add(new Vector4(bullet.Model.Position.X, bullet.Model.Position.Y, bullet.Model.Position.Z, 0));
                else
                bulletsPos.Add(new Vector4(bullet.Model.Position.X, bullet.Model.Position.Y, bullet.Model.Position.Z, 1));
            }

            return bulletsPos;
        }
        public override void Intersect(InteractiveModel interactive)
        {
            if (this == interactive)
            { return; }
            foreach (SpitMissle sm in bullets)
            {
                sm.Intersect(interactive);
            }
            foreach (BoundingSphere b in model.Spheres)
            {

                foreach (BoundingSphere b2 in interactive.Model.Spheres)
                {
                    if (b.Intersects(b2))
                    {
                        Console.WriteLine("Plujka");
                    }
                }
            }

        }
        public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
            foreach (SpitMissle sm in bullets)
            {
                sm.Draw(camera);
            }
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
        public override string ToString()
        {
            return this.GetType().Name + " " + armor;
        }

        #region SpitMissle
        public class SpitMissle : InteractiveModel
        {
            public float speed=100;
            public bool hit = false;
            public float time_ = 0;
            public float time_to_point;
            public List<PointInTime> points = new List<PointInTime>();
            public Curve3D trajectory;
            public Vector3 targetPos;
            public SpitMissle(LoadModel model, Vector3 targtPosition)
                : base(model)
            {
                float distance = Math.Abs(model.Position.Y - targtPosition.Y) - Math.Abs(targtPosition.X - model.Position.X);
                time_to_point =  distance/speed;
                points.Add(new PointInTime(model.Position, 0));
                points.Add(new PointInTime(targtPosition,2000));
                trajectory = new Curve3D(points);
                
            }
            public SpitMissle()
                : base()
            { }
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
                        hit = true;
                    }
                 }
                
            }
            public void Hit(InteractiveModel b)
            {
                if(b.GetType().IsSubclassOf(typeof(Unit)))
                { 
                b.Hp -= 1;
                Console.WriteLine("Dostałą z kulki!");
                ((Unit)b).LifeBar.LifeLength -= ((Unit)b).LifeBar.LifeLength * ((100*1)/b.Hp );
                b.hasBeenHit = true;
                b.Model.Hit = true;
                SoundController.SoundController.Play(SoundController.SoundEnum.RangeHit);
                }
            }
            public override void Update(GameTime time)
            {
                base.Update(time);
                time_ += (float)time.ElapsedGameTime.TotalMilliseconds;
                //model.Position = trajectory.GetPointOnCurve(time_);
                model.Position = new Vector3(trajectory.GetPointOnCurve(time_).X, StaticHelpers.StaticHelper.GetHeightAt(trajectory.GetPointOnCurve(time_).X, trajectory.GetPointOnCurve(time_).Z), trajectory.GetPointOnCurve(time_).Z);

            }
            public override void Draw(GameCamera.FreeCamera camera)
            {
                model.Draw(camera);
            }
        }
        #endregion SpitMissle
    }
}

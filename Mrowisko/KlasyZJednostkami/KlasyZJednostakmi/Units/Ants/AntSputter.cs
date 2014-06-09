﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Ants
{
    public class AntSplitter : Ant
    {
        private List<SplitMissle> bullets = new List<SplitMissle>();

        public List<SplitMissle> Bullets
        {
            get { return bullets; }
            set { bullets = value; }
        }
        public AntSplitter()
            : base()
        {
            bullets = new List<SplitMissle>();
        }
        public AntSplitter(LoadModel model)
            : base(model)
        {
            bullets = new List<SplitMissle>();

        }
        public AntSplitter(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            bullets = new List<SplitMissle>();

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
        public override void Attack(InteractiveModel a)
        {
            if (elapsedTime >= atackInterval)
            {
                bullets.Add(new SplitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/trigger"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), a.Model.Position));
                Console.WriteLine("ATACK!!!!");
            }
        }
        public override void Intersect(InteractiveModel interactive)
        {
            if (this == interactive)
            { return; }
            foreach (SplitMissle sm in bullets)
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
            foreach (SplitMissle sm in bullets)
            {
                sm.Draw(camera);
            }
        }
        #region SplitMissle
        public class SplitMissle : InteractiveModel
        {
            public bool hit = false;
            public float time_ = 0;
            public List<PointInTime> points = new List<PointInTime>();
            public Curve3D trajectory;
            public Vector3 targetPos;
            public SplitMissle(LoadModel model, Vector3 targtPosition)
                : base(model)
            {
                float distance = Math.Abs(model.Position.X - targtPosition.Y) + Math.Abs(model.Position.X - targtPosition.Y);

                points.Add(new PointInTime(model.Position, 0));
                points.Add(new PointInTime(targtPosition, 20*distance/10));
                trajectory = new Curve3D(points);
            }
            public SplitMissle()
                : base()
            { }
            public override void Intersect(InteractiveModel interactive)
            {
                if (this == interactive)
                { return; }



                    if (this.model.BoundingSphere.Intersects(interactive.Model.BoundingSphere))
                    {

                        Hit(interactive);
                        hit = true;
                    }
                
            }
            public void Hit(InteractiveModel b)
            {
                // b.Hp -= 10;
                Console.WriteLine("Dostałą z kulki!");
                SoundController.SoundController.Play(SoundController.SoundEnum.RangeHit);
            }
            public override void Update(GameTime time)
            {
                base.Update(time);
                time_ += (float)time.ElapsedGameTime.TotalMilliseconds;
                model.Position = trajectory.GetPointOnCurve(time_);


            }
            public override void Draw(GameCamera.FreeCamera camera)
            {
                model.Draw(camera);
            }
        }
        #endregion SplitMissle
    }
}

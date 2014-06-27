using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    public class Cancer : Predator
    {

        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float time = 0.0f;
        private float time_to_move = 1.0f;
        private float time_dmg = 0.0f;
        private Vector3 destination = new Vector3(50, 40, 50); //cel do ktorego ma isc rak
        private bool has_reached = false;
        private int rgn = 120;
        private int damage = 30;
        public Cancer(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval, float Scope, float AttackSpeed, int Damage)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {

        }

        public Cancer(LoadModel model, List<InteractiveModel> ants)
            : base(model)
        {
            selectable = false;
            this.Ants = ants;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.Hp = 100;
            this.modelHeight = 40;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if(has_reached == true)
            {
                for (int i = 0; i < Ants.Count; i++)
                {
                    float spr = (float)Math.Sqrt(Math.Pow(Ants[i].Model.Position.X - this.Model.Position.X, 2.0) + (float)Math.Pow(Ants[i].Model.Position.Z - this.Model.Position.Z, 2.0));
                    if (spr <= rgn && this != Ants[i])
                    {
                        if (Ants[i] is Unit && !(Ants[i] is Predator))
                        {
                            if (!(this.Model.BoundingSphere.Intersects(Ants[i].Model.BoundingSphere)))
                                this.reachTarget(gameTime, this, Ants[i].Model.Position);
                            else
                            {
                                time_dmg += (float)gameTime.ElapsedGameTime.TotalSeconds;
                                if (time_dmg > 3.0f)
                                {
                                    this.model.switchAnimation("Atack");
                                    Ants[i].hasBeenHit = true;
                                    Ants[i].Hp -= damage;
                                    ((Unit)Ants[i]).LifeBar.LifeLength -= ((Unit)Ants[i]).LifeBar.LifeLength * ((100 * (float)damage) / (float)Ants[i].Hp);
                                    time_dmg = 0;
                                }
                            }
                            break;
                        }
                    }

                }
            }
            else
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > time_to_move)
                {
                    this.reachTarget(gameTime, this, destination);
                    if (this.Model.Position.X == destination.X && this.Model.Position.Z == destination.Z)
                    {
                        has_reached = true;
                    }
                }
            }
        }

    }

}



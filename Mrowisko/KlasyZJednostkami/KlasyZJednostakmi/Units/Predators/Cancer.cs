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
        private Vector3 destination = new Vector3(100, 40, 500); //cel do ktorego ma isc rak
        private bool has_reached = false;
        private int rgn = 350;
        private int damage = 30;
        private int counter = 0;
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
            this.MaxHp = 100;
            this.modelHeight = 40;
            destination = new Vector3(this.model.Position.X-100,this.model.Position.Y,this.model.Position.Z-300);

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (has_reached == true)
            {
                for (int i = 0; i < Ants.Count; i++)
                {
                    //float spr = (float)Math.Sqrt(Math.Pow(Ants[i].Model.Position.X - this.Model.Position.X, 2.0) + (float)Math.Pow(Ants[i].Model.Position.Z - this.Model.Position.Z, 2.0));
                      float spr=Vector2.Distance(new Vector2(this.Model.BoundingSphere.Center.X, this.Model.BoundingSphere.Center.Z),new Vector2(Ants[i].Model.BoundingSphere.Center.X, Ants[i].Model.BoundingSphere.Center.Z));
                    if (spr <= rgn && this != Ants[i])
                    {
                        counter++;
                        if (Ants[i] is Unit && !(Ants[i] is Predator))
                        {
                            if (Vector2.Distance(new Vector2(this.Model.BoundingSphere.Center.X, this.Model.BoundingSphere.Center.Z), new Vector2(Ants[i].Model.BoundingSphere.Center.X, Ants[i].Model.BoundingSphere.Center.Z)) > this.Model.BoundingSphere.Radius * 1.75)
                            {
                                this.model.switchAnimation("Walk");
                                this.reachTargetAutonomus(gameTime, Ants[i].Model.Position);
                                this.model.Rotation = new Vector3(this.model.Rotation.X, StaticHelpers.StaticHelper.TurnToFace(new Vector2(this.model.Position.X, this.model.Position.Z), new Vector2(Ants[i].Model.Position.X, Ants[i].Model.Position.Z), this.model.Rotation.Y, 1.05f), model.Rotation.Z);
                            }
                            else if (Vector2.Distance(new Vector2(this.Model.BoundingSphere.Center.X, this.Model.BoundingSphere.Center.Z), new Vector2(Ants[i].Model.BoundingSphere.Center.X, Ants[i].Model.BoundingSphere.Center.Z)) <= this.Model.BoundingSphere.Radius * 1.75)
                            {
                                this.model.switchAnimation("Atack");
                                time_dmg += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                                if (time_dmg > 3.0f)
                                {
                                    
                                    Ants[i].hasBeenHit = true;
                                    if (Ants[i].foe == null)
                                    {
                                        Ants[i].foe = this;
                                    }
                                    Ants[i].Hp -= damage;
                                    ((Unit)Ants[i]).LifeBar.LifeLength -= ((Unit)Ants[i]).LifeBar.LifeLength * ((float)damage / Ants[i].MaxHp);
                                    time_dmg = 0;
                                }
                               
                            }
                            break;
                        }
                    }

                }
                if (counter == 0 && has_reached == true)
                {
                    this.model.switchAnimation("Idle");
                }
                else
                {
                    counter = 0;
                }
            }
            else
            {
                this.model.switchAnimation("Walk");
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > time_to_move)
                {
                    this.reachTargetAutonomus(gameTime, destination);
                    if (this.Model.Position.X == destination.X && this.Model.Position.Z == destination.Z)
                    {
                        has_reached = true;
                    }
                    else
                    {
                        has_reached = false;
                    }
                }
            }

        }
        public override string ToString()
        {
            return "Cancer " + hp + "/" + maxHp;
        }

    }

}



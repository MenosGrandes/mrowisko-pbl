using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    class SunDew:Unit
    {
    
     public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float Scope;
        private float AttackSpeed;
        private int Damage;

        public SunDew(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float AttackSpeed, int Damage)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.AttackSpeed=AttackSpeed;
        this.Damage=Damage;
    }
        public void update(GameTime gameTime)
        {
                foreach(Unit model in Ants)
                {
                    float lenght = (float)Math.Sqrt(Math.Pow(model.Model.Position.X - this.Model.Position.X, 2.0f) + Math.Pow(model.Model.Position.Z - this.Model.Position.Z, 2.0f));
                    if(lenght<=Scope)
                    {
                        AttackSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (AttackSpeed > 2.0f)
                    {
                        if (model.Hp > 0)
                            model.Hp -= Damage;
                        AttackSpeed = 0.0f;
                    }
                    }
                }
            

        }
    
    }
     
    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    public class SunDew:Predator
    {
    
     public List<InteractiveModel> Ants = new List<InteractiveModel>();


        private float trawienie = 0.0f;
        private float czas_trawienia = 10.0f;
        private bool trawienie_flaga = false;
        public SunDew(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float AttackSpeed, int Damage)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
     
    }

        public SunDew(LoadModel model, List<InteractiveModel> ants)
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
            this.Model.Position = new Vector3(this.Model.Position.X, 0.0f, this.Model.Position.Z);
            this.modelHeight = 10;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (trawienie_flaga == true)
            {
                trawienie += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (trawienie > czas_trawienia)
                {
                    Console.WriteLine("strawilem");
                    this.Model.Position = new Vector3(this.Model.Position.X, 0.0f, this.Model.Position.Z);
                    trawienie_flaga = false;
                    trawienie = 0.0f;
                }
            }
            else
            {
                //foreach (InteractiveModel model in Ants)
                for (int i = 0; i < Ants.Count;i++)
                {
                    if (this.Model.BoundingSphere.Contains(Ants[i].Model.BoundingSphere) == ContainmentType.Intersects && this!=Ants[i] && Ants[i] is Unit)
                    // if (this.Model.BoundingSphere.Intersects(model.Model.BoundingSphere))
                    {
                        this.Model.Position = new Vector3(this.Model.Position.X, StaticHelpers.StaticHelper.GetHeightAt(this.Model.Position.X, this.Model.Position.Z) + modelHeight, this.Model.Position.Z);
                        Console.WriteLine("proboje_zjec"+Ants[i].GetType());
                        Ants[i].Hp = -1;
                        trawienie_flaga = true;
                        break;
                    }
                }
            }
        }
    
    }
     
    }


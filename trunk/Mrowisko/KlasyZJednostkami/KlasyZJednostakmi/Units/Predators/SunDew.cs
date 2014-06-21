using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    public class SunDew:Unit
    {
    
     public List<InteractiveModel> Ants = new List<InteractiveModel>();


        private float trawienie = 0.0f;
        private float czas_trawienia = 5.0f;
        private bool trawienie_flaga = false;
        public SunDew(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float AttackSpeed, int Damage)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
     
    }

        public SunDew(LoadModel model, List<InteractiveModel> ants)
            : base(model)
        {
      
            this.Ants = ants;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));

        }


        public void update(GameTime gameTime)
        {
            if (trawienie_flaga == true)
            {
                trawienie += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (trawienie > czas_trawienia)
                {
                    trawienie_flaga = false;
                    trawienie = 0.0f;
                }
            }
            else
            {
                foreach (Unit model in Ants)
                {
                    if (this.Model.BoundingSphere.Contains(model.Model.Position) == ContainmentType.Contains)
                    {
                        Ants.Remove(model);
                        trawienie_flaga = true;
                    }
                }
            }
        }
    
    }
     
    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
namespace Logic.Units.Allies
{
    class Beetle:Unit
    {
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float Scope;
        private float ArmorBuff;
        private bool flag;

        public Beetle(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float ArmorBuff)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.ArmorBuff = ArmorBuff;
        flag = false;

    }
        public void update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D))
            {
                foreach(Unit model in Ants)
                {
                    float lenght = (float)Math.Sqrt(Math.Pow(model.Model.Position.X - this.Model.Position.X, 2.0f) + Math.Pow(model.Model.Position.Z - this.Model.Position.Z, 2.0f));
                    if(lenght<=Scope && flag==false)
                    {
                        model.Armor = model.Armor + ArmorBuff;
                        flag = true;
                    }

                }
            }

        }
    
    }
}

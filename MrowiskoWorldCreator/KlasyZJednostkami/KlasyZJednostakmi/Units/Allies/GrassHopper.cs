using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Logic.Units.Ants;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Units.Allies
{
    [Serializable]
    public class GrassHopper : Allie
    {
        private float Scope;
        private float ArmorBuffValue;



        public GrassHopper(LoadModel model)
            : base(model)
        {
            Scope = 100;
            ArmorBuffValue = 100;
          
        }
        public GrassHopper()
            : base()
        {

        }
        public override void Update(GameTime time)
        {
            base.Update(time);
        }
        public override string ToString()
        {
            return this.GetType().Name + base.model.Selected;
        }
     

    }

}

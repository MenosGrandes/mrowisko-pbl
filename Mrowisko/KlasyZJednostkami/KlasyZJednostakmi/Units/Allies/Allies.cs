using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    class Allies:Unit
    {
     public Allies(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
              : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            base.elapsedTime = 0;
           }
        public Allies(LoadModel model):base(model)
           { base.elapsedTime = 0; }
        public Allies()
            : base()
        { base.elapsedTime = 0; }
   
    }
}

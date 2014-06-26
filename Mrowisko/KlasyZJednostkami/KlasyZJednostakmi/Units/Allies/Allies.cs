using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    [Serializable]
    public class Allie:Unit
    {
     public Allie(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
              : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            base.elapsedTime = 0;
           }
        public Allie(LoadModel model):base(model)
           { base.elapsedTime = 0; }
        public Allie()
            : base()
        { base.elapsedTime = 0; }
   
    }
}

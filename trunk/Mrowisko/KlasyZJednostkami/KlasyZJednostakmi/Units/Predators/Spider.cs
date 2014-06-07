using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Predators
{
    [Serializable]
    public class Spider:Predator
    {
         public Spider():base()
       { }
       public Spider(LoadModel model):base(model)
       {

       }
       public Spider(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model)
           : base(hp, armor, strength, range, cost, buildingTime, model)
       { }
    }
}

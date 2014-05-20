using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    [Serializable]
    public class Ant:Unit
    {
           public Ant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model):base(hp,  armor,  strength,  range,  cost,  buildingTime,  model)
        { }
        public Ant()
        {}
        public Ant(LoadModel model):base(model)
        { }
    }
}

using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    [Serializable]
    public class Predator : Unit
    {
        public Predator()
            : base()
        { }
        public Predator(LoadModel model)
            : base(model)
        {

        }
        public Predator(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model)
            : base(hp, armor, strength, range, cost, buildingTime, model)
        { }
    }
}

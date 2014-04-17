using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    
    class Building:InteractiveModel
    {

        protected float buildingTime;

        public float BuildingTime
        {
            get { return buildingTime; }
            set { buildingTime = value; }
        }

        protected int cost = 0;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        } 



    }
}

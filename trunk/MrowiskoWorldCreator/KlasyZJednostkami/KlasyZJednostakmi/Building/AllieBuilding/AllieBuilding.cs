using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AllieBuilding
{   [Serializable]
    public class AllieBuilding:Building
    {
       public bool hasBeenVisited = false;

        public AllieBuilding(LoadModel model):base(model)
        { }
       
    }
}

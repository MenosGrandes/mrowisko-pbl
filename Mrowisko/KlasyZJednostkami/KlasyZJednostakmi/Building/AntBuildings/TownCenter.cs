using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings
{
    [Serializable]
    public class TownCenter : Building
    {
        public TownCenter()
            : base()
        { }
        public TownCenter(Map.LoadModel model)
            : base(model)
        {

        }
    }
}

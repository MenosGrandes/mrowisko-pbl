using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Logic.Units.Predators
{
    [Serializable]
    public class Spider:Predator
    {   
        public Spider()
            : base()
        { }
        public Spider(LoadModel model)
            : base(model)
        {

        }
    }
}

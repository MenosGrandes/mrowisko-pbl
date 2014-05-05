using Map;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{
    public class HyacyntFarm:SeedFarm
    {
        public HyacyntFarm()
            : base()
        { }
        new public void Draw()
        {
            Console.WriteLine(this.GetType());
        }
    }
}

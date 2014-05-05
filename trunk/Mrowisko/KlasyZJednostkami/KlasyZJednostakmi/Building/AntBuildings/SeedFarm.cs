using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings
{
    public class SeedFarm:Building
    {
        public SeedFarm():base()
        { }
        new public void Draw()
        {
            Console.WriteLine(this.GetType());
        }
    }
}

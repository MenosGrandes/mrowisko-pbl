using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{
    public class DicentraFarm : SeedFarm
    {

        public DicentraFarm()
            : base()
        { }
        new public void Draw()
        {
            Console.WriteLine(this.GetType());
        }
    }
}

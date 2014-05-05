using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{
    class ChelidoniumFarm:SeedFarm
    {
        public ChelidoniumFarm()
            : base()
        { }
        new public void Draw()
        {
            Console.WriteLine(this.GetType());
        }

    }
}

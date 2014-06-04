using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{
   public class ChelidoniumFarm:SeedFarm
    {
    
        public ChelidoniumFarm( LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime,float cropTime):base( model,_capacity,_durability,_cost,_buildingTime,cropTime)
        { 
        
        }

        public ChelidoniumFarm()
        { }
        public override Logic.Meterials.Material addCrop()
        {  
            return new Logic.Meterials.Chelidonium();
        }
        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds/100;
        }

    }
}

using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{

    public class DicentraFarm : SeedFarm    {
        public DicentraFarm( LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime,float cropTime):base( model,_capacity,_durability,_cost,_buildingTime,cropTime)
        {
            base.cropTime = cropTime;

        }

        public DicentraFarm()
        { }
        public override Logic.Meterials.Material addCrop()
        {  
            return new Logic.Meterials.Dicentra();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            base.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100;
        }

    }
}

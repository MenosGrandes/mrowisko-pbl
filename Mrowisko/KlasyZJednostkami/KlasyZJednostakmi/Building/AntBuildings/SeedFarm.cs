using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings
{
    public class SeedFarm:Building
    {
        

        public SeedFarm(LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime, float cropTime)
            : base( model,_capacity,_durability,_cost,_buildingTime)
        {
            base.cropTime = cropTime;
        }

        public SeedFarm()
        { }




        public override void Update(GameTime gameTime)
        {
            ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds/10;
           // timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

       
    }
}

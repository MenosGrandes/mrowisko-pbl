using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AntBuildings.SeedFarms
{
    public class HyacyntFarm : SeedFarm    {
        public HyacyntFarm( LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime,float cropTime):base( model,_capacity,_durability,_cost,_buildingTime,cropTime)
        { 
        
        }
         public override void  Draw(FreeCamera camera)
        {
            model.Draw(camera);
        }
        public HyacyntFarm()
        { }
        public override Logic.Meterials.Material addCrop()
        {  
            return new Logic.Meterials.Hyacynt();
        }
        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds/100;
        }


       

    }
}

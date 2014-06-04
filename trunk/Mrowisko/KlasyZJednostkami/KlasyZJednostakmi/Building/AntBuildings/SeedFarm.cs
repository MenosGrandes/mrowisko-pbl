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
        protected float cropTime;

        public float CropTime
        {
            get { return cropTime; }
            set { cropTime = value; }
        }
        public float timeElapsed;

        public SeedFarm(LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime, float cropTime)
            : base( model,_capacity,_durability,_cost,_buildingTime)
        {
            this.cropTime = cropTime;
        }

        public SeedFarm()
        { }




        public override void Update(GameTime gameTime)
        {   
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

       
    }
}

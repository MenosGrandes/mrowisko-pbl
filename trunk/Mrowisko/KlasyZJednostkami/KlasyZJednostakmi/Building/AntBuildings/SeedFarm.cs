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
        public SeedFarm(ContentManager content, LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime)
            : base(content, model,_capacity,_durability,_cost,_buildingTime)
        { }
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
        }
        public SeedFarm()
        { }
    }
}

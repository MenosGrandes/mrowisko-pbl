using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Logic.Building.AntBuildings.Granary
{
    public class AntGranary:Building
    {
        private int materialsCapacity;

        public int MaterialsCapacity
        {
            get { return materialsCapacity; }
            set { materialsCapacity = value; }
        }
        public AntGranary(LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime,int _materialCapacity):base(model,_capacity,_durability,_cost,_buildingTime)
        {

            this.materialsCapacity = _materialCapacity;
        }
        public AntGranary(LoadModel model):base(model)
        {

        }
        public override string ToString()
        {
            return GetType().Name;
        }

    }
}

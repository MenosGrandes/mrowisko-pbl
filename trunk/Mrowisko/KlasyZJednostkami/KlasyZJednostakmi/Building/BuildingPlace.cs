using Logic.Building.AntBuildings;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building
{
    public class BuildingPlace:Building
    {
        private Building building;

        public Building Building
        {
            get { return building; }
            set { building = value; }
        }
        public BuildingPlace(LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime)
            : base(model, _capacity, _durability, _cost, _buildingTime)
        {
                                                    
        }
        public BuildingPlace():base()
        {

        }
        public void Build(Building b)
        {
            this.building = b;
        }
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
            if(Building!=null)
            {
                building.Model.Draw(View, Projection);
            }
        }
    }
}

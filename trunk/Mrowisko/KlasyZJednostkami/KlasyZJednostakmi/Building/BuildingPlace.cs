using Logic.Building.AntBuildings;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building
{
    [Serializable]
    public class BuildingPlace : Building
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
        public BuildingPlace(LoadModel model)
            : base(model)
        { }
        public BuildingPlace()
            : base()
        {

        }
        public void Build(Building b)
        {
            this.building = b;
        }
        public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
            if (Building != null)
            {
                building.Model.Draw(camera);
            }
        }
    }
}

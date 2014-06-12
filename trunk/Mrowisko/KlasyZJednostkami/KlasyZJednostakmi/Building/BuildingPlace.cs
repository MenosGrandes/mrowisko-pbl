using Logic.Building.AntBuildings;
using Logic.Building.AntBuildings.SeedFarms;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building
{
    [Serializable]
    public class BuildingPlace : Building
    {
        private Building house;

        public Building House
        {
            get { return house; }
            set { house = value; }
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
            this.house = b;
        }
        public override void Draw(GameCamera.FreeCamera camera)
        {
            if (house != null)
            {
                house.Model.Draw(camera);
            }
            else
            {
                model.Draw(camera);

            }
        }
        public void Build1()
        {
            this.house = new AntBuildings.Granary.AntGranary(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/kopiec"), this.model.Position, Vector3.Zero, this.model.Scale, StaticHelpers.StaticHelper.Device, this.model.light));
        }


        public void BuildHyacyntFarm()
        {
            this.house = new HyacyntFarm(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/h1"), this.model.Position, Vector3.Zero, this.model.Scale, StaticHelpers.StaticHelper.Device, this.model.light), 1000, 100, 10, 10, 100);
        }

        public void BuildDicentraFarm()
        {
            this.house = new DicentraFarm(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/h2"), this.model.Position, Vector3.Zero, this.model.Scale, StaticHelpers.StaticHelper.Device, this.model.light), 1000, 100, 10, 10, 100);
        }
    }
}

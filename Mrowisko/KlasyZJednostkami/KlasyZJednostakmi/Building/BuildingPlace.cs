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
        private float time;

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
        public void BuildAntGranary()
        {
            this.house = new AntBuildings.Granary.AntGranary(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/kopiec"), new Vector3(this.model.Position.X, this.model.Position.Y - this.model.BoundingSphere.Radius * 2, this.model.Position.Z), Vector3.Zero, this.model.Scale, StaticHelpers.StaticHelper.Device, this.model.light));
        }


        public void BuildHyacyntFarm()
        {
            this.house = new HyacyntFarm(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/h1"), new Vector3(this.model.Position.X, this.model.Position.Y - this.model.BoundingSphere.Radius * 2, this.model.Position.Z), Vector3.Zero, new Vector3(this.model.Scale.X+0.5f,this.model.Scale.Y+0.5f,this.model.Scale.Z+0.5f), StaticHelpers.StaticHelper.Device, this.model.light), 1000, 100, 10, 10, 1000);
        }

        public void BuildDicentraFarm()
        {
            this.house = new DicentraFarm(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/h2"), new Vector3(this.model.Position.X, this.model.Position.Y - this.model.BoundingSphere.Radius*2, this.model.Position.Z), Vector3.Zero, this.model.Scale, StaticHelpers.StaticHelper.Device, this.model.light), 1000, 100, 10, 10, 1000);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.time = 0;
            if (this.house!=null && this.house.Model.Position.Y < this.model.Position.Y)
            {
                this.raisingBuilding = true;
                 time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (time > 0.01f)
                        {
                            this.house.Model.Position += new Vector3(0,1,0);
                            this.house.Model.Rotation += new Vector3(0, 0.05f, 0);
                        }
            }
            else
            {
                this.raisingBuilding = false;
                if (this.house != null)
                this.house.Built = true;
            }
            if (this.house!=null && House.Built )
            {

          
            House.Update(gameTime);
            if (House.GetType().BaseType == typeof(SeedFarm))
            {
                //if((((BuildingPlace)model).(SeedFarm)Building).timeElapsed > (((BuildingPlace)model).(SeedFarm)Building).CropTime))
              
            }

            }

        }
    }
}

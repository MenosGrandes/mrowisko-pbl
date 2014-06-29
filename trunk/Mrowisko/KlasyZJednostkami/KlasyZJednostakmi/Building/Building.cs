
using Microsoft.Xna.Framework.Content;
using Map;
using System;
using Microsoft.Xna.Framework;
namespace Logic.Building
{
    [Serializable]
    public class Building : InteractiveModel
    {

        private bool productUnits=false;

        public bool ProductUnits
        {
            get { return productUnits; }
            set { productUnits = value; }
        }
        private bool productCrop=true;

        public bool ProductCrop
        {
            get { return productCrop; }
            set { productCrop = value; }
        }
        private bool built=false;

        public bool Built
        {
            get { return built; }
            set { built = value; }
        }

        protected float buildingTime;

        public float BuildingTime
        {
            get { return buildingTime; }
            set { buildingTime = value; }
        }

        protected int cost = 0;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        protected int durability;

        public int Durability
        {
            get { return durability; }
            set { durability = value; }
        }

        protected int capacity;

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }



        public Building(LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime)
            : base(model)
        {
            this.model = model;
            this.capacity = _capacity;
            this.cost = _cost;
            this.buildingTime = _buildingTime;
        }
        public Building()
        {
            this.model = model;

        }
        public Building(LoadModel model)
            : base(model)
        {
            this.model = model;

        }
        public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

        
        }

    }
}

﻿
using Microsoft.Xna.Framework.Content;
using Map;
using System;
using Microsoft.Xna.Framework;
namespace Logic.Building
{
    [Serializable]
    public class Building : InteractiveModel
    {

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
           // model.CreateBoudingBox();
        }
        public Building()
        {
            this.model = model;

            //model.CreateBoudingBox();
        }
        public Building(LoadModel model)
            : base(model)
        {
            this.model = model;

          //  model.CreateBoudingBox();
        }
        public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
        }
        public override void Update(GameTime gameTime)
        { }

    }
}


using Microsoft.Xna.Framework.Content;
using Map;
using System;
namespace Logic.Building
{
    
    public class Building:InteractiveModel
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

        public Building(ContentManager content, LoadModel model, int _capacity, int _durability, int _cost, float _buildingTime)
            : base(content, model)
        {
            this.model = model;
            this.content = content;
            this.capacity = _capacity;
            this.cost = _cost;
            this.buildingTime = _buildingTime;
        }
        public Building()
        { }
         new public void Draw()
        {

            Console.WriteLine(this.GetType());
        }
    }
}

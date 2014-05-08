using Logic.Meterials;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logic.Units.Ants
{
    public class AntPeasant:Ant
    {
        public int MaxCapacity { get { return maxCapacity; } }
        private int maxCapacity;
        public int Capacity { 
      get
        {
            return capacity;
      }
            set { }
        }
        private int capacity;
        public AntPeasant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model,int maxCapacity):base(hp,armor,strength,range,cost,buildingTime,model)
        {
            this.maxCapacity = maxCapacity;
            this.capacity = 0;
        }
        public override void gaterMaterial(Material material)
        {
            if(material.Model.Scale.X >0 && capacity<maxCapacity)
            { 
                material.Model.Scale -= new Vector3(0.01f, 0.01f, 0.01f);
                capacity++;
            }
            if(maxCapacity==capacity)
            {
                Console.WriteLine("jestem pelny");
            }
        }

        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
        }
        public override int releaseMaterial()
        {
            int c = capacity;
            capacity = 0;
            return 10;
        }

    }
}

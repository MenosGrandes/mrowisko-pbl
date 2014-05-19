using Logic.Meterials;
using Logic.Meterials.MaterialCluster;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


namespace Logic.Units.Ants
{
    [Serializable]
    public class AntPeasant : Ant
    {
        private List<Material> materials = new List<Material>();
        public int wood2;
        public int rock2;


        public int MaxCapacity { get { return maxCapacity; } }
        private int maxCapacity;
        public int Capacity
        {
            get
            {
                return capacity;
            }
            set { }
        }
        private int capacity;

        public float gaterTime;
        public float elapsedTime;
        public AntPeasant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime)
            : base(hp, armor, strength, range, cost, buildingTime, model)
        {
            this.maxCapacity = maxCapacity;
            this.capacity = 0;
            this.gaterTime = gaterTime;
            rock2 = 0;
            wood2 = 0;

        }
        public override void gaterMaterial(Material material)
        {

            if (elapsedTime >= gaterTime)
            {
                if (material.Model.Scale.X > 0 && capacity < maxCapacity)
                {

                    switch (material.GetType().Name)
                    {

                        case "Log": materials.Add(new Wood()); wood2++; ((Log)material).removeWood(1);
                            ((Log)material).Model.Scale = new Vector3((float)((float)((Log)material).ClusterSize / (float)((Log)material).MaxClusterSize));

                            break;
                        case "Rock": materials.Add(new Stone()); rock2++; ((Rock)material).removeRock(1);
                            ((Rock)material).Model.Scale = new Vector3((float)((float)((Rock)material).ClusterSize / (float)((Rock)material).MaxClusterSize));

                            break;

                    }
                    capacity++;

                }

            }
            elapsedTime = 0.0f;

        }
        public override void Update(GameTime time)
        {

            this.elapsedTime += time.ElapsedGameTime.Milliseconds;
        }
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
        }
        public override List<Material> releaseMaterial()
        {
            List<Material> mat = new List<Material>(materials);
            capacity = 0;
            wood2 = 0;
            rock2 = 0;
            materials.Clear();
            return mat;
        }
        public override string ToString()
        {
            return this.GetType().Name + " " + model.Position + " ";
        }


    }
}

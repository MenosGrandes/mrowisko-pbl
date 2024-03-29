﻿using Logic.Building.AntBuildings.Granary;
using Logic.Meterials;
using Logic.Meterials.MaterialCluster;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logic.Units.Ants
{
    [Serializable]

    public class AntPeasant : Ant
    {
        [NonSerialized]
        private List<Material> materials = new List<Material>();
        [NonSerialized]
        public int wood2;
        [NonSerialized]
        public int rock2;
        private Material GaterMaterialObject;
        public Material gaterMaterialObject;
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
        public AntPeasant(LoadModel model)
            : base(model)
        {
            this.maxCapacity = 100;
            this.capacity = 0;
            this.gaterTime = 10;
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
                            ((Log)material).Model.Scale = new Vector3((float)((float)((Log)material).ClusterSize / (float)((Log)material).MaxClusterSize)) * material.Model.Scale;

                            break;
                        case "Rock": materials.Add(new Stone()); rock2++; ((Rock)material).removeRock(1);
                            ((Rock)material).Model.Scale = new Vector3((float)((float)((Rock)material).ClusterSize / (float)((Rock)material).MaxClusterSize)) * material.Model.Scale;

                            break;

                    }
                    capacity++;

                }

            }
            elapsedTime = 0.0f;

        }
        public override void Update(GameTime time)
        {
            base.Update(time);
            this.elapsedTime += time.ElapsedGameTime.Milliseconds;
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
        public override void Intersect(InteractiveModel interactive)
        {
            if (this == interactive)
            { return; }
            foreach (BoundingSphere b in model.Spheres)
            {

                foreach (BoundingSphere b2 in interactive.Model.Spheres)
                {

                    if (b.Intersects(b2))
                    {
                        if (gaterMaterialObject == interactive)
                        {
                            if (interactive.GetType().BaseType == typeof(Material))
                            {
                                if (gaterTime < elapsedTime)
                                {
                                    gaterMaterial((Material)interactive);

                                }
                            }
                        }
                        else if (interactive.GetType() == typeof(AntGranary))
                        {
                            Console.WriteLine("Oddaje");
                            //AntHill.Player.addMaterial(releaseMaterial());
                            this.materials.Clear();
                            Console.WriteLine(Capacity);
                        }

                    }

                }
            }
        }
        public override void setGaterMaterial(Material m)
        {
            if (m != gaterMaterialObject)
            {
                gaterMaterialObject = m;

            }
        }

    }

}


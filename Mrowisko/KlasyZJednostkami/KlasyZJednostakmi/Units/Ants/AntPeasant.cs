using Logic.Building.AntBuildings.Granary;
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
    public class AntPeasant:Ant
    {
        private List<Material> materials = new List<Material>();
        public  int wood2;
        public  int rock2;
        private Material GaterMaterialObject;
        public Material  gaterMaterialObject;
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

        public float gaterTime;
        public float elapsedTime;
        public AntPeasant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model,int maxCapacity,float gaterTime):base(hp,armor,strength,range,cost,buildingTime,model)
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
                if(material.Model.Scale.X >0 && capacity<maxCapacity)
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
          //  this.model.tempPosition = this.model.Position;
            this.elapsedTime += time.ElapsedGameTime.Milliseconds ;
            LifeBar.CreateBillboardVerticesFromList(this.Model.Position+new Vector3(0,10,0));
        }
        public override void Draw(Matrix View, Matrix Projection, GameCamera.FreeCamera camera)
        {
            model.Draw(View, Projection);
            //LifeBar.healthDraw(View,Projection,camera.Position);
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
            if(this==interactive)
            { return ; }
                    foreach(BoundingSphere b in model.Spheres)
                    {

                           foreach(BoundingSphere b2 in interactive.Model.Spheres)
                           {


                             if (b.Intersects(b2))
                             {
                                 if(gaterMaterialObject==interactive)
                                 {
                                     Console.WriteLine(gaterMaterialObject.Model.Position);
                                 if(interactive.GetType().BaseType==typeof(Material))
                                 {
                                     if (gaterTime < elapsedTime)
                                     {
                                         gaterMaterial((Material)interactive);
                                     }
                                 }
                                 }
                                 else if(interactive.GetType()==typeof(AntGranary))
                                 {
                                  //   Player.addMaterial(((AntPeasant)model2).releaseMaterial());

                                 }
                                 
                             }

                           }
                   }
            }
        public override void setGaterMaterial(Material m)
        {
            if(m!=gaterMaterialObject)
            {
                gaterMaterialObject = m;

                Console.WriteLine(gaterMaterialObject.Model.Position);
            }
        }
        }

    }


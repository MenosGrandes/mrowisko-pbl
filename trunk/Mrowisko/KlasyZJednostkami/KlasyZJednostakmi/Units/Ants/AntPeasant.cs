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
        [Serializable]

    public class AntPeasant:Ant
    {
        [NonSerialized]
        private List<Material> materials = new List<Material>();
             [NonSerialized]
        public  int wood2;
             [NonSerialized]
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
        public AntPeasant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            this.armor = 20; this.maxCapacity = maxCapacity;
            this.capacity = 0;
            this.gaterTime = gaterTime;
            rock2 = 0;
            wood2 = 0;
            base.elapsedTime = 0;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            LifeBar.LifeLength =model.Scale.X;
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            circle.Scale = model.Scale.X * 10;
           this.Model.switchAnimation("Idle");
           this.armorAfterBuff = armor * 2;
        }
        public AntPeasant(LoadModel model)
            : base(model)
        {
            this.armor = 20;
            this.maxCapacity = 100;
            this.capacity = 0;
            this.gaterTime = 10;
            rock2 = 0;
            wood2 = 0;
           elapsedTime = 0;
           LifeBar.LifeLength = model.Scale.X*100;
           circle.Scale = this.model.Scale.Y * 120;
           LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
           circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
           this.armorAfterBuff = armor * 2;
           hp = 100;
           this.Model.switchAnimation("Idle");

        }
        public override void gaterMaterial(Material material)
        {

              
                if(capacity<maxCapacity)
                {
                  //  Console.WriteLine(material.GetType().Name);
                    switch (material.GetType().Name)
                   {

                       case "Log": ((Log)material).removeWood(1);
                                     materials.Add(new Wood());
                                    wood2++; ;
                          // ((Log)material).Model.Scale = new Vector3((float)((float)((Log)material).ClusterSize / (float)((Log)material).MaxClusterSize)) * material.Model.Scale;
     
                           break;
                       case "Rock": materials.Add(new Stone()); rock2++; ((Rock)material).removeRock(1);
                          // ((Rock)material).Model.Scale = new Vector3((float)((float)((Rock)material).ClusterSize / (float)((Rock)material).MaxClusterSize))*material.Model.Scale;
     
                           break;
                     
                   }
                    capacity++;
              
                
            }
            elapsedTime = 0.0f;
            
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameCamera.FreeCamera camera,float time)
        {
            model.Draw(camera,time);
            
        }
        public override void DrawSelected(GameCamera.FreeCamera camera)
        {
            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3(0,1,0)*model.Scale*50);
            LifeBar.healthDraw(camera);
            
        }

        public override void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {
            circle.CreateBillboardVerticesFromList(model.Position + new Vector3(-2, 0.1f, -0.1f) * model.Scale * 50);
            circle.healthDraw(camera);
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

            if(gaterMaterialObject!=null)
            {
                //Console.WriteLine(gaterMaterialObject);
            }
            if (model.BoundingSphere.Intersects(interactive.Model.Spheres[0]))
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
                    Logic.Player.Player.addMaterial(releaseMaterial());
                    this.materials.Clear();
                  //  Console.WriteLine(Capacity);
                }
            }

                    
            }
        public override void setGaterMaterial(Material m)
        {
            if(m!=gaterMaterialObject)
            {
                gaterMaterialObject = m;

            }
        }
        public override string ToString()
        {
            return this.GetType().Name + " " + model.Selected+armor;
        }
            
        }

    }


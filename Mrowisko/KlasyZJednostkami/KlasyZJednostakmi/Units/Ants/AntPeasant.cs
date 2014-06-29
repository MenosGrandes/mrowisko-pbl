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

    public class AntPeasant : Ant
    {
        [NonSerialized]
        private List<Material> materials = new List<Material>();
        [NonSerialized]
        public int wood2;
        [NonSerialized]
        public int rock2;
        public Material gaterMaterialObject;
        public int MaxCapacity { get { return maxCapacity; } }
        private int maxCapacity;
        [NonSerialized]
        public bool ImGatering = false;
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
        public AntPeasant(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            this.armor = 10; this.maxCapacity = maxCapacity;
            this.capacity = 0;
            this.gaterTime = gaterTime;
            rock2 = 0;
            wood2 = 0;
            base.elapsedTime = 0;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            LifeBar.LifeLength = model.Scale.X;
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            circle.Scale = model.Scale.X * 10;
            this.Model.switchAnimation("Idle");
            this.armorAfterBuff = armor * 2;
            this.modelHeight = 14;
           this.MaxHp = 100;
        }
        public AntPeasant(LoadModel model)
            : base(model)
        {
            this.armor = 10;
            this.maxCapacity = 100;
            this.capacity = 0;
            this.gaterTime = 1000;
            rock2 = 0;
            wood2 = 0;
            elapsedTime = 0;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.armorAfterBuff = armor * 2;
            hp = 100;
            this.Model.switchAnimation("Idle");
            this.modelHeight = 14;
           this.MaxHp = 100;

        }
        public override void gaterMaterial(Material material)
        {


            if (capacity < maxCapacity)
            {

                //  Console.WriteLine(material.GetType().Name);
                switch (material.GetType().Name)
                {

                    case "Log": //this.model.playerTarget.X = material.Model.Position.X;
                        // this.model.playerTarget.Z = material.Model.Position.Z;
                        this.destination = new Vector2(material.Model.Position.X, material.Model.Position.Z);
                        materials.Add(new Wood());
                        wood2++;
                        //material.ClusterSize--;
                        ((Log)material).removeWood(1);
                        ((Log)material).Model.Scale = new Vector3((float)((float)((Log)material).ClusterSize / (float)((Log)material).MaxClusterSize));// * material.Model.Scale;

                        break;
                    case "Rock": //this.model.playerTarget.X = material.Model.Position.X;
                        //this.model.playerTarget.Z = material.Model.Position.Z;
                        this.destination = new Vector2(material.Model.Position.X, material.Model.Position.Z);
                        materials.Add(new Stone());
                        rock2++;
                        //((Rock)material).ClusterSize--;
                        ((Rock)material).removeRock(1);
                        ((Rock)material).Model.Scale = new Vector3((float)((float)((Rock)material).ClusterSize / (float)((Rock)material).MaxClusterSize));//*material.Model.Scale;

                        break;

                }
                capacity++;


            }
            else
            {
                ImGatering = false;
            }
            elapsedTime = 0.0f;

        }
        public override void Update(GameTime time)
        {
            base.Update(time);
            if(ImGatering)
            {
                this.model.switchAnimation("Atack");

            }

        }

        public override void Draw(GameCamera.FreeCamera camera, float time)
        {
            model.Draw(camera, time);

        }
        public override void DrawSelected(GameCamera.FreeCamera camera)
        {
            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3(0, 1, 0) * model.Scale * 50);
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
            base.Intersect(interactive);
            if (this == interactive)
            { return; }

            if (interactive.GetType().IsSubclassOf(typeof(Material)))
            {
                if (model.BoundingSphere.Intersects(new BoundingSphere(interactive.Model.Spheres[0].Center, interactive.Model.Spheres[0].Radius + 24)))
                {
                    if (gaterMaterialObject == interactive)
                    {

                        if (gaterTime < elapsedTime)
                        {
                            ImGatering = true;
                            gaterMaterial((Material)gaterMaterialObject);
                           // SoundController.SoundController.Play(SoundController.SoundEnum.Gater);
                            Logic.Player.Player.addMaterial(releaseMaterial());
                            materials.Clear();
                        }

                    }

                }

            }
        }
        public override void setGaterMaterial(Material m)
        {

            if (m != gaterMaterialObject)
            {
                if(m==null)
                {
                    gaterMaterialObject = null;
                    return;
                }
                float distance = float.MaxValue;
                Node nearest = new Node();
                gaterMaterialObject = m;
                foreach (Node n in m.nodes)
                {
                    if (distance > Vector2.Distance(n.centerPosition, new Vector2(model.Position.X, model.Position.Z)))
                    {
                        distance = Vector2.Distance(n.centerPosition, new Vector2(model.Position.X, model.Position.Z));
                        nearest = n;
                    }
                }
                PathFinder.SearchNearest(ref nearest);
                if (PathFinder.Search(myNode, nearest))
                {
                    MovementPath = new Queue<Node>(PathFinder.finalPath);
                    Moving = true;
                    PathFinder.finalPath.Clear();
                }


            }




        }
        public override string ToString()
        {
            return this.GetType().Name + " " + model.Selected + armor;
        }

    }

}


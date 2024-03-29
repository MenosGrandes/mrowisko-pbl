﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;
using Logic.Meterials;

namespace Logic
{
    [Serializable]
    public class InteractiveModel
    {
        public Vector2 miniMapPosition;
        protected Node myNode;

        public Node MyNode
        {
            get { return myNode; }
            set { myNode = value; }
        }
        public bool ArmorBuff=false;

        protected float cropTime;
        public bool snr = false;

        public float CropTime
        {
            get { return cropTime; }
            set { cropTime = value; }
        }
        public uint modelHeight = 0;

        protected float armorAfterBuff;

        public float ArmorAfterBuff
        {
            get { return armorAfterBuff; }
            set { armorAfterBuff = value; }
        }

        protected float armor;

        public float Armor
        {
            get { return armor; }
            set { armor = value; }
        }
        protected float strength;

        public float Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        protected float range;

        public float Range
        {
            get { return range; }
            set { range = value; }
        }



        protected float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public float rangeOfSight = 250.0f;
        public bool attacking = false;
        public bool ImMoving = false;
        public bool hasBeenHit=false;
        public float tim_to_Show_Hit=4;
        public float temp_time;
        public bool selectable = true;
        public bool raisingBuilding = false;
        public Vector3 transportTarget;
       
        public InteractiveModel target;
        public InteractiveModel foe;

        protected ContentManager content;

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        protected LoadModel model;


        public LoadModel Model
        {
            get { return model; }
            set { model = value; }
        }


        protected int hp;

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        protected int maxHp;

        public int MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }

        public float ElapsedTime;

        protected float elapsedTime
        {
            get { return ElapsedTime; }
            set { ElapsedTime = value; }
        }

        [NonSerialized]
        protected HUD.LifeBar lifeBar;

        public HUD.LifeBar LifeBar
        {
            get { return lifeBar; }
            set { lifeBar = value; }
        }

        protected HUD.Circle circle;

        public HUD.Circle Circle
        {
            get { return circle; }
            set { circle = value; }
        }

        public virtual List<Vector4> spitPos()
        {
            return null;
        }
            
        public bool snared = false;
        public float time_snared = 0.0f;
        public InteractiveModel( LoadModel model)
        {         
            this.model=model;
            this.elapsedTime = 0;
            this.lifeBar = new HUD.LifeBar(1);
            this.circle = new HUD.Circle();
            this.myNode = getMyNode();
        }

        public InteractiveModel()
        {
            this.lifeBar = new HUD.LifeBar(1);
            this.circle = new HUD.Circle();
          
        }
        public virtual void Draw(GameCamera.FreeCamera camera,float time)
        {
            model.Draw(camera, time);
        }

        public virtual void Draw(GameCamera.FreeCamera camera)
         {
             model.Draw(camera);
         }
        public virtual void DrawOpaque(GameCamera.FreeCamera camera,float Alpha,LoadModel model2)
        {
            model.DrawOpague(camera, Alpha, model2);
        }
        public virtual void gaterMaterial(Material material)
        {
        }
        public virtual void Intersect(InteractiveModel interactive)
    {
    }

        public virtual List<Material> releaseMaterial()
        {
            return null;
        }

        public virtual void obstaclesOnRoad(List<InteractiveModel> obstacles)
        {
            
        }

        public virtual List<Unit> releaseAnts()
        {
            return new List<Unit>();

        }

        public virtual Logic.Meterials.Material addCrop()
        {
            return null;
        }
        public virtual void Update(GameTime time)
        {
            this.elapsedTime += time.ElapsedGameTime.Milliseconds;
            if (hasBeenHit == true) {

                temp_time += (float)time.ElapsedGameTime.Milliseconds / 1000;
            if (temp_time > tim_to_Show_Hit)
            {

                temp_time = 0.0f;
                hasBeenHit = false;
                this.model.Hit = false;
            }
                
            }
        }
        public virtual void setGaterMaterial(Material m)
        { }
        public virtual void Attack(GameTime gameTime)
        {
            Console.WriteLine("Attack!! :: " + target);
        }

        public virtual bool spitter()
        {
            return false;
        }

        public virtual void DrawSelected(GameCamera.FreeCamera camera)
        {

            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3(0, 1, 0) * model.Scale * 50);
            LifeBar.healthDraw(camera);
        }

        public virtual void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {

        }
        public bool CheckRayIntersection(Ray ray)
        {

            foreach (BoundingSphere sp in model.Spheres)
            {
                if (ray.Intersects(sp) != null) return true;

            }

            if(model.B_Box!=null)
            {
                if (ray.Intersects(model.B_Box) != null) return true;
            }
           

                   //if (ray.Intersects(model.BoundingSphere) != null) return true;
                 
            return false;  
        }
        public bool CheckFrustumIntersection(BoundingFrustum boundingFrustum)
        {
  
                ContainmentType con = boundingFrustum.Contains(model.BoundingSphere);
                if (con == ContainmentType.Contains || con == ContainmentType.Intersects) return true;
                return false;
        }
        public Node getMyNode()
        {
            //if(myNode==get)
                    foreach(Node n in PathFinderManagerNamespace.PathFinderManager.tileList)
                    {
                        if(n.Box.Contains(new Vector3(model.Position.X,15,model.Position.Z))==ContainmentType.Contains)
                        {
                           return n;
                        }
                    }
                    return new Node();
        }
    }
}

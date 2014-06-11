using System;
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
        public bool ImMoving = false;
        public bool hasBeenHit=false;
        public float tim_to_Show_Hit=4;
        public float temp_time;

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

        public virtual List<Vector3> spitPos()
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
        public virtual void Attack(InteractiveModel a)
        {
           // Console.WriteLine("Attack!!");
        }

        public virtual bool spitter()
        {
            return false;
        }

        public virtual void DrawSelected(GameCamera.FreeCamera camera)
        {
            
        }

        public virtual void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {

        }
        public bool CheckRayIntersection(Ray ray)
        {

                if (ray.Intersects(model.BoundingSphere) != null) return true;
            
            return false;
        }
        public bool CheckFrustumIntersection(BoundingFrustum boundingFrustum)
        {
  
                ContainmentType con = boundingFrustum.Contains(model.BoundingSphere);
                if (con == ContainmentType.Contains || con == ContainmentType.Intersects) return true;
                return false;
        }
    }
}

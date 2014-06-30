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


        protected LoadModel model;


        public LoadModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public bool selected;
        protected int hp;

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        public bool snared = false;
        public float time_snared = 0.0f;

        public InteractiveModel( LoadModel model)
        {         
            this.model=model;
        }

        public InteractiveModel()
        {           
        }
         public virtual void Draw(Matrix View, Matrix Projection)
        {
        }

        public virtual void Draw(GameCamera.FreeCamera camera)
         {

          model.Draw(camera);

         }
        public virtual void Draw(GameCamera.FreeCamera camera, float time)
        {
            model.Draw(camera, time);
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
            model.Update(time);
        }
        public virtual void setGaterMaterial(Material m)
        { }

    }
}

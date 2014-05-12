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
    public class InteractiveModel
    {
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
        public virtual void gaterMaterial(Material material)
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
        { }

    }
}

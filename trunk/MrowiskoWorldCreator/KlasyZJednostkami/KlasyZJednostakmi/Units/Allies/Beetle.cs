using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Logic.Units.Ants;
using Microsoft.Xna.Framework.Graphics;
namespace Logic.Units.Allies
{
    [Serializable]
    public class Beetle:Allie
    {
        [NonSerialized]
        public InteractiveModel sfereModel;
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float Scope;
        private float ArmorBuffValue;
  
        public Beetle(LoadModel model):base(model)
        {
          
        }
        public Beetle():base()
        {

        }
      
        public override string ToString()
        {
            return this.GetType().Name + base.model.Selected;
        }
        public override void Draw(GameCamera.FreeCamera camera, float time)
        {
            base.Draw(camera, time);
        }
    
    }
}

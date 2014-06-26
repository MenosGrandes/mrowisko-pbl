using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Ants
{
    [Serializable]
    public class StrongAnt:Ant
    {
   
       
        public StrongAnt(LoadModel model)
            : base(model)
        {
           
        }
       
        public override void Update(GameTime time)
        {
            base.Update(time);

        }
        

 
  
      

        
      
        
        public override string ToString()
        {
            return this.GetType().Name + " " + armor;
        }

      
           
            public override void Draw(GameCamera.FreeCamera camera)
            {
                model.Draw(camera);
            }
        }
     
    }


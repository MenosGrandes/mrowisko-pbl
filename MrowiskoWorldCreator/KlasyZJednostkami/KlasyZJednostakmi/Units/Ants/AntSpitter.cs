using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Units.Ants
{
    [Serializable]
    public class AntSpitter : Ant
    {

       
        public AntSpitter(LoadModel model)
            : base(model)
        {
           
        }

       
       
        public override void Draw(GameCamera.FreeCamera camera)
        {
            model.Draw(camera);
        }

        
    }
}

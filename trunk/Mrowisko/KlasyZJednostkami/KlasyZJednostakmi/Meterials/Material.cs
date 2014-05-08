using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials
{
   public class Material:InteractiveModel
    {    
       public Material(LoadModel model):base(model)
       {

       }
       public Material()
       { }
       public override void Draw(Matrix View, Matrix Projection)
       {
           model.Draw(View, Projection);
       }
    }
}

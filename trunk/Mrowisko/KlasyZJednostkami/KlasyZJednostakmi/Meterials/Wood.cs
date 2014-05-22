using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials
{
    public class Wood:Material
    {
        public Wood(LoadModel model):base(model)
        {

        }
        public Wood()
        { }
        public override void Draw(Matrix View, Matrix Projection, float time)
        {
            model.Draw(View, Projection, time);
        }
    }
}

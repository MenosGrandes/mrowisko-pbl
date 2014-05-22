using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials
{
    public class Stone:Material
    {

                public Stone(LoadModel model):base(model)
        {

        }
                public Stone()
        { }
        public void Draw(Matrix View, Matrix Projection, float time)
        {
            model.Draw(View, Projection, time);
        }
    }
}

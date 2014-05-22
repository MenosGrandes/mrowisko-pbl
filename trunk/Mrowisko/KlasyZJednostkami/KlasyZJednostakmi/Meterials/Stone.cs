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
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
        }
    }
}

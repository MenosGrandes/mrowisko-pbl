using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials
{       

    public class Hyacynt:Material {
                public Hyacynt(LoadModel model):base(model)
        {

        }
                public Hyacynt()
        { }
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
        }
    }
}

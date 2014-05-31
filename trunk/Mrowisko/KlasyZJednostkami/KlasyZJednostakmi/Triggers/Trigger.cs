using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Triggers
{
    public class Trigger : InteractiveModel
    {
        public Trigger(LoadModel model)
            : base(model)
        {

        }
        public Trigger()
            : base()
        {

        }
        public override void Intersect(InteractiveModel interactive)
        {

            if (interactive == this)
            {
                return;
            }

            if (model.BoundingSphere.Intersects(interactive.Model.BoundingSphere))
            {
                Console.WriteLine("Wlazło w pułapke");
            }
        }
    }
}


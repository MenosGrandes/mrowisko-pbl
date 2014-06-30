using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AllieBuilding
{
    public class AllieBuilding:Building
    {
       public bool hasBeenVisited = false;

        public AllieBuilding(LoadModel model):base(model)
        { }
        public override void Intersect(InteractiveModel interactive)
        {
            if (this == interactive || hasBeenVisited == true)
            { return; }

            if (interactive.GetType().IsSubclassOf(typeof(Ant)))
            {
                if (this.model.BoundingSphere.Intersects(interactive.Model.BoundingSphere))
                    hasBeenVisited = true;
            }
        }
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}

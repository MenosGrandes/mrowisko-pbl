using Logic.Building.AntBuildings;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building
{
    public class BuildingPlace:InteractiveModel
    {
        private Building building;

        public Building Building
        {
            get { return building; }
            set { building = value; }
        }
        public BuildingPlace(LoadModel model):base(model)
        {
                                                    
        }
        public BuildingPlace():base()
        {

        }
        public void Build(Building b)
        {
            this.building = b;
        }
        public override void Draw(Matrix View, Matrix Projection)
        {
            model.Draw(View, Projection);
            if(Building!=null)
            {
                building.Model.Draw(View, Projection);
            }
        }
    }
}

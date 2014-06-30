using Logic.Meterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Building.AllieBuilding
{
    [Serializable]
    public class BeetleBuilding:AllieBuilding
    {
        public BeetleBuilding(LoadModel model):base(model)
        {
           // Model.BuildBoundingSphereGrassHopper();  

        }
        
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

         
    }
}

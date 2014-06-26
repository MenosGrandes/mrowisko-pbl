using Logic.Building.AntBuildings.Granary;
using Logic.Meterials;
using Logic.Meterials.MaterialCluster;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Logic.Units.Ants
{
    [Serializable]
    public class Queen : Ant
    {


        public Queen(LoadModel model)
            : base(model)
        {

        }
       
        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameCamera.FreeCamera camera,float time)
        {
            model.Draw(camera,time);

        }
       


    }

}


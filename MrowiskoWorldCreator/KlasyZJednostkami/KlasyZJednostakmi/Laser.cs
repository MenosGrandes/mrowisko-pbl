using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Laser:InteractiveModel
    {
        private Curve3D movementPath;

        public Curve3D MovementPath
        {
            get { return movementPath; }
            set { movementPath = value; }
        }
        private float time = 0;
        Laser():base()
        {

        }
        public Laser(LoadModel model,Curve3D _movementPath):base(model)
        {
            movementPath = _movementPath;
        }


        public override void Draw(FreeCamera camera)
        {
            model.Draw(camera);
        }
         public override void Update(GameTime _time)
         {
             time += (float)_time.ElapsedGameTime.TotalMilliseconds;
             model.Position = movementPath.GetPointOnCurve(time);
         }
    }
}

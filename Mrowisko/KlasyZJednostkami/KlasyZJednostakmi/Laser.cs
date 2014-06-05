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

        private bool canStart;

        public bool CanStart
        {
            get { return canStart; }
            set { canStart = value; }
        }
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
            canStart = false;
        }

        public override void Intersect(InteractiveModel interactive)
        { 
                 if(this==interactive)
            { return ; }
                 foreach (BoundingSphere b in model.Spheres)
                 {

                     foreach (BoundingSphere b2 in interactive.Model.Spheres)
                     {


                         if (b.Intersects(b2))
                         { Console.WriteLine("Laser!!!"); }
                     }
                 }
        }
         public override void Draw( GameCamera.FreeCamera camera)
        {  if(canStart==false)
        { return; }
            model.Draw(camera);
        }
         public override void Update(GameTime _time)
         {
             if (canStart == false)
             { return; }
             time += (float)_time.ElapsedGameTime.TotalMilliseconds;
             model.Position = movementPath.GetPointOnCurve(time);
         }
        public void Start()
         {
             canStart = true;
         }
    }
}

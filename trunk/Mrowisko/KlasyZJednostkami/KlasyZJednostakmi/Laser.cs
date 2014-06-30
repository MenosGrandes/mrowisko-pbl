using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                 if(this==interactive || !interactive.GetType().IsSubclassOf(typeof(Unit)))
            { return ; }



                         if (this.model.Spheres[0].Intersects(interactive.Model.BoundingSphere))
                         {
                             interactive.Hp -= (int)1;
                             ((Unit)interactive).LifeBar.LifeLength = ((Unit)interactive).LifeBar.LifeLength - ((Unit)interactive).LifeBar.LifeLength * ((float)5 / interactive.MaxHp);
                         }
                 
        }
         public override void Draw( GameCamera.FreeCamera camera)
        {  
        }
         public override void DrawOpaque(GameCamera.FreeCamera camera, float Alpha, LoadModel model2)
         {
             if (canStart == false)
             { return; }
             base.DrawOpaque(camera, Alpha, model2);
         }
         public override void Update(GameTime _time)
         {
             if (canStart == false)
             { return; }
             time += (float)_time.ElapsedGameTime.TotalMilliseconds;
             model.Position = new Vector3(movementPath.GetPointOnCurve(time).X, StaticHelpers.StaticHelper.GetHeightAt(movementPath.GetPointOnCurve(time).X, movementPath.GetPointOnCurve(time).Z), movementPath.GetPointOnCurve(time).Z);
         }
        public void Start()
         {
             canStart = true;
         }
    }
}

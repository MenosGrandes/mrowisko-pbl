using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Transporter:Unit
    {

        public List<Unit> transportAnt = new List<Unit>();
        public Transporter():base()
        { }
        public Transporter(LoadModel model):base(model)
        {
            hp = 10000;
            selectable = true;
        }
        public Transporter(LoadModel model, List<Unit> _transportAnt)
            : base(model)
        {
            
            transportAnt.Capacity = 2;
            this.transportAnt = _transportAnt;  
            
            hp = 10000;
            selectable = true;
        }
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {

            base.Update(time);
            MyNode = this.getMyNode();

            if(transportAnt.Count==2)
            {
                Console.WriteLine("TRANSFORM");
            }
        }
        public override void Draw(GameCamera.FreeCamera camera, float time)
        {
            base.Draw(camera, time);
        }
        public void releaseAnts(ref List<InteractiveModel> models)
        {
            models.AddRange(transportAnt);

        }
        public override void DrawSelected(GameCamera.FreeCamera camera)
        {
            LifeBar.CreateBillboardVerticesFromList(model.Position + new Vector3(0, 1, 0) * model.Scale * 50);
            LifeBar.healthDraw(camera);

        }

        public override void DrawSelectedCircle(GameCamera.FreeCamera camera)
        {
            circle.CreateBillboardVerticesFromList(model.Position + new Vector3(-2, 0.1f, -0.1f) * model.Scale * 50);
            circle.healthDraw(camera);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Logic.Units.Ants;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Units.Allies
{    [Serializable]
    public class GrassHopper:Allie
   {
        private float Scope;
        private float ArmorBuffValue;


        public GrassHopper(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float ArmorBuff)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.ArmorBuffValue = ArmorBuff;
        hp = 1000;
        this.modelHeight = 28;
    }
        public GrassHopper(LoadModel model):base(model)
        {
            Scope = 100;
            ArmorBuffValue = 100;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            hp = 1000;
            this.modelHeight = 28;
        }
        public GrassHopper()
            : base()
        {

        }
       public override void Update(GameTime time)
        {
            base.Update(time);
        }
        public override string ToString()
        {
            return this.GetType().Name + base.model.Selected;
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

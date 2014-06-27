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

            public Queen(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
        {
            this.armor = 20; 
            base.elapsedTime = 0;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            LifeBar.LifeLength = model.Scale.X;
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            circle.Scale = model.Scale.X * 10;
            this.Model.switchAnimation("Idle");
            this.armorAfterBuff = armor * 2;
            hp = 100;
            this.modelHeight = 1;
        }
        public Queen(LoadModel model)
            : base(model)
        {
            this.armor = 20;
           
            elapsedTime = 0;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.armorAfterBuff = armor * 2;
            hp = 100;
            this.Model.switchAnimation("Idle");
            this.modelHeight = 1;
            

        }
       
        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameCamera.FreeCamera camera,float time)
        {
            model.Draw(camera,time);

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


        public override void Intersect(InteractiveModel interactive)
        {
           
        }
        
        public override string ToString()
        {
            return this.GetType().Name + " " + model.Selected + armor;
        }

    }

}


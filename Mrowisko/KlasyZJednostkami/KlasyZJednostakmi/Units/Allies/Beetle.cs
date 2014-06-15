using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Logic.Units.Ants;
using Microsoft.Xna.Framework.Graphics;
namespace Logic.Units.Allies
{
    public class Beetle:Unit
    {
        public InteractiveModel sfereModel;
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float Scope;
        private float ArmorBuffValue;


        public Beetle(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float ArmorBuff)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.ArmorBuffValue = ArmorBuff;

    }
        public Beetle(LoadModel model,List<InteractiveModel>ants):base(model)
        {
            sfereModel=new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/sferazuka"),model.Position,Vector3.Zero,Vector3.One,StaticHelpers.StaticHelper.Device,model.light));
            Scope = 100;
            ArmorBuffValue = 100;
            this.Ants = ants;
        }
        public Beetle():base()
        {

        }
        public override void DrawOpaque(GameCamera.FreeCamera camera, float Alpha,LoadModel model2)
         {
 	         
             StaticHelpers.StaticHelper.Device.BlendState = BlendState.AlphaBlend;
             base.DrawOpaque(camera, Alpha,model2);
             StaticHelpers.StaticHelper.Device.BlendState = BlendState.Opaque;

        }

            
        public override void Update(GameTime time)
        {
            base.Update(time);
          
                foreach(InteractiveModel model in Ants)
                {
                    if (Ants.GetType() == typeof(AntSpitter) || Ants.GetType() == typeof(AntPeasant))
                    {
                        continue;
                    }
                    float lenght = (float)Math.Sqrt(Math.Pow(model.Model.Position.X - this.Model.Position.X, 2.0f) + Math.Pow(model.Model.Position.Z - this.Model.Position.Z, 2.0f));
                    if (lenght <= Scope )
                    {
                        model.ArmorBuff = true;
                    }
                    else
                    {
                        model.ArmorBuff = false;

                    }

                }


        }
        public override string ToString()
        {
            return this.GetType().Name + base.model.Selected;
        }

    
    }
}

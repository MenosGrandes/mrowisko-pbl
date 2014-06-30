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
    [Serializable]
    public class Beetle:Allie
    {
        [NonSerialized]
        public InteractiveModel sfereModel;
        public List<InteractiveModel> Ants = new List<InteractiveModel>();
        private float Scope;
        private float ArmorBuffValue;
        private float time = 0.0f;


        public Beetle(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, int maxCapacity, float gaterTime, float atackInterval,float Scope,float ArmorBuff)
            : base(hp, armor, strength, range, cost, buildingTime, model, atackInterval)
    {
        this.Scope = Scope;
        this.ArmorBuffValue = ArmorBuff;
        this.modelHeight = 30;
        this.MaxHp = 100;

    }
        public Beetle(LoadModel model,List<InteractiveModel>ants):base(model)
        {
            sfereModel=new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/sferazuka"),model.Position,Vector3.Zero,new Vector3(4.5f),StaticHelpers.StaticHelper.Device,model.light));
            Scope = 300;
            ArmorBuffValue = 100;
            this.Ants = ants;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.Hp = 100;
            this.modelHeight = 30;
            this.MaxHp = 100;
            this.strength = 10;
            model.switchAnimation("Idle");
            
        }
        public Beetle(LoadModel model)
            : base(model)
        {
            sfereModel = new InteractiveModel(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/sferazuka"), model.Position, Vector3.Zero, new Vector3(4.5f), StaticHelpers.StaticHelper.Device, model.light));
            Scope = 300;
            ArmorBuffValue = 100;
            LifeBar.LifeLength = model.Scale.X * 100;
            circle.Scale = this.model.Scale.Y * 120;
            LifeBar.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/health_bar"));
            circle.update(StaticHelpers.StaticHelper.Content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Textures/HudTextures/elipsa"));
            this.Hp = 200;
            this.modelHeight = 30;
            this.MaxHp = this.Hp;
            this.strength = 10;
            model.switchAnimation("Idle");
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

        public void removeMyself()
        {
            foreach (InteractiveModel model in Ants)
            {
                if (model == this)
                {
                    Ants.Remove(model);
                    return;
                }
            }
        }    
        public override void Update(GameTime time)
        {
            base.Update(time);
            sfereModel.Model.Position = this.model.Position;
                foreach(InteractiveModel model2 in Ants)
                {
                    if (Ants.GetType() == typeof(AntSpitter) || Ants.GetType() == typeof(AntPeasant))
                    {
                        continue;
                    }
                    float lenght = Vector2.Distance(new Vector2(model2.Model.Position.X,model2.Model.Position.Z),new Vector2(Model.Position.X,Model.Position.Z));
                    //float lenght = (float)Math.Sqrt(Math.Pow(model.Model.Position.X - this.Model.Position.X, 2.0f) + Math.Pow(model.Model.Position.Z - this.Model.Position.Z, 2.0f));
                    if (lenght <= Scope )
                    {
                        model2.ArmorBuff = true;
                    }
                    else
                    {
                        model2.ArmorBuff = false;

                    }

                }


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

        public override void Attack(GameTime gameTime)
        {

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
          
            this.model.switchAnimation("Atack");
            //if (range <= Math.Abs(model.Position.X - a.Model.Position.Y) + Math.Abs(model.Position.X - a.Model.Position.Y))
            if (time > 2.0f)
            {
                if (this.target.Hp <= 0)
                {
                    this.target = null;
                    this.attacking = false;
                    if (this.ImMoving)
                        this.model.switchAnimation("Walk");
                    else
                    {
                        this.model.switchAnimation("Idle");
                    }
                }
                else
                {
                    this.target.Hp -= (int)this.strength;
                    Console.WriteLine(this.target.Hp);

                    ((Unit)this.target).LifeBar.LifeLength -= ((Unit)this.target).LifeBar.LifeLength * ((this.strength) / this.MaxHp);
                }
                //  bullets.Add(new SpitMissle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/shoot"), this.getPosition(), this.getRotation(), new Vector3(0.3f), StaticHelpers.StaticHelper.Device, this.model.light), target.Model.Position));
                time = 0;
            }
        }
    
    }
}

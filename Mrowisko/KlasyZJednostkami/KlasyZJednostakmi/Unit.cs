using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Map;
//using Skills;

namespace Logic
{
    [Serializable]
    public class Unit:InteractiveModel
    {
        //tutaj sa elementy ktore kazda jednostka bedzie miec. content po to zeby w konstruktorze 
        //kazdej dziedziczacej klasy przypisac odpowiedni model na stale
    
      
        protected float armor;

        public float Armor
        {
            get { return armor; }
            set { armor = value; }
        }
        protected float strength;

        public float Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        protected float range;

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        protected float speed;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }


        protected float buildingTime;

        public float BuildingTime
        {
            get { return buildingTime; }
            set { buildingTime = value; }
        }

        protected int cost = 0;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        [NonSerialized]
        protected HUD.LifeBar lifeBar;

        public HUD.LifeBar LifeBar
        {
            get { return lifeBar; }
            set { lifeBar = value; }
        }

        protected float atackInterval;

        public float AtackInterval
        {
            get { return atackInterval; }
            set { atackInterval = value; }
        }
        

       /*
        protected List<Skill> skillsList;

        public List<Skill> SkillsList
        {
            get { return skillsList; }
            set { skillsList = value; }
        }
       */



        public Unit(int hp, float armor, float strength, float range, int cost, float buildingTime, LoadModel model, float atackInterval)
            : base(model)
        {
            this.lifeBar = new HUD.LifeBar(1);

            this.hp = hp;
            this.armor = armor;
            this.strength = strength;
            this.range = range;
            this.cost = cost;
            this.buildingTime = buildingTime;
            this.atackInterval = atackInterval;
            base.elapsedTime = 0; 

        }
        public Unit(): base()
        {
            this.lifeBar = new HUD.LifeBar(1);
            base.elapsedTime = 0;
        }
        public Unit(LoadModel model):base(model)
        {
            this.lifeBar = new HUD.LifeBar(1);
            base.elapsedTime = 0;
        }
        public Vector3 getPosition()
         {
             return this.model.Position;
         }
        public void setPosition(Vector3 position)
        {
            this.model.Position = position;
        }

        public Vector3 getRotation()
        {
            return this.model.Rotation;
        }

        public void setRotation(Vector3 rotation)
        {
            this.model.Rotation = rotation;
        }
       
    }
}

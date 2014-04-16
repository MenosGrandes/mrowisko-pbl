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
using Skills;

namespace Logic
{
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
        protected List<Skill> skillsList;

        public List<Skill> SkillsList
        {
            get { return skillsList; }
            set { skillsList = value; }
        }
       

         Unit(int hp, float armor, float strength)
        {
            
            this.hp = hp;
            this.armor = armor;
            this.strength = strength;
        }

    }
}

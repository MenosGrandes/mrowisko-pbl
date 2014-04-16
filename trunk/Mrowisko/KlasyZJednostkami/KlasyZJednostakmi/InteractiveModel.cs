﻿using System;
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
using Map;

namespace Logic
{
    public class InteractiveModel
    {
        protected ContentManager content;

        protected ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        protected LoadModel model;


        protected LoadModel Model
        {
            get { return model; }
            set { model = value; }
        }


        protected int hp;

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }

        public InteractiveModel(ContentManager content)
        {
            this.content = content;
        }

        public InteractiveModel()
        {
            
        }
    }
}
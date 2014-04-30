﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightsAndShadows
{
    public class Light
    {
        float lightPower;

        public float LightPower
        {
            get { return lightPower; }
            set { lightPower = value; }
        }
        float ambient;

        public float Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }
        Vector3 lightPos;

        public Vector3 LightPos
        {
            get { return lightPos; }
            set { lightPos = value; }
        }
        public Light(float lightPower, float ambient, Vector3 lightPos)
        {
            this.ambient = ambient;
            this.lightPos = lightPos;
            this.lightPower = lightPower;
        }

       public Vector3 lightPosChange(float time)
        {
           // (xLightPos.x/**sin(radians(xTime2))*/, abs(xLightPos.y/**sin(radians(xTime2))*/), xLightPos.z/**sin(radians(xTime2))*/)
            return new Vector3(this.LightPos.X * (float)Math.Sin(MathHelper.ToRadians(time)), this.LightPos.Y , this.LightPos.Z * (float)Math.Sin(MathHelper.ToRadians(time)));
        }

    
    }
}

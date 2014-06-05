using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public struct PointInTime
    {
        public Vector3 point;
        public float time;
       public PointInTime(Vector3 point ,float time)
       {
           this.point = point;
           this.time = time;
       }
    }
}

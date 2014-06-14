using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public static class QuadNodeController
    {
        public static List<QuadNode> QuadNodeList=new List<QuadNode>();
        public static Vector3 getIntersectedQuadNode(Ray intersected)
        {
            
            foreach(QuadNode q in QuadNodeController.QuadNodeList)
            {
                if((intersected.Intersects(q.Bounds))!=null)
                {
                   return q.Bounds.Min + (q.Bounds.Max - q.Bounds.Min) / 2;
                }
            }

            return Vector3.Zero;  
        }
    }
}

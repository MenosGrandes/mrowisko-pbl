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
        public static List<QuadNode> QuadNodeList2 = new List<QuadNode>();

        public static bool ustawione = false;
        public static Vector3 getIntersectedQuadNode(Ray intersected)
        {
            Vector3 v = Vector3.Zero;
            BoundingBox tmp;  
            foreach(QuadNode q in QuadNodeController.QuadNodeList)
            {
                if((intersected.Intersects(q.Bounds))!=null)
                {

                    // prezri vsetky bunky terenu v patchi a zisti ktoru malu bunku pretina
                    int minX = (int)q.Bounds.Min.X;
                    int minZ = (int)q.Bounds.Min.Z;
                    int maxX = (int)q.Bounds.Max.X;
                    int maxZ = (int)q.Bounds.Max.Z;

                    for (int j = minX; j < maxX; j++)
                    {
                        for (int k = minZ; k < maxZ; k++)
                        {
                            v.X = j;
                            v.Y = StaticHelpers.StaticHelper.GetHeightAt(k, j);
                            v.Z = k;

                            tmp.Min = v;
                            tmp.Max = v + new Vector3(1);


                            if (intersected.Intersects(tmp) != null)
                            {
                                return v;
                            }
                        }
                    }
                       
                    return q.Bounds.Min + (q.Bounds.Max - q.Bounds.Min) /2;

                    
                 
                    
                }
            }
            

            return Vector3.Zero;  
        }

        

    }
}

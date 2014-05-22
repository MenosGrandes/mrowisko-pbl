using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials.MaterialCluster
{
    public class Rock:Material
    {

        private List<Stone> stone = new List<Stone>();

       public int ClusterSize
       {
           get
           {
               return stone.Count;
           }
           set
           {
               value = ClusterSize;
           }

       }
       private int clusterSize;

       public Rock(LoadModel model,int clusterSize):base(model,clusterSize)
       {
          
           for(int i=0;i<clusterSize;i++)
           {
               stone.Add(new Stone());
           }
           
       }
       public void Draw(Matrix View, Matrix Projection, float time)
       {
           model.Draw(View, Projection, time);
       }
       public void removeRock(int n)
       {
           if (stone.Count != 0) { stone.RemoveRange(0, 1); }

       }
    }
}

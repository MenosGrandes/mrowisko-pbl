using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials.MaterialCluster
{
   public class Log:Material
    {

              private List<Wood> wood = new List<Wood>();

       public int ClusterSize
       {
           get
           {
               return wood.Count;
           }
           set
           {
               wood.RemoveRange(0, 1);
           }

       }
       public int MaxClusterSize;
       private int clusterSize;

       public Log(LoadModel model,int clusterSize):base(model)
       {
           this.clusterSize = clusterSize;
           this.MaxClusterSize = clusterSize;
           for(int i=0;i<clusterSize;i++)
           {
               wood.Add(new Wood());
           }
           
       }
       public override void Draw(Matrix View, Matrix Projection)
       {
           model.Draw(View, Projection);
       }

   }

}

using GameCamera;
using Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Meterials.MaterialCluster
{
[Serializable]
   public class Log:Material
    {

              private List<Wood> wood = new List<Wood>();

       new public  int ClusterSize
       {
           get
           {
               return wood.Count;
           }
           set
           {
               value = ClusterSize;
           }


       }

       public Log(LoadModel model,int ClusterSize):base(model,ClusterSize)
       {
           this.ClusterSize = ClusterSize;
           for (int i = 0; i < ClusterSize; i++)
           {
               wood.Add(new Wood());
           }
           
       }

       public void removeWood(int n)
       {
              wood.RemoveRange(0, 1); 

       }

   }

}

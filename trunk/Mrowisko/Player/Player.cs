using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Meterials;
namespace AntHill
{
   public static class  Player
    {
      static private List<Wood> Wood=new List<Wood>();
     static  public int wood
       {
           get
           {
               return Wood.Count;
           }
       } 
            public static void addWood()
            {
                   Wood.Add(new Wood());
            }
            public static void addWood(int n)
            {
                for (int i = 0; i < n;i++ )
                    Wood.Add(new Wood());
            }
    }

}

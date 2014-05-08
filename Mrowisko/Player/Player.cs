using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Meterials;
namespace AntHill
{
    public class Player
    {
       private List<Wood> Wood=new List<Wood>();
       public int wood
       {
           get
           {
               return Wood.Count;
           }
       } 
            public void addWood()
            {
                   Wood.Add(new Wood());
            }
            public  void addWood(int n)
            {
                for (int i = 0; i < n;i++ )
                    Wood.Add(new Wood());
            }
    }

}

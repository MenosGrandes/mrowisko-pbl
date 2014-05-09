using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Meterials;
using Logic.Meterials.MaterialCluster;
namespace AntHill
{
   public static class  Player
   {
       public static List<Material> materials = new List<Material>();
       #region AddMaterial
       public static void addMaterial(List<Material> material)
            {
                materials.AddRange(material);
            }
       #endregion
       public static int wood
       {
           get
           {
               return materials.Count(mat => mat.GetType().Name =="Wood" );
           }
       }
       public static int stone
       {
           get
           {
               return materials.Count(mat => mat.GetType().Name == "Stone");
           }
       }
   }

}

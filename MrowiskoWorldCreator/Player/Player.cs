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
      #region MaterialLists
       public static List<Material> materials = new List<Material>();
      #endregion
       #region AddMaterial
       public static void addMaterial(List<Material> material)
            {
                materials.AddRange(material);
            }
       public static void addMaterial(Material material)
       {
           materials.Add(material);
       }
       #endregion
       #region Material GET SET
       public static int wood
       {
           get
           {
               return materials.Count(mat => mat.GetType() == typeof(Wood));
           }
       }
       public static int stone
       {
           get
           {
               return materials.Count(mat => mat.GetType() == typeof(Stone));
           }
       }
       public static int hyacynt
       {
           get
           {
               return materials.Count(mat => mat.GetType() == typeof(Hyacynt));
           }
       }
       public static int dicentra
       {
           get
           {
               return materials.Count(mat => mat.GetType() == typeof(Dicentra));
           }
       }
       public static int chelidonium
       {
           get
           {
               return materials.Count(mat => mat.GetType() == typeof(Chelidonium));
           }
       }                                                                       
       #endregion
   }

}

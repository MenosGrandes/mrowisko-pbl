using Logic;
using Logic.Units.Ants;
using Map;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Controlers
{
   public static class LoadModelsFromFile
    {
       public static List<InteractiveModel> listOfAllInteractiveModelsFromFile = new List<InteractiveModel>();

       public static void Load()
       {

           using (Stream stream = File.Open("dupa.bin", FileMode.Open))
           {
               var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

               List<InteractiveModel> salesman = (List<InteractiveModel>)bformatter.Deserialize(stream);

               foreach (InteractiveModel model in salesman)
               {

                   //CreatorController.models.Add(model);
                   if (model.GetType().Name == "AntPeasant")
                   {
                       AntPeasant p = new AntPeasant(null);
                       p.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/mrowka_01"), model.Model.Position, model.Model.Rotation, model.Model.Scale,StaticHelpers.StaticHelper.Device,null);

                       listOfAllInteractiveModelsFromFile.Add(p);


                   }

               }
           }


       }

    }
}

using Logic;
using Logic.Building;
using Logic.Meterials.MaterialCluster;
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
           /*
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
            */
           using (Stream stream = File.Open("dupa.bin", FileMode.Open))
           {

               var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

               List<InteractiveModel> salesman = (List<InteractiveModel>)bformatter.Deserialize(stream);
               foreach (InteractiveModel model in salesman)
               {

                   switch (model.GetType().Name)
                   {
                       case "AntPeasant":
                           AntPeasant p = new AntPeasant(null);
                           p.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/mrowka_01"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,null);

                           listOfAllInteractiveModelsFromFile.Add(p);

                           break;
                       case "Log":

                           Log g = new Log(null, ((Log)model).ClusterSize);
                           g.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/log"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, null);

                           listOfAllInteractiveModelsFromFile.Add(g);

                           break;
                       case "Rock":


                           Rock q = new Rock(null, ((Rock)model).ClusterSize);
                           q.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/stone2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, null);
                           listOfAllInteractiveModelsFromFile.Add(q);




                           break;
                       case "BuildingPlace":


                           BuildingPlace w = new BuildingPlace(null);
                           w.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, null);

                           listOfAllInteractiveModelsFromFile.Add(w);




                           break;
                   }


               }
           }

       }

    }
}

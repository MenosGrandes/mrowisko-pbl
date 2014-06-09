using Logic;
using Logic.Building;
using Logic.Building.AntBuildings.Granary;
using Logic.EnviroModel;
using Logic.Meterials.MaterialCluster;
using Logic.Units.Ants;
using Logic.Units.Predators;
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
       public static LightsAndShadows.Light _light;
       public static void setLight(LightsAndShadows.Light light)
       {
           _light = light;
       }
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
           System.Windows.Forms.OpenFileDialog a = new System.Windows.Forms.OpenFileDialog();
           if (a.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           { 
           using (Stream stream = File.Open(a.FileName, FileMode.Open))
           {

               var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

               List<InteractiveModel> salesman = (List<InteractiveModel>)bformatter.Deserialize(stream);
               foreach (InteractiveModel model in salesman)
               {
                    Console.WriteLine(model.GetType().BaseType.Name);
                   switch (model.GetType().Name)
                   {
                       case "AntPeasant":
                           AntPeasant p = new AntPeasant(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/queen"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));
                           p.AtackInterval = 10;
                           p.Model.switchAnimation("Atack");
                           listOfAllInteractiveModelsFromFile.Add(p);
                          // models.Add(new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(Content.Load<Model>("queen"), new Vector3(150, 0, 0), new Vector3(0, 6, 0), new Vector3(0.4f), GraphicsDevice,Content, light), 10000, 10))
                           break; 
                       case "Log":

                           Log g = new Log(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//log"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light), ((Log)model).ClusterSize);
                          
                           listOfAllInteractiveModelsFromFile.Add(g);

                           break;
                       case "Rock":


                           Rock q = new Rock(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//stone2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light), ((Rock)model).ClusterSize);
                           listOfAllInteractiveModelsFromFile.Add(q);




                           break;
                       case "BuildingPlace":


                           BuildingPlace w = new BuildingPlace( new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                           

                           listOfAllInteractiveModelsFromFile.Add(w);




                           break;

                       case "AntGranary":


                           AntGranary ag = new AntGranary(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//antGranary"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));

                           listOfAllInteractiveModelsFromFile.Add(ag);
                            
                            break;



                       case "TownCenter":


                            Logic.Building.AntBuildings.TownCenter ad = new Logic.Building.AntBuildings.TownCenter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//townCenter"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                            

                            listOfAllInteractiveModelsFromFile.Add(ad);

                            break;

                       case "Spider":


                            Spider s = new Spider(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//spider"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,StaticHelpers.StaticHelper.Content,_light));
                            s.AtackInterval = 10;
                            s.Model.switchAnimation("Jump");
                            listOfAllInteractiveModelsFromFile.Add(s);




                            break;
                       case "Tree":


                            Tree t = new Tree(null);
                            t.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree1"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light);

                            listOfAllInteractiveModelsFromFile.Add(t);




                            break;
                       case "Tree2":


                            Tree2 t2 = new Tree2(null);
                            t2.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light);

                            listOfAllInteractiveModelsFromFile.Add(t2);




                            break;
                                    
                   }


               }
           }
           }
       }

    }
}

﻿using Logic;
using Logic.Building;
using Logic.Building.AntBuildings.Granary;
using Logic.EnviroModel;
using Logic.Meterials.MaterialCluster;
using Logic.Units.Ants;
using Logic.Units.Predators;
using Map;
using Microsoft.Xna.Framework;
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
          
           System.Windows.Forms.OpenFileDialog a = new System.Windows.Forms.OpenFileDialog();
           if (a.ShowDialog() == System.Windows.Forms.DialogResult.OK)
           { 
           using (Stream stream = File.Open(a.FileName, FileMode.Open))
           {

               var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

               List<InteractiveModel> salesman = (List<InteractiveModel>)bformatter.Deserialize(stream);
               foreach (InteractiveModel model in salesman)
               {
                    Console.WriteLine(model.GetType().Name);
                   switch (model.GetType().Name)
                   {
                       case "AntPeasant":
                           AntPeasant p = new AntPeasant(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/ant"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));
                           p.AtackInterval = 10;
                           p.Hp =300;
                           p.gaterTime = 10;
                           p.Model.switchAnimation("Atack");
                           listOfAllInteractiveModelsFromFile.Add(p);
                          // models.Add(new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(Content.Load<Model>("queen"), new Vector3(150, 0, 0), new Vector3(0, 6, 0), new Vector3(0.4f), GraphicsDevice,Content, light), 10000, 10))
                           p.Model.CreateBoudingBox();
                          break; 
                       case "Log":

                           Log g = new Log(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//log"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light), ((Log)model).ClusterSize);
                           g.Model.BuildBoundingSphereMaterial();
                           listOfAllInteractiveModelsFromFile.Add(g);

                           break;
                       case "Rock":


                           Rock q = new Rock(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//stone2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light), ((Rock)model).ClusterSize);
                            q.Model.BuildBoundingSphereMaterial()  ;
                           listOfAllInteractiveModelsFromFile.Add(q);




                           break;
                       case "BuildingPlace":


                           BuildingPlace w = new BuildingPlace( new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                           w.Model.BuildBoundingSphereMaterial();

                           listOfAllInteractiveModelsFromFile.Add(w);




                           break;

                       case "AntGranary":


                           AntGranary ag = new AntGranary(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//antGranary"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                           ag.Model.BuildBoundingSphereMaterial();

                           listOfAllInteractiveModelsFromFile.Add(ag);
                            
                            break;



                       case "TownCenter":


                            Logic.Building.AntBuildings.TownCenter ad = new Logic.Building.AntBuildings.TownCenter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//townCenter"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                            //ad.Model.CreateBoudingBox();
                            ad.Model.B_Box = BoundingBox.CreateFromSphere(ad.Model.BoundingSphere);
                            ad.Model.BuildBoundingSphereMaterial();

                            listOfAllInteractiveModelsFromFile.Add(ad);

                            break;

                       case "Spider":


                            Spider s = new Spider(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//spider"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,StaticHelpers.StaticHelper.Content,_light));
                            s.AtackInterval = 10;
                            s.Hp = 1000;

                            s.Model.switchAnimation("Idle");
                            listOfAllInteractiveModelsFromFile.Add(s);




                            break;  /* 
                       case "Tree":


                            Tree t = new Tree(null);
                            t.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree1"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light);
                         //  t.Model.BuildBoundingSphereMaterial();

                            listOfAllInteractiveModelsFromFile.Add(t);




                            break;  
                     case "Tree2":


                            Tree2 t2 = new Tree2(null);
                            t2.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light);
                           //t2.Model.BuildBoundingSphereMaterial();

                            listOfAllInteractiveModelsFromFile.Add(t2);




                            break;      */
                       case "Cone":
                           
                            Cone c = new Cone(null);
                            c.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//123"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light);
                          //  c.Model.BuildBoundingSphereMaterial();
                           // c.Model.B_Box = BoundingBox.CreateFromSphere(c.Model.Spheres[0]);
                            listOfAllInteractiveModelsFromFile.Add(c);

                           break;
                       case "Cone1":

                           Cone1 c1 = new Cone1(null);
                           c1.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//Szyszka1"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light);
                         //  c1.Model.BuildBoundingSphereMaterial();
                           
                           // c1.Model.B_Box = BoundingBox.CreateFromSphere(c1.Model.Spheres[0]);
                           listOfAllInteractiveModelsFromFile.Add(c1);

                           break;
                       case "Grass":

                           Grass gr = new Grass(null);
                           gr.Model = new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//Szyszka2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light);
                           gr.Model.BuildBoundingSphereMaterial();
                                                     // c1.Model.CreateBoudingBox();

                           listOfAllInteractiveModelsFromFile.Add(gr);

                           break;
                                    
                   }


               }
           }
           }
       }

    }
}
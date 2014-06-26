using Logic;
using Logic.Building;
using Logic.Building.AntBuildings.Granary;
using Logic.EnviroModel;
using Logic.Meterials.MaterialCluster;
using Logic.Units.Allies;
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
                          break;
                       case "StrongAnt":
                          StrongAnt sa = new StrongAnt(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/strongAnt"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));
                          sa.Model.switchAnimation("Atack");

                          listOfAllInteractiveModelsFromFile.Add(sa);
                          break;
                       case "Queen":
                          Queen qq = new Queen(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/queen"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));
                         qq.Model.switchAnimation("Atack");

                          listOfAllInteractiveModelsFromFile.Add(qq);
                          break;
                       case "AntSpitter":
                          AntSpitter asd = new AntSpitter(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models/plujka"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));
                          asd.Model.switchAnimation("Atack");

                          listOfAllInteractiveModelsFromFile.Add(asd);
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


                            s.Model.switchAnimation("Idle");
                            listOfAllInteractiveModelsFromFile.Add(s);



                            break;   
                       case "Tree":


                            Tree t = new Tree(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree1"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light));
            
                         //  t.Model.BuildBoundingSphereMaterial();

                            listOfAllInteractiveModelsFromFile.Add(t);




                            break;  
                     case "Tree2":


                            Tree2 t2 = new Tree2(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//tree2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,_light));
                           //t2.Model.BuildBoundingSphereMaterial();

                            listOfAllInteractiveModelsFromFile.Add(t2);




                            break;      
                       case "Cone":
                           
                            Cone c = new Cone(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//Szyszka2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                            
                          //  c.Model.BuildBoundingSphereMaterial();
                           // c.Model.B_Box = BoundingBox.CreateFromSphere(c.Model.Spheres[0]);
                            listOfAllInteractiveModelsFromFile.Add(c);

                           break;
                       case "Cone1":

                           Cone1 c1 = new Cone1(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//Szyszka1"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                         //  c1.Model.BuildBoundingSphereMaterial();
                           
                           // c1.Model.B_Box = BoundingBox.CreateFromSphere(c1.Model.Spheres[0]);
                           listOfAllInteractiveModelsFromFile.Add(c1);

                           break;
                       case "Grass":

                           Grass gr = new Grass(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//Szyszka2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, _light));
                           gr.Model.BuildBoundingSphereMaterial();
                                                     // c1.Model.CreateBoudingBox();

                           listOfAllInteractiveModelsFromFile.Add(gr);

                           break;
                       case "GrassHopper":

                           GrassHopper gr1 = new GrassHopper(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//grasshopper"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device,StaticHelpers.StaticHelper.Content, _light));
                          
                        // c1.Model.CreateBoudingBox();
                           gr1.Model.switchAnimation("Idle");
                           listOfAllInteractiveModelsFromFile.Add(gr1);

                           break;
                       case "Beetle":

                           Beetle beetle = new Beetle(new LoadModel(StaticHelpers.StaticHelper.Content.Load<Model>("Models//beetle"), model.Model.Position, model.Model.Rotation, model.Model.Scale, StaticHelpers.StaticHelper.Device, StaticHelpers.StaticHelper.Content, _light));

                           // c1.Model.CreateBoudingBox();
                           beetle.Model.switchAnimation("Idle");
                           listOfAllInteractiveModelsFromFile.Add(beetle);

                           break;
                   
                   }
                   

               }
           }
           }
       }

    }
}

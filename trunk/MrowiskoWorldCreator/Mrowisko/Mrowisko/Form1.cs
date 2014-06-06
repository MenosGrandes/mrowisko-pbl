using Logic;
using Logic.Meterials.MaterialCluster;
using Logic.Units.Ants;
using Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleStaticHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Logic.Building;
using Logic.Building.AntBuildings.Granary;
namespace AntHill
{
    public partial class Form1 : Form
    {
        List<string> _items = new List<string>();
        private InteractiveModel model;
        private bool selected=false;
        private int selectedIndex;
        public Form1()
        {
            InitializeComponent();


        }
        public IntPtr ButtonAddHandle
        {
            get { return btnAdd.Handle; }
        }

        public IntPtr ButtonRemoveHandle
        {
            get { return btnDel.Handle; }
        }

        public IntPtr CanvasHandle
        {
            get { return canvas.Handle; }
        }

        public Size ViewportSize
        {
            get { return canvas.Size; }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Life is brutal
            Application.Exit();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            
            
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < checkedListBox1.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox1.SetItemChecked(ix, false);
        }

        private void GameWindowClicke(object sender, EventArgs e)
        {
           if(model!=null && selected==true)
           {
               model.Model.Position = CreatorController.MousePosition;
               CreatorController.models.Add(model);
               _items.Add(model.ToString());
               listBox2.DataSource = null;
               listBox2.DataSource = _items;
           }
           selected = false;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int curItem = listBox2.SelectedIndex;

            if (listBox2.Items.Count>0 &&curItem>=0)
            {
                Console.WriteLine(curItem);
                listBox2.DataSource = null;
                CreatorController.models.RemoveAt(curItem);

                _items.RemoveAt(curItem);
                listBox2.Items.Remove(listBox2.SelectedItem);

                listBox2.DataSource = _items;     
            }                                     
        }
         private void EditObjectScale(InteractiveModel model2)
        {

            model2.Model.Scale = new Vector3((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value);

        }
         private void EditObjectRotation(InteractiveModel model2)
         {

             model2.Model.Rotation =new Vector3( (float)numericUpDown4.Value,(float)numericUpDown5.Value,(float)numericUpDown6.Value);

         }
         private void EditObjectPosition(InteractiveModel model2)
         {

             model2.Model.Position = new Vector3((float)numericUpDown9.Value, (float)numericUpDown8.Value, (float)numericUpDown7.Value);

         }




         private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
         {
             selectedIndex= listBox2.SelectedIndex;
             foreach(InteractiveModel model in CreatorController.models)
             {
                 model.selected = false;
             }
             if (listBox2.SelectedIndex >= 0)
             {
                 (CreatorController.models.ElementAt(selectedIndex)).selected = true;
               //  numericUpDown7.Value = (decimal)CreatorController.models.ElementAt(selectedIndex).Model.Position.X;
               //  numericUpDown8.Value = (decimal)CreatorController.models.ElementAt(selectedIndex).Model.Position.Y;
               //  numericUpDown9.Value = (decimal)CreatorController.models.ElementAt(selectedIndex).Model.Position.Z;
             }

         }


         private void ChangedScale(object sender, EventArgs e)
         {    if(selectedIndex>=0)
             EditObjectScale(CreatorController.models[selectedIndex]);

         }

         private void ChangedRotation(object sender, EventArgs e)
         {
             if (selectedIndex >= 0)
             EditObjectRotation(CreatorController.models[selectedIndex]);

             Console.WriteLine("12312213");
         }



        private void button3_Click(object sender, EventArgs e)
        {

            Load();
        }
        public  void Save()
        {

            using (Stream stream = File.Open("dupa.bin", FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, CreatorController.models);
            }


        }
        public void Load()
        {
            
            using (Stream stream = File.Open("dupa.bin", FileMode.Open))
            {

                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                List<InteractiveModel> salesman = (List<InteractiveModel>)bformatter.Deserialize(stream);
                listBox2.DataSource = null;
                if(salesman==CreatorController.models)
                {
                    return;
                }
                foreach(InteractiveModel model in salesman)
                {

                switch(model.GetType().Name)
                {   
                    case "AntPeasant":
                       AntPeasant p = new AntPeasant(null);
                     p.Model = new LoadModel(CreatorController.content.Load<Model>("Models/mrowka_01"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);
                    
                     CreatorController.models.Add(p);
                     _items.Add(p.ToString());     
                        
                        break;
                    case "Log":
                       
                           Log g = new Log(null,((Log)model).ClusterSize);
                     g.Model = new LoadModel(CreatorController.content.Load<Model>("Models/log"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);

                     CreatorController.models.Add(g);
                     _items.Add(g.ToString());

                        break;
                    case "Rock": 
                        
                           
                           Rock q = new Rock(null,((Rock)model).ClusterSize);
                           q.Model = new LoadModel(CreatorController.content.Load<Model>("Models/stone2"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);

                     CreatorController.models.Add(q);
                     _items.Add(q.ToString());

                        
                        
                        break;
                    case "BuildingPlace":


                        BuildingPlace w = new BuildingPlace(null);
                        w.Model = new LoadModel(CreatorController.content.Load<Model>("Models/buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);

                        CreatorController.models.Add(w);
                        _items.Add(w.ToString());



                        break;
                    case "AntGranary":


                        AntGranary ad = new AntGranary(null);
                        ad.Model = new LoadModel(CreatorController.content.Load<Model>("Models/buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);

                        CreatorController.models.Add(ad);
                        _items.Add(ad.ToString());



                        break;

                    case "TownCenter":


                        Logic.Building.AntBuildings.TownCenter ag = new Logic.Building.AntBuildings.TownCenter(null);
                        ag.Model = new LoadModel(CreatorController.content.Load<Model>("Models/buildingPlace"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);

                        CreatorController.models.Add(ag);
                        _items.Add(ag.ToString());



                        break;



                }
                
                  
            }
                listBox2.DataSource = _items;
            }


        }

        private void PositionChanged(object sender, System.EventArgs e)
        {
            if (selectedIndex >= 0)
                EditObjectPosition(CreatorController.models[selectedIndex]);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
           
        }



        private void btnAdd_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("asdasd");
            selected = true;
            foreach (string name in checkedListBox1.CheckedItems)
            {
                switch (name)
                {
                    case "AntPeasant":
                        {
                            AntPeasant p = new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(CreatorController.content.Load<Model>("Models/mrowka_01"), CreatorController.MousePosition, new Vector3(0, 6, 0), new Vector3(0.5f), CreatorController.device), 10, 100);
                            model = p;
                            break;
                        }
                    case "Log":
                        {
                            Log p = new Log(new LoadModel(CreatorController.content.Load<Model>("Models/log"), CreatorController.MousePosition, Vector3.Zero, Vector3.One, CreatorController.device), Convert.ToInt16(textBox1.Text));
                            model = p;
                            break;
                        }
                    case "Rock":
                        {
                            Rock p = new Rock(new LoadModel(CreatorController.content.Load<Model>("Models/stone2"), CreatorController.MousePosition, Vector3.Zero, Vector3.One, CreatorController.device), Convert.ToInt16(textBox1.Text));
                            model = p;
                            break;
                        }
                    case "BuildingPlace":
                        {
                            BuildingPlace p = new BuildingPlace(new LoadModel(CreatorController.content.Load<Model>("Models//buildingPlace"), CreatorController.MousePosition, Vector3.Zero, Vector3.One, CreatorController.device));
                            model = p;
                            break;
                        }
                    case "AntGranary":
                        {
                            AntGranary p = new AntGranary(new LoadModel(CreatorController.content.Load<Model>("Models//buildingPlace"), CreatorController.MousePosition, Vector3.Zero, Vector3.One, CreatorController.device), 100, 100, 100, 23, 10000);
                            model = p;
                            break;
                        }
                    case "TownCenter":
                        {
                            Logic.Building.AntBuildings.TownCenter p = new Logic.Building.AntBuildings.TownCenter(new LoadModel(CreatorController.content.Load<Model>("Models//buildingPlace"), CreatorController.MousePosition, Vector3.Zero, Vector3.One, CreatorController.device));
                            model = p;
                            break;
                        }
                }
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void button3_Click_1(object sender, System.EventArgs e)
        {
            Load();
        }

        private void button1_Click_1(object sender, System.EventArgs e)
        {
            if (selectedIndex > -1) { 
            decimal d1 = (decimal)CreatorController.models[selectedIndex].Model.Position.Z;
            decimal d2 = (decimal)CreatorController.models[selectedIndex].Model.Position.Y;
            decimal d3 = (decimal)CreatorController.models[selectedIndex].Model.Position.X;
            numericUpDown7.Value = d1;
            numericUpDown8.Value = d2;
            numericUpDown9.Value = d3;
            } 
            }

        private void ChangedRotation(object sender, ScrollEventArgs e)
        {

        }


   











    }
}

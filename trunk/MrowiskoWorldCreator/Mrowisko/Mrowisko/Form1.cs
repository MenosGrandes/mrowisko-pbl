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
            selected = true;   
             foreach (string name in checkedListBox1.CheckedItems)
                {
             switch(name)
             {
                 case "AntPeasant":
                     {
                         AntPeasant p = new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(CreatorController.content.Load<Model>("Models/mrowka_01"), CreatorController.MousePosition, new Vector3(0, 6, 0), new Vector3(0.5f), CreatorController.device), 10, 100);
                         model = p;
                         break; }
                 case "Log":
                     {
                         Log p = new Log(new LoadModel(CreatorController.content.Load<Model>("Models/stone2"), new Vector3(200, 12, 42), Vector3.Zero, Vector3.One, CreatorController.device), 10000);
                         model = p;
                         break;
                     }
             }
             }
            
            
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
             if (listBox2.SelectedIndex>=0)
                 (CreatorController.models.ElementAt(selectedIndex)).selected = true;



         }


         private void ChangedScale(object sender, EventArgs e)
         {    if(selectedIndex>=0)
             EditObjectScale(CreatorController.models[selectedIndex]);

         }

         private void ChangedRotation(object sender, EventArgs e)
         {
             if (selectedIndex >= 0)
             EditObjectRotation(CreatorController.models[selectedIndex]);
         }


      
        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            saveFileDialog1.ShowDialog();
          //  Save();
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

                foreach(InteractiveModel model in salesman)
            {

                //CreatorController.models.Add(model);
                 if(model.GetType().Name=="AntPeasant")
                 {
                     AntPeasant p = new AntPeasant(null);
                     p.Model = new LoadModel(CreatorController.content.Load<Model>("Models/mrowka_01"), model.Model.Position, model.Model.Rotation, model.Model.Scale, CreatorController.device);
                    
                     CreatorController.models.Add(p);
                     _items.Add(p.ToString());
                         

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
            numericUpDown7.Value = (decimal)CreatorController.models[selectedIndex].Model.Position.X;
            numericUpDown8.Value = (decimal)CreatorController.models[selectedIndex].Model.Position.Y;
            numericUpDown9.Value = (decimal)CreatorController.models[selectedIndex].Model.Position.Z;
            Console.WriteLine(numericUpDown7.Value);
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }







    }
}

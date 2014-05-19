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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntHill
{
    public partial class Form1 : Form
    {
        List<string> _items = new List<string>();
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

             foreach (string name in checkedListBox1.CheckedItems)
                {
             switch(name)
             {
                 case "AntPeasant":
                     {
                         AntPeasant p = new AntPeasant(10, 10, 10, 10, 10, 10, new LoadModel(CreatorController.content.Load<Model>("Models/mrowka_01"), new Vector3(100, 10, 500), new Vector3(0, 6, 0), new Vector3(0.5f), CreatorController.device), 10, 100);
                         CreatorController.models.Add(p);
                         _items.Add(p.ToString());
                         listBox2.DataSource = null;
                         listBox2.DataSource = _items;
                         break; }
                 case "Log":
                     {
                         Log p = new Log(new LoadModel(CreatorController.content.Load<Model>("Models/stone2"), new Vector3(200, 12, 42), Vector3.Zero, Vector3.One, CreatorController.device), 10000);
                         CreatorController.models.Add(p);
                         _items.Add(p.ToString());
                         listBox2.DataSource = null;
                         listBox2.DataSource = _items;
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
    }
}

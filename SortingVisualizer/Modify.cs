using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Class;

namespace WindowsFormsApplication2
{
    public partial class Modify : Form
    {
        
        public static List<int> myList = new List<int>(); // List to show in listbox
        public static bool dataChanged = false; // Flag to check for data change

        public Modify(List<Bar> list)
        {
            myList.Clear();
            foreach (var item in (list))
            {
                myList.Add(item.height);
            }
            InitializeComponent();
        }

        // Return new list 
        public static List<Bar> NewList()
        {
            List<Bar> updatedBars = new List<Bar>();
            for (int i = 0; i < myList.Count; i++)
            {
                Bar newBar = new Bar();
                newBar.height = myList[i];
                newBar.barColor = Color.White;
                updatedBars.Add(newBar);
            }
            return updatedBars;
        }
        // Load list to listbox
        public void LoadList()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = myList;
            listBox1.Refresh();
        }
        private void Modify_Load(object sender, EventArgs e)
        {
            dataChanged = false;
            LoadList();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            nmbValue.Value = myList[listBox1.SelectedIndex];
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            myList[listBox1.SelectedIndex] = Convert.ToInt32(nmbValue.Value);
            LoadList();
            dataChanged = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

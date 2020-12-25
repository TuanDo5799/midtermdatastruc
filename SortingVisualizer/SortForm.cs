using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Class;

namespace WindowsFormsApplication2
{
    public partial class SortForm : Form
    {
        #region Define Variables

        public List<Bar> bars = new List<Bar>(); // Create a list of bar objects
        public bool IsSorted = false;   // Flag to check if array is sorted
        public string filePath;   // Store file path as a string


        // Timer, counter variables
        public Clock timer = new Clock(); // New object of Clock
        public System.Threading.Timer sort_timer = null;  // Create a timer
        public int comparions = 0;       // Count comparisons
        public int arrayaccess = 0;      // Count array accesses
        DateTime time_start;            // Store start time
        TimeSpan timeSpan_elapsed;      // Store time elapsed
        string time;                    // Store time string for display

        //Sizes

        int arraySize = 101;         // Number of elements
        int barWidth = 1;            // Bar width
        int barHeight = 0;           // Bar height

        #endregion

        public SortForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            cmbAlgorithm.SelectedIndex = 0;
            timer.SortSpeed = 40;
            GetBarSize();
        }

        #region Methods

        public bool CheckSorted()
        {
            for (int i = 1; i < bars.Count(); ++i)
            {
                if (bars[i].height < bars[i - 1].height)
                {
                    IsSorted = false;
                    return false;
                }
            }
            IsSorted = true;
            return true;
        }
        public void DisableButton()
        {
            cmbAlgorithm.Enabled = false;
            nmbSize.Enabled = false;
            btnStop.Enabled = false;
            btnStart.Enabled = false;
            btnFile.Enabled = false;
            btnModify.Enabled = false;
            btnRandom.Enabled = false;

            // Clear counter
            comparions = 0;
            arrayaccess = 0;
        }
        public void EnableButton()
        {
            cmbAlgorithm.Enabled = true;
            nmbSize.Enabled = true;
            btnFile.Enabled = true;
            btnStop.Enabled = true;
            btnStart.Enabled = true;
            btnModify.Enabled = true;
            btnRandom.Enabled = true;
        }

        public void RandomizeBar()
        {
            IsSorted = false;
            Random rd = new Random();
            //Clears the array if its not empty already
            if (bars.Count() > 0)
            {
                bars.Clear();
            }

            for (int i = barHeight; i < arraySize * barHeight; i += barHeight)
            {
                Bar b = new Bar();
                b.height = rd.Next(0, 500);
                b.width = barWidth;
                b.barColor = Color.White;
                bars.Add(b);
            }

        }
        public void GetBarSize()
        {
            if (arraySize <= 327)
                barWidth = (mainScreen.Width - 10 - arraySize) / arraySize;
            else
                barWidth = (mainScreen.Width - 10) / arraySize;
            barHeight = (this.Height - 50) / arraySize;
        }
        
        //public void StopTimer()
        //{
            //sort_timer.Change(Timeout.Infinite, Timeout.Infinite);
        //}
        

        private void CheckForChange()
        {
            if (Modify.dataChanged == true)
            {
                bars.Clear();
                GetBarSize();
                bars = Modify.NewList();
                Refresh();
            }

            CheckSorted();
        }
        #endregion


        #region Events

        private void mainScreen_Paint(object sender, PaintEventArgs e)
        {
            Color barColor = new Color();
            for (int i = 0; i < bars.Count(); i++)
            {
                Rectangle rect = new Rectangle(10 + (i * barWidth) + i, (mainScreen.Height - 10) - bars[i].height, barWidth, bars[i].height);
                barColor = bars[i].barColor;
                Brush toUse = new SolidBrush(barColor);
                e.Graphics.FillRectangle(toUse, rect);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            arraySize = Convert.ToInt32(nmbSize.Value);
            GetBarSize();
            RandomizeBar();
        }

        public async Task wait()
        {
            await Task.Delay(timer.SortSpeed);
            this.Refresh();
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            arraySize = (int)nmbSize.Value + 1;
            GetBarSize();
            RandomizeBar();
            Refresh();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            
            if (!CheckSorted())
            { 
                DisableButton();
                switch (cmbAlgorithm.SelectedIndex)
                {
                    case 0: Algorithm.SelectionSort(this); break;
                    case 1: Algorithm.InsertionSort(this); break;
                    //case 2: Algorithm.ShellSort(this); break;
                    case 3: Algorithm.BubbleSort(this); break;
                    case 4:
                        {
                            //StartTimer();
                            Algorithm.QuickSort(this, 0, this.bars.Count - 1);
                            //StopTimer();
                            Algorithm.FinalizeSort(this);
                            break;
                        }
                    case 5: Algorithm.CocktailSort(this); break;
                    //case 6: Algorithm.HeapSort(this); break;
                    default: break;
                }
            }
            else
            {
                MessageBox.Show("This list of number is already sorted", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void speedBar_ValueChanged(object sender, EventArgs e)
        {
            lblSpeed.Text = speedBar.Value + " ms";
            timer.SortSpeed = speedBar.Value;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "You file must be in this format in order to work properly" + Environment.NewLine +
                "First line: One number define number of elements" + Environment.NewLine +
                "Second line: Contain list of numbers which are seperated by space. Ex: 3 4 5", "Caution",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if(dr == DialogResult.OK)
            { 
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                filePath = openFileDialog.FileName;
            else if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            else
                filePath = string.Empty;
            if (filePath != null)
            {
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    arraySize = Int32.Parse(lines[0]);
                    var myList = new List<string>(lines[1].Split(' '));
                    arraySize = myList.Count(); // Just to make sure that the first line is wrong
                    nmbSize.Value = arraySize; // Show the size on the numeric box
                    barWidth = (mainScreen.Width - 10 - arraySize) / arraySize;
                    if (arraySize > 0)
                    {
                        bars.Clear();
                        for (int i = 0; i < myList.Count(); i++)
                        {
                            Bar item = new Bar();
                            item.height = Int32.Parse((myList[i]));
                            item.barColor = Color.White;
                            item.width = barWidth;
                            bars.Add(item);
                        }

                        Refresh();
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("You haven't choose the file!");
            }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!CheckSorted() && IsSorted == false)
            {
                bars.Sort((x, y) => x.height.CompareTo(y.height));
                Refresh();
            }
            else
            {
                MessageBox.Show("This list of number is already sorted", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Modify mdf = new Modify(this.bars);
            mdf.ShowDialog();
            CheckForChange();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            dr = MessageBox.Show("Are you sure", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
                Close();
        }
        

        #endregion

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mainScreen_Click(object sender, EventArgs e)
        {

        }
    }
}

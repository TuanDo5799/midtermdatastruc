using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace WindowsFormsApplication2.Class
{
    class Algorithm
    {
        #region Methods
        public async static void FinalizeSort(SortForm form) // Clear bar color when the sorting finished
        {
            for (int i = 0; i < form.bars.Count(); i++)
            {
                while (form.bars[i].barColor != Color.LightBlue)
                    form.bars[i].barColor = Color.LightBlue;
                await form.wait();
            }
            form.EnableButton();   // Enable some buttons that not allow when running
        }
        #endregion


        #region Algorithms

        // Selection Sort
        public async static void SelectionSort(SortForm form)
        {
            int min = 0; // Store minimum index
            if (!form.IsSorted)   // Check if form is not sorted yet
            {
                //form.StartTimer(); 
                for (int i = 0; i < form.bars.Count() - 1; i++)
                {
                    min = i;
                    form.bars[min].barColor = Color.Crimson; // Set red color to mark sorted element
                    for (var j = i + 1; j < form.bars.Count(); j++) // Let j point at next bar
                    {
                        form.arrayaccess++;
                        form.bars[j].barColor = Color.LightGreen; // Mark color for comparing item
                        if (form.bars[j].height < form.bars[min].height
                        ) // Compare if the next bar is lower than current bar
                        {
                            form.comparions++;
                            form.arrayaccess += 2;
                            min = j; // If it true, set minimum index to j
                            form.bars[min].barColor = Color.CornflowerBlue; // Mark color for minimum element in each loop
                        }

                    }
                    // Swap minimum element for each loop
                    int minimum = form.bars[min].height;
                    form.bars[min].height = form.bars[i].height;
                    form.bars[i].height = minimum;
                    form.arrayaccess += 4;

                    await form.wait();
                    // wait for the delay time
                }
                form.IsSorted = true;  // Set flag for sorted
                //form.StopTimer();
            }

            FinalizeSort(form); // Clear bars color
        }

        // Insertion Sort
        public async static void InsertionSort(SortForm form)
        {
            if (!form.IsSorted) // Check if array is sorted
            {
                //form.StartTimer();
                for (int i = 1; i < form.bars.Count(); ++i)
                {
                    int second = form.bars[i].height;   // Get second value
                    int j = i - 1;  // j is the first value
                    form.comparions++; 
                    while (j >= 0 && form.bars[j].height > second) // While next value still bigger than current value
                    {
                        form.bars[i].barColor = Color.CornflowerBlue; // Change bar color to blue to marker
                        form.bars[j + 1].height = form.bars[j].height;    // Set the next value to current value
                        j = j - 1;  // Decreasing j
                    }
                    form.bars[j + 1].height = second;   // Set second position to the next value
                    form.bars[j + 1].barColor = Color.Crimson; // Set color to mark
                    form.arrayaccess += 5;
                    await form.wait();
                }
                form.IsSorted = true;
                //form.StopTimer();
            }
            FinalizeSort(form);
        }
        
        // Bubble Sort
        public async static void BubbleSort(SortForm form)
        {
            if (!form.IsSorted)
            {
                //form.StartTimer();
                for (int i = 0; i < form.bars.Count(); i++) 
                {
                    for (int j = 1; j < form.bars.Count() - i; ++j)
                    {
                        form.comparions++; // Increase comparisons for each loops
                        // Check if previous value is higher than next value
                        if (form.bars[j - 1].height > form.bars[j].height)
                        {
                            form.arrayaccess += 2;  //Increase array accesses
                            //These three lines just swap the two elements
                            int num = form.bars[j].height;  
                            form.bars[j].height = form.bars[j - 1].height;
                            form.bars[j - 1].height = num;
                            // Mark color for first and second bar
                            form.bars[j].barColor = Color.GreenYellow;
                            form.bars[j - 1].barColor = Color.LimeGreen;
                            //aux++;
                        }
                    }
                    await form.wait(); // Wait for delay time
                }
                form.IsSorted = true;
                //form.StopTimer();
            }
            FinalizeSort(form);
        }
        // Quick Sort

        // This function return partition position
        public static int Partition(SortForm form, int low, int high)
        {
            // Select a pivot point.
            int pivot = form.bars[high].height;
            form.bars[high].barColor = Color.LightGreen; // Mark pivot bar color
            int lowIndex = (low - 1); // Set lower index

            // Reorder the collection.
            for (int j = low; j < high; j++)
            {
                // Check if low value is smaller than pivot value
                if (form.bars[j].height <= pivot)
                {
                    form.comparions += 2;
                    lowIndex++;             // If it true then increase low index
                    // Swap bar at low index with bar at pivot index
                    int temp = form.bars[lowIndex].height;
                    form.bars[lowIndex].height = form.bars[j].height;
                    form.bars[j].height = temp;
                    // Mark color for bar at low and high index
                    form.bars[lowIndex].barColor = Color.CornflowerBlue;
                    form.bars[j].barColor = Color.Crimson;
                    form.Refresh();
                }
                form.arrayaccess += 7;  // Increase 7 array accesses for each loop
            }
            // Finally swap bar at high index with bar next to low index
            int temp1 = form.bars[lowIndex + 1].height;
            form.bars[lowIndex + 1].height = form.bars[high].height;
            form.bars[high].height = temp1;
            form.arrayaccess += 6;
            return lowIndex + 1;
        }

        /* low  --> Starting index,  high  --> Ending index */
        public async static void QuickSort(SortForm form, int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(form, low, high);

                // Recursively continue sorting the array
                QuickSort(form, low, partitionIndex - 1);   // Before partitioning index
                QuickSort(form, partitionIndex + 1, high);  // After partitioning index
                await form.wait();
            }
        }

       
        // Cocktail Shaker Sort
        public async static void CocktailSort(SortForm form)
        {
            bool isSwapped = true;
            int start = 0;
            int end = form.bars.Count;
            //form.StartTimer();
            while (isSwapped == true)
            {
                //Reset this flag.  It is possible for this to be true from a prior iteration.
                isSwapped = false;
                //Do a bubble sort on this array, from low to high.  If something changed, make isSwapped true.
                for (int i = start; i < end - 1; ++i)
                {
                    if (form.bars[i].height > form.bars[i + 1].height)
                    {
                        // Swap current bar with next bar
                        int temp = form.bars[i].height;
                        form.bars[i].height = form.bars[i+1].height;
                        form.bars[i+1].height = temp;
                        form.bars[i+1].barColor = Color.LightGreen;
                        form.bars[i].barColor = Color.Crimson;
                        isSwapped = true;
                    }
                    form.comparions++;
                    form.arrayaccess += 7;
                    await form.wait();
                    // form.Refresh();
                }


                //If no swaps are made, the array is sorted.
                if (isSwapped == false)
                    break;

                //We need to reset the isSwapped flag for the high-to-low pass
                isSwapped = false;

                //The item we just moved is in its rightful place, so we no longer need to consider it unsorted.
                end = end - 1;

                //Now we bubble sort from high to low
                for (int i = end - 1; i >= start; i--)
                {
                    if (form.bars[i].height > form.bars[i+1].height)
                    {
                        int temp = form.bars[i].height;
                        form.bars[i].height = form.bars[i+1].height;
                        form.bars[i+1].height = temp;
                        form.bars[i+1].barColor = Color.CornflowerBlue;
                        form.bars[i].barColor = Color.LightGreen;
                        isSwapped = true;
                    }
                    form.comparions++;
                    form.arrayaccess += 8;
                    await form.wait();
                }
                //Finally, we need to increase the starting point for the next low-to-high pass.
                start = start + 1;
            }
            //form.StopTimer();
            FinalizeSort(form);
        }

        #endregion
    }
}

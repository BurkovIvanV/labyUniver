using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Compare_4_sorts
{
    class Program
    {
        private static Random random = new Random();

        static int[] GenerateArray(int lenght) //генерация массива случайными числами
        {
            var array = new int[lenght];
            for (int i = 0; i < lenght; i++)
                array[i] = random.Next(10000);
            return array;
        }

        static void Vivod(int[] b)
        {
            foreach (var i in b)
                Console.Write(i + " ");
            Console.WriteLine();
        }

        #region Пузырьковая сортировка
        static void BubbleSort(int[] array) // пузырьковая сортировка
        {
            for (int i = 0; i < array.Length; i++)
                for (int j = 0; j < array.Length - 1; j++)
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
            // Vivod(array);
        }
        #endregion

        #region Быстрая сортировка

        static void HoareSort(int[] array, int start, int end)
        {
            if (end == start) return;
            var pivot = array[end];
            var storeindex = start;
            for (int i = start; i <= end - 1; i++)
                if (array[i] <= pivot)
                {
                    var temp = array[i];
                    array[i] = array[storeindex];
                    array[storeindex] = temp;
                    storeindex++;
                }

            var n = array[storeindex];
            array[storeindex] = array[end];
            array[end] = n;
            if (storeindex > start) HoareSort(array, start, storeindex - 1);
            if (storeindex < end) HoareSort(array, storeindex + 1, end);
        }

        static void HoareSort(int[] array)
        {
            HoareSort(array, 0, array.Length - 1);
            //Vivod(array);
        }

        #endregion

        #region Сортировка слиянием

        static int[] temporaryarray;
        static void Merge(int[] array, int start, int middle, int end)
        {
            var leftPtr = start;
            var rigtPtr = middle + 1;
            var lenght = end - start + 1;
            for (int i = 0; i < lenght; i++)
            {
                if (rigtPtr > end || (leftPtr <= middle && array[leftPtr] < array[rigtPtr]))
                {

                    temporaryarray[i] = array[leftPtr];
                    leftPtr++; //поставили элемент - забыли про нулевую (и далее) ячейку

                }
                else
                {
                    temporaryarray[i] = array[rigtPtr];
                    rigtPtr++;
                }
            }

            for (int i = 0; i < lenght; i++)
                array[i + start] = temporaryarray[i];

        }

        static void MergeSort(int[] array, int start, int end)
        {
            if (start == end) return;
            var middle = (start + end) / 2;
            MergeSort(array, start, middle);
            MergeSort(array, middle + 1, end);
            Merge(array, start, middle, end);
        }

        static void MergeSort(int[] array)
        {
            temporaryarray = new int[array.Length];
            MergeSort(array, 0, array.Length - 1);
            //Vivod(array);
        }
        #endregion

        #region замер времени сортировки
        static Dictionary <int, double> SortTime(Action<int[]> sortProcedure)
        {
            List<int[]> arrays = new List<int[]>();
            var speedSorts = new Dictionary<int, double>();
            int j = 0;

            for (int i = 1; i <= 10000; i += 500)
            {
                arrays.Add(GenerateArray(i));
            }
            for (int i = 1; i <= 10000; i += 500)
            { 
                var watch = new Stopwatch();
                watch.Start();
                sortProcedure(arrays[j]);
              
                watch.Stop();
                speedSorts.Add(i, watch.ElapsedMilliseconds);
                j++;
            }

            foreach (var e in speedSorts)
            {
                Console.WriteLine(e.Key - 1 + "\t" + e.Value);
            }

            return speedSorts;
        }
        #endregion

        static void Main()
        {
            Console.WriteLine("Пузырьковая сортировка");
            //var dictionaryBubble = SortTime(BubbleSort);
            Console.WriteLine("Сортировка слиянием");
            var dictionaryMerge = SortTime(MergeSort);
            Console.WriteLine("Быстрая сортировка");
            var dictionaryHoare = SortTime(HoareSort);
            Console.WriteLine("Библиотечная сортировка");
            
            var dictionaryMathSort = SortTime(Array.Sort);
            Console.ReadKey();
        }
    }
}
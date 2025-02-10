using System;
using System.IO;
using System.Threading.Tasks;

namespace QuickSortApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "nombres.txt";
            string outputFile = "file_name_tries.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file \"{inputFile}\" not found.");
                return;
            }

            // Read names from input file
            string[] names = File.ReadAllLines(inputFile);

            // Sort the names using parallel quicksort
            QuickSort(names, 0, names.Length - 1);

            // Write the sorted names to the output file
            File.WriteAllLines(outputFile, names);
            Console.WriteLine($"Sorted names have been written to \"{outputFile}\".");
        }

        static void QuickSort(string[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivotIndex = Partition(arr, left, right);
                Parallel.Invoke(
                    () => QuickSort(arr, left, pivotIndex - 1),
                    () => QuickSort(arr, pivotIndex + 1, right)
                );
            }
        }

        static int Partition(string[] arr, int left, int right)
        {
            string pivot = arr[right];
            int i = left;

            for (int j = left; j < right; j++)
            {
                if (string.Compare(arr[j], pivot, StringComparison.Ordinal) <= 0)
                {
                    Swap(arr, i, j);
                    i++;
                }
            }
            Swap(arr, i, right);
            return i;
        }

        static void Swap(string[] arr, int i, int j)
        {
            string temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }
}

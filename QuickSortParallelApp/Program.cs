using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuickSortParallelApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "nombres.txt";
            string outputFile = "nombres_tries.txt";

            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"{inputFile} not found, generating test file with 1000000 random numbers.");
                GenerateTestFile(inputFile, 1000000);
            }

            int[] numbers = File.ReadAllLines(inputFile)
                                .Select(line => int.Parse(line.Trim()))
                                .ToArray();

            QuickSortParallel(numbers, 0, numbers.Length - 1);

            File.WriteAllLines(outputFile, numbers.Select(n => n.ToString()));

            Console.WriteLine($"Sorted numbers have been written to {outputFile}");
        }

        public static void GenerateTestFile(string fileName, int count)
        {
            Random rnd = new Random();
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < count; i++)
                {
                    writer.WriteLine(rnd.Next());
                }
            }
        }

        public static void QuickSortParallel(int[] array, int left, int right)
        {
            if (left < right)
            {
                // Use parallel execution for arrays larger than 500 elements
                if (right - left > 500)
                {
                    int pivot = Partition(array, left, right);
                    
                    // Create parallel tasks for both halves
                    Parallel.Invoke(
                        () => QuickSortParallel(array, left, pivot - 1),
                        () => QuickSortParallel(array, pivot + 1, right)
                    );
                }
                else
                {
                    // Use regular quicksort for smaller arrays
                    int pivot = Partition(array, left, right);
                    QuickSortParallel(array, left, pivot - 1);
                    QuickSortParallel(array, pivot + 1, right);
                }
            }
        }

        public static int Partition(int[] array, int left, int right)
        {
            int pivot = array[right];
            int i = left - 1;
            for (int j = left; j < right; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }
            Swap(array, i + 1, right);
            return i + 1;
        }

        public static void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}

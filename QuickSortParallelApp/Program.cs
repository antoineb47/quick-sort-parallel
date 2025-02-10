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
            int threshold = 10000;
            if (left < right)
            {
                int pivotIndex = Partition(array, left, right);

                Task leftTask = null;
                Task rightTask = null;

                if ((pivotIndex - 1 - left) > threshold)
                {
                    leftTask = Task.Run(() => QuickSortParallel(array, left, pivotIndex - 1));
                }
                else
                {
                    QuickSortParallel(array, left, pivotIndex - 1);
                }

                if ((right - pivotIndex - 1) > threshold)
                {
                    rightTask = Task.Run(() => QuickSortParallel(array, pivotIndex + 1, right));
                }
                else
                {
                    QuickSortParallel(array, pivotIndex + 1, right);
                }

                leftTask?.Wait();
                rightTask?.Wait();
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

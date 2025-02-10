using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        // Input and output file names
        string inputFile = "nombres.txt";
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Input file not found: " + inputFile);
            return;
        }

        // Read all lines from the input file
        string[] lines = File.ReadAllLines(inputFile);
        if (lines.Length == 0)
        {
            Console.WriteLine("The file is empty.");
            return;
        }

        // Sort the lines using parallel quicksort
        ParallelQuickSort(lines, 0, lines.Length - 1);

        // Create the output file name based on input file name
        string outputFile = Path.GetFileNameWithoutExtension(inputFile) + "_tries.txt";
        File.WriteAllLines(outputFile, lines);

        Console.WriteLine("Sorted output written to " + outputFile);
    }

    static void ParallelQuickSort(string[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right);
            Task.WaitAll(
                Task.Run(() => ParallelQuickSort(arr, left, pivotIndex - 1)),
                Task.Run(() => ParallelQuickSort(arr, pivotIndex + 1, right))
            );
        }
    }

    static int Partition(string[] arr, int left, int right)
    {
        string pivot = arr[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (String.Compare(arr[j], pivot, StringComparison.Ordinal) < 0)
            {
                i++;
                Swap(arr, i, j);
            }
        }
        Swap(arr, i + 1, right);
        return i + 1;
    }

    static void Swap(string[] arr, int i, int j)
    {
        string temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}

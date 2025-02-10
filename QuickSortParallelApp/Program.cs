using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace QuickSortParallelApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // Create test file if it doesn't exist
                if (!File.Exists("nombres.txt"))
                {
                    var random = new Random();
                    var randomNumbers = Enumerable.Range(1, 1000).Select(x => random.Next(-10000, 10000)).ToList();
                    await File.WriteAllLinesAsync("nombres.txt", randomNumbers.Select(n => n.ToString()));
                    Console.WriteLine("Created test file 'nombres.txt' with 1000 random numbers");
                }

                // Read numbers from file
                var lines = await File.ReadAllLinesAsync("nombres.txt");
                var numbers = lines.Select(int.Parse).ToArray();

                Console.WriteLine($"Sorting {numbers.Length} numbers using parallel quicksort...");
                var sortedNumbers = await QuickSortParallelAsync(numbers);

                // Write sorted numbers to output file
                string outputFile = Path.GetFileNameWithoutExtension("nombres.txt") + "_tries.txt";
                await File.WriteAllLinesAsync(outputFile, sortedNumbers.Select(n => n.ToString()));
                Console.WriteLine($"Sorted numbers written to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static Task<int[]> QuickSortParallelAsync(int[] arr)
        {
            if (arr == null)
                return Task.FromResult<int[]>(null);
            if (arr.Length <= 1)
                return Task.FromResult(arr);

            var result = arr.ToArray(); // Create a copy to avoid modifying the input array
            return Task.Run(async () =>
            {
                await QuickSortParallelInternalAsync(result, 0, result.Length - 1);
                return result;
            });
        }

        private static async Task QuickSortParallelInternalAsync(int[] arr, int left, int right)
        {
            if (left < right)
            {
                if (right - left < 1000) // For small arrays, use regular quicksort
                {
                    QuickSortSequential(arr, left, right);
                    return;
                }

                int pivot = Partition(arr, left, right);

                // Sort the sub-arrays in parallel
                await Task.WhenAll(
                    QuickSortParallelInternalAsync(arr, left, pivot - 1),
                    QuickSortParallelInternalAsync(arr, pivot + 1, right)
                );
            }
        }

        private static void QuickSortSequential(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);
                QuickSortSequential(arr, left, pivot - 1);
                QuickSortSequential(arr, pivot + 1, right);
            }
        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]); // Swap elements
                }
            }

            (arr[i + 1], arr[right]) = (arr[right], arr[i + 1]); // Place pivot in correct position
            return i + 1;
        }
    }
}

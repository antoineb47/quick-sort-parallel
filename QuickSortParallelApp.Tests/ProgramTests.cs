using System;
using System.Threading.Tasks;
using Xunit;
using QuickSortParallelApp;

namespace QuickSortParallelApp.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task QuickSortParallel_EmptyArray_ReturnsEmptyArray()
        {
            // Arrange
            int[] arr = Array.Empty<int>();

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task QuickSortParallel_SingleElement_ReturnsSameArray()
        {
            // Arrange
            int[] arr = { 1 };

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0]);
        }

        [Fact]
        public async Task QuickSortParallel_UnsortedArray_ReturnsSortedArray()
        {
            // Arrange
            int[] arr = { 64, 34, -25, 12, 22, -11, 90, 0 };
            int[] expected = { -25, -11, 0, 12, 22, 34, 64, 90 };

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task QuickSortParallel_DuplicateElements_ReturnsSortedArray()
        {
            // Arrange
            int[] arr = { 5, 2, 5, 3, 2, 1, 5 };
            int[] expected = { 1, 2, 2, 3, 5, 5, 5 };

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task QuickSortParallel_LargeArray_ReturnsSortedArray()
        {
            // Arrange
            var random = new Random(42); // Use seed for reproducible tests
            int[] arr = new int[10000];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(-10000, 10000);
            }

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            for (int i = 1; i < result.Length; i++)
            {
                Assert.True(result[i - 1] <= result[i], 
                    $"Array not sorted at index {i}: {result[i - 1]} > {result[i]}");
            }
        }

        [Fact]
        public async Task QuickSortParallel_NullArray_ReturnsNull()
        {
            // Arrange
            int[] arr = null;

            // Act
            var result = await Program.QuickSortParallelAsync(arr);

            // Assert
            Assert.Null(result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace QuickSortParallelApp.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void TestQuickSortParallel()
        {
            // Arrange
            int[] unsorted = new int[] { 64, 34, 25, 12, 22, 11, 90 };
            int[] expected = new int[] { 11, 12, 22, 25, 34, 64, 90 };

            // Act
            Program.QuickSortParallel(unsorted, 0, unsorted.Length - 1);

            // Assert
            CollectionAssert.AreEqual(expected, unsorted);
        }

        [TestMethod]
        public void TestPartition()
        {
            // Arrange
            int[] array = new int[] { 64, 34, 25, 12, 22, 11, 90 };
            int left = 0;
            int right = array.Length - 1;

            // Act
            int pivotIndex = Program.Partition(array, left, right);

            // Assert
            Assert.IsTrue(pivotIndex >= left && pivotIndex <= right);
            for (int i = left; i < pivotIndex; i++)
            {
                Assert.IsTrue(array[i] <= array[pivotIndex]);
            }
            for (int i = pivotIndex + 1; i <= right; i++)
            {
                Assert.IsTrue(array[i] >= array[pivotIndex]);
            }
        }

        [TestMethod]
        public void TestSwap()
        {
            // Arrange
            int[] array = new int[] { 1, 2 };
            int expected1 = array[1];
            int expected0 = array[0];

            // Act
            Program.Swap(array, 0, 1);

            // Assert
            Assert.AreEqual(expected1, array[0]);
            Assert.AreEqual(expected0, array[1]);
        }

        [TestMethod]
        public void TestFileGeneration()
        {
            // Arrange
            string testFile = "test_numbers.txt";
            int count = 100;

            // Act
            Program.GenerateTestFile(testFile, count);

            // Assert
            Assert.IsTrue(File.Exists(testFile));
            var lines = File.ReadAllLines(testFile);
            Assert.AreEqual(count, lines.Length);
            Assert.IsTrue(lines.All(line => int.TryParse(line, out _)));

            // Cleanup
            File.Delete(testFile);
        }
    }
}

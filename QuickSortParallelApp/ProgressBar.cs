using System;

namespace QuickSortParallelApp
{
    public class ProgressBar : IDisposable
    {
        private readonly int totalSteps;
        private int currentStep;
        private readonly int barLength;
        private readonly object lockObject = new object();

        public ProgressBar(int totalSteps)
        {
            this.totalSteps = totalSteps;
            this.currentStep = 0;
            this.barLength = 50; // Length of the progress bar
            Console.WriteLine();
        }

        public void Update(int step = 1)
        {
            lock (lockObject)
            {
                currentStep += step;
                if (currentStep > totalSteps)
                    currentStep = totalSteps;

                float percentage = (float)currentStep / totalSteps;
                int filledLength = (int)(barLength * percentage);

                string progressBar = new string('█', filledLength) + new string('░', barLength - filledLength);
                int percent = (int)(percentage * 100);

                Console.Write($"\r[{progressBar}] {percent}% - Processing {currentStep}/{totalSteps}");

                if (currentStep == totalSteps)
                    Console.WriteLine();
            }
        }

        public void Reset()
        {
            lock (lockObject)
            {
                currentStep = 0;
                Console.Write($"\r[{new string('░', barLength)}] 0% - Processing 0/{totalSteps}");
            }
        }

        public void Dispose()
        {
            Console.WriteLine();
        }
    }
}

using System;
using System.Collections.Generic;

namespace Spreadsheet
{
    internal class ProgramLoop
    {
        private readonly Calculating _calculating;
        private readonly DownloadingData _downloadingData;

        public ProgramLoop()
        {
            _calculating = new Calculating();
            _downloadingData = new DownloadingData();
        }

        public void Run()
        {
            Console.WriteLine("Provide numbers:");
            _downloadingData.GetNumbers();
            Console.WriteLine("Provide operation:");
            var operation = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(operation))
            {
                Console.WriteLine("Provide data: ");
                operation = Console.ReadLine();
            }
            var result = _calculating.CalculateOperation(operation);
            Console.WriteLine(result.ToString());
        }    
    }
}

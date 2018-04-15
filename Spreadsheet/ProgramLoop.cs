using System;
using System.Collections.Generic;

namespace Spreadsheet
{
    internal class ProgramLoop
    {
        private readonly Calculating _calculating;
        private readonly DownloadingData _downloadingData;
        public List<Data> Table;

        public ProgramLoop()
        {
            _calculating = new Calculating();
            _downloadingData = new DownloadingData(_calculating);
            Table = new List<Data>();
        }

        public void Run()
        {
            string input;
            Console.WriteLine("Provide numbers:");

            do
            {
                input = Console.ReadLine();
                try
                {
                    var nextRow = _downloadingData.GetNumbers(input, Table);
                    foreach (var item in nextRow)
                    {
                        Table.Add(item);
                    }
                }
                catch
                {
                    Console.WriteLine("Wrong data!");
                }
            } while (input.Length == 0 || input[input.Length - 1] != ';');

            Console.WriteLine("Provide operation:");
            var operation = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(operation))
            {
                Console.WriteLine("Provide data: ");
                operation = Console.ReadLine();
            }
            //try
            //{
            var result = _calculating.CalculateOperation(operation, Table);
            Console.WriteLine(result.ToString());
            //}
            //catch
            //{
            //    Console.WriteLine("Wrong operation!");
            //}
            Console.ReadLine();
        }
    }
}

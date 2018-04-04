using System;
using System.Text;

namespace Spreadsheet
{
    internal class DownloadingData
    {
        private readonly Calculating _calculating;

        public DownloadingData()
        {
            _calculating = new Calculating();
        }

        public void GetNumbers()
        {
            var newItem = new StringBuilder();
            ConsoleKeyInfo key;
            var rowNumber = 'A';
            var columnNumber = 0;
            
            do
            {
                do
                {
                    do
                    {
                        key = Console.ReadKey();
                        newItem.Append(key.KeyChar);
                    } while (key.Key != ConsoleKey.Oem5 && (key.Key != ConsoleKey.Oem1) && (key.Key != ConsoleKey.Enter));
                    newItem.Remove(newItem.Length - 1, 1);
                    columnNumber++;
                    var result = _calculating.CalculateOperation(newItem.ToString());
                    if (result == null)
                    {
                        Console.WriteLine("Incorrect operation or syntax");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    Data.Table.Add(rowNumber + columnNumber.ToString(), Double.Parse(result));
                    newItem.Clear();
                } while (key.Key != ConsoleKey.Enter && (key.Key != ConsoleKey.Oem1));
                var top = Console.CursorTop;
                Console.SetCursorPosition(0, top + 1);
                rowNumber++;
                columnNumber = 0;
            } while (key.Key != ConsoleKey.Oem1);
        }
    }
}

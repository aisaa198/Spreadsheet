using System;
using System.Collections.Generic;

namespace Spreadsheet
{
    public class DownloadingData
    {
        private readonly ICalculating _calculating;
        private char _columnName = 'A';

        public DownloadingData(ICalculating calculating)
        {
            _calculating = calculating;
        }

        public List<Data> GetNumbers(string input, List<Data> table)
        {
            var rowNumber = 1;

            if (input.Length > 0 && input[input.Length - 1] == ';')
            {
                input = input.TrimEnd(';');
            }

            var rowData = input.Split('|');
            var nextRow = new List<Data>();

            foreach (var item in rowData)
            {
                var result = _calculating.CalculateOperation(item.ToString(), table);
                if (Double.TryParse(result, out Double number))
                {
                    nextRow.Add(new Data { Address = _columnName + rowNumber.ToString(), Value = number });
                    rowNumber++;
                }
                else return null;
                
            }

            _columnName++;
            return nextRow;
        }
    }
}

using System.Collections.Generic;

namespace Spreadsheet
{
    public interface ICalculating
    {
        string CalculateOperation(string operation, List<Data> table);
    }
}
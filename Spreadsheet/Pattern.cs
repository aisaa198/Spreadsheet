using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spreadsheet
{
    internal class Pattern
    {
        public Regex CellAddress = new Regex("[A-Z]+[0-9]+");
        public List<char> Numbers = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ',' };
        public List<char> FirstPriority = new List<char> {'*', '/' };
        public List<char> SecondPriority = new List<char> { '+', '-' };
    }
}

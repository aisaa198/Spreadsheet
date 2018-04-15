using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Spreadsheet
{
    public class Calculating : ICalculating
    {
        private Pattern _pattern = new Pattern();
        private List<char> _stack = new List<char>();
        private List<char> _exit = new List<char>();

        public string CalculateOperation(string operation, List<Data> table)
        {
            var transformedOperation = TransformCellAddressToNumber(operation, table);
            var operationInPostfixNotation = TransformToPostfixNotation(transformedOperation);
            var result = Calculate(operationInPostfixNotation);
            _stack.Clear();
            _exit.Clear();

            return result;
        }

        public string TransformCellAddressToNumber(string input, List<Data> table)
        {
            var transformedInput = new StringBuilder(input);
            var matches = _pattern.CellAddress.Matches(input);

            foreach (Match match in matches)
            {
                var tableValue = table.Single(x => x.Address == match.Value).Value;
                transformedInput.Replace(match.Value, tableValue.ToString());
            }

            return transformedInput.ToString();
        }

        public List<string> TransformToPostfixNotation(string operation)
        {
            var splitOperation = operation.ToCharArray();

            for (var i = 0; i < splitOperation.Length; i++)
            {
                switch (splitOperation[i])
                {
                    case var testChar when _pattern.Numbers.Contains(testChar):
                        _exit.Add(splitOperation[i]);
                        break;
                    case '(':
                        _stack.Add(splitOperation[i]);
                        break;
                    case ')':
                        for (var j = _stack.Count - 1; j >= 0; j--)
                        {
                            if (_stack[j] != '(')
                            {
                                AddToExit(_stack[j], j);
                            }
                            else
                            {
                                _stack.RemoveAt(j);
                                break;
                            }
                        }
                        break;
                    case var testChar when _pattern.FirstPriority.Contains(testChar):
                        _exit.Add(' ');
                        while (_stack.Count > 0 && _pattern.FirstPriority.Contains(_stack[_stack.Count - 1]))
                        {
                            AddToExit(_stack[_stack.Count - 1], _stack.Count - 1);
                        }
                        _stack.Add(splitOperation[i]);
                        break;
                    case var testChar when _pattern.SecondPriority.Contains(testChar):
                        _exit.Add(' ');
                        while (_stack.Count > 0 && (_pattern.FirstPriority.Contains(_stack[_stack.Count - 1]) || _pattern.SecondPriority.Contains(_stack[_stack.Count - 1])))
                        {
                            AddToExit(_stack[_stack.Count - 1], _stack.Count - 1);
                        }
                        _stack.Add(splitOperation[i]);
                        break;
                    default:
                        return null;
                }
            }

            while(_stack.Count > 0)
            {
                AddToExit(_stack[_stack.Count - 1], _stack.Count - 1);
            }
            var result = String.Join("", _exit.ToArray()).Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            return (result.Contains("(") || result.Contains(")"))? null : result;
        }

        public void AddToExit(char x, int index = -1)
        {
            _exit.Add(' ');
            _exit.Add(x);
            _exit.Add(' ');

            if (index >= 0)
            {
                _stack.RemoveAt(index);
            }
        }

        public string Calculate(List<string> operation)
        {
            if (operation == null || operation.Count == 0) return null;
            
            var stack = new List<string>();
            double result;

            foreach (var item in operation)
            {
                if (item.ToCharArray().All(x => _pattern.Numbers.Contains(x)))
                {
                    stack.Add(item);
                }
                else if (stack.Count > 1 && (_pattern.FirstPriority.Contains(Char.Parse(item)) || _pattern.SecondPriority.Contains(Char.Parse(item))))
                {
                    var b = Double.Parse(stack[stack.Count - 1]);
                    var a = Double.Parse(stack[stack.Count - 2]);

                    stack.RemoveRange(stack.Count - 2, 2);

                    switch (item)
                    {
                        case "+":
                            result = a + b;
                            break;
                        case "-":
                            result = a - b;
                            break;
                        case "*":
                            result = a * b;
                            break;
                        case "/":
                            result = a / b;
                            break;
                        default:
                            result = 0;
                            break;
                    }
                    stack.Add(result.ToString());
                }
                else return null;
            }

            if (stack.Count != 1 || stack[0] == "") return null;
            return stack[0].ToString();
        }
    }
}

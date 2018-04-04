using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Spreadsheet
{
    public class Calculating
    {
        public string CalculateOperation(string operation)
        {
            var transformedOperation = TransformCellAddressToNumber(operation, Data.Table);
            var operationInPostfixNotation = TransformToPostfixNotation(transformedOperation);
            var result = Calculate(operationInPostfixNotation);
            return result;
        }

        private string TransformCellAddressToNumber(string input, Dictionary<string, double> dataTable)
        {
            var x = new Regex("[A-Z]+[0-9]+");
            var transformedInput = new StringBuilder(input);

            var matches = x.Matches(input);
            foreach (Match match in matches)
            {
                if (dataTable.ContainsKey(match.Value))
                {
                    transformedInput.Replace(match.Value, dataTable[match.Value].ToString());
                }
                else
                {
                    Console.WriteLine($"{match.Value} is not found.");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            return transformedInput.ToString();
        }

        private string TransformToPostfixNotation(string operation)
        {
            var stack = new List<char>();
            var exit = new List<char>();
            var digit = new Regex("[0-9]");
            var firstPriority = new Regex("[*,/]");
            var secondPriority = new Regex("[+,-]");
            var splitOperation = operation.ToCharArray();

            for (var i = 0; i < splitOperation.Length; i++)
            {
                if (digit.IsMatch(splitOperation[i].ToString()) || splitOperation[i] == ',')
                {
                    exit.Add(splitOperation[i]);
                }
                else if (splitOperation[i] == '(')
                {
                    stack.Add(splitOperation[i]);
                }
                else if (splitOperation[i] == ')')
                {
                    for (var j = stack.Count - 1; j >= 0; j--)
                    {
                        if (stack[j] != '(')
                        {
                            exit.Add(' ');
                            exit.Add(stack[j]);
                            exit.Add(' ');
                            stack.RemoveAt(j);
                        }
                        else
                        {
                            stack.RemoveAt(j);
                            break;
                        }
                    }
                }
                else if (splitOperation[i] == '+' || splitOperation[i] == '-' || splitOperation[i] == '*' || splitOperation[i] == '/')
                {
                    exit.Add(' ');
                    while (stack.Count > 0 && secondPriority.IsMatch(splitOperation[i].ToString()) && ((firstPriority.IsMatch(stack[stack.Count - 1].ToString()) || secondPriority.IsMatch(stack[stack.Count - 1].ToString()))))
                    {
                        exit.Add(stack[stack.Count - 1]);
                        exit.Add(' ');
                        stack.RemoveAt(stack.Count - 1);
                    }
                    while (stack.Count > 0 && firstPriority.IsMatch(splitOperation[i].ToString()) && firstPriority.IsMatch(stack[stack.Count - 1].ToString()))
                    {
                        exit.Add(stack[stack.Count - 1]);
                        exit.Add(' ');
                        stack.RemoveAt(stack.Count - 1);
                    }

                    stack.Add(splitOperation[i]);
                }
                else
                {
                    var top = Console.CursorTop;
                    Console.SetCursorPosition(0, top + 1);
                    Console.WriteLine("Incorrect operation or syntax");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }

            if (stack.Count > 0)
            {
                for (var k = stack.Count - 1; k >= 0; k--)
                {
                    exit.Add(' ');
                    exit.Add(stack[k]);
                    exit.Add(' ');
                    stack.RemoveAt(k);
                }
            }

            return String.Join("", exit.ToArray());
        }

        public string Calculate(string operation)
        {
            var splitOperation = operation.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var stack = new List<string>();
            var digit = new Regex("[0-9]");
            double result;
            
            foreach (var item in splitOperation)
            {
                if (digit.IsMatch(item))
                {
                    stack.Add(item);
                }
                else
                {
                    var b = Double.Parse(stack[stack.Count - 1]);
                    var a = Double.Parse(stack[stack.Count - 2]);

                    stack.RemoveRange(stack.Count - 2, 2);

                    switch (item)
                    {
                        case "+":
                            result = Add(a, b);
                            break;
                        case "-":
                            result = Subtract(a, b);
                            break;
                        case "*":
                            result = Multiply(a, b);
                            break;
                        case "/":
                            result = Divide(a, b);
                            break;
                        default:
                            result = 0;
                            break;
                    }
                    stack.Add(result.ToString());
                }
            }

            if (stack.Count == 0) return null;

            return stack[stack.Count - 1].ToString();
        }

        private double Add(double a, double b)
        {
            return a + b;
        }

        private double Subtract(double a, double b)
        {
            return a - b;
        }

        private double Multiply(double a, double b)
        {
            return a * b;
        }

        private double Divide(double a, double b)
        {
            return a / b;
        }
    }
}

using System;
using System.Collections.Generic;
using FieldsCalculator;

namespace PostfixNotation
{
    public class MathPostfixNotation
    {
        static int mod(int x, int m, bool log = true)
        {
            var r = (x % m + m) % m;
            if (log) Console.WriteLine($"{x} mod {m} = {r}");
            return r;
        }

        static public int Calculate(string input, int size)
        {

            string output = GetExpression(input);
            Console.WriteLine(output);
            return Counting(output.Replace('.', ','), size);

        }

        public static (int g, int x, int y) egcd(int a,  int b) 
        {
            if (a == 0)
                return (b, 0, 1);
            else 
            {
                var data = egcd(mod(b, a, log:false), a);
                return (data.g, data.y - (b / (int)a) * data.x, data.x);
            }
        }

        public static int mulinv(int b, int n) => mod(egcd(b, n).x, n);

        static public string GetExpression(string input)
        {
            string output = string.Empty;
            Stack<char> operStack = new Stack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (IsDelimeter(input[i]))
                    continue;

                if (input[i] == '-' && ((i > 0 && !Char.IsDigit(input[i - 1])) || i == 0))
                {
                    i++;
                    output += "-";  
                }

                if (Char.IsDigit(input[i]))
                {
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i];
                        i++;

                        if (i == input.Length) break;
                    }

                    output += " ";
                    i--;
                }

                if (IsOperator(input[i]))
                {
                    if (input[i] == '(')
                        operStack.Push(input[i]);
                    else if (input[i] == ')')
                    {
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';

                            s = operStack.Pop();
                        }
                    }
                    else
                    {
                        if (operStack.Count > 0)
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                output += operStack.Pop().ToString() + " ";

                        operStack.Push(char.Parse(input[i].ToString()));

                    }
                }
            }

            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output;

        }

        static private int Counting(string output, int size)
        {
            string result;

            string[] mas = output.Split(' ');

            for (int i = 0; i < mas.Length; i++) 

                switch (mas[i])
                {
                    case "+":
                        result = (mod((mod(int.Parse(mas[i - 2]), size) + mod(int.Parse(mas[i - 1]), size)), size)).ToString();

                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;


                    case "-":
                        result = (mod((mod(int.Parse(mas[i - 2]), size) - mod(int.Parse(mas[i - 1]), size)), size)).ToString();

                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;

                    case "*":
                        result = (mod((mod(int.Parse(mas[i - 2]), size) * mod(int.Parse(mas[i - 1]), size)), size)).ToString();

                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;

                    case "/":
                        if (int.Parse(mas[i - 1]) == 0) throw new Exception("Ошибка! Обнаружено деление на 0");
                        result = (mod(int.Parse(mas[i - 2]), size) * mulinv(mod(int.Parse(mas[i - 1]), size), size)).ToString();

                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;
                }

            int a = 0;
            bool parsed = int.TryParse(mas[0], out a);
            return a;
        }

        static private byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                default: return 5;
            }
        }

        static private bool IsOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        static private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }
    }
}

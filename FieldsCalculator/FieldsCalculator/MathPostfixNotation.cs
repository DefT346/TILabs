using System;
using System.Collections.Generic;
using FieldsCalculator;

namespace PostfixNotation
{
    //Класс рассчёта математического выражения
    public class MathPostfixNotation
    {
        static int mod(int x, int m, bool log = true)
        {
            var r = (x % m + m) % m;
            if (log) Console.WriteLine($"{x} mod {m} = {r}");
            return r;
        }

        //"входной" метод класса
        static public int Calculate(string input, Field field)
        {

            string output = GetExpression(input); //преобразование выражения в постфиксную запись
            Console.WriteLine(output);
            return Counting(output.Replace('.', ','), field); //решение выражения

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

        public static int mulinv(int b, int n) {
            var data = egcd(b, n);
            //Console.WriteLine($"{data.g},{data.x}");
            //if (data.g != 1)
            //    throw new Exception($"Ошибка поиска обратного числа для {b} в поле {n}");
            return mod(data.x, n);

        }

        //метод перевода выражения в постфиксную запись
        static public string GetExpression(string input)
        {
            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {

                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //проверка на отрицательное число: если знак "-" в начале строки или перед знаком "-" нет числа 
                if (input[i] == '-' && ((i > 0 && !Char.IsDigit(input[i - 1])) || i == 0))
                {
                    i++;
                    output += "-";//в переменную для чисел добавляется знак "-"    
                }

                //Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i])) //Если оператор
                {

                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';

                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи

        }

        //метод решения OPN
        static private int Counting(string output, Field field)
        {
            string result;

            string[] mas = output.Split(' ');

            for (int i = 0; i < mas.Length; i++) 

                switch (mas[i])
                {
                    case "+"://если найдена операция сложения
                        result = (mod((mod(int.Parse(mas[i - 2]), field.size) + mod(int.Parse(mas[i - 1]), field.size)), field.size)).ToString();//выполняем сложение и переводим ее в строку



                        mas[i - 2] = result;//на место 1-ого операнда записывается результат (как если бы a=a+b)
                        for (int j = i - 1; j < mas.Length - 2; j++)//удаляем из массива второй операнд и знак арифм действия
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);//обрезаем массив элементов на 2 удаленнх элемента
                        i -= 2;
                        break;


                    case "-"://далее все аналогично
                        result = (mod((mod(int.Parse(mas[i - 2]), field.size) - mod(int.Parse(mas[i - 1]), field.size)), field.size)).ToString();



                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;

                    case "*":
                        result = (mod((mod(int.Parse(mas[i - 2]), field.size) * mod(int.Parse(mas[i - 1]), field.size)), field.size)).ToString();



                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;

                    case "/":
                        //result = (int.Parse(mas[i - 2]) / int.Parse(mas[i - 1])).ToString();
                        result = (mod(int.Parse(mas[i - 2]), field.size) * mulinv(mod(int.Parse(mas[i - 1]), field.size), field.size)).ToString();


                        mas[i - 2] = result;
                        for (int j = i - 1; j < mas.Length - 2; j++)
                            mas[j] = mas[j + 2];
                        Array.Resize(ref mas, mas.Length - 2);
                        i -= 2;
                        break;


                    //case "^":
                    //    result = (mod((int)Math.Pow(mod(int.Parse(mas[i - 2]), field.size), mod(int.Parse(mas[i - 1]), field.size)), field.size)).ToString();



                    //    mas[i - 2] = result;
                    //    for (int j = i - 1; j < mas.Length - 2; j++)
                    //        mas[j] = mas[j + 2];
                    //    Array.Resize(ref mas, mas.Length - 2);
                    //    i -= 2;
                    //    break;


                }

            int a = 0;
            bool parsed = int.TryParse(mas[0], out a);
            return a;
        }

        //Метод возвращает приоритет оператора
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
                case '^': return 5;
                default: return 6;
            }
        }

        //Метод возвращает true, если проверяемый символ - оператор
        static private bool IsOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
        static private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FieldsCalculator
{
    public class ResultData
    {
        public int result;
        public string calculationLog;
    }

    public class Field
    {
        private int _size = -1;
        public int size
        {
            get
            {
                return _size;
            }
        } 
        public Field(int size)
        {
            this._size = size;
        }

        public int Reverse(int main, int div)
        {
            return 0;
        }

        public int Reverse(int num)
        {
            return 0;
        }

        public int Summ(int a, int b) => (a + b) % size;

        public int Sub(int a, int b) => (a - b) % size;

        public int Multi(int a, int b) => (a * b) % size;


        public ResultData CalculateExpression(string expression)
        {


            return new ResultData { result = 1, calculationLog = "" };
        }
    }
}

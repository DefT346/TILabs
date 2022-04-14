using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coding
{
    public class Polinom
    {
        public List<int> extents { get; protected set; }
        public Polinom(string code)
        {
            extents = new List<int>();
            for (int i = 0; i < code.Length; i++)
            {
                if(code[i] == '1')
                {
                    extents.Add(i);
                }
            }
        }

        public Polinom(BitArray bitArray)
        {
            extents = new List<int>();
            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    extents.Add(i);
                }
            }
        }

        protected Polinom(List<int> extents)
        {
            this.extents = extents;
        }

        public int GetMaxExtend()
        {
            //if ()
            return extents.Max();
        }

        public static Polinom operator *(Polinom a, Polinom b)
        {
            List<int> resultExtents = new List<int>();
            List<int> exeptions = new List<int>();
            foreach(var ela in a.extents)
            {
                foreach (var elb in b.extents)
                {
                    int p = ela + elb;
                    if (resultExtents.Contains(p)) exeptions.Add(p);

                    resultExtents.Add(p);
                }
            }

            foreach (var el in exeptions)
            {
                resultExtents.RemoveAll(p => p == el);
            }
            return new Polinom(resultExtents);
        }

        public override string ToString()
        {
            string result = "";
            foreach (var el in extents)
            {
                result += $"[{el}] ";
            }
            return result;
        }
    }
}

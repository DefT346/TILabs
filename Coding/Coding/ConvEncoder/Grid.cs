using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Coding
{
    public class Grid
    {
        string[] summators; /* 101, 111, 010, 110 и т. д.*/
        public Grid(string[] summators)
        {
            this.summators = summators;
        }


        public List<bool> RunGeneration(Point start, BitArray code, int steps, int summatorsCount, bool writeLines = false)
        {
            //var info = "";
            var bits = new List<bool>();
            Generation(start, steps);
            return bits;

            void Generation(Point startPoint, int maxStep)
            {
                Point tempPoint = startPoint;

                for (int s = 0; s < maxStep; s++)
                {
                    tempPoint.value0 = CompareSummator(tempPoint.register, 0, out string newReg0);
                    tempPoint.value1 = CompareSummator(tempPoint.register, 1, out string newReg1);
                    tempPoint.link0 = new Point(newReg0);
                    tempPoint.link1 = new Point(newReg1);
                    if (writeLines)
                    {
                        GridDrawer.AddLine(tempPoint.step, tempPoint.register, newReg0, tempPoint.value0, Color.Red, 1);
                        GridDrawer.AddLine(tempPoint.step, tempPoint.register, newReg1, tempPoint.value1, Color.Green, 1);
                    }
                    tempPoint.link0.step = tempPoint.step + 1;
                    tempPoint.link1.step = tempPoint.step + 1;

                    if (tempPoint.step >= steps) return;
                    //if (code[startPoint.step] == "") return;

                    var value = "";
                    for (int i = tempPoint.step * summatorsCount; i < tempPoint.step * summatorsCount + summatorsCount; i++)
                    {
                        value += code[i] == true ? '1' : '0';
                    }


                    var w0 = HWeight(tempPoint.value0, value);
                    var w1 = HWeight(tempPoint.value1, value);


                    if (w0 == 0)
                    {
                        //info += "0";
                        bits.Add(false);
                        GridDrawer.AddLine(tempPoint.step, tempPoint.register, tempPoint.link0.register, "", Color.Blue, 2);
                        tempPoint = tempPoint.link0;
                        //Generation(tempPoint.link0, maxStep);
                    }
                    else if (w1 == 0)
                    {
                        //info += "1";
                        bits.Add(true);
                        GridDrawer.AddLine(tempPoint.step, tempPoint.register, tempPoint.link1.register, "", Color.Blue, 2);
                        tempPoint = tempPoint.link1;
                        //Generation(tempPoint.link1, maxStep);
                    }
                }

                //if (startPoint.step > maxStep) return;
                
            }
        }

        //public List<bool> RunGeneration(Point start, BitArray code, int steps, int summatorsCount, bool writeLines = false)
        //{
        //    //var info = "";
        //    var bits = new List<bool>();
        //    Generation(start, steps);
        //    return bits;

        //    void Generation(Point startPoint, int maxStep)
        //    {
        //        if (startPoint.step > maxStep) return;
        //        startPoint.value0 = CompareSummator(startPoint.register, 0, out string newReg0);
        //        startPoint.value1 = CompareSummator(startPoint.register, 1, out string newReg1);
        //        startPoint.link0 = new Point(newReg0);
        //        startPoint.link1 = new Point(newReg1);
        //        if (writeLines)
        //        {
        //            GridDrawer.AddLine(startPoint.step, startPoint.register, newReg0, startPoint.value0, Color.Red, 1);
        //            GridDrawer.AddLine(startPoint.step, startPoint.register, newReg1, startPoint.value1, Color.Green, 1);
        //        }
        //        startPoint.link0.step = startPoint.step + 1;
        //        startPoint.link1.step = startPoint.step + 1;

        //        if (startPoint.step >= steps) return;
        //        //if (code[startPoint.step] == "") return;

        //        var value = "";
        //        for (int i = startPoint.step * summatorsCount; i < startPoint.step * summatorsCount + summatorsCount; i++)
        //        {
        //            value += code[i] == true ? '1' : '0';
        //        }


        //        var w0 = HWeight(startPoint.value0, value);
        //        var w1 = HWeight(startPoint.value1, value);


        //        if (w0 == 0)
        //        {
        //            //info += "0";
        //            bits.Add(false);
        //            GridDrawer.AddLine(startPoint.step, startPoint.register, startPoint.link0.register, "", Color.Blue, 2);
        //            Generation(startPoint.link0, maxStep);
        //        }
        //        else if (w1 == 0)
        //        {
        //            //info += "1";
        //            bits.Add(true);
        //            GridDrawer.AddLine(startPoint.step, startPoint.register, startPoint.link1.register, "", Color.Blue, 2);
        //            Generation(startPoint.link1, maxStep);
        //        }
        //    }
        //}


        public static int HWeight(string a, string b)
        {
            int w = 0;
            for(int i =0; i< a.Length; i++)
            {
                if (i >= b.Length)
                {
                    w = a.Length - b.Length;
                    break;
                }
                if (a[i] != b[i]) w++;
            }
            return w;
        }
        //public string RegAdd(string reg, object bit) => bit.ToString() + reg.Substring(0, reg.Length - 1);

        public string CompareSummator(string reg, object bit, out string newReg)
        {
            string result = "";
            string register = bit.ToString() + reg.Substring(0, reg.Length);
            newReg = register.Substring(0, register.Length - 1);
            foreach (var summator in summators)
            {
                byte xorSum = 0;
                for(int i = 0; i < register.Length; i++)
                {
                    if (summator[i] == '1')
                        xorSum ^= byte.Parse(register[i].ToString()); 
                }
                result += xorSum;
            }
            return result;
        }

    }

    public class Point
    {
        public string register;
        public Point link0;
        public Point link1;
        public string value0;
        public string value1;
        public int step = 0;

        public Point(string register)
        {
            this.register = register;
        }
    }

    public class R3Map<T> : IEnumerable
    {
        public R3Map()
        {
            this.Values = new Dictionary<(int x, int y), T>();
        }

        public Dictionary<(int x, int y), T> GetDict()
        {
            return this.Values;
        }

        public Dictionary<(int x, int y), T> Values
        {
            get;
            set;
        }

        public T this[int x, int y]
        {
            get
            {
                return Values.ContainsKey((x, y)) ? this.Values[(x, y)] : default(T);
            }

            set
            {
                if (IsNull(value))
                    this.Values.Remove((x, y));
                else
                    this.Values[(x, y)] = value;
            }
        }

        private bool IsNull(T value)
        {
            if (value is ValueType)
            {
                return false;
            }
            return null == (object)value;
        }

        public void Clear()
        {
            this.Values = new Dictionary<(int x, int y), T>();
        }

        public int Count()
        {
            return this.Values.Count;
        }

        public void DirectRemove(int x, int y)
        {
            this.Values.Remove((x, y));
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Values).GetEnumerator();
        }

    }
}

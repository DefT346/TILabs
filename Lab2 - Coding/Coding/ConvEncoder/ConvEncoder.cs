using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Coding
{
    public static class ConvEncoder
    {
        public static byte[] Encode(string text, params string[] summators) => Encode(Encoding.UTF8.GetBytes(text), summators);

        public static byte[] Encode(byte[] data, params string[] summators)
        {
            if (data.Length == 0) return new byte[0];
            BitArray info = new BitArray(data);
            return EncodeInfo(info, summators);
        }

        public static byte[] EncodeInfo(string info, params string[] summators)
        {
            bool[] arr = new bool[info.Length];
            for (int i = 0; i < info.Length; i++)
                arr[i] = info[i] == '1' ? true : false;
            return EncodeInfo(new BitArray(arr), summators);
        }

        public static byte[] EncodeInfo(BitArray info, params string[] summators)
        {
            var i = new Polinom(info);

            List<Polinom> polinoms = new List<Polinom>();

            foreach (var summator in summators)
            {
                var newPolinom = new Polinom(summator);
                if (newPolinom.extents.Count == 0) throw new Exception("Ошибка кодирования: обнаружен нулевой сумматор");
                polinoms.Add(i * newPolinom);
            }

            return BitArrayToByteArray(CompareCode(polinoms.ToArray()));
        }

        public static string DecodeText(byte[] code, params string[] summators) => System.Text.Encoding.UTF8.GetString(Decode(code, summators));

        public static byte[] Decode(byte[] code, params string[] summators)
        {
            if (code.Length == 0) return code;
            GridDrawer.Clear();
            //var data = code.Split(' ');

            int summatorsCount = summators.Length;

            BitArray data = new BitArray(code);

            int steps = data.Length / summatorsCount;

            string inputReg = "";
            for (int i = 0; i < summators[0].Length - 1; i++) inputReg += "0";

            var startPoint = new Point(inputReg);

            Grid grid = new Grid(summators);
            var newData = grid.RunGeneration(startPoint, data, steps, summatorsCount, writeLines: true);

            GridDrawer.Draw(steps, false);

            return BitArrayToByteArray(new BitArray(newData.ToArray()));
        }

        private static BitArray CompareCode(params Polinom[] polinoms)
        {
            int max = 0;
            foreach (var p in polinoms)
            {
                int m = p.GetMaxExtend();
                if (m > max) max = m;
            }

            string rawCode = "";
            List<bool> code = new List<bool>();

            for (int i = 0; i < max + 1; i++)
            {
                for (int r = 0; r < polinoms.Length; r++)
                {
                    if (polinoms[r].extents.Contains(i))
                    {
                        rawCode += "1";
                        code.Add(true);

                        //code[1]
                    }
                    else
                    {
                        rawCode += "0";
                        code.Add(false);
                    }
                }
                rawCode += " ";

            }
            Console.WriteLine($"Raw encoded code: {rawCode}");
            return new BitArray(code.ToArray());
        }

        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

    }
}

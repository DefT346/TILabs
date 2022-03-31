using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sumMatrix1.SetSummators(new string[] { "0001", "0110", "1001", "1010", "0010", "0110" });

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = "Обработка...";
            textBox2.Update();

            var summators = sumMatrix1.GetSummators();
            if (checkBox1.Checked) GridDrawer.Init(pictureBox1, summators[0].Length - 1);

            try
            {
                Console.WriteLine($"Исходное сообщение: {Encoding.UTF8.GetBytes(textBox1.Text).Length} байт");
                var encoded = ConvEncoder.Encode(textBox1.Text, summators);
                Console.WriteLine($"Закодированное сообщение: {encoded.Length} байт");
                textBox2.Text = ConvEncoder.DecodeText(encoded, summators);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);000
                textBox2.Text = ex.Message;
            }

            GridDrawer.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = new string[] { "011", "001", "011" };
            GridDrawer.Init(pictureBox1, s[0].Length);

            var w = ConvEncoder.EncodeInfo("01101", s);
            var d = ConvEncoder.Decode(w, s);
            var b = new BitArray(d);
            
            Console.WriteLine(b.ToBitString());
        }


    }

    public static class Extension
    {
        public static string ToBitString(this BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}

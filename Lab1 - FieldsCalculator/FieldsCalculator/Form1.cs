using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostfixNotation;

namespace FieldsCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private bool isMousePress = false;
        private Point _clickPoint;
        private Point _formStartPoint;


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePress = true;
            _clickPoint = Cursor.Position;
            _formStartPoint = Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePress)
            {
                var cursorOffsetPoint = new Point( //считаем смещение курсора от старта
                    Cursor.Position.X - _clickPoint.X,
                    Cursor.Position.Y - _clickPoint.Y);

                Location = new Point( //смещаем форму от начальной позиции в соответствии со смещением курсора
                    _formStartPoint.X + cursorOffsetPoint.X,
                    _formStartPoint.Y + cursorOffsetPoint.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePress = false;
            _clickPoint = Point.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int fieldsize = int.Parse(textBox2.Text);
                if (!validateSimple(fieldsize)) throw new Exception("Введённый размер поля не является простым числом");
                var result = MathPostfixNotation.Calculate(textBox1.Text, fieldsize) % fieldsize;
                label1.Text = result.ToString();
            }
            catch(Exception ex)
            {
                label1.Text = ex.Message;
            }
            //label1.Text = MathPostfixNotation.mulinv(3,13).ToString();
        }

        private static bool validateSimple(int value)
        {
            for (int i = 2; i <= value / 2; i++)
            {
                if (value % i == 0)
                {
                    return false;
                }
            }
            return true;

        }
    }
}

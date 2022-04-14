using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Coding
{
    public partial class SummatorElement : UserControl
    {
        //private int elementsCount = 0;
        public delegate void Delete(SummatorElement i);
        public Delete deleteAction;
        int _id = 0;
        public int id { get { return _id; }  set { _id = value; label1.Text = value.ToString();  } }

        public SummatorElement()
        {
            InitializeComponent();
            //temp.Add(bigCheckBox1);
            //GenerateElements(5);
        }


        public string GetValues()
        {
            string result = "";
            foreach(var el in temp)
            {
                result += el.Checked ? 1 : 0;
            }
            return result;
        }
        //public SummatorElement(int id, int registerSize)
        //{
        //    InitializeComponent();
        //    this.id = id;
        //    GenerateElements(registerSize);
        //}
        public List<UI.BigCheckBox> temp = new List<UI.BigCheckBox>();
        public void GenerateElements(int count)
        {
            int maxid = 0;
            for (int i = 0; i < temp.Count; i++)
            {
                if (i > temp.Count) continue;
                if (temp[i] == null) continue;
                if (temp[i].id > maxid) maxid = temp[i].id;
                if (temp[i].id != 0 && temp[i].id > count)
                {
                    this.Controls.Remove(temp[i]);
                    temp.Remove(temp[i]);
                }
            }

            var elementExample = bigCheckBox1;
            this.Controls.Remove(bigCheckBox1);
            elementExample.id = 0;
            for (int i = maxid; i < count; i++)
            {
                var element = new UI.BigCheckBox();
                element.Width = elementExample.Width;
                element.Height = elementExample.Height;
                element.Text = elementExample.Text;
                element.Checked = false;
                element.Location = new System.Drawing.Point(elementExample.Location.X + (elementExample.Width + elementExample.Width / 2) * i, elementExample.Location.Y);
                element.id = i + 1;
                this.Controls.Add(element);
                temp.Add(element);
            }
            int x = elementExample.Location.X + (elementExample.Width + elementExample.Width / 2) * count;
            button1.Location = new System.Drawing.Point(x, elementExample.Location.Y + elementExample.Height / 2 - button1.Height / 2);

            this.Width = elementExample.Location.X + (elementExample.Width + elementExample.Width / 2) * (count) + button1.Width + elementExample.Width / 2;
        }

        public void UpdateElements()
        {
            string row = "";
            foreach(var v in temp)
            {
                v.Refresh();
                row += $"{(v.Checked ? 1 : 0)};";
            }
            Debug.WriteLine(row);
        }

        private void bigCheckBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            deleteAction?.Invoke(this);
        }
    }
}

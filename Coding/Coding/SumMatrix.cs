using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Coding
{
    public partial class SumMatrix : UserControl
    {
        int regCount = 5;
        public SumMatrix()
        {
            InitializeComponent();
            // Инициализация шаблона
            summatorElement1.GenerateElements(regCount);
            summatorElement1.deleteAction += Delete;
            summators.Add(summatorElement1);
            // Предгенерация
            Render(regCount);
        }

        
        public void SetSummators(string[] load)
        {
            regCount = load[0].Length;
            for (int i = 0; i < load.Length; i++)
            {
                SetSummator(i, load[i]);
            }
        }

        public string[] GetSummators()
        {
            List<string> sums = new List<string>();
            foreach(var summ in summators)
                sums.Add(summ.GetValues());
            return sums.ToArray();
        }

        List<SummatorElement> summators = new List<SummatorElement>();

        private void Delete(SummatorElement el)
        {
            if (summators.Count < 2) return;

            this.Controls.Remove(el);
            summators.Remove(el);
            Render(regCount);
            UpdateAddButtonPosition();
        }

        private void RegenerateElements(int count)
        {
            for(int i =0; i< summators.Count; i++)
            {
                summators[i].GenerateElements(count);
                if (i - 1 > -1)
                {
                    summators[i].id = summators[i - 1].id + 1;
                    summators[i].Location = new System.Drawing.Point(summators[i - 1].Location.X, summators[i - 1].Location.Y + summators[i - 1].Height);
                }
            }
        }

        List<Label> temp = new List<Label>();
        public void Render(int cols)
        {

            RegenerateElements(cols);

            foreach (Label lbl in temp)
            {
                this.Controls.Remove(lbl);
            }

            
            var numsExample = label2;
            for (int i = 1; i < cols; i++)
            {
                var newLabel = new Label();
                newLabel.Font = numsExample.Font;
                newLabel.Width = numsExample.Width;
                newLabel.Height = numsExample.Height;
                newLabel.Text = i.ToString();
                newLabel.Location = new System.Drawing.Point(summatorElement1.Location.X + summatorElement1.temp[i].Location.X + summatorElement1.temp[i].Width / 3, numsExample.Location.Y);
                newLabel.BringToFront();

                this.Controls.Add(newLabel);
                temp.Add(newLabel);
                newLabel.Refresh();
            }
            this.Update();

            panel1.Location = new System.Drawing.Point(summatorElement1.Location.X + summatorElement1.temp[0].Location.X, panel1.Location.Y);
            panel1.Width = 3 * summatorElement1.temp[0].Width / 2 * cols - summatorElement1.temp[0].Width / 2;

            button2.Location = new System.Drawing.Point(panel1.Location.X + panel1.Width - button2.Width, button2.Location.Y);
            button1.Location = new System.Drawing.Point(panel1.Location.X, button2.Location.Y);

            label1.Location = new System.Drawing.Point(panel1.Location.X + panel1.Width / 2 - label1.Width /2, label1.Location.Y);

            button3.Width = panel1.Width;
            button3.Location = new System.Drawing.Point(panel1.Location.X, button3.Location.Y);
        }

        public void AddNewSummator()
        {
            SetSummator(summators[summators.Count - 1].id + 1);
        }

        public void SetSummator(int id, string values = "")
        {
            var summator = summators.Find(p => p.id == id);
            if (summator != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    summator.temp[i].Checked = values[i] == '1' ? true : false;
                }
            }
            else
            {
                var newSummator = new SummatorElement();
                newSummator.id = id;
                newSummator.Location = new System.Drawing.Point(summators[summators.Count - 1].Location.X, GetLastSummatorPos());
                newSummator.GenerateElements(regCount);
                newSummator.deleteAction += Delete;
                summators.Insert(summators.Count, newSummator);

                for (int i = 0; i< values.Length; i++)
                {
                    newSummator.temp[i].Checked = values[i] == '1' ? true : false;
                }

                this.Controls.Add(newSummator);
                UpdateAddButtonPosition();
                newSummator.UpdateElements();
            }

            Render(regCount);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Generate(5);
            AddNewSummator();
        }

        private void UpdateAddButtonPosition()
        {
            button3.Location = new System.Drawing.Point(button3.Location.X, GetLastSummatorPos());
        }


        private int GetLastSummatorPos()
        {
            return summators[summators.Count - 1].Location.Y + summators[summators.Count - 1].Height;
        }
        private void SumMatrix_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            regCount++;
            Render(regCount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (regCount < 3) return;
            regCount--;
            Render(regCount);

        }
    }
}

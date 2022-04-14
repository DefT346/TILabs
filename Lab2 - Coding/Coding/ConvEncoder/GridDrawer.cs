using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public static class GridDrawer
{
    private static List<Line> drawLines;
    private static List<string> regs;
    private static Control canvas;
    private static int registerSize;
    public static bool initialized { get; private set; } = false; 

    public class Line
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public string value;
        public Color color;
        public int width;

        public Line(int x1, int y1, int x2, int y2, string value, Color color, int width)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.value = value;
            this.color = color;
            this.width = width;
        }
    }

    public static void Init(Control canvas, int registerSize)
    {
        Clear();
        regs = new List<string>();
        GridDrawer.canvas = canvas;
        regs = RunReqTable(registerSize);
        GridDrawer.registerSize = registerSize;
        initialized = true;
    }

    public static void AddLine(int generation, string reg1, string reg2, string value, Color color, int width)
    {
        if (!initialized) return;

        var y1 = regs.IndexOf(reg1);
        var y2 = regs.IndexOf(reg2);
        var x1 = generation;
        var x2 = generation + 1;
        drawLines.Add(new Line(x1, y1, x2, y2, value, color, width));
    }

    public static void Clear()
    {
        drawLines = new List<Line>();
    }

    public static void Dispose()
    {
        if (!initialized) return;

        initialized = false;
        drawLines.Clear(); drawLines = null;
        regs.Clear(); regs = null;
        canvas = null;
        registerSize = 0;
    }

    public static void Draw(int cols, bool values = true, bool points = false)
    {
        if (!initialized) return;

        float x_offset = 30;
        float y_offset = 10;
        float x_scale = 10/*(canvas.Width) / (float)cols*/;
        float y_scale = (canvas.Height - 10f) / (float)Math.Pow(2, registerSize);

        Bitmap grid = new Bitmap((int)(cols * x_scale + 100), canvas.Height);
        Graphics g = Graphics.FromImage(grid);
        g.Clear(Color.White);

        //Pen blackPen = new Pen(Color.Black, 1);
        Brush brush = new SolidBrush(Color.Black);



        int pointSize = 3;

        Font drawFont = new Font("Arial", 10);
        for (int y = 0; y < Math.Pow(2, registerSize); y++)
        {
            g.DrawString(regs[y], drawFont, brush, new PointF(1, y * y_scale + y_offset - 7));

            if (points)
                for (int x = 0; x < cols; x++)
                {
                    g.FillEllipse(brush, new RectangleF(x * x_scale + x_offset - pointSize / 2f, y * y_scale + y_offset - pointSize / 2f, pointSize, pointSize));
                }
            else
                g.DrawLine(new Pen(new SolidBrush(Color.Gray),1), new PointF(x_offset, y * y_scale + y_offset), new PointF(cols*x_scale, y * y_scale + y_offset));

        }

        foreach (var l in drawLines)
        {
            var pen = new Pen(l.color, l.width);


            var x1 = l.x1 * x_scale + x_offset;
            var y1 = l.y1 * y_scale + y_offset;

            var x2 = l.x2 * x_scale + x_offset;
            var y2 = l.y2 * y_scale + y_offset;

            if (values) g.DrawString(l.value, new Font("Arial", 8), brush, new PointF((x1 + x2) / 2f, (y1 + y2) / 2f));

            var point1 = new PointF(x1, y1);
            var point2 = new PointF(x2, y2);

            g.DrawLine(pen, point1, point2);
            g.FillEllipse(brush, new RectangleF(point2.X - pointSize / 2f, point2.Y - pointSize / 2f, pointSize, pointSize));
        }
        //g.DrawLine(, 2), new Point(0,0), new Point(100, 100));
        canvas.Width = grid.Width;
        ((PictureBox)canvas).Image = grid;
    }

    private static List<string> RunReqTable(int size)
    {
        List<string> result = new List<string>();
        int[] nums = new int[size + 1];
        ReqTable();
        return result;
        void ReqTable(int gen = 0)
        {
            gen++;
            if (gen >= size + 1)
            {
                var res = "";
                for (int i = 1; i < size + 1; i++)
                {
                    res += $"{nums[i]}";
                }

                result.Add(res);
                return;
            }

            for (int A = 0; A <= 1; A++)
            {
                nums[gen] = A;
                ReqTable(gen);
            }

        }
    }

}


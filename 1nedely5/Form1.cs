using System;
using System.Drawing;
using System.Windows.Forms;
using NCalc;

namespace _1nedely5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем функцию из TextBox
            string functionString = textBox1.Text;

            // Задаем параметры графика
            double xMin = -10;
            double xMax = 10;
            double step = 0.1;

            // Создаем Bitmap для рисования графика
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White); // Заполняем фон белым

            // Создаем объект Expression
            Expression expression = new Expression(functionString);

            // Рисуем оси координат (примерно)
            g.DrawLine(Pens.Black, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2); // Ось X
            g.DrawLine(Pens.Black, pictureBox1.Width / 2, 0, pictureBox1.Width / 2, pictureBox1.Height); // Ось Y

            // === Находим минимальное и максимальное значения y ===
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            for (double x = xMin; x <= xMax; x += step)
            {
                expression.Parameters["x"] = x;
                object result = expression.Evaluate();

                if (result is double y)
                {
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }

            // === Рисуем график функции ===
            PointF previousPoint = PointF.Empty;
            bool firstPoint = true;

            for (double x = xMin; x <= xMax; x += step)
            {
                expression.Parameters["x"] = x;
                object result = expression.Evaluate();

                if (result is double y)
                {
                    // Преобразование координат
                    float pictureBoxX = (float)((x - xMin) / (xMax - xMin) * pictureBox1.Width);

                    // Масштабируем y
                    float pictureBoxY = (float)(pictureBox1.Height - (y - minY) / (maxY - minY) * pictureBox1.Height);

                    PointF currentPoint = new PointF(pictureBoxX, pictureBoxY);

                    if (!firstPoint && !float.IsNaN(previousPoint.X) && !float.IsNaN(previousPoint.Y) && !float.IsNaN(currentPoint.X) && !float.IsNaN(currentPoint.Y))
                    {
                        // Рисуем линию
                        g.DrawLine(Pens.Blue, previousPoint, currentPoint);
                    }
                    else
                    {
                        firstPoint = false;
                    }

                    previousPoint = currentPoint;
                }
                else
                {
                    // Обработка ошибок
                    Console.WriteLine($"Ошибка при вычислении x = {x}");
                }
            }

            // Отображаем Bitmap в PictureBox
            pictureBox1.Image = bmp;
        }
    }
}

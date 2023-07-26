using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;

namespace CourseWork
{
    public partial class Form1 : Form
    {
        private SoundPlayer soundPlayer;

        bool stopped = false;
        Stopwatch sw = new Stopwatch();
        private int tickCounter = 0;
        private double speed = 1;
        Color circleColor = Color.Red;
        Label misslabel;
        private Random random = new Random();
        private long score;
        private int missedCircles = 0;
        private void circle_Click(object sender, EventArgs e)
        {
            Circle clickedCircle = (Circle)sender;
            Controls.Remove(clickedCircle);
            clickedCircle.Dispose();
            score += (int)(tickCounter * 0.1);
            soundPlayer.Play();
            clickedCircle.Invalidate();
        }
        private void CreateCircle(int x, int y, bool israising)
        {

            Circle circle = new Circle(true)
            {
                Location = new Point(x, y),
                Width = 2,
                Height = 2,
                FlatStyle = FlatStyle.Flat,
            };
            circle.BackColor = circleColor;
            circle.FlatAppearance.BorderSize = 0;
            circle.FlatAppearance.MouseOverBackColor = circle.BackColor;
            circle.FlatAppearance.MouseDownBackColor = circle.BackColor;
            circle.BackgroundImage = Properties.Resources.circle;
            circle.BackgroundImageLayout = ImageLayout.Stretch;
            circle.Click += circle_Click;
            Controls.Add(circle);
            Controls.SetChildIndex(circle, 0);
            
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tickCounter++;
            label2.Text = $"Рахунок: {score}";
            timeLabel.Text = $"Час: {sw.Elapsed:hh\\:mm\\:ss}";
            label1.Text = $"Рівень: {(((speed-1)/2)*10)+1}";
            foreach (var item in this.Controls)
            {
                if (item is Circle)
                {
                    if (((Circle)item).Size.Width<=125 && ((Circle)item).IsRaising == true)
                    {
                        if (tickCounter % 2 == 0)
                        {
                            ((Circle)item).Location = new Point(((Circle)item).Location.X - 1, ((Circle)item).Location.Y - 1);
                        }
                        ((Circle)item).Size = new Size(((Circle)item).Size.Width + 1, ((Circle)item).Size.Height + 1);
                    }
                    else if(((Circle)item).Size.Width > 0)
                    {
                        ((Circle)item).IsRaising = false;
                        if (tickCounter % 2 == 0)
                        {
                            ((Circle)item).Location = new Point(((Circle)item).Location.X + 1, ((Circle)item).Location.Y + 1);
                        }
                        ((Circle)item).Size = new Size(((Circle)item).Size.Width - 1, ((Circle)item).Size.Height - 1);
                    }
                    else if (((Circle)item).Size.Width == 0)
                    {
                        misslabel = new Label()
                        {
                            Text = ":(",
                            Location = new Point(((Circle)item).Location.X - 20,
                            ((Circle)item).Location.Y - 47),
                            ForeColor = Color.Red,
                            Font = new Font("Arial", Font.Size + 5, FontStyle.Bold)
                    };
                        panel1.Controls.Add(misslabel);
                        Controls.Remove(((Circle)item));
                        ((Circle)item).Dispose();
                        score = (int)(score * 0.8);
                        missedCircles++;
                    }
                }
            }
            switch (missedCircles)
            {
                case 0:
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = false;
                    break;
                case 1:
                    pictureBox1.Visible = true;
                    break;
                case 2:
                    pictureBox2.Visible = true;
                    break;
                case 3:
                    pictureBox3.Visible = true;
                    break;
                default:
                    break;
            }
            // Зупиняємо гру, коли гравець пропускає 3 кола
            if (missedCircles == 3)
            {
                label3.Visible = true;
                label3.Text = $"Гру завершено. \n Спробуй ще раз!";
                button2.Enabled = false;
                foreach (var item in Controls)
                {
                    if(item is Circle)
                    {
                        ((Circle)item).Visible = false;
                    }                    
                }
                foreach (var item in Controls)
                {
                    if (item is Circle)
                    {
                        Controls.Remove((Circle)item);
                        ((Circle)item).Dispose();
                    }
                }
                timer1.Stop();

            }
            // Генеруємо випадкові координати для створення кола
            int x = random.Next(panel1.Location.X + 65, panel1.Width - 65);
            int y = random.Next(panel1.Location.Y + 65, panel1.Height - 65);

            if (tickCounter % (((int)(60 / speed))) == 0)
            {
                CreateCircle(x, y, true);
            }

            if (tickCounter % 900 == 0)
            {
                speed+=0.2;
                missedCircles = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "СПОЧАТКУ")
            {
                Application.Restart();
            }
            else
            {
                timer1.Start();
                label3.Visible = false;
                sw.Start();
                button1.Width = 270;
                button1.Text = "СПОЧАТКУ";
                button2.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.circle_ico;
            score = 0;
            label3.BringToFront();
            panel1.SendToBack();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            timer1.Interval = 17; // Интервал оновлення в милісекундах
            timer1.Tick += timer1_Tick;

            soundPlayer = new SoundPlayer(Properties.Resources.pop);
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            score = (long)(score * 0.9);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (stopped == false)
            {
                timer1.Stop();
                stopped = true;
                foreach (var item in Controls)
                {
                    if (item is Circle)
                    {
                        ((Circle)item).Visible = false;
                    }
                }
                label3.Visible = true;
                label3.Text = "Пауза";
            }
            else
            {
                timer1.Start();
                stopped = false;
                foreach (var item in Controls)
                {
                    if (item is Circle)
                    {
                        ((Circle)item).Visible = true;
                    }
                }
                label3.Visible = false;
            }
            
        }
    }
}

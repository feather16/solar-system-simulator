using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SolarSystemSimulator
{
    public class MainWindow : Form
    {
        private readonly static int WIDTH = 1080, HEIGHT = 720;
        private readonly static Vector2 WINDOW_SIZE = new Vector2(WIDTH, HEIGHT);

        private const double GRAVITY_CONST = 6.66743e-11;

        public List<Planet> planets = new List<Planet>();
        public MainWindow()
        {
            // ダブルバッファリング設定
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Timer timer = new Timer();
            timer.Interval = SolarSystemSimulator.INTERVAL;
            timer.Tick += new EventHandler(update);
            timer.Start();

            UpdateStyles();
            ClientSize = new Size(WIDTH, HEIGHT);
            FormBorderStyle = FormBorderStyle.FixedSingle; // ウィンドウサイズ変更禁止
            MaximizeBox = false; // 最大化ボタン無効化
            AutoScaleMode = AutoScaleMode.None; // 自動スケーリング無効


            Console.WriteLine("Hello!");

            const string path = "config.txt";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            foreach(string line in File.ReadAllLines(path))
            {
                string line1 = line;
                // #がある場合
                if(line.IndexOf('#') != -1)
                {
                    line1 = line.Substring(0, line.IndexOf('#'));
                }

                if (line1.Length == 0) continue;

                Planet planet = new Planet();

                foreach(string token in line1.Split(',').Select(s => s.Trim()))
                {
                    string[] strs = token.Split('=');
                    string property = strs[0].Trim(), value = strs[1].Trim();

                    switch (property)
                    {
                        case "x":
                            planet.x = double.Parse(value);
                            break;
                        case "y":
                            planet.y = double.Parse(value);
                            break;
                        case "vx":
                            planet.vx = double.Parse(value);
                            break;
                        case "vy":
                            planet.vy = double.Parse(value);
                            break;
                        case "name":
                            planet.name = value;
                            break;
                        case "mass":
                            planet.mass = double.Parse(value);
                            break;
                        case "color":
                            planet.color = Color.FromName(value);
                            break;
                        default:
                            Console.WriteLine("Undefined Property " + property);
                            break;
                    }
                }

                planets.Add(planet);
            }
        }
        bool plusPressed = false, minusPressed = false;
        int ex = 0;
        private void update(object sender, EventArgs e)
        {
            Refresh();

            bool plusPressedNow = UserInput.isKeyPressed(Keys.Oemplus);
            bool minusPressedNow = UserInput.isKeyPressed(Keys.OemMinus);


            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                // 拡大
                if(plusPressedNow && !plusPressed)
                {
                    ex++;
                }
                // 縮小
                if(minusPressedNow && !minusPressed)
                {
                    ex--;
                }
            }
            plusPressed = plusPressedNow;
            minusPressed = minusPressedNow;

            // 衝突したら削除
            /*{
                bool breakFlag = false;
                do
                {
                    for (int i = 0; i < planets.Count; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            Planet p1 = planets[i], p2 = planets[j];
                            double r = p1.distanceTo(p2);
                            if (r < 100)
                            {
                                breakFlag = true;
                                planets.Remove(p1);
                                planets.Remove(p2);
                                break;
                            }
                        }
                        if (breakFlag) break;
                    }
                } while (breakFlag);
            }*/

            // 重力計算
            for(int i = 0; i < planets.Count; i++)
            {
                for(int j = 0; j < i; j++)
                {
                    Planet p1 = planets[i], p2 = planets[j];
                    double r = p1.distanceTo(p2) + double.Epsilon;
                    Vector2 p1ToP2 = (p2.p - p1.p) / r; // p1 -> p2
                    double f = GRAVITY_CONST * p1.mass * p2.mass / (r * r);
                    p1.appliedForce(f * p1ToP2);
                    p2.appliedForce(-f * p1ToP2);
                }
            }

            // 移動
            foreach(Planet planet in planets)
            {
                planet.move((double)3600 * 24 * 30 * 2 / 20);
            }

            // 場外に出たら削除
            for (int i = planets.Count - 1; i >= 0; i--)
            {
                try
                {
                    _ = (float)planets[i].p.x;
                    _ = (float)planets[i].p.y;
                }catch(OverflowException)
                {
                    planets.RemoveAt(i);
                }
            }

            Text = "planets.Count = " + planets.Count;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            graphics.FillRectangle(Brushes.Black, 0, 0, WIDTH, HEIGHT);

            foreach (Planet planet in planets)
            {
                double circleRadius = 10;
                Vector2 pos = toWindowPoint(planet.p) - new Vector2(circleRadius, circleRadius);
                try
                {
                    graphics.FillEllipse(new SolidBrush(planet.color),
                        (float)pos.x,
                        (float)pos.y,
                        (float)circleRadius * 2,
                        (float)circleRadius * 2
                        );
                    graphics.DrawString(planet.name, new Font("Arial", 12), Brushes.White,
                        (float)pos.x - 7,
                        (float)pos.y
                       );
                }
                catch (Exception ex)
                {
                }
            }
        }

        private Vector2 toWindowPoint(Vector2 p)
        {
            double min = -4.5e12 * Math.Pow(1.5, -ex);
            double max = 4.5e12 * Math.Pow(1.5, -ex);

            double scale = Math.Min(WIDTH, HEIGHT) / (max - min);
            return p * scale + WINDOW_SIZE / 2;
        }
    }
}
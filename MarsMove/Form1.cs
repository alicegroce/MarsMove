using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
using OpenTK.Math;


namespace MarsMove
{
    public partial class Form1 : Form
    {
        const double T = 59355072; // Период вращения Марса вокруг Солнца
        const double mu = 132712440018.761; // Гравитационный параметр Солнца
        const double e = 0.0933941; // Эксцентриситет Марса

        double a = Formul.SemimajorAxis(mu, T), b, c; // Большая полуось, малая полуось, фокальное расстояние
        double n = Formul.MeanMotion(T); 
        double M0 = Formul.MeanAnomaly(Formul.EccentricAnomaly(e, 0), e);
        double theta, M, E, r, speed;

        bool StartStop = true;
        Timer timer;
        const int interval = 25;
        double t = 0;
        const double T2 = 5; // Период вращения моделирования
        const double delta_t = (interval / 1000.0) * T / T2;
        double scale_ratio;

        // переменные для хранения данных о тектурах
        Bitmap bmpSpace;
        Bitmap bmpSun;
        Bitmap bmpMars;
        const double bgsize = 22; // размер фона
        const double bgdepth = 11; // отдаленность фона

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(0.5f, 0.5f, 0.75f, 1.0f); // цвет фона
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // очистка буферов цвета и глубины

            GL.LoadIdentity();

            // координаты положения наблюдателя в лабиринте
            GL.Translate(0, 0, -11);

            // Слонце
            GLTexture.LoadTexture(bmpSun);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
            GL.PushMatrix();
            Sphere(1, 20, 20, 0, 0, 0);
            GL.PopMatrix();
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            // Марс
            GLTexture.LoadTexture(bmpMars);
            GL.Enable(EnableCap.Texture2D);
            //GL.Enable(EnableCap.Lighting);
            GL.PushMatrix();
            Sphere(0.4, 20, 20, Math.Cos(theta) * r * scale_ratio, Math.Sin(theta) * r * scale_ratio, 0, true);
            GL.PopMatrix();
            //GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);
            // Орбита
            GL.Begin(PrimitiveType.LineLoop);
              for (double i = 0; i < 2 * Math.PI; i += 2 * Math.PI / 360)
                  GL.Vertex2(Math.Cos(i) * a * scale_ratio - c * scale_ratio, Math.Sin(i) * b * scale_ratio);
            GL.End();

            // Фон
            GLTexture.LoadTexture(bmpSpace);
            GL.Enable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
              GL.TexCoord2(0, 0);
              GL.Vertex3(-bgsize, -bgsize, -bgdepth);
              GL.TexCoord2(0, 1);
              GL.Vertex3(-bgsize, bgsize, -bgdepth);
              GL.TexCoord2(1, 1);
              GL.Vertex3(bgsize, bgsize, -bgdepth);
              GL.TexCoord2(1, 0);
              GL.Vertex3(bgsize, -bgsize, -bgdepth);
            GL.End();
            GL.Disable(EnableCap.Texture2D);

            GL.Flush();
            GL.Finish();
            glControl1.SwapBuffers();
        }

        private void CalcOrbit()
        {
            M = Formul.MeanAnomaly(M0, n, t, 0);
            E = Formul.AboutEccentricAnomaly(e, M);
            theta = Formul.TrueAnomaly(e, E);
            r = Formul.PositionVectorLength(a, e, E);
            speed = Formul.OrbitalVelocity(mu, r, a);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Light0);
            bmpSpace = new Bitmap("Space.jpg");
            bmpSun = new Bitmap("Sun.jpg");
            bmpMars = new Bitmap("Mars.jpg");
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            UpdateData();
            scale_ratio = 7 / r;
            b = a * Math.Sqrt(1 - Math.Pow(Form1.e, 2));
            c = a * Form1.e;

            timer = new Timer() { Interval = 25, Enabled = true };
            timer.Tick += MoveMars;
        }

        private void MoveMars(object sender, EventArgs e)
        {
            if (StartStop)
            {
                t += delta_t;
                if (t > T)
                {
                    t -= T;
                }
            }
            UpdateData();
            glControl1.Invalidate();
        }

        public void UpdateData()
        {
            CalcOrbit();
            lRadius.Text = "Радиус (км) = " + r;
            lSpeed.Text = "Скорость (км/с) = " + speed;
        }

        // изображение сферы
        void Sphere(double r, int nx, int ny, double sx, double sy, double sz, bool rotate_texture = false)
        {
            int ix, iy;
            double x, y, z, tex_x, tex_y;


            for (iy = 0; iy < ny; ++iy)
            {
                tex_y = (double)iy / (double)ny;

                GL.Begin(PrimitiveType.QuadStrip);
                for (ix = 0; ix <= nx; ++ix)
                {
                    tex_x = (double)ix / (double)nx + (rotate_texture ? theta - Math.Floor(theta) : 0);

                    x = r * Math.Sin(iy * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx)+sx;
                    y = r * Math.Sin(iy * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx)+sy;
                    z = r * Math.Cos(iy * Math.PI / ny)+sz;
                    GL.Normal3(x, y, z); //нормаль направлена от центра
                    GL.TexCoord2(tex_x, tex_y);
                    GL.Vertex3(x, y, z);

                    x = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx)+sx;
                    y = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + sy;
                    z = r * Math.Cos((iy + 1) * Math.PI / ny)+sz;
                    GL.Normal3(x, y, z);
                    GL.TexCoord2(tex_x, tex_y + 1.0 / (double)ny);
                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.5, 0.5, -0.5, 0.5, 0.5, 50);
            GL.MatrixMode(MatrixMode.Modelview);
            glControl1.Invalidate();
        }

        private void bStartStop_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bStartStop.Text = (StartStop = !StartStop) ? "Стоп" : "Старт";
        }
    }
}

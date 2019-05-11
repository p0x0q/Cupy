using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cupy
{
    public partial class Captcha : Form
    {
        Point MD = new Point();
        Point MU = new Point();
        Bitmap bmp;
        bool view = false;
        public Color User_Color = Color.Black;

        public Size Display_Size = new Size(0,0);
        public Point Display_Location = new Point(0,0);

    public Form1 main_f;
        public Captcha(Form1 fo)
        {
            InitializeComponent();
            main_f = fo;

            //            pictureBox1.Size = new Size(this.Width, this.Height);
            //            pictureBox1.Location = new Point(0,0);



            //this.TransparencyKey = Color.White;//System.Drawing.SystemColors.Control
            //this.BackColor = Color.Black;
            this.Opacity = 0.3;
            //pictureBox1.BackColor = Color.Black;
            //this.BackColor = Color.Black;
            //pictureBox1.BackColor = Color.FromArgb(0, Color.Black);
            //this.TransparencyKey = Color.Black;
            main_f.Visible = false;

            foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            {
                /*
                //ディスプレイのデバイス名を表示
                nao0x0.PC.DebugLog("デバイス名: " + s.DeviceName);
                //ディスプレイの左上の座標を表示
                nao0x0.PC.DebugLog("X:" + s.Bounds.X + " Y:" + s.Bounds.Y);
                //ディスプレイの大きさを表示
                nao0x0.PC.DebugLog("高さ:" + s.Bounds.Height + " 幅:" + s.Bounds.Width);
                */

                Display_Size.Width += s.Bounds.Width;
                Display_Size.Height += s.Bounds.Height;

                Display_Location.X = Math.Min(Display_Location.X, s.Bounds.X);
                Display_Location.Y = Math.Min(Display_Location.Y, s.Bounds.Y);
            }
 //           MessageBox.Show("DW:" + Display_Size.Width + " DH:" + Display_Size.Height + " DX:" + Display_Location.X + " DY:" + Display_Location.Y);
            this.Size = new Size(Display_Size.Width, Display_Size.Height);
            this.Location = new Point(Display_Location.X, Display_Location.Y);

        }

        public void Captcha_Run(int x,int y,int width,int height)
        {
            Run.Visible = false;
            this.Visible = false;
            /*
            main_f.display_captcha = new Rectangle(this.Location.X + 8, this.Location.Y + 27, this.Size.Width - 15, this.Size.Height - 33);
            main_f.Cupy_run(main_f.display_captcha);
            */
            //this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height
            main_f.display_captcha = new Rectangle(x,y,width,height);
            main_f.Cupy_run(main_f.display_captcha);

            main_f.Visible = true;
            this.Close();
        }

        private void Run_Click(object sender, EventArgs e)
        {
            //Captcha_Run();
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // 描画フラグON
            view = true;

            // Mouseを押した座標を記録
            MD.X = e.X;
            MD.Y = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Point start = new Point();
            Point end = new Point();

            // Mouseを離した座標を記録
            MU.X = e.X;
            MU.Y = e.Y;

            //System.Diagnostics.Debug.WriteLine("MouseUp({0},{1})->({2},{3})", MD.X, MD.Y, MU.X, MU.Y);

            // 座標から(X,Y)座標を計算
            GetRegion(MD, MU, ref start, ref end);

            // 領域を描画
            DrawRegion(start, end);

            //PictureBox1に表示する
            pictureBox1.Image = bmp;

            // 描画フラグOFF
            view = false;

            int x = 0;
            int y = 0;

            //左上から右下にキャプチャーしようとした場合
            x = MU.X - MD.X;
            y = MU.Y - MD.Y;

            if (x < 0 || y < 0)
            {
                //右下から左上にキャプチャーしようとした場合
                x = MD.X - MU.X;
                y = MD.Y - MU.Y;
                MD.X -= x;
                MD.Y -= y;
            }

            if (y < 0)
            {
                int s = y;
                y *= -1;
                y -= s;

            }



            Captcha_Run(Display_Location.X + MD.X, Display_Location.Y + MD.Y, x, y);
            //Captcha_Run(start.X,start.Y,MU.X,MU.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point();
            Point start = new Point();
            Point end = new Point();

            // 描画フラグcheck
            if (view == false)
            {
                return;
            }

            // カーソルが示している場所の座標を取得
            p.X = e.X;
            p.Y = e.Y;

            // 座標から(X,Y)座標を計算
            GetRegion(MD, p, ref start, ref end);

            //System.Diagnostics.Debug.WriteLine("Move ({0},{1})", e.X, e.Y);

            // 領域を描画
            DrawRegion(start, end);

            //PictureBox1に表示する
            pictureBox1.Image = bmp;
        }

        private void GetRegion(Point p1, Point p2, ref Point start, ref Point end)
        {
            start.X = Math.Min(p1.X, p2.X);
            start.Y = Math.Min(p1.Y, p2.Y);

            end.X = Math.Max(p1.X, p2.X);
            end.Y = Math.Max(p1.Y, p2.Y);
        }

        private int GetLength(int start, int end)
        {
            return Math.Abs(start - end);
        }

        private void DrawRegion(Point start, Point end)
        {
            Pen blackPen = new Pen(Color.White);
            Graphics g = Graphics.FromImage(bmp);

            // 描画する線を点線に設定
            blackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            // 画面を消去
            g.Clear(User_Color);

            // 領域を描画
            g.DrawRectangle(blackPen, start.X, start.Y, GetLength(start.X, end.X), GetLength(start.Y, end.Y));

            g.Dispose();
        }

        private void Captcha_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}

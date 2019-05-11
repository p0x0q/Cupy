using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cupy
{
    public partial class Form1 : Form
    {
        public Size initial_size = new Size();
    public Form1()
        {
            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;

            DLL.main.appcheck(DLL.main.myappname());
            DLL.main.FormStart("Cupy", DLL.main.myappname());
            //DLL.form.Icon("chat", Properties.Resources.Icon, this);
            DLL.form.Form_Closing(this);
            var result = DLL.main.thisupdate(DLL.main.myappname(), "Cupy", false);


            this.Size = initial_size;
            if (DLL.main.FileSearch(nao0x0.Directory.AppData + @".p0x0q\Apps\Cupy\images\" + nao0x0.LoginData.userid) == false)
            {
                DLL.main.ForderCreate(nao0x0.Directory.AppData + @".p0x0q\Apps\Cupy\images\" + nao0x0.LoginData.userid);
            }

            this.Text += " Ver." + result.local_version;

            initial_size = this.Size;
            this.Size = new Size(365, 210);
            //System.Drawing.Color.Teal;
        }

        public Rectangle display_captcha;
        public void Cupy_run(Rectangle rect)
        {
            try
            {
                //MessageBox.Show(rect.Y+","+rect.X+"\r\nHeight:"+rect.Height+"\r\nWidth:"+rect.Width);
                string imageid = "[temp]" + DLL.main.RandomPassWord(10);
                Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                string dir = nao0x0.Directory.AppData + @".p0x0q\Apps\Cupy\images\" + nao0x0.LoginData.userid + @"\" + imageid + ".png";

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
                }
                bmp.Save(dir, ImageFormat.Png);
                this.pictureBox1.Image = bmp;
                string result =  nao0x0.network.FileUpload(dir, "https://cupy.p0x0q.com/up.php?user_session="+nao0x0.LoginData.user_session);
                nao0x0.PC.DebugLog(result);
                Hashtable hash = nao0x0.JSON.ToHashtable(result);


                if(hash["is_upload"].ToString() == "success")
                {
                    string filename = hash["filename"].ToString();
                    string newdir = nao0x0.Directory.AppData + @".p0x0q\Apps\Cupy\images\" + nao0x0.LoginData.userid + @"\" + filename;
                    DLL.main.FileMove(dir, newdir);
                    string url = "https://cupy.p0x0q.com/" + filename;
                    textBox_URLResult.Text = url;
                    //DLL.main.BrowserOpen(url);
                }
                else
                {
                    nao0x0.PC.DebugLog(result);
                    DLL.Message.AlertWindow("画像アップロード中にエラーが発生しました：" + hash["code"]);
                }
            }
            catch(Exception e)
            {
                DLL.Message.AlertWindow(DLL.main.appid + " - キャプチャーエラー",e.Message);
                nao0x0.PC.DebugLog("画像キャプチャー中にエラーが発生しました\r\n" + e.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Captcha f = new Captcha(this);
            f.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cupy
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception e)
            {
                if (!System.IO.File.Exists("Lib.dll"))
                {
                    MessageBox.Show("起動に必要なファイルが見つからないため、ダウンロードしアプリケーションを再起動します");
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.DownloadFile("http://nao0x0.sakura.ne.jp/Library/app/data/Expension-Download-Data/Lib.dll", @"Lib.dll");
                    wc.Dispose();
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("Lib.dllの互換性エラーです。10分程度待つとサーバーに更新が入りダウンロードし直すと起動できる場合があります\r\n\r\n SupportMesasge:ベータバージョンが適用されている場合があります\r\nErrorMessage:" + e.Message, "chat");
                }
            }
        }
    }
}

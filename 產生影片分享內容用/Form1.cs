using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace 產生影片分享內容用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//該值確定是否可以選擇多個檔案
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "文字檔案(*.txt) | *.txt";
            dialog.InitialDirectory = textBox9.Text;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox9.Text = dialog.FileName;
                Properties.Settings.Default.outputFilePath = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox9.Text.Length <= 1)
            {
                MessageBox.Show("請選擇要寫入的檔案。");
                return;
            }
            string str =
            "\r\n[原創] {0} [{1}]({2}@MG@{3})\r\n\r\n" +
            "【影片名稱】：{4}" + "\r\n" +
            "【影片大小】：{5}GB" + "\r\n" +
            "【影片格式】：{1}" + "\r\n" +
            "【有／無碼】：{3}" + "\r\n" +
            "【檔案空間】：MEGA" + "\r\n" +
            "【解壓密碼】：he01204046分享於伊莉論壇" + "\r\n" +
            "【分享期限】：系統自刪" + "\r\n" +
            "【預覽圖片】：" + "\r\n" + "\r\n" +
            "【影片載點】：{6}" + "\r\n" + "\r\n" +
            "【其他EYNY影片】：http://www.eyny.com/channel/UCU7aqRZ9J5" + "\r\n" + "\r\n" + "\r\n" +
           " [原創][{7}] {0} [{1}]({2}@{3})" + "\r\n" + "\r\n" +
            "【影片名稱】：{4}" + "\r\n" +
            "【影片格式】：{1}" + "\r\n" +
            "【字幕語言】：無" + "\r\n" +
            "【是否有碼】：{3}" + "\r\n" +
            "【影片大小】：{5}GB" + "\r\n" +
            "【驗證全碼】：{8}" + "\r\n" +
            "【作種期限】：1個月" + "\r\n" +
            "【影片截圖】：" + "\r\n" + "\r\n" +
            "【其他EYNY影片】：http://www.eyny.com/channel/UCU7aqRZ9J5" + "\r\n" + "\r\n" + "\r\n" +
            "-----------------------------------------------------------------------------------------------";

            string txt_temp = System.IO.File.ReadAllText(textBox9.Text);

            string txt = string.Format(str,
                textBox1.Text, comboBox1.SelectedItem.ToString(), textBox4.Text, textBox6.Text,
                textBox2.Text,
                textBox3.Text, textBox7.Text, textBox8.Text.Length <= 4 ? "" : textBox8.Text.Substring(0, 4), textBox8.Text);

            System.IO.File.WriteAllText(textBox9.Text, txt_temp + txt);

            MessageBox.Show("寫入完成");

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.outputFilePath.Length <= 0)
                textBox9.Text = System.IO.Directory.GetCurrentDirectory();
            else
                textBox9.Text = Properties.Settings.Default.outputFilePath;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text.Replace("...", "…");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int maxLenhth = Encoding.UTF8.GetBytes("HKGL-001 可愛的羅莉學生放學後和老師在愛情酒店課後教學  伊藤はる").Length;
            int nowLenhth = Encoding.UTF8.GetBytes(textBox1.Text).Length; 
            if (nowLenhth > maxLenhth)
            {
                label10.Text = "中文標題超過 [ " + (nowLenhth - maxLenhth) + " ] 個字元，請修改！";
                label10.ForeColor = Color.Red;
            }
            else
            {
                label10.Text = $"中文標題共計 [ {nowLenhth} ] 個字元，剩餘 [ {maxLenhth- nowLenhth} ] 個字元可輸入。";
                label10.ForeColor = Color.Green;
            }
        }
    }
}

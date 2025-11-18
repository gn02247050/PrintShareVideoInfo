
namespace 產生影片分享內容用
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            textBox3 = new System.Windows.Forms.TextBox();
            textBox4 = new System.Windows.Forms.TextBox();
            textBox7 = new System.Windows.Forms.TextBox();
            textBox8 = new System.Windows.Forms.TextBox();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            textBox9 = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label10 = new System.Windows.Forms.Label();
            comboBox2 = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(17, 16);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 19);
            label1.TabIndex = 0;
            label1.Text = "日文標題";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(17, 57);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(69, 19);
            label2.TabIndex = 1;
            label2.Text = "中文標題";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(17, 101);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(69, 19);
            label3.TabIndex = 2;
            label3.Text = "大小(GB)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(202, 101);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(69, 19);
            label4.TabIndex = 3;
            label4.Text = "影片格式";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(453, 101);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(84, 19);
            label5.TabIndex = 4;
            label5.Text = "影片副檔名";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(595, 101);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(54, 19);
            label6.TabIndex = 5;
            label6.Text = "馬賽克";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(17, 146);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(84, 19);
            label7.TabIndex = 6;
            label7.Text = "影片下載點";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(17, 186);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(54, 19);
            label8.TabIndex = 7;
            label8.Text = "種子碼";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(105, 53);
            textBox1.Margin = new System.Windows.Forms.Padding(4);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(615, 27);
            textBox1.TabIndex = 8;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(105, 13);
            textBox2.Margin = new System.Windows.Forms.Padding(4);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(615, 27);
            textBox2.TabIndex = 9;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(105, 98);
            textBox3.Margin = new System.Windows.Forms.Padding(4);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(88, 27);
            textBox3.TabIndex = 10;
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(546, 98);
            textBox4.Margin = new System.Windows.Forms.Padding(4);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(50, 27);
            textBox4.TabIndex = 11;
            textBox4.Text = "MP4";
            // 
            // textBox7
            // 
            textBox7.Location = new System.Drawing.Point(105, 142);
            textBox7.Margin = new System.Windows.Forms.Padding(4);
            textBox7.Name = "textBox7";
            textBox7.Size = new System.Drawing.Size(615, 27);
            textBox7.TabIndex = 14;
            // 
            // textBox8
            // 
            textBox8.Location = new System.Drawing.Point(105, 182);
            textBox8.Margin = new System.Windows.Forms.Padding(4);
            textBox8.Name = "textBox8";
            textBox8.Size = new System.Drawing.Size(615, 27);
            textBox8.TabIndex = 15;
            // 
            // textBox9
            // 
            textBox9.Location = new System.Drawing.Point(105, 231);
            textBox9.Margin = new System.Windows.Forms.Padding(4);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new System.Drawing.Size(526, 27);
            textBox9.TabIndex = 16;
            textBox9.Text = "C:\\Users\\gn022\\Downloads\\新增資料夾\\上傳\\發表資料.txt";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(17, 234);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(69, 19);
            label9.TabIndex = 17;
            label9.Text = "輸出目標";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(640, 231);
            button1.Margin = new System.Windows.Forms.Padding(4);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(81, 29);
            button1.TabIndex = 18;
            button1.Text = "瀏覽檔案";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(609, 281);
            button2.Margin = new System.Windows.Forms.Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(112, 29);
            button2.TabIndex = 19;
            button2.Text = "完成並寫入";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "LD/240P", "LD/360P", "SD/416P", "SD/480P", "HD/720P", "FHD/1080P", "FHD/1088P", "2K/1440P", "4K/2160P", "8K/4320P" });
            comboBox1.Location = new System.Drawing.Point(280, 98);
            comboBox1.Margin = new System.Windows.Forms.Padding(4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(154, 27);
            comboBox1.TabIndex = 20;
            comboBox1.Text = "FHD/1088P";
            // 
            // label10
            // 
            label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label10.Location = new System.Drawing.Point(17, 281);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(539, 29);
            label10.TabIndex = 1;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "有碼", "無碼" });
            comboBox2.Location = new System.Drawing.Point(657, 98);
            comboBox2.Margin = new System.Windows.Forms.Padding(4);
            comboBox2.Name = "comboBox2";
            comboBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            comboBox2.Size = new System.Drawing.Size(64, 27);
            comboBox2.TabIndex = 20;
            comboBox2.Text = "有碼";
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(753, 324);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label9);
            Controls.Add(textBox9);
            Controls.Add(textBox8);
            Controls.Add(textBox7);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label10);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Form1";
            Text = "影片分享內容產生器(附加Webp轉Jpg功能)";
            Load += Form1_Load;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            DragLeave += Form1_DragLeave;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}


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
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            textBox9 = new System.Windows.Forms.TextBox();
            label9 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            tabControl1 = new System.Windows.Forms.TabControl();
            btnAddTab = new System.Windows.Forms.Button();
            btnDeleteTab = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // textBox9
            // 
            textBox9.Location = new System.Drawing.Point(98, 318);
            textBox9.Margin = new System.Windows.Forms.Padding(4);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new System.Drawing.Size(569, 27);
            textBox9.TabIndex = 16;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(10, 321);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(69, 19);
            label9.TabIndex = 17;
            label9.Text = "輸出目標";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(675, 316);
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
            button2.Location = new System.Drawing.Point(651, 353);
            button2.Margin = new System.Windows.Forms.Padding(4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(112, 29);
            button2.TabIndex = 19;
            button2.Text = "完成並寫入";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabControl1
            // 
            tabControl1.Location = new System.Drawing.Point(11, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(752, 299);
            tabControl1.TabIndex = 21;
            // 
            // btnAddTab
            // 
            btnAddTab.Location = new System.Drawing.Point(10, 352);
            btnAddTab.Name = "btnAddTab";
            btnAddTab.Size = new System.Drawing.Size(100, 29);
            btnAddTab.TabIndex = 22;
            btnAddTab.Text = "+ 新增頁籤";
            btnAddTab.UseVisualStyleBackColor = true;
            btnAddTab.Click += btnAddTab_Click;
            // 
            // btnDeleteTab
            // 
            btnDeleteTab.Location = new System.Drawing.Point(116, 352);
            btnDeleteTab.Name = "btnDeleteTab";
            btnDeleteTab.Size = new System.Drawing.Size(100, 29);
            btnDeleteTab.TabIndex = 23;
            btnDeleteTab.Text = "- 刪除目前頁籤";
            btnDeleteTab.UseVisualStyleBackColor = true;
            btnDeleteTab.Click += btnDeleteTab_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(776, 389);
            Controls.Add(btnDeleteTab);
            Controls.Add(btnAddTab);
            Controls.Add(tabControl1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label9);
            Controls.Add(textBox9);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Form1";
            Text = "影片分享內容產生器(多頁籤版)";
            Load += Form1_Load;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            DragLeave += Form1_DragLeave;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnAddTab;
        private System.Windows.Forms.Button btnDeleteTab;
    }
}
namespace 產生影片分享內容用
{
    partial class JavdbBrowserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            panel1 = new System.Windows.Forms.Panel();
            labelHint = new System.Windows.Forms.Label();
            btnContinue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = System.Drawing.Color.White;
            webView21.Dock = System.Windows.Forms.DockStyle.Fill;
            webView21.Location = new System.Drawing.Point(0, 0);
            webView21.Name = "webView21";
            webView21.Size = new System.Drawing.Size(800, 450);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // panel1
            // 
            panel1.Controls.Add(labelHint);
            panel1.Controls.Add(btnContinue);
            panel1.Location = new System.Drawing.Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(776, 37);
            panel1.TabIndex = 1;
            // 
            // labelHint
            // 
            labelHint.AutoSize = true;
            labelHint.Location = new System.Drawing.Point(58, 8);
            labelHint.Name = "labelHint";
            labelHint.Size = new System.Drawing.Size(511, 19);
            labelHint.TabIndex = 1;
            labelHint.Text = "如果頁面出現機器人驗證或 18+ 提示，請先在這個視窗中完成驗證，再按下";
            // 
            // btnContinue
            // 
            btnContinue.Enabled = false;
            btnContinue.Location = new System.Drawing.Point(575, 3);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new System.Drawing.Size(198, 29);
            btnContinue.TabIndex = 0;
            btnContinue.Text = "驗證完成，繼續";
            btnContinue.UseVisualStyleBackColor = true;
            btnContinue.Click += btnContinue_Click;
            // 
            // JavdbBrowserForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(webView21);
            Name = "JavdbBrowserForm";
            Text = "JavdbBrowserForm";
            Load += JavdbBrowserForm_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.Button btnContinue;
    }
}
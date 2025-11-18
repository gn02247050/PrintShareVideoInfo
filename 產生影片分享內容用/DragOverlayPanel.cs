using System;
using System.Drawing;
using System.Windows.Forms;

namespace 產生影片分享內容用
{
    public class DragOverlayPanel : Panel
    {
        public bool ShowText { get; set; } = false;
        public string OverlayText { get; set; } = "請將 MP4 或 webp 檔案拖曳到此視窗";

        public DragOverlayPanel()
        {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.SupportsTransparentBackColor,
                          true);

            this.BackColor = Color.Transparent;
            this.Enabled = false;     // 不擋滑鼠事件
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!ShowText || string.IsNullOrEmpty(OverlayText))
                return;

            using (Font drawFont = new Font("Microsoft JhengHei", 24, FontStyle.Bold))
            using (Brush brushOutline = new SolidBrush(Color.FromArgb(200, 255, 255, 255)))
            using (Brush brushText = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
            {
                SizeF size = e.Graphics.MeasureString(OverlayText, drawFont);
                float x = (this.ClientSize.Width - size.Width) / 2;
                float y = (this.ClientSize.Height - size.Height) / 2;

                // 白色淡淡外框
                e.Graphics.DrawString(OverlayText, drawFont, brushOutline, x - 1, y - 1);
                e.Graphics.DrawString(OverlayText, drawFont, brushOutline, x + 1, y - 1);
                e.Graphics.DrawString(OverlayText, drawFont, brushOutline, x - 1, y + 1);
                e.Graphics.DrawString(OverlayText, drawFont, brushOutline, x + 1, y + 1);

                // 中間的半透明黑字
                e.Graphics.DrawString(OverlayText, drawFont, brushText, x, y);
            }
        }
    }
}

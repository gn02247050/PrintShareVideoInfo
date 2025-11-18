using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;   // HttpUtility.UrlEncode 用
using ImageMagick;

namespace 產生影片分享內容用
{
    public partial class Form1 : Form
    {
        private DragOverlayPanel overlayPanel;
        private bool _showDragText = false;
        private WindowsMediaPlayer _wmp;
        public Form1()
        {
            InitializeComponent();
            // 初始化 WMP 物件
            _wmp = new WindowsMediaPlayer();


            InitializeDragOverlay(); // ← 加這行
        }
        private void InitializeDragOverlay()
        {
            overlayPanel = new DragOverlayPanel();
            overlayPanel.Dock = DockStyle.Fill;
            overlayPanel.OverlayText = "請將 MP4 或 webp 檔案拖曳到此視窗";
            overlayPanel.ShowText = false;       // 預設不顯示
            overlayPanel.Visible = false;

            this.Controls.Add(overlayPanel);
            overlayPanel.BringToFront();         // 確保在最上層
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
                textBox1.Text, comboBox1.SelectedItem.ToString(), textBox4.Text, comboBox2.SelectedItem.ToString(),
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
                label10.Text = $"中文標題共計 [ {nowLenhth} ] 個字元，剩餘 [ {maxLenhth - nowLenhth} ] 個字元可輸入。";
                label10.ForeColor = Color.Green;
            }
        }

        private async void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (overlayPanel != null)
            {
                overlayPanel.ShowText = false;
                overlayPanel.Visible = false;
            }

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length == 0)
                return;

            // 只取 .webp
            var webpFiles = files
                .Where(f => string.Equals(Path.GetExtension(f), ".webp", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // 只取 .mp4
            var mp4Files = files
                .Where(f => string.Equals(Path.GetExtension(f), ".mp4", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!mp4Files.Any() && !webpFiles.Any())
            {
                MessageBox.Show("沒有拖進任何 .mp4 或 .webp 檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(webpFiles.Any())
            {
                foreach (var webp in webpFiles)
                {
                    try
                    {
                        ConvertWebpToJpg(webp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"轉換圖片失敗：{Path.GetFileName(webp)}\r\n{ex.Message}",
                            "錯誤",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            if (mp4Files.Any())
            {
                // 1) 總大小 (所有 mp4 檔)
                long totalBytes = mp4Files.Sum(f => new FileInfo(f).Length);
                double sizeGB = totalBytes / (1024d * 1024d * 1024d);
                textBox3.Text = sizeGB.ToString("0.0#"); // 2位小數

                // 2) 只取第一個檔案的標題與寬度
                string firstFile = mp4Files[0];

                try
                {
                    IWMPMedia media = _wmp.newMedia(firstFile);

                    // 影片標題 (metadata Title，沒有就用檔名)
                    string title = media.getItemInfo("Title");
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        title = Path.GetFileNameWithoutExtension(firstFile);
                    }
                    title = title.Replace(" - 見放題ch デラックス - FANZA月額動画", "").Trim();

                    // 取高度或寬度，當作基準 P 值
                    // 一般 240P / 360P... 其實是垂直解析度，所以這裡優先取 WM/VideoHeight
                    string heightStr = media.getItemInfo("WM/VideoHeight");
                    int basisP;
                    if (!int.TryParse(heightStr, out basisP))
                    {
                        // 如果高度取不到，退而求其次用寬度
                        string widthStr = media.getItemInfo("WM/VideoWidth");
                        if (!int.TryParse(widthStr, out basisP))
                        {
                            basisP = 0; // 真的都沒取到就當作未知
                        }
                    }

                    SetResolutionComboByNearestP(basisP);

                    // 假設 firstFileTitle 是你從 mp4 抓到的標題字串
                    await FetchJavdbInfoAsync(title);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"讀取影片資訊失敗：{Path.GetFileName(firstFile)}\r\n{ex.Message}",
                        "錯誤",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;

                if (overlayPanel != null)
                {
                    overlayPanel.ShowText = true;
                    overlayPanel.Visible = true;
                    overlayPanel.Invalidate();
                    overlayPanel.BringToFront();
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 根據高度(或寬度)選擇最接近的解析度項目
        /// </summary>
        private void SetResolutionComboByNearestP(int p)
        {
            if (p <= 0)
            {
                comboBox1.SelectedIndex = -1;
                return;
            }

            // 對應關係：顯示文字 + 對應的 P 值
            var options = new[]
            {
                new { Text = "LD/240P",   P = 240 },
                new { Text = "LD/360P",   P = 360 },
                new { Text = "SD/416P",   P = 416 },
                new { Text = "SD/480P",   P = 480 },
                new { Text = "HD/720P",   P = 720 },
                new { Text = "FHD/1080P", P = 1080 },
                new { Text = "FHD/1088P", P = 1088 },
                new { Text = "2K/1440P",  P = 1440 },
                new { Text = "4K/2160P",  P = 2160 },
                new { Text = "8K/4320P",  P = 4320 },
            };

            // 找 P 值最接近的那個
            var best = options
                .OrderBy(o => Math.Abs(o.P - p))
                .First();

            // 在 ComboBox 中選到它
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (string.Equals(comboBox1.Items[i].ToString(), best.Text,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    comboBox1.SelectedIndex = i;
                    return;
                }
            }

            // 萬一沒找到（理論上不會）
            comboBox1.SelectedIndex = -1;
        }
        private void Form1_DragLeave(object sender, EventArgs e)
        {
            if (overlayPanel != null)
            {
                overlayPanel.ShowText = false;
                overlayPanel.Visible = false;
            }
        }

        private void ConvertWebpToJpg(string webpPath)
        {
            string dir = Path.GetDirectoryName(webpPath)!;
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(webpPath);
            string jpgPath = Path.Combine(dir, filenameWithoutExt + ".jpg");

            // 若已存在同名 jpg，可視情況覆寫或跳過
            if (File.Exists(jpgPath))
            {
                // 這裡選擇覆寫，如需跳過可改成 return。
                //AddLog($"[覆寫] 已存在 {Path.GetFileName(jpgPath)}，重新產生。");
            }

            using (var image = new MagickImage(webpPath))
            {
                image.Format = MagickFormat.Jpeg;
                image.Quality = 100; // 可自己調整品質 (1-100)
                image.Write(jpgPath);
            }

            //刪除舊檔案
            File.Delete(webpPath);

            //AddLog($"[成功] {Path.GetFileName(webpPath)} → {Path.GetFileName(jpgPath)}");
        }
        private async Task FetchJavdbInfoAsync(string rawTitle)
        {
            // 查詢前先把標題設為當前影片的標題
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke(new Action(() =>
                {
                    textBox2.Text = rawTitle;
                    textBox1.Text = rawTitle;
                }));
            }
            else
            {
                textBox2.Text = rawTitle;
                textBox1.Text = rawTitle;
            }
            if (string.IsNullOrWhiteSpace(rawTitle))
                return;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(20);
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                    "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                // 1. 搜尋頁
                string query = System.Web.HttpUtility.UrlEncode(rawTitle.Trim());
                string searchUrl = $"https://javdb.com/search?q={query}&f=all";

                string searchHtml = await client.GetStringAsync(searchUrl);
                var searchDoc = new HtmlAgilityPack.HtmlDocument();
                searchDoc.LoadHtml(searchHtml);

                // 先定位到 movie-list 區塊
                var movieListNode = searchDoc.DocumentNode
                    .SelectSingleNode("//div[contains(@class,'movie-list')]");

                if (movieListNode == null)
                {
                    // 沒找到搜尋結果區塊就直接放棄
                    return;
                }

                // 所有影片盒子
                var movieLinks = movieListNode
                    .SelectNodes(".//a[contains(@class,'box')]") ?? new HtmlAgilityPack.HtmlNodeCollection(null);

                var normalizedTarget = NormalizeTitle(rawTitle);

                var best = movieLinks
                            .Select(a =>
                            {
                                var title = a.GetAttributeValue("title", "").Trim();
                                var score = Similarity(normalizedTarget, title);
                                return new
                                {
                                    Node = a,
                                    Title = title,
                                    Score = score
                                };
                            })
                            // 過濾掉 title 空的
                            .Where(x => !string.IsNullOrEmpty(x.Title))
                            // 由高到低排序，取第一個
                            .OrderByDescending(x => x.Score)
                            .FirstOrDefault();

                double threshold = 0.5; // 你可以依實測調整，0.7 也可以

                if (best != null && best.Score >= threshold)
                {

                }
                else
                {
                    // 沒有相似度夠高的結果，可以視為「沒找到」或給使用者提示
                    MessageBox.Show("找不到與標題相符的影片。");
                    return;
                }

                var relativeHref = best.Node.GetAttributeValue("href", ""); // e.g. /v/4D0pE

                // 從 body 的 data-domain 組完整網址
                var bodyNode = searchDoc.DocumentNode.SelectSingleNode("//body");
                var domain = bodyNode?.GetAttributeValue("data-domain", "https://javdb.com")
                             ?? "https://javdb.com";

                // 組成完整網址
                var detailUrl = new Uri(new Uri(domain), relativeHref).ToString();
                // detailUrl => https://javdb563.com/v/4D0pE  之類

                // 2. 詳細頁（包含 over18 modal + section 內容）
                string detailHtml = await client.GetStringAsync(detailUrl);
                var detailDoc = new HtmlAgilityPack.HtmlDocument();
                detailDoc.LoadHtml(detailHtml);

                // ⭐ 忽略上面的 over18-modal，直接找到真正內容的 section
                var sectionNode = detailDoc.DocumentNode
                    .SelectSingleNode("//section[contains(@class,'section')]");

                if (sectionNode == null)
                {
                    MessageBox.Show("找不到 <section class=\"section\">。");
                    return;
                }

                // 進一步精準到 video-detail 區塊
                var videoDetailNode = sectionNode.SelectSingleNode(
                    ".//div[contains(@class,'video-detail')]"
                );

                if (videoDetailNode == null)
                {
                    MessageBox.Show("在 section 裡找不到 .video-detail 區塊。");
                    return;
                }

                // 3. 找 h2.title.is-4
                var titleContainer = videoDetailNode.SelectSingleNode(
                    ".//h2[contains(@class,'title') and contains(@class,'is-4')]"
                );

                if (titleContainer == null)
                {
                    MessageBox.Show("在 video-detail 裡找不到 class='title is-4'。");
                    return;
                }

                // 4. 取第一個與第二個 <strong>
                var strongNodes = titleContainer.SelectNodes(".//strong");
                if (strongNodes == null || strongNodes.Count < 1)
                {
                    MessageBox.Show("title is-4 裡找不到任何 <strong>。");
                    return;
                }

                string firstStrong = strongNodes[0].InnerText.Trim();              // IESP-515
                string secondStrong = strongNodes.Count > 1
                    ? strongNodes[1].InnerText.Trim()                               // 社區人妻的憂鬱 早乙女瑠衣
                    : string.Empty;

                // 第一個 TextBox：第一個<strong> + 空白 + 第二個<strong>
                string firstTextBoxValue = string.IsNullOrEmpty(secondStrong)
                    ? firstStrong
                    : $"{firstStrong} {secondStrong}";

                // 5. 找第一個 <span>（優先 origin-title，找不到再 fallback）
                HtmlAgilityPack.HtmlNode spanNode =
                    titleContainer.SelectSingleNode(".//span[contains(@class,'origin-title')]")
                    ?? titleContainer.SelectSingleNode(".//span");

                string secondTextBoxValue;

                if (spanNode != null)
                {
                    string spanText = spanNode.InnerText.Trim(); // 団地妻の憂い 早乙女ルイ
                                                                 // 第二個 TextBox：第一個<strong> + 空白 + 第一個<span>
                    secondTextBoxValue = $"{firstStrong} {spanText}";
                }
                else
                {
                    // 找不到 <span> → 第二個 textbox = 第一個 textbox
                    secondTextBoxValue = firstTextBoxValue;
                }

                // 6. 寫回 UI
                if (textBox2.InvokeRequired)
                {
                    textBox2.Invoke(new Action(() =>
                    {
                        textBox2.Text = secondTextBoxValue;
                        textBox1.Text = firstTextBoxValue;
                    }));
                }
                else
                {
                    textBox2.Text = secondTextBoxValue;
                    textBox1.Text = firstTextBoxValue;
                }
            }
        }
        string NormalizeTitle(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            // 可依需求再調整
            var normalized = s.Trim();

            // 全形空白 → 半形空白
            normalized = normalized.Replace('　', ' ');

            // 移除空白（有時候空白差一格就會影響距離）
            normalized = string.Concat(normalized.Where(c => !char.IsWhiteSpace(c)));

            // 你如果有中日文全形/半形轉換需求，可以再加一層
            // 這裡先簡單處理
            return normalized;
        }

        int LevenshteinDistance(string s, string t)
        {
            if (s == t) return 0;
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,    // 刪除
                            d[i, j - 1] + 1     // 插入
                        ),
                        d[i - 1, j - 1] + cost // 取代
                    );
                }
            }

            return d[n, m];
        }

        // 回傳 0~1 之間，1 = 完全相同
        double Similarity(string a, string b)
        {
            var s = NormalizeTitle(a);
            var t = NormalizeTitle(b);

            if (s.Length == 0 && t.Length == 0) return 1.0;
            if (s.Length == 0 || t.Length == 0) return 0.0;

            int dist = LevenshteinDistance(s, t);
            int maxLen = Math.Max(s.Length, t.Length);

            return 1.0 - (double)dist / maxLen;
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            if (_wmp != null)
            {
                try { _wmp.close(); } catch { }
                _wmp = null;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_showDragText)
            {
                string text = "請將 MP4 檔案拖曳到此視窗";

                using (Font drawFont = new Font("Microsoft JhengHei", 24, FontStyle.Bold))
                using (SolidBrush semiBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))  // 120 = 半透明
                {
                    SizeF size = e.Graphics.MeasureString(text, drawFont);

                    float x = (this.ClientSize.Width - size.Width) / 2;
                    float y = (this.ClientSize.Height - size.Height) / 2;

                    e.Graphics.DrawString(text, drawFont, semiBrush, x, y);
                }
            }
        }
    }
}

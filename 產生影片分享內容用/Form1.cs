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
using HtmlAgilityPack;
using System.Web;   // HttpUtility.UrlEncode 用
using ImageMagick;
using System.Text.Json; // 用於暫存資料的 JSON 序列化

namespace 產生影片分享內容用
{
    public partial class Form1 : Form
    {
        private DragOverlayPanel overlayPanel;
        private WindowsMediaPlayer _wmp;
        private readonly string _cacheFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "autosave.json");
        private bool _isRestoring = false; // 防止恢復期間重複觸發存檔

        // 定義一個類別來記錄每個頁籤獨立的控制項控制權
        public class VideoPageContext
        {
            public TabPage Page { get; set; }
            public TextBox TextBox1 { get; set; } // 中文標題
            public TextBox TextBox2 { get; set; } // 日文標題
            public TextBox TextBox3 { get; set; } // 大小(GB)
            public TextBox TextBox4 { get; set; } // 影片副檔名
            public TextBox TextBox7 { get; set; } // 影片下載點
            public TextBox TextBox8 { get; set; } // 種子碼
            public ComboBox ComboBox1 { get; set; } // 影片格式
            public ComboBox ComboBox2 { get; set; } // 馬賽克
            public Label Label10 { get; set; }     // 字數提示外框
        }

        // 用於 JSON 暫存的資料結構
        public class VideoSaveData
        {
            public string PageText { get; set; }
            public string TextBox1Text { get; set; }
            public string TextBox2Text { get; set; }
            public string TextBox3Text { get; set; }
            public string TextBox4Text { get; set; }
            public string TextBox7Text { get; set; }
            public string TextBox8Text { get; set; }
            public string ComboBox1Text { get; set; }
            public string ComboBox2Text { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            _wmp = new WindowsMediaPlayer();
            InitializeDragOverlay();
        }

        private void InitializeDragOverlay()
        {
            overlayPanel = new DragOverlayPanel();
            overlayPanel.Dock = DockStyle.Fill;
            overlayPanel.OverlayText = "請將 MP4 或 webp 檔案拖曳到此視窗";
            overlayPanel.ShowText = false;
            overlayPanel.Visible = false;

            this.Controls.Add(overlayPanel);
            overlayPanel.BringToFront();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.outputFilePath.Length <= 0)
                textBox9.Text = System.IO.Directory.GetCurrentDirectory();
            else
                textBox9.Text = Properties.Settings.Default.outputFilePath;

            // 嘗試從暫存檔恢復資料
            RestoreStateFromCache();
        }

        // 動態新增頁籤與內部所有控制項
        private VideoPageContext AddNewTabPage(VideoSaveData savedData = null)
        {
            string pageName = savedData != null ? savedData.PageText : $"新影片 {tabControl1.TabPages.Count + 1}";
            TabPage newPage = new TabPage(pageName);
            newPage.UseVisualStyleBackColor = true;

            // 建立控制項
            Label l1 = new Label { Text = "日文標題", Location = new Point(17, 16), AutoSize = true };
            TextBox t2 = new TextBox { Location = new Point(105, 13), Size = new Size(615, 27), Text = savedData?.TextBox2Text ?? "" }; // 日文

            Label l2 = new Label { Text = "中文標題", Location = new Point(17, 57), AutoSize = true };
            TextBox t1 = new TextBox { Location = new Point(105, 53), Size = new Size(615, 27), Text = savedData?.TextBox1Text ?? "" }; // 中文

            Label l3 = new Label { Text = "大小(GB)", Location = new Point(17, 101), AutoSize = true };
            TextBox t3 = new TextBox { Location = new Point(105, 98), Size = new Size(88, 27), Text = savedData?.TextBox3Text ?? "" };

            Label l4 = new Label { Text = "影片格式", Location = new Point(202, 101), AutoSize = true };
            ComboBox c1 = new ComboBox { Location = new Point(280, 98), Size = new Size(154, 27), Text = savedData?.ComboBox1Text ?? "FHD/1088P" };
            c1.Items.AddRange(new object[] { "LD/240P", "LD/360P", "SD/416P", "SD/480P", "HD/720P", "FHD/1080P", "FHD/1088P", "2K/1440P", "4K/2160P", "8K/4320P" });

            Label l5 = new Label { Text = "影片副檔名", Location = new Point(453, 101), AutoSize = true };
            TextBox t4 = new TextBox { Location = new Point(546, 98), Size = new Size(50, 27), Text = savedData?.TextBox4Text ?? "MP4" };

            Label l6 = new Label { Text = "馬賽克", Location = new Point(595, 101), AutoSize = true };
            ComboBox c2 = new ComboBox { Location = new Point(657, 98), Size = new Size(64, 27), Text = savedData?.ComboBox2Text ?? "有碼" };
            c2.Items.AddRange(new object[] { "有碼", "無碼" });

            Label l7 = new Label { Text = "影片下載點", Location = new Point(17, 146), AutoSize = true };
            TextBox t7 = new TextBox { Location = new Point(105, 142), Size = new Size(615, 27), Text = savedData?.TextBox7Text ?? "" };

            Label l8 = new Label { Text = "種子碼", Location = new Point(17, 186), AutoSize = true };
            TextBox t8 = new TextBox { Location = new Point(105, 182), Size = new Size(615, 27), Text = savedData?.TextBox8Text ?? "" };

            Label l10 = new Label { BorderStyle = BorderStyle.Fixed3D, Location = new Point(17, 230), Size = new Size(539, 29) };

            // 將控制項加入 Page
            newPage.Controls.AddRange(new Control[] { l1, t2, l2, t1, l3, t3, l4, c1, l5, t4, l6, c2, l7, t7, l8, t8, l10 });

            // 綁定上下文關聯
            VideoPageContext ctx = new VideoPageContext
            {
                Page = newPage,
                TextBox1 = t1,
                TextBox2 = t2,
                TextBox3 = t3,
                TextBox4 = t4,
                TextBox7 = t7,
                TextBox8 = t8,
                ComboBox1 = c1,
                ComboBox2 = c2,
                Label10 = l10
            };
            newPage.Tag = ctx;

            // 綁定事件
            t2.TextChanged += (s, ev) => {
                t2.Text = t2.Text.Replace("...", "…");

                // 如果不是在載入暫存，日文變更時才自動蓋掉中文
                if (!_isRestoring && string.IsNullOrEmpty(t1.Text)) t1.Text = t2.Text;

                // 擷取日文標題第一個空白之前的文字資料作為頁籤標題
                string rawText = t2.Text.Trim();
                if (!string.IsNullOrEmpty(rawText))
                {
                    int spaceIndex = rawText.IndexOf(' ');
                    if (spaceIndex == -1) spaceIndex = rawText.IndexOf(' '); // 全形空白
                    newPage.Text = spaceIndex > 0 ? rawText.Substring(0, spaceIndex) : rawText;
                }
                else
                {
                    newPage.Text = "新影片";
                }
                SaveStateToCache(); // 資料變更，觸發暫存
            };

            t1.TextChanged += (s, ev) => {
                int maxLenhth = Encoding.UTF8.GetBytes("HKGL-001 可愛的羅莉學生放學後和老師在愛情酒店課後教學  伊藤はる").Length;
                int nowLenhth = Encoding.UTF8.GetBytes(t1.Text).Length;
                if (nowLenhth > maxLenhth)
                {
                    l10.Text = "中文標題超過 [ " + (nowLenhth - maxLenhth) + " ] 個字元，請修改！";
                    l10.ForeColor = Color.Red;
                }
                else
                {
                    l10.Text = $"中文標題共計 [ {nowLenhth} ] 個字元，剩餘 [ {maxLenhth - nowLenhth} ] 個字元可輸入。";
                    l10.ForeColor = Color.Green;
                }
                SaveStateToCache(); // 資料變更，觸發暫存
            };

            // 其餘控制項欄位變更時也觸發自動存檔
            t3.TextChanged += (s, ev) => SaveStateToCache();
            t4.TextChanged += (s, ev) => SaveStateToCache();
            t7.TextChanged += (s, ev) => SaveStateToCache();
            t8.TextChanged += (s, ev) => SaveStateToCache();
            c1.SelectedIndexChanged += (s, ev) => SaveStateToCache();
            c2.SelectedIndexChanged += (s, ev) => SaveStateToCache();

            tabControl1.TabPages.Add(newPage);
            if (!_isRestoring) tabControl1.SelectedTab = newPage;

            return ctx;
        }

        // 儲存目前所有分頁狀態至本機檔案
        private void SaveStateToCache()
        {
            if (_isRestoring) return;

            try
            {
                var dataList = new List<VideoSaveData>();
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Tag is VideoPageContext ctx)
                    {
                        dataList.Add(new VideoSaveData
                        {
                            PageText = page.Text,
                            TextBox1Text = ctx.TextBox1.Text,
                            TextBox2Text = ctx.TextBox2.Text,
                            TextBox3Text = ctx.TextBox3.Text,
                            TextBox4Text = ctx.TextBox4.Text,
                            TextBox7Text = ctx.TextBox7.Text,
                            TextBox8Text = ctx.TextBox8.Text,
                            ComboBox1Text = ctx.ComboBox1.Text,
                            ComboBox2Text = ctx.ComboBox2.Text
                        });
                    }
                }

                string jsonString = JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_cacheFilePath, jsonString, Encoding.UTF8);
            }
            catch { /* 忽略暫存寫入失敗，避免卡住主要 UI 流程 */ }
        }

        // 讀取本機暫存檔恢復先前作業
        private void RestoreStateFromCache()
        {
            if (!File.Exists(_cacheFilePath))
            {
                AddNewTabPage(); // 無暫存檔，正常開啟空分頁
                return;
            }

            _isRestoring = true;
            try
            {
                string jsonString = File.ReadAllText(_cacheFilePath, Encoding.UTF8);
                var dataList = JsonSerializer.Deserialize<List<VideoSaveData>>(jsonString);

                if (dataList != null && dataList.Count > 0)
                {
                    foreach (var data in dataList)
                    {
                        AddNewTabPage(data);
                    }
                }
                else
                {
                    AddNewTabPage();
                }
            }
            catch
            {
                AddNewTabPage(); // 如果暫存檔損毀，預防性開啟空白頁
            }
            finally
            {
                _isRestoring = false;
                if (tabControl1.TabPages.Count > 0) tabControl1.SelectedIndex = 0;
            }
        }

        // 取得目前作用中頁籤的控制項上下文
        private VideoPageContext GetCurrentContext()
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Tag is VideoPageContext ctx)
            {
                return ctx;
            }
            return null;
        }

        private void btnAddTab_Click(object sender, EventArgs e)
        {
            AddNewTabPage();
            SaveStateToCache();
        }

        private void btnDeleteTab_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count <= 1)
            {
                MessageBox.Show("請至少保留一個影片分頁。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("確定要刪除目前選取的分頁嗎？未儲存的資料將會遺失。", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                SaveStateToCache(); // 更新暫存
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "請選擇寫入檔案";
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
            var ctx = GetCurrentContext();
            if (ctx == null) return;

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
                ctx.TextBox1.Text, ctx.ComboBox1.Text, ctx.TextBox4.Text, ctx.ComboBox2.Text,
                ctx.TextBox2.Text,
                ctx.TextBox3.Text, ctx.TextBox7.Text, ctx.TextBox8.Text.Length <= 4 ? "" : ctx.TextBox8.Text.Substring(0, 4), ctx.TextBox8.Text);

            System.IO.File.WriteAllText(textBox9.Text, txt_temp + txt);

            MessageBox.Show("寫入完成");

            // 寫入完成後移除該分頁
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.Remove(ctx.Page);
            }
            else
            {
                ctx.TextBox1.Text = "";
                ctx.TextBox2.Text = "";
                ctx.TextBox3.Text = "";
                ctx.TextBox4.Text = "MP4";
                ctx.TextBox7.Text = "";
                ctx.TextBox8.Text = "";
                ctx.ComboBox1.SelectedIndex = 6; // 預設 FHD/1088P
                ctx.ComboBox2.SelectedIndex = 0; // 預設 有碼
            }
            SaveStateToCache(); // 完成寫入後更新並覆蓋本機暫存檔
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

            var webpFiles = files.Where(f => string.Equals(Path.GetExtension(f), ".webp", StringComparison.OrdinalIgnoreCase)).ToList();
            var mp4Files = files.Where(f => string.Equals(Path.GetExtension(f), ".mp4", StringComparison.OrdinalIgnoreCase)).ToList();

            if (!mp4Files.Any() && !webpFiles.Any())
            {
                MessageBox.Show("沒有拖進任何 .mp4 或 .webp 檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (webpFiles.Any())
            {
                foreach (var webp in webpFiles)
                {
                    try { ConvertWebpToJpg(webp); }
                    catch (Exception ex) { MessageBox.Show($"轉換圖片失敗：{Path.GetFileName(webp)}\r\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }

            if (mp4Files.Any())
            {
                var ctx = GetCurrentContext();
                if (ctx == null) return;

                long totalBytes = mp4Files.Sum(f => new FileInfo(f).Length);
                double sizeGB = totalBytes / (1024d * 1024d * 1024d);
                ctx.TextBox3.Text = sizeGB.ToString("0.0#");

                string firstFile = mp4Files[0];
                try
                {
                    IWMPMedia media = _wmp.newMedia(firstFile);
                    string title = media.getItemInfo("Title");
                    if (string.IsNullOrWhiteSpace(title)) title = Path.GetFileNameWithoutExtension(firstFile);
                    title = title.Replace(" - 見放題ch デラックス - FANZA月額動画", "").Trim();

                    string heightStr = media.getItemInfo("WM/VideoHeight");
                    int basisP;
                    if (!int.TryParse(heightStr, out basisP))
                    {
                        string widthStr = media.getItemInfo("WM/VideoWidth");
                        if (!int.TryParse(widthStr, out basisP)) basisP = 0;
                    }

                    SetResolutionComboByNearestP(ctx.ComboBox1, basisP);

                    await FetchJavdbInfoAsync(ctx, title);

                    var newDir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(firstFile)!, ctx.TextBox2.Text));
                    foreach (var mp4 in mp4Files)
                    {
                        var destPath = Path.Combine(newDir.FullName, Path.GetFileName(mp4));
                        File.Move(mp4, destPath);
                    }

                    var copyResourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Copy");
                    if (Directory.Exists(copyResourceDir))
                    {
                        string txtPath = Path.Combine(copyResourceDir, "he01204046@伊莉論壇.txt");
                        string urlPath = Path.Combine(copyResourceDir, "伊莉討論區.url");
                        if (File.Exists(txtPath)) File.Copy(txtPath, Path.Combine(newDir.FullName, "he01204046@伊莉論壇.txt"), true);
                        if (File.Exists(urlPath)) File.Copy(urlPath, Path.Combine(newDir.FullName, "伊莉討論區.url"), true);
                    }

                    Clipboard.SetText(title);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"讀取影片資訊失敗：{Path.GetFileName(firstFile)}\r\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            SaveStateToCache(); // 拖曳排程完成後進行一次存檔
        }

        private void SetResolutionComboByNearestP(ComboBox cb, int p)
        {
            if (p <= 0) { cb.SelectedIndex = -1; return; }
            var options = new[] {
                new { Text = "LD/240P",   P = 240 }, new { Text = "LD/360P",   P = 360 },
                new { Text = "SD/416P",   P = 416 }, new { Text = "SD/480P",   P = 480 },
                new { Text = "HD/720P",   P = 720 }, new { Text = "FHD/1080P", P = 1080 },
                new { Text = "FHD/1088P", P = 1088 }, new { Text = "2K/1440P",  P = 1440 },
                new { Text = "4K/2160P",  P = 2160 }, new { Text = "8K/4320P",  P = 4320 }
            };

            var best = options.OrderBy(o => Math.Abs(o.P - p)).First();
            for (int i = 0; i < cb.Items.Count; i++)
            {
                if (string.Equals(cb.Items[i].ToString(), best.Text, StringComparison.OrdinalIgnoreCase))
                {
                    cb.SelectedIndex = i;
                    return;
                }
            }
            cb.SelectedIndex = -1;
        }

        private async Task FetchJavdbInfoAsync(VideoPageContext ctx, string rawTitle)
        {
            if (ctx.TextBox2.InvokeRequired)
            {
                ctx.TextBox2.Invoke(new Action(() => { ctx.TextBox2.Text = rawTitle; ctx.TextBox1.Text = rawTitle; }));
            }
            else
            {
                ctx.TextBox2.Text = rawTitle; ctx.TextBox1.Text = rawTitle;
            }
            if (string.IsNullOrWhiteSpace(rawTitle)) return;

            using (var browserForm = new JavdbBrowserForm())
            {
                browserForm.Show();
                string query = System.Web.HttpUtility.UrlEncode(rawTitle.Trim());
                string searchUrl = $"https://javdb.com/search?q={query}&f=all";

                string searchHtml = await browserForm.LoadPageAndGetHtmlAsync(
                    searchUrl, new[] { "div.movie-list" },
                    "目前畫面看起來不像正常的搜尋結果頁，可能是機器人驗證或其他畫面。請在此視窗中完成驗證或操作，直到搜尋結果出現，再按「驗證完成，繼續」。"
                );
                if (string.IsNullOrEmpty(searchHtml)) { MessageBox.Show("無法取得搜尋頁面 HTML。"); browserForm.Close(); return; }

                var searchDoc = new HtmlAgilityPack.HtmlDocument();
                searchDoc.LoadHtml(searchHtml);
                var movieListNode = searchDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'movie-list')]");
                if (movieListNode == null) { MessageBox.Show("搜尋頁找不到 movie-list 區塊。"); browserForm.Close(); return; }

                var movieLinks = movieListNode.SelectNodes(".//a[contains(@class,'box')]") ?? new HtmlAgilityPack.HtmlNodeCollection(null);
                var normalizedTarget = NormalizeTitle(rawTitle);
                var best = movieLinks.Select(a => {
                    var title = a.GetAttributeValue("title", "").Trim();
                    var score = Similarity(title, normalizedTarget);
                    return new { Node = a, Title = title, Score = score };
                }).Where(x => !string.IsNullOrEmpty(x.Title)).OrderByDescending(x => x.Score).FirstOrDefault();

                if (best == null) { MessageBox.Show("找不到與標題相符的影片。"); browserForm.Close(); return; }

                var relativeHref = best.Node.GetAttributeValue("href", "");
                var bodyNode = searchDoc.DocumentNode.SelectSingleNode("//body");
                var domain = bodyNode?.GetAttributeValue("data-domain", "https://javdb.com") ?? "https://javdb.com";
                var detailUrl = new Uri(new Uri(domain), relativeHref).ToString();

                string detailHtml = await browserForm.LoadPageAndGetHtmlAsync(
                    detailUrl, new[] { "div.video-detail" },
                    "目前畫面看起來不像正常的詳細頁，可能是機器人驗證／18+確認或其他畫面。請在此視窗中完成操作，直到詳細內容出現，再按「驗證完成，繼續」。"
                );
                if (string.IsNullOrEmpty(detailHtml)) { MessageBox.Show("無法取得詳細頁面 HTML。"); browserForm.Close(); return; }

                var detailDoc = new HtmlAgilityPack.HtmlDocument();
                detailDoc.LoadHtml(detailHtml);
                var sectionNode = detailDoc.DocumentNode.SelectSingleNode("//section[contains(@class,'section')]");
                var videoDetailNode = sectionNode?.SelectSingleNode(".//div[contains(@class,'video-detail')]");
                var titleContainer = videoDetailNode?.SelectSingleNode(".//h2[contains(@class,'title') and contains(@class,'is-4')]");

                if (titleContainer == null) { MessageBox.Show("找不到細節區塊。"); browserForm.Close(); return; }

                var strongNodes = titleContainer.SelectNodes(".//strong");
                if (strongNodes == null || strongNodes.Count < 1) { MessageBox.Show("找不到識別碼。"); browserForm.Close(); return; }

                string firstStrong = strongNodes[0].InnerText.Trim();
                string secondStrong = strongNodes.Count > 1 ? strongNodes[1].InnerText.Trim() : string.Empty;
                string firstTextBoxValue = string.IsNullOrEmpty(secondStrong) ? firstStrong : $"{firstStrong} {secondStrong}";

                HtmlAgilityPack.HtmlNode spanNode = titleContainer.SelectSingleNode(".//span[contains(@class,'origin-title')]") ?? titleContainer.SelectSingleNode(".//span");
                string secondTextBoxValue = spanNode != null ? $"{firstStrong} {spanNode.InnerText.Trim()}" : firstTextBoxValue;

                if (ctx.TextBox2.InvokeRequired)
                {
                    ctx.TextBox2.Invoke(new Action(() => { ctx.TextBox2.Text = secondTextBoxValue; ctx.TextBox1.Text = firstTextBoxValue; }));
                }
                else
                {
                    ctx.TextBox2.Text = secondTextBoxValue; ctx.TextBox1.Text = firstTextBoxValue;
                }
                browserForm.Close();
            }
            SaveStateToCache(); // 網路抓取完畢資料填入後，進行暫存更新
        }

        string NormalizeTitle(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            var normalized = s.Trim().Replace(' ', ' ');
            return string.Concat(normalized.Where(c => !char.IsWhiteSpace(c)));
        }

        int LevenshteinDistance(string s, string t)
        {
            if (s == t) return 0;
            if (string.IsNullOrEmpty(s)) return t.Length;
            if (string.IsNullOrEmpty(t)) return s.Length;
            int n = s.Length, m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        double Similarity(string a, string b)
        {
            var s = NormalizeTitle(a); var t = NormalizeTitle(b);
            if (string.IsNullOrEmpty(s) && string.IsNullOrEmpty(t)) return 1.0;
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t)) return 0.0;
            if (s == t) return 1.0;
            if (t.Contains(s, StringComparison.OrdinalIgnoreCase)) return 0.98;
            bool allFound = true;
            foreach (var c in s) { if (!t.Contains(c)) { allFound = false; break; } }
            if (allFound) return 0.95;
            int dist = LevenshteinDistance(s, t);
            int maxLen = Math.Max(s.Length, t.Length);
            return 1.0 - (double)dist / maxLen;
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
            else { e.Effect = DragDropEffects.None; }
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            if (overlayPanel != null) { overlayPanel.ShowText = false; overlayPanel.Visible = false; }
        }

        private void ConvertWebpToJpg(string webpPath)
        {
            string dir = Path.GetDirectoryName(webpPath)!;
            string filenameWithoutExt = Path.GetFileNameWithoutExtension(webpPath);
            string jpgPath = Path.Combine(dir, filenameWithoutExt + ".jpg");
            using (var image = new MagickImage(webpPath))
            {
                image.Format = MagickFormat.Jpeg;
                image.Quality = 100;
                image.Write(jpgPath);
            }
            File.Delete(webpPath);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (_wmp != null) { try { _wmp.close(); } catch { } _wmp = null; }
        }
    }
}
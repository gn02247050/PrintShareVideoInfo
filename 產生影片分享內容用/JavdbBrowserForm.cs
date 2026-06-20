using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json; // 用來解 WebView2 傳回的 JSON 字串

namespace 產生影片分享內容用
{
    public partial class JavdbBrowserForm : Form
    {
        private TaskCompletionSource<bool> _userContinueTcs;

        public JavdbBrowserForm()
        {
            InitializeComponent();
        }

        private async void JavdbBrowserForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化 WebView2，一次即可，之後重複用同一個核心
                await webView21.EnsureCoreWebView2Async();
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化瀏覽器失敗：" + ex.Message);
                Close();
            }
        }

        /// <summary>
        /// 載入指定 URL，檢查頁面是否符合期望（某些 CSS selector 出現）。
        /// 若不符合 → 顯示提示，等使用者操作驗證後按「繼續」，最後回傳目前頁面的 HTML。
        /// </summary>
        /// <param name="url">要瀏覽的網址</param>
        /// <param name="requiredSelectors">
        /// 預期頁面上應該要出現的 CSS selector（只要其中一個有命中就算 OK）
        /// </param>
        /// <param name="hintWhenBlocked">當判斷不符合期望時，顯示給使用者的提示文字</param>
        public async Task<string> LoadPageAndGetHtmlAsync(string url, string[] requiredSelectors, string hintWhenBlocked)
        {
            // 確保 CoreWebView2 存在
            await webView21.EnsureCoreWebView2Async();

            // 先把提示區復原
            labelHint.Text = string.Empty;
            btnContinue.Visible = false;
            btnContinue.Enabled = false;

            // 1. 導頁
            var navTcs = new TaskCompletionSource<bool>();
            void NavCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                webView21.CoreWebView2.NavigationCompleted -= NavCompleted;
                navTcs.TrySetResult(true);
            }

            webView21.CoreWebView2.NavigationCompleted += NavCompleted;
            webView21.CoreWebView2.Navigate(url);

            await navTcs.Task; // 等這次導覽完成

            // 2. 用 JS 檢查 DOM 是否看起來「正常」
            //    requiredSelectors 是一組 CSS selector，只要有一個命中就算 OK
            string selectorsJson = JsonConvert.SerializeObject(requiredSelectors);
            string jsCheck = $@"
            (function(){{
                var sels = {selectorsJson};
                var ok = false;
                for (var i = 0; i < sels.length; i++) {{
                    if (document.querySelector(sels[i])) {{
                        ok = true;
                        break;
                    }}
                }}
                return {{ ok: ok }};
            }})();";

            string checkResultJson = await webView21.ExecuteScriptAsync(jsCheck);

            // 直接反序列化成物件
            var checkResult = JsonConvert.DeserializeObject<PageCheckResult>(checkResultJson);
            bool ok = checkResult.ok;

            if (!ok)
            {
                // 判斷為「不像正常頁面」（可能是機器人驗證、18+、或其他擋頁）
                // → 顯示提示，等使用者自己操作完成再按「繼續」
                labelHint.Text = hintWhenBlocked;
                btnContinue.Visible = true;
                btnContinue.Enabled = true;

                _userContinueTcs = new TaskCompletionSource<bool>();
                await _userContinueTcs.Task; // 在這裡「停住」等使用者按鈕
            }

            // 3. 不管是自動就 OK，還是使用者介入後 OK
            //    最後抓目前整個頁面的 HTML
            string jsGetHtml = "document.documentElement.outerHTML";
            string htmlJson = await webView21.ExecuteScriptAsync(jsGetHtml);
            string html = JsonConvert.DeserializeObject<string>(htmlJson);

            // 抓完這頁 HTML 就回傳
            return html;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            // 使用者按「驗證完成，繼續」
            btnContinue.Enabled = false;
            btnContinue.Visible = false;
            labelHint.Text = "正在繼續處理，請稍候…";

            _userContinueTcs?.TrySetResult(true);
        }
    }
    // 定義一個對應的 C# 類別
    public class PageCheckResult
    {
        public bool ok { get; set; }
    }
}

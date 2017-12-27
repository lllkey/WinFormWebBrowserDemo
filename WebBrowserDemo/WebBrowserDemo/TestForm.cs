using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using WebBrowserDemo.Utils;

namespace WebBrowserDemo
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]//COM+组件可见
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            LogHelper.GetLogHelper().LogInit();
        }

        [DllImport("user32.dll")]
        public extern static int FindWindow(string lpclassname, string lpwindowname);

        [DllImport("user32.dll")]
        public extern static void SetForegroundWindow(int handle);

        private void MainFormLoad(object sender, EventArgs e)
        {
            string basePath = Application.StartupPath;
            int binPos = basePath.LastIndexOf("bin");
            basePath = basePath.Remove(binPos);
            string htmlPath = basePath + @"html\testShow.htm";
            webBrowser1.Navigate("about:blank");

            LogHelper.GetLogHelper().CreateLog(htmlPath);
            
            // 读取文件内容，再加载
            string content = File.ReadAllText(htmlPath);
            webBrowser1.Document.Write(content);

            // 直接导航到本地文件
            webBrowser1.Navigate(htmlPath);
         
            // https
            int iHandle = FindWindow(null, "安全警报");
            SetForegroundWindow(iHandle);
            System.Windows.Forms.SendKeys.SendWait("Y%");

            webBrowser1.ObjectForScripting = this;//具体公开的对象,这里可以公开自定义对象
            
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(ShowTitle);
            webBrowser1.DocumentTitleChanged += OnDocumentTitleChanged;
            webBrowser1.StatusTextChanged += OnStatusTextChanged;
            webBrowser1.ProgressChanged += OnProgressChanged;
            webBrowser1.CanGoBackChanged += OnWebBrwoserChanged;
            webBrowser1.CanGoForwardChanged += OnWebBrwoserChanged;
        }

        void OnWebBrwoserChanged(object objSrc, EventArgs args)
        {
            btnBack.Enabled = webBrowser1.CanGoBack;
            btnForward.Enabled = webBrowser1.CanGoForward;
            if (args == null)
            {
                return;
            }
            LogHelper.GetLogHelper().CreateLog(args.ToString() +"   "+args.GetType().TypeHandle.ToString());
        }

        // Event handlers for caption bar and status bar.
        void OnDocumentTitleChanged(object objSrc, EventArgs args)
        {
            WebBrowser wb = objSrc as WebBrowser;
            this.Text = wb.DocumentTitle;

            if (wb.Url != null && wb.Url.ToString().Length > 0)
                this.Text += " \x2014 " + wb.Url.ToString();
        }

        void OnStatusTextChanged(object objSrc, EventArgs args)
        {
            WebBrowser wb = objSrc as WebBrowser;
            statusLabel.Text = wb.StatusText;
        }

        void OnProgressChanged(object obj, WebBrowserProgressChangedEventArgs args)
        {
            if (args.MaximumProgress > 0 && args.CurrentProgress <= args.MaximumProgress && (htmlProgress.Visible = (args.CurrentProgress != args.MaximumProgress)))
                htmlProgress.Value = (int)(100 * args.CurrentProgress / args.MaximumProgress);
        }

        private void ShowTitle(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle;
            textUrl.Text = webBrowser1.Url.OriginalString;
        }

        private void btnAlert_Click(object sender, EventArgs e)
        {
            // 获取html中内容
            MessageBox.Show(webBrowser1.Document.Body.OuterText);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            // 调用js中的函数
            webBrowser1.Document.InvokeScript("Run", new object[] { "CShareFunction" });
        }

        // js的函数调用，js中调用方法：window.external.ShowMsg(str);
        public void ShowMsg(string msg)
        {
            MessageBox.Show("hahaha: "+msg);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string url = textUrl.Text;
            if(!url.Contains(":") && !url.StartsWith("http"))
            {
                url = "http://" + url;
            }
            webBrowser1.Navigate(url);
        }

        private void textUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnGo_Click(null, null);
        }

        private void textUrl_MouseClick(object sender, MouseEventArgs e)
        {
            textUrl.SelectAll();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.IO;
using QTIUtility;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace EACDMLinqUtiliy
{
    public partial class LoginMobile : Form
    {
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        public string authUrl = "http://p8500:81/webapps/ea-eacdata-bb_bb60/auth_1.jsp";
        public string token = null;
        private bool loginSuccess = false;
        private string waitMsg = "Please wait...";
        private Uri loginPage = null;
        private int id = 0;
        private string loginCookie = null;
        private WebBrowser wb = null;
        public string BbPluginStem;
        int testInterval = 500;
        int loginTries = 5;
        TimeSpan ts;
        private string loginStem = "/webapps/login/";
        private string loginUrl = null;
        

        public LoginMobile()
        {
            InitializeComponent();
            authUrl = BbQuery.getBbLoginUrl();
            Uri LoginUrl = new Uri(BbQuery.BbUrl);
            loginUrl = LoginUrl.Scheme + "://" + LoginUrl.Host + ":" + LoginUrl.Port + loginStem;
            
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            


        }
        

        private void btnBb_Click(object sender, EventArgs e)
        {
            // this button only logs out
            // may be able to do this on initialise
            wb.Url = new Uri(authUrl + "?logout=1");
            tsStatus.Text = waitMsg;




        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
        }

        private int count = 0;
        private void wbBb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            Debug.WriteLine("At doc completed: Url = " + e.Url.AbsoluteUri + " count = " + count.ToString() + " elapsed = " + ts.TotalMilliseconds.ToString());
            tbMemo.AppendText("At doc completed: Url = " + e.Url.AbsoluteUri + " count = " + count.ToString() + " elapsed = " + ts.TotalMilliseconds.ToString() + Environment.NewLine);
            
            bool ok = false;
            HtmlDocument doc = null;
            if (wb.Document.Window.Frames.Count > 0)
            {
                Debug.WriteLine("About to traverse with frames." );
                for (int i = 0; i < wb.Document.Window.Frames.Count; i++)
                {
                    doc = wb.Document.Window.Frames[i].Document;
                    button = null;

                    ok = traverse(doc.All, tbUser_id.Text.Trim(), tbPassword.Text.Trim(), 0);
                    if (ok)
                    {
                        break;
                    }
                }
            }
            else
            {
                doc = wb.Document;
                button = null;
                Debug.WriteLine("About to traverse noframes.");
                ok = traverse(doc.All, tbUser_id.Text.Trim(), tbPassword.Text.Trim(), 0);
            }
            
            loginSuccess = false;
            tbMemo.AppendText("At completed after traverse: loginsuccess = "+loginSuccess.ToString()+" ok: "+ok.ToString()+" button is "+((button == null)? "null":"not null"));
            if (ok && (button != null))
            {
                button.InvokeMember("click");
                tbMemo.AppendText(Environment.NewLine+"clicked" + Environment.NewLine);
            }

            if (ok /* && !e.Url.AbsolutePath.ToUpper().Contains("LOGIN") */ && !loginSuccess)
            {

                loginCookie = wb.Document.Cookie;
                UpdateCookies(loginCookie);
                int counter = loginTries;
                while (!loginSuccess && counter > 0)
                {
                    Logger.__SpecialLogger("Counter: " + counter.ToString());
                    counter--;
                    Thread.Sleep(testInterval);
                    testLogin();
                }
                if (loginSuccess)
                {
                   // wb.Stop();
                    wb.Navigated -= new WebBrowserNavigatedEventHandler(wbBb_Navigated);
                    wb.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
                    InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                    wb.Document.Window.Navigate(new Uri(authUrl));
                    wb.Hide();
                    btnCancel.Text = "Logged in";
                    tsStatus.Text = BbQuery.BbLoggedInName;
                }
                else
                {
                  //  wb.Stop();
                    MessageBox.Show("Not logged in!!");
                }
            }

        }

        private void wbBb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            
        }
        private void testLogin()
        {
            //tbMemo.AppendText("At text login " + authUrl + Environment.NewLine);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
            req.UserAgent = BbQuery.BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            req.CookieContainer = BbQuery.cc;
            req.Timeout = 10000;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                if (resp.ContentLength > 0)
                {
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                //    tbMemo.AppendText("test login status description = " + resp.StatusDescription + Environment.NewLine);
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr, XmlReadMode.InferSchema);
                    if (ds.Tables.Count > 0)
                    {
                        string name = "";
                        string firstname = "";
                        string lastname = "";
                        string username = "";
                        if (ds.Tables.Contains("user"))
                        {
                            DataTable user = ds.Tables["user"];
                            id = Convert.ToInt32(user.Rows[0]["id"]);
                            firstname = user.Rows[0]["firstname"].ToString();
                            lastname = user.Rows[0]["lastname"].ToString();
                            name = user.Rows[0]["firstname"].ToString() + " " + user.Rows[0]["lastname"];
                            username = user.Rows[0]["username"].ToString();
                            //    Logger.__SpecialLogger("Lastname = " + lastname);
                            if (user.Columns.Contains("version"))
                            {
                                BbQuery.BbVersion = user.Rows[0]["version"].ToString();
                            }
                            if (user.Columns.Contains("pluginversion"))
                            {
                                BbQuery.BbPluginVersion = user.Rows[0]["pluginversion"].ToString();
                                //    Logger.__SpecialLogger("BbPluginVersion: " + BbQuery.BbPluginVersion);
                            }
                            string Bbstime = user.Rows[0]["servertime"].ToString();
                            BbQuery.BbServerTime = Bbstime.Substring(0, Bbstime.Length - 3);
                            try
                            {
                                DateTime bbNow = Convert.ToDateTime(BbQuery.BbServerTime);
                                BbQuery.BbServerTimeSpan = bbNow - DateTime.Now;
                            }
                            catch
                            {

                            }
                        }
                        if (!firstname.Equals("") && !lastname.Equals("") /*&& (firstname.ToUpper().IndexOf("BLACKBOARD") < 0) */
                            && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {
                            loginSuccess = true;
                            BbQuery.BbLoginString = name + " logged in.";
                            BbQuery.BbLoggedInName = name;
                            btnCancel.Text = "Return";
                            //  BbQuery.cc = cc;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                        }
                    }
                    else
                    {
                        //  Logger.__SpecialLogger("Response OK but no table");
                      //  tbMemo.AppendText("Response OK but no table" + Environment.NewLine);
                    }
                    sr.Close();
                }
                else
                {
                    
                   // tbMemo.AppendText("At test with Response OK but with NO content" + Environment.NewLine);
                }
            }
            else
            {
                //  Logger.__SpecialLogger("At test with Response not OK (" + resp.StatusDescription+")");
              //  tbMemo.AppendText("At test with Response not OK (" + resp.StatusDescription + ")" + Environment.NewLine);
            }
        }
        private void UpdateCookies(string cookies)
        {
            Uri theUrl = new Uri(authUrl);
            string[] cooks = cookies.Split(';');
            foreach (string cooky in cooks)
            {
                if (cooky.Split('=')[0].Trim().ToUpper().Equals("SESSION_ID"))
                {
                    Cookie c = new Cookie(cooky.Split('=')[0].Trim(), cooky.Split('=')[1].Trim());
                    //     Logger.__SpecialLogger("Set cookie: " + c.Name + "=" + c.Value+"; path="+c.Path+"; Expires="+c.Expires.ToShortDateString()+"; HttpOnly="+c.HttpOnly.ToString() );
                    BbQuery.cc = new CookieContainer(1000);
                    BbQuery.cc.Add(theUrl, c);
                    //     Logger.__SpecialLogger("Added session_id cookie to cc.");
                }
            }
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            wb.Dispose();
            wb = null;
            Close();
        }

        private void btnGoToLogin_Click(object sender, EventArgs e)
        {
            if (loginPage != null)
            {
                wb.Url = loginPage;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Logger.__SpecialLogger(Environment.NewLine);
            Logger.__SpecialLogger("At loading...");
            wb = new WebBrowser();
            //    wb.Hide();
            wb.Top = 0;
            wb.Left = 0;
            wb.Width = this.Width - 30;
            wb.Height = this.Height - pnBottom.Height - 30;
            wb.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left);
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            // wb.Navigated += new WebBrowserNavigatedEventHandler(wbBb_Navigated);
            // wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.ScriptErrorsSuppressed = true;
            this.Controls.Add(wb);
            tsStatus.Text = waitMsg;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            wb.Navigate(new Uri(loginUrl));
        }

        private static HtmlElement button = null;
        private bool traverse(HtmlElementCollection c, string username, string password, int retValue)
        {

            foreach (HtmlElement h in c)
            {
                //Logger.__SpecialLogger("Tag: " + h.TagName);
                if (h.Children != null)
                {
                    foreach (HtmlElement n in h.Children)
                    {
                        if (n.GetAttribute("type") != null && n.GetAttribute("type").ToUpper().Equals("TEXT"))
                        {
                            n.SetAttribute("value", username);
                            retValue = (retValue | 1);
                        }
                        if (n.GetAttribute("type") != null && n.GetAttribute("type").ToUpper().Equals("PASSWORD"))
                        {
                            n.SetAttribute("value", password);
                            retValue = (retValue | 2);
                        }
                        if (n.GetAttribute("type").ToUpper().Equals("SUBMIT") 
                            || n.GetAttribute("type").ToUpper().Equals("BUTTON") 
                            || n.GetAttribute("type").ToUpper().Equals("IMAGE") )
                        {
                            button = n;
                            retValue = (retValue | 4);
                        }
                        if ((retValue & 7) == 7)
                        {
                            return true;
                        }
                        if (n.Children != null)
                        {
                            traverse(n.Children, username, password, retValue);
                        }
                    }
                }
                else
                {

                    if (h.GetAttribute("type") != null && h.GetAttribute("type").ToUpper().Equals("TEXT"))
                    {
                        h.SetAttribute("value", username);
                        retValue = (retValue | 1);
                    }
                    if (h.GetAttribute("type") != null && h.GetAttribute("type").ToUpper().Equals("PASSWORD"))
                    {
                        h.SetAttribute("value", password);
                        retValue = (retValue | 2);
                    }
                    if (h.GetAttribute("type").ToUpper().Equals("SUBMIT")
                            || h.GetAttribute("type").ToUpper().Equals("BUTTON")
                            || h.GetAttribute("type").ToUpper().Equals("IMAGE"))
                    {
                        button = h;
                        retValue = (retValue | 4);
                    }
                    if ((retValue & 7) == 7)
                    {
                        return true;
                    }
                }
            }
            return (retValue & 7) == 7;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
using System.Net.Security;

namespace EACDMLinqUtiliy
{
    public partial class Login : Form
    {

        public string authUrl = "http://p8500:81/webapps/ea-eacdata-bb_bb60/auth_1.jsp";
        //  private CookieContainer cc = new CookieContainer();
        public string token = null;
        private bool loginSuccess = false;
        private string waitMsg = "Please wait...";
        private string doneMsg = "Done";
        private Uri loginPage = null;
        private string domain = null;

        private int id = 0;
        private string loginCookie = null;
        public WebBrowser wb = null;

        DateTime prevDate;
        DateTime newDate;
        TimeSpan ts;

        public Login()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            authUrl = BbQuery.getBbLoginUrl();
            
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
        }
        public string getToken()
        {
            string retvalue = null;
            string url = BbQuery.getBbCheckUrl();
            
            string tokenString = getResponse(url);
            if (tokenString.Split('=').Length > 1)
            {
                string[] tokens = tokenString.Split('=');
                retvalue = tokens[1].Trim();
                this.token = retvalue;
            }
            else
            {
                retvalue = tokenString;
            }
            return retvalue;

        }
        private string getResponse(string url)
        {

            string retValue = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("secret", BbQuery.products);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            req.KeepAlive = true;
            req.Timeout = 60000;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    if (resp.ContentLength != 0)
                    {
                        StreamReader sr = new StreamReader(resp.GetResponseStream());
                        retValue = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine;
            }
            return retValue;
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                e.Handled = true;
            }
        }

        private void btnBb_Click(object sender, EventArgs e)
        {
            
            wb.Navigate(new Uri(authUrl + "?logout=1"));
            tsLabel1.Text = waitMsg;




        }
        private int count = 0;
        private void wbBb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            newDate = DateTime.Now;
            if (!wb.Visible)
            {
                wb.Visible = true;
            }
            ts = newDate - prevDate;
            prevDate = DateTime.Now;

           
            if (count > 0)
            {
                Thread.Sleep(1500);
                loginCookie = wb.Document.Cookie;
                Logger.__SpecialLogger("login test "+e.Url.AbsoluteUri+" cookie: "+loginCookie);
                tsLabel1.Text = waitMsg;

                UpdateCookies(loginCookie);

                testLogin();
                if (loginSuccess)
                {
                    wb.Stop();
                    MessageBox.Show("Logged in");
                    wb.Hide();
                    btnCancel_Click(sender, new EventArgs());
                    btnCancel.Text = "Logged in";
                }
            }
            if (wb.Url.Query.IndexOf("logout") > 0)
            {
                // wb.Document.Cookie = null;
                wb.Navigate(new Uri(authUrl));
            }
            if (loginPage == null)
            {
                loginPage = new Uri(wb.Url.AbsoluteUri);
            }
            if (!wb.Url.Equals(loginPage))
            {
                btnGoToLogin.Visible = true;
            }
            else
            {
                btnGoToLogin.Visible = false;
            }
            tsLabel1.Text = doneMsg;
            count++;
        }

        private void wbBb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            if (count > 0)
            {
                loginCookie = wb.Document.Cookie;
            //    Utilities.__SpecialLogger("login navigated "+e.Url.AbsoluteUri+" cookie: "+loginCookie);
                tsLabel1.Text = waitMsg;

                UpdateCookies(loginCookie);
                Thread.Sleep(1500);
                testLogin();
                if (loginSuccess)
                {
                    wb.Stop();
                    MessageBox.Show("Logged in");
                    wb.Hide();
                    btnCancel_Click(sender, new EventArgs());
                    btnCancel.Text = "Logged in";
                }
            }
        }
        private void testLogin()
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
            req.UserAgent = BbQuery.BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add("secret", BbQuery.products);
            req.AllowAutoRedirect = true;
            req.CookieContainer = BbQuery.cc;
            req.Timeout = 10000;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == HttpStatusCode.OK)
            {
               // Utilities.__SpecialLogger("test login response OK");
                StreamReader sr = new StreamReader(resp.GetResponseStream());
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
#if (mil)
                        string Bbstime = user.Rows[0]["servertime"].ToString();
                        BbQuery.BbServerTime = Bbstime.Substring(0, Bbstime.Length - 3);

                        if (user.Columns.Contains("version"))
                        {
                            BbQuery.BbVersion = user.Rows[0]["version"].ToString();
                        }
                        if (user.Columns.Contains("pluginversion"))
                        {
                            BbQuery.BbPluginVersion = user.Rows[0]["pluginversion"].ToString();
                            if (BbQuery.BbPluginVersion.CompareTo("1.1.4") >= 0)
                            {
                                BbQuery.AllowSR = true;
                            }
                        }
                        try
                        {
                            DateTime bbNow = Convert.ToDateTime(BbQuery.BbServerTime);
                            BbQuery.BbServerTimeSpan = bbNow - DateTime.Now;
                        }
                        catch
                        {

                        }
#endif
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
                        BbQuery.BbUsername = username;
                    }
                }
                sr.Close();

            }
            else
            {
                Logger.__SpecialLogger("test login response status: "+resp.StatusDescription);
                Logger.__SpecialLogger("test login url: " + req.Address.AbsolutePath);
            }
            resp.Close();
            
        }
        private void UpdateCookies(string cookies)
        {
            string[] cooks = cookies.Split(';');
            foreach (string cooky in cooks)
            {
                if (cooky.Split('=')[0].Trim().ToUpper().Equals("SESSION_ID"))
                {
                    Cookie c = new Cookie(cooky.Split('=')[0].Trim(), cooky.Split('=')[1].Trim());
                    BbQuery.cc.Add(new Uri(authUrl), c);
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
                //wb.Url = loginPage;
                wb.Navigate(loginPage);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Logger.__SpecialLogger("Starting load form");
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            count = 0;
            wb = new WebBrowser();
            wb.Top = 0;
            wb.Left = 0;
            wb.Width = this.Width - 30;
            wb.Height = this.Height - pnBottom.Height - 30;
            wb.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left);
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            wb.Navigated += new WebBrowserNavigatedEventHandler(wbBb_Navigated);
            wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.ScriptErrorsSuppressed = true;
            this.Controls.Add(wb);
            wb.BringToFront();
            prevDate = DateTime.Now;
            wb.Url = new Uri(authUrl + "?logout=1");
            tsLabel1.Text = waitMsg;
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            Logger.__SpecialLogger("Starting browse " + ts.TotalSeconds.ToString());
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Logger.__SpecialLogger("Validate is being called");
            return true;
        }



    }
}

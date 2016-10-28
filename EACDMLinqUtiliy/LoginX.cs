using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class LoginX : Form
    {

        public string authUrl = "http://p8500:81/webapps/ea-eacdata-bb_bb60/auth_1.jsp";
        //  private CookieContainer cc = new CookieContainer();
        public string token = null;
        private bool loginSuccess = false;
        private int loginTries = 0;
        private int clickCount = 0;
        private string waitMsg = "Please wait...";
        private string failedMsg = "Login failed. Please try again.";
        private Uri loginPage = null;
        private BackgroundWorker bw;
        private int id = 0;
        private string loginCookie = null;
        //  public WebBrowser wb = null;
        private HtmlElement button = null;
        private int count = 0;
        private List<string> uris = null;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        private bool IsNavigating;


        public LoginX()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            authUrl = BbQuery.getBbLoginUrl();
            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);


        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine(" At bw completed");

            loginSuccess = (bool)e.Result;


            if (loginSuccess)
            {
                Debug.WriteLine(" At bw completed with loginSuccess true");
                string ccs = getAllCookies();
                Debug.WriteLine("Cookies at complete: " + ccs);
                wb.Stop();
                MessageBox.Show(BbQuery.BbLoginString);
                //wb.Hide();
                //wb.Dispose();
                btnCancel_Click(sender, new EventArgs());
                btnCancel.Text = "Logged in";


            }
            else
            {
                //if ((loginTries - clickCount) == 1)
                //{
                //    Debug.WriteLine(" At bw completed with loginSuccess false");
                //    btnLogin.Enabled = true;
                //    tsLabel1.Text = "Login failed. Try again.";
                //    tbUser_id.Text = "";
                //    tbPassword.Text = "";
                //    tbUser_id.Focus();
                //    InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                //    wb.Url = new Uri(authUrl + "?logout=1");
                //}

            }
        }

        private string getAllCookies()
        {
            string retValue = "none";
            string ccHeader = BbQuery.cc.GetCookieHeader(new Uri(authUrl, UriKind.Absolute));
            if (!string.IsNullOrEmpty(ccHeader))
            {
                retValue = ccHeader;
            }

            return retValue;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (sender as BackgroundWorker);
            e.Result = (bool)testLogin(bw, e);
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
            wb.Navigate(new Uri(authUrl + "?logout=1")); // this starts redirect
            tsLabel1.Text = waitMsg;
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Trim().Equals("") || tbUser_id.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please enter username and password");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            while (wb.IsBusy)
            {
                Application.DoEvents();
            }
            loginCookie = wb.Document.Cookie;
            UpdateCookies(loginCookie);
            clickCount = loginTries;
            loginTries++;
            ClickIt();
            btnLogin.Enabled = false;
            pbBusy.Visible = true;
            tsLabel1.Text = waitMsg;



        }

        private void ClickIt()
        {
            Debug.WriteLine("Trying to CLICK with user_id = " + tbUser_id.Text + " and password = " + tbPassword.Text);
            bool ok = false;
            HtmlDocument doc = null;
            if (wb.Document.Window.Frames.Count > 0)
            {
                Debug.WriteLine("About to traverse with frames.");
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
            Debug.WriteLine("At completed after traverse: loginsuccess = " + loginSuccess.ToString() + " ok: " + ok.ToString() + " button is " + ((button == null) ? "null" : "not null"));
            if (ok && (button != null))
            {
                button.InvokeMember("click");
                Debug.WriteLine("clicked");
            }
        }


        private void wbBb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Application.DoEvents();
            if (tsLabel1.Text.Equals(failedMsg))
            {
                tbPassword.Focus();
                tbPassword.SelectAll();
            }
            else
            {
                tbUser_id.Focus();
            }
            Debug.WriteLine("Focused: user = " + tbUser_id.Focused.ToString() + " password = " + tbPassword.Focused.ToString());
            loginCookie = wb.Document.Cookie;
            Debug.WriteLine("Document complete " + e.Url.AbsoluteUri + " count = " + count.ToString() + " cookie: " + loginCookie);
            if (wb.Url.Query.IndexOf("logout") > 0)
            {
                wb.Navigate(new Uri(authUrl));
                return;
            }
            Debug.WriteLine("Complete wb has focus: " + wb.Focused.ToString());
            if (count > 0 && loginTries > 0 && !loginSuccess)
            {
                
                
                bool ok = true;
                IEnumerator us = uris.GetEnumerator();
                while (us.MoveNext())
                {
                    string u = (string)us.Current;
                    if (u.Contains("frameset.jsp"))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    if (loginCookie.Contains("_session") && authUrl.ToUpper().Contains("MARYLAND"))
                    {
                        return;
                    }
                    Debug.WriteLine("RESTARTING from complete");
                    count = 0;
                    loginTries = 1;
                    uris.Clear();
                    if (authUrl.ToUpper().Contains("DAVENPORT"))
                    {
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                        wb.Url = new Uri(authUrl + "?logout=1");
                        btnLogin.Enabled = true;
                        pbBusy.Visible = false;
                        tsLabel1.Text = failedMsg;
                        return;
                    }
                    btnLogin.Enabled = true;
                    pbBusy.Visible = false;
                    tsLabel1.Text = failedMsg;
                    
                }
            }
            count++;
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            IsNavigating = true;
        }

        private void wbBb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

            IsNavigating = false;
            Debug.WriteLine("At Navigated: " + e.Url.AbsoluteUri);
            //if (tsLabel1.Text.Equals(waitMsg))
            //{
            //    lbUrl.Text = e.Url.GetLeftPart(UriPartial.Path);
            //    Application.DoEvents();

            //}
            if (uris.Contains(e.Url.AbsoluteUri))
            {
                uris.Add(e.Url.AbsoluteUri);
            }
            loginCookie = wb.Document.Cookie;
            UpdateCookies(loginCookie);
            Console.WriteLine("At Navigated cookie: " + loginCookie);
            if (loginTries > 0 && loginCookie.ToUpper().Contains("SESSION_ID"))
            {
                Debug.WriteLine("At Navigated TRYING LOGIN");
                if (!loginSuccess)
                {
                    loginCookie = wb.Document.Cookie;
                    Console.WriteLine("Navigated: " + loginCookie);
                    //  Debug.WriteLine("At Navigated TRYING LOGIN");
                    UpdateCookies(loginCookie);
                    while (bw.IsBusy)
                    {
                        Application.DoEvents();
                    }
                    bw.RunWorkerAsync();
                    while (bw.IsBusy)
                    {
                        Application.DoEvents();
                    }
                    if (!loginSuccess)
                    {
                        Debug.WriteLine("At Navigated TRYING ALTERNATE");
                        if (Check(tbUser_id.Text.Trim().ToUpper(), getCookie("session_id")))
                        {
                            loginSuccess = true;
                            bw_RunWorkerCompleted(sender, new RunWorkerCompletedEventArgs(loginSuccess, null, false));
                        }
                        else
                        {

                            count = 0;
                            loginTries = 0;
                            uris.Clear();
                            btnLogin.Enabled = true;
                            pbBusy.Visible = false;
                            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                            BbQuery.cc = new CookieContainer();

                            tsLabel1.Text = failedMsg;
                            //if (tsLabel1.Text.Equals(failedMsg))
                            //{

                            //    tbPassword.Focus();
                            //    tbPassword.SelectAll();
                            //}
                            //else
                            //{
                            //    tbUser_id.Focus();
                            //}

                            wb.Url = new Uri(authUrl + "?logout=1");




                        }
                    }


                }
            }
        }
        private bool testLogin(BackgroundWorker bw, DoWorkEventArgs e)
        {
            bool retValue = false;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
                req.UserAgent = BbQuery.BbUserAgent;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("secret", BbQuery.products);
                req.AllowAutoRedirect = false;
                req.CookieContainer = BbQuery.cc;
                req.Timeout = 10000;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
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

                        }
                        if (!firstname.Equals("") && !lastname.Equals("") /*&& (firstname.ToUpper().IndexOf("BLACKBOARD") < 0) */
                            && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {

                            retValue = true;
                            BbQuery.BbLoginString = name + " logged in.";
                            BbQuery.BbLoggedInName = name;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            BbQuery.BbUsername = username;
                        }
                    }
                    sr.Close();

                }
                else
                {
                    Debug.WriteLine("test login response status: " + resp.StatusDescription);
                    Debug.WriteLine("test login url: " + req.Address.AbsoluteUri);
                    retValue = false;
                }
                resp.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                retValue = false;
            }
            return retValue;

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
                wb.Navigate(loginPage);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Debug at load");
            count = 0;
            // tsLabel1.TabIndex = -1;
            //wb = new WebBrowser();
            //wb.Top = 0;
            //wb.Left = 0;
            //wb.Width = this.Width - 30;
            //wb.Height = this.Height - pnBottom.Height - 30;
            wb.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left);
            wb.ScriptErrorsSuppressed = true;
            
            // this.Controls.Add(wb);
            // wb.BringToFront();
            wb.Visible = false;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            wb.Navigated += new WebBrowserNavigatedEventHandler(wbBb_Navigated);
            wb.Navigating += new WebBrowserNavigatingEventHandler(wb_Navigating);
            wb.GotFocus += new EventHandler(wb_GotFocus);
            uris = new List<string>();
            wb.Url = new Uri(authUrl + "?logout=1");
            Debug.WriteLine("Starting browse " + authUrl + "?logout=1");
        }



        void wb_GotFocus(object sender, EventArgs e)
        {
            Debug.WriteLine("wb HAS FOCUS (" + wb.Focused.ToString() + ") and mouse is captured (" + wb.Capture.ToString() + ")");
            tbUser_id.Focus();

        }

        private bool traverse(HtmlElementCollection c, string username, string password, int retValue)
        {

            foreach (HtmlElement h in c)
            {
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
                            || n.GetAttribute("type").ToUpper().Equals("IMAGE"))
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

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Debug.WriteLine("Validate is being called");
            return true;
        }

        private void LoginX_Activated(object sender, EventArgs e)
        {
            tbUser_id.Focus();

        }

        private string getCookie(string name)
        {
            string retValue = "";
            IEnumerator cs = BbQuery.cc.GetCookies(new Uri(authUrl)).GetEnumerator();
            while (cs.MoveNext())
            {
                Cookie c = (Cookie)cs.Current;
                if (c.Name.Equals(name))
                {
                    retValue = c.Value;
                }
            }
            return retValue;

        }
        /*if (ds.Tables.Count > 0)
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

                        }
                        if (!firstname.Equals("") && !lastname.Equals("") 
                            && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {

                            retValue = true;
                            BbQuery.BbLoginString = name + " logged in.";
                            BbQuery.BbLoggedInName = name;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            BbQuery.BbUsername = username;
                        }
                    }*/
        private bool Check(string user_id, string session_id)
        {
            bool retValue = false;
            BbQuery.getBbParams();

            user_id = user_id.ToUpper();
            StringBuilder sb = new StringBuilder();
            sb.Append("select u.pk1 as id, u.firstname, u.lastname,u.user_id as username ");
            sb.Append(" from sessions s ");
            sb.Append(" inner join users u on u.user_id = s.user_id ");
            sb.Append(" where upper(s.user_id) = '" + user_id + "' and s.md5 = '" + session_id + "' ");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), BbQuery.BbUrl, BbQuery.BbToken, true);
            DataTable user = rr.execute();
            if (user != null && user.Rows.Count > 0)
            {
                string name = "";
                string firstname = "";
                string lastname = "";
                string username = "";

                id = Convert.ToInt32(user.Rows[0]["id"]);
                firstname = user.Rows[0]["firstname"].ToString();
                lastname = user.Rows[0]["lastname"].ToString();
                name = user.Rows[0]["firstname"].ToString() + " " + user.Rows[0]["lastname"];
                username = user.Rows[0]["username"].ToString();
                if (!firstname.Equals("") && !lastname.Equals("")
                            && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                {

                    retValue = true;
                    BbQuery.BbLoginString = name + " logged in.";
                    BbQuery.BbLoggedInName = name;
                    BbQuery.BbloggedIn = true;
                    BbQuery.BbInstructor_id = id;
                    BbQuery.BbUsername = username;
                }


            }

            return retValue;

        }



    }
}

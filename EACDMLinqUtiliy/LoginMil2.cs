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
//using Skybound.Gecko;


namespace EACDMLinqUtiliy
{
    public partial class LoginMil2 : Form
    {

        public string authUrl = "http://p8500:81/webapps/ea-eacdata-bb_bb60/auth_1.jsp";
        //  private CookieContainer cc = new CookieContainer();
        public string token = null;
        private bool loginSuccess = false;
        private string waitMsg = "Please wait...";
        private string doneMsg = "Done";
        private Uri loginPage = null;
        private DataTable dt = null;
        private int id = 0;
        private string loginCookie = null;
        private BackgroundWorker bw;
        private bool posted = false;
        DateTime prevDate;
        DateTime newDate;
        TimeSpan ts;

        public LoginMil2()
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
            if (!e.Cancelled)
            {

                dt = (DataTable)e.Result;
            }


        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            object[] args = (object[])e.Argument;
            CookieContainer cc = (CookieContainer)args[0];
            e.Result = DoLogin(cc);

        }

        private DataTable DoLogin(CookieContainer cc)
        {
            DataTable r = new DataTable();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
            HttpWebResponse resp = null;
            req.UserAgent = BbQuery.BbUserAgent;
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add("secret", BbQuery.products);
            req.AllowAutoRedirect = true;
            req.CookieContainer = cc;
            req.Timeout = 10000;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr, XmlReadMode.InferSchema);
                    if (ds.Tables.Count > 0)
                    {
                        r = ds.Tables[0];
                    }
                    else
                    {
                        r.TableName = "error";
                        r.Columns.Add("errortype", typeof(string));
                        r.Columns.Add("errormessage", typeof(string));
                        DataRow dr = r.NewRow();
                        dr["errortype"] = "empty";
                        dr["errormessage"] = "empty";
                        r.Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {

                r.TableName = "error";
                r.Columns.Add("errortype", typeof(string));
                r.Columns.Add("errormessage", typeof(string));
                DataRow dr = r.NewRow();
                dr["errortype"] = ex.GetType().ToString();
                dr["errormessage"] = ex.Message;
                r.Rows.Add(dr);
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }
            return r;
        }

        


        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                e.Handled = true;
            }
        }

        private int count = 0;




        private void UpdateCookies(string cookies)
        {
            string[] cooks = cookies.Split(';');
            foreach (string cooky in cooks)
            {
                if (cooky.Split('=')[0].Trim().ToUpper().Contains("SESSION_ID"))
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
            if (BbQuery.BbLoggedInName != null)
            {
                MessageBox.Show(BbQuery.BbLoggedInName);
            }
            Close();
        }

        private void btnGoToLogin_Click(object sender, EventArgs e)
        {
            if (loginPage != null)
            {
                //wb.Url = loginPage;
                wb.Navigate(loginPage.AbsoluteUri);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            count = 1;
            wb.DocumentCompleted += new EventHandler(wb_DocumentCompleted);    // new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            wb.Navigated += new GeckoNavigatedEventHandler(wb_Navigated); //new WebBrowserNavigatedEventHandler(wbBb_Navigated);
            wb.Navigating += new GeckoNavigatingEventHandler(wb_Navigating);
            wb.Visible = true;
            prevDate = DateTime.Now;
            wb.Navigate(authUrl);
            tsLabel1.Text = waitMsg;
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            posted = false;
            Logger.__SpecialLogger("Starting browse " + ts.TotalSeconds.ToString());
        }

        void wb_DocumentCompleted(object sender, EventArgs e)
        {
            //  DeleteCache();
            newDate = DateTime.Now;

            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            Logger.__SpecialLogger("Doc comp " + wb.Url.AbsoluteUri + " at " + ts.TotalSeconds.ToString());
            if (count > 0)
            {
                Thread.Sleep(200);


                tsLabel1.Text = waitMsg;
                if (loginSuccess)
                {
                    if (bw.IsBusy)
                    {
                        bw.CancelAsync();
                    }
                    wb.Stop();
                    MessageBox.Show("Doc completed " + BbQuery.BbLoginString);
                    wb.Hide();

                    btnCancel_Click(sender, new EventArgs());
                    btnCancel.Text = "Logged in";
                    return;

                }
                if (bw.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                loginCookie = wb.Document.Cookie;

                if (loginCookie != null && loginCookie.ToUpper().Contains("SESSION_ID") && !wb.IsBusy)
                {
                    UpdateCookies(loginCookie);

                    // for login 2
                    string stem = wb.Url.AbsoluteUri;
                    int qpos = stem.IndexOf("?");
                    int fpos = stem.IndexOf("webapps/portal/frameset"); // qpos = -1 fpos = 16
                    bool doIt = (fpos > 0 && (qpos < 0 || fpos < qpos)) || posted;
                    Logger.__SpecialLogger(" => login cookie at doc complete qpos = " + qpos.ToString() + " fpos = " + fpos.ToString() + " " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
                    if (doIt)
                    {

                        object[] args = new object[2];
                        args[0] = BbQuery.cc;
                        args[1] = dt;
                        bw.RunWorkerAsync(args);
                        while (bw.IsBusy)
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }
                        loginSuccess = checkLoginStatus(dt);

                        if (loginSuccess)
                        {
                            wb.Stop();

                            MessageBox.Show("Doc complete " + BbQuery.BbLoginString);
                            //   wb.Hide();

                            btnCancel_Click(sender, new EventArgs());
                            btnCancel.Text = "Logged in";

                        }
                        //  wb.Navigate(authUrl);
                    }
                }
            }

            // tsLabel1.Text = doneMsg;
            count++;
        }

        private bool checkLoginStatus(DataTable dt)
        {
            string name = "";
            string firstname = "";
            string lastname = "";
            string username = "";
            if (dt.TableName.Equals("user") && dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                id = Convert.ToInt32(dr["id"]);
                firstname = dr["firstname"].ToString();
                lastname = dr["lastname"].ToString();
                name = dr["firstname"].ToString() + " " + dr["lastname"];
                username = dr["username"].ToString();
#if (mil)
                string Bbstime = dr["servertime"].ToString();
                BbQuery.BbServerTime = Bbstime.Substring(0, Bbstime.Length - 3);

                if (dt.Columns.Contains("version"))
                {
                    BbQuery.BbVersion = dr["version"].ToString();
                }
                if (dt.Columns.Contains("pluginversion"))
                {
                    BbQuery.BbPluginVersion = dr["pluginversion"].ToString();
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

                if (!firstname.Equals("") && !lastname.Equals("")
                    && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                {
                    loginSuccess = true;
                    BbQuery.BbLoginString = name + " logged in.";
                    BbQuery.BbLoggedInName = name;
                    btnCancel.Text = "Return";
                    BbQuery.BbloggedIn = true;
                    BbQuery.BbInstructor_id = id;
                    BbQuery.BbUsername = username;
                }

            }
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = DateTime.Now;
            Logger.__SpecialLogger("finished check login at " + ts.TotalSeconds.ToString());
            return loginSuccess;
        }

        void wb_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            Logger.__SpecialLogger(" => login cookie at navigate " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
            if (loginSuccess)
            {
                wb.Stop();
                MessageBox.Show("Navigated " + BbQuery.BbLoginString);
                //wb.Hide();

                btnCancel_Click(sender, new EventArgs());
                btnCancel.Text = "Logged in";
                return;

            }
            if (wb.IsBusy)
            {
                return;
            }

            loginCookie = wb.Document.Cookie;
            if (loginCookie != null && loginCookie.ToUpper().Contains("SESSION_ID"))
            {
                Logger.__SpecialLogger(authUrl + " login cookie at navigated " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
                UpdateCookies(loginCookie);
                object[] args = new object[2];
                args[0] = BbQuery.cc;
                args[1] = dt;
                bw.RunWorkerAsync(args);
                while (bw.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                loginSuccess = checkLoginStatus(dt);

                if (loginSuccess)
                {
                    wb.Stop();
                    MessageBox.Show("Navigated " + BbQuery.BbLoginString);
                    // wb.Hide();

                    btnCancel_Click(sender, new EventArgs());
                    btnCancel.Text = "Logged in";

                }
            }
        }

        void wb_Navigating(object sender, GeckoNavigatingEventArgs e)
        {
            if (wb.Document != null)
            {
                GeckoElementCollection ec = wb.Document.GetElementsByTagName("INPUT");
                if (ec != null && ec.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (GeckoElement el in ec)
                    {
                        IEnumerator els = el.Attributes.GetEnumerator();
                        while(els.MoveNext())
                        {
                            GeckoAttribute at = (GeckoAttribute) els.Current;
                            sb.Append(at.Name+ " " +at.Value + Environment.NewLine);
                            // type password
                            if(at.Name.ToUpper().Equals("TYPE") && at.Value.ToUpper().Equals("PASSWORD"))
                            {
                                posted = true;
                            }
                        }
                        
                    }
                    Logger.__SpecialLogger("Navigate document text: " + sb.ToString());
                }
            }
            if (loginSuccess)
            {
                wb.Stop();
                MessageBox.Show("Navigating " + BbQuery.BbLoginString);
                //wb.Hide();

                btnCancel_Click(sender, new EventArgs());
                btnCancel.Text = "Logged in";
                return;

            }
            if (wb.IsBusy)
            {
                return;
            }

            loginCookie = wb.Document.Cookie;
            if (loginCookie != null && loginCookie.ToUpper().Contains("SESSION_ID"))
            {
                Logger.__SpecialLogger(authUrl + " login cookie at navigating " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
                UpdateCookies(loginCookie);
                object[] args = new object[2];
                args[0] = BbQuery.cc;
                args[1] = dt;
                bw.RunWorkerAsync(args);
                while (bw.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                loginSuccess = checkLoginStatus(dt);

                if (loginSuccess)
                {
                    wb.Stop();
                    MessageBox.Show("Navigating " + BbQuery.BbLoginString);
                    // wb.Hide();

                    btnCancel_Click(sender, new EventArgs());
                    btnCancel.Text = "Logged in";

                }
            }
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Logger.__SpecialLogger("Validate is being called");
            return true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            wb.ViewSource(wb.Url.AbsoluteUri);
        }



    }
}

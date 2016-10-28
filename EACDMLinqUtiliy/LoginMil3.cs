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
    public partial class LoginMil3 : Form
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
        private bool posted = false;
        private int spCounter = 0;
        DateTime prevDate;
        DateTime newDate;
        TimeSpan ts;

        public LoginMil3()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            authUrl = BbQuery.getBbLoginUrl();



        }

        private DataTable DoLogin(CookieContainer cc)
        {
            Logger.__SpecialLogger("Starting DoLogin");
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
                Logger.__SpecialLogger("Reached response with " + resp.StatusDescription);
                if (resp.StatusCode == HttpStatusCode.OK)
                {

                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    DataSet ds = new DataSet();
                    ds.ReadXml(sr, XmlReadMode.InferSchema);
                    if (ds.Tables.Count > 0)
                    {
                        r = ds.Tables[0];
                        Logger.__SpecialLogger("Should have made good table.");
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
                        Logger.__SpecialLogger("Made error table.");
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
                Logger.__SpecialLogger("Exception: " + ex.Message);
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




        private bool UpdateCookies(string cookies)
        {
            bool retValue = false;
            if (cookies != null && !cookies.Equals(""))
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
            }

            IEnumerator cs = BbQuery.cc.GetCookies(new Uri(authUrl)).GetEnumerator();
            while (cs.MoveNext())
            {
                Cookie c = (Cookie)cs.Current;
                if (c.Name.ToUpper().Equals("SESSION_ID"))
                {
                    RecordIt("Current cookie name " + c.Name);
                    retValue = true;
                    break;
                }
            }
            return retValue;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            wb.Dispose();
            wb = null;
            if (BbQuery.BbLoggedInName != null)
            {
       //         MessageBox.Show(BbQuery.BbLoggedInName);
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
            tsLabel1.Text = waitMsg;
            posted = false;
            spCounter = 0;
            RecordIt(Environment.NewLine + "Starting browse " + ts.TotalSeconds.ToString());
            wb.Navigate(authUrl + "?logout=1");
        }

        void wb_DocumentCompleted(object sender, EventArgs e)
        {
            RecordIt("Doc comp " + wb.Url.AbsoluteUri + " at ");

            if (wb.Url.AbsoluteUri.Contains("logout"))
            {
                wb.Navigate(authUrl);
                return;
            }

            bool ok = UpdateCookies(wb.Document.Cookie);
            RecordIt("ok = " + ok.ToString());
            if (ok)
            {


                // for login 3
                string stem = wb.Url.AbsoluteUri;
                int qpos = stem.IndexOf("?");
                int fpos = stem.IndexOf("webapps/portal/frameset"); // qpos = -1 fpos = 16
                if (fpos > 0 && (qpos < 0 || fpos < qpos ))
                {
                    spCounter++;
                }
                
                bool doIt = (spCounter > 0);
                RecordIt(" => login cookie at doc complete qpos = " + qpos.ToString() + " fpos = " + fpos.ToString() + " " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
                if (doIt)
                {
                   // wb.Navigate(authUrl);
                    
                    dt = DoLogin(BbQuery.cc);
                    loginSuccess = checkLoginStatus(dt);
                    RecordIt("Checking login status from competed");
                    if (loginSuccess)
                    {
                        wb.Stop();

                       // MessageBox.Show("Doc complete " + BbQuery.BbLoginString);
                        wb.Hide();

                        btnCancel_Click(sender, new EventArgs());
                        btnCancel.Text = "Logged in";

                    }
                }
            }
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

            RecordIt("finished check login at ");
            return loginSuccess;
        }

        void wb_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            UpdateCookies(wb.Document.Cookie);
            //RecordIt("Checking login status from navigated");
            //dt = DoLogin(BbQuery.cc);
            //loginSuccess = checkLoginStatus(dt);

            //if (loginSuccess)
            //{
            //    wb.Stop();

            //    MessageBox.Show("Doc navigated " + BbQuery.BbLoginString);
            //    //   wb.Hide();

            //    btnCancel_Click(sender, new EventArgs());
            //    btnCancel.Text = "Logged in";

            //}



            //if (loginCookie != null && loginCookie.ToUpper().Contains("SESSION_ID"))
            //{
            //    Logger.__SpecialLogger(authUrl + " login cookie at navigated " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);

            //}
        }

        void wb_Navigating(object sender, GeckoNavigatingEventArgs e)
        {

            UpdateCookies(wb.Document.Cookie);
            RecordIt(" At Navigating");
            //RecordIt("Checking login status from navigated");
            //dt = DoLogin(BbQuery.cc);
            //loginSuccess = checkLoginStatus(dt);

            //if (loginSuccess)
            //{
            //    wb.Stop();

            //    MessageBox.Show("Doc navigated " + BbQuery.BbLoginString);
            //    //   wb.Hide();

            //    btnCancel_Click(sender, new EventArgs());
            //    btnCancel.Text = "Logged in";

            //}
            //if (wb.Document != null)
            //{

            //    if (wb.Document.Frames.Count > 0)
            //    {
            //        Logger.__SpecialLogger("Navigate document has frames ");
            //        StringBuilder sb = new StringBuilder();
            //        for (int j = 0; j < wb.Document.Frames.Count; j++)
            //        {
            //            GeckoElementCollection ec = wb.Document.Frames[j].GetElementsByTagName("INPUT");
            //            if (ec != null && ec.Count > 0)
            //            {

            //                foreach (GeckoElement el in ec)
            //                {
            //                    IEnumerator els = el.Attributes.GetEnumerator();
            //                    while (els.MoveNext())
            //                    {
            //                        GeckoAttribute at = (GeckoAttribute)els.Current;
            //                        sb.Append(at.Name + " " + at.Value + Environment.NewLine);
            //                        // type password
            //                        if (at.Name.ToUpper().Equals("TYPE") && at.Value.ToUpper().Equals("PASSWORD"))
            //                        {
            //                            posted = true;
            //                        }
            //                    }

            //                }
            //                Logger.__SpecialLogger("Navigate document text: " + sb.ToString());
            //            }
            //        }
            //    }
            //else
            //{
            //    GeckoElementCollection ec = wb.Document.GetElementsByTagName("INPUT");
            //    if (ec != null && ec.Count > 0)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (GeckoElement el in ec)
            //        {
            //            IEnumerator els = el.Attributes.GetEnumerator();
            //            while (els.MoveNext())
            //            {
            //                GeckoAttribute at = (GeckoAttribute)els.Current;
            //                sb.Append(at.Name + " " + at.Value + Environment.NewLine);
            //                // type password
            //                if (at.Name.ToUpper().Equals("TYPE") && at.Value.ToUpper().Equals("PASSWORD"))
            //                {
            //                    posted = true;
            //                }
            //            }

            //        }
            //        Logger.__SpecialLogger("Navigate document text: " + sb.ToString());
            //    }
            //}
            //}
            //if (loginSuccess)
            //{
            //    wb.Stop();
            //    MessageBox.Show("Navigating " + BbQuery.BbLoginString);
            //    //wb.Hide();

            //    btnCancel_Click(sender, new EventArgs());
            //    btnCancel.Text = "Logged in";
            //    return;

            //}


            //loginCookie = wb.Document.Cookie;
            //if (loginCookie != null && loginCookie.ToUpper().Contains("SESSION_ID"))
            //{
            //    Logger.__SpecialLogger(authUrl + " login cookie at navigating " + wb.Url.AbsoluteUri + " cookie: " + loginCookie);
            //    UpdateCookies(loginCookie);
            //    DoLogin(BbQuery.cc);
            //    loginSuccess = checkLoginStatus(dt);
            //    if (loginSuccess)
            //    {
            //        wb.Stop();
            //        MessageBox.Show("Navigating " + BbQuery.BbLoginString);
            //        // wb.Hide();

            //        btnCancel_Click(sender, new EventArgs());
            //        btnCancel.Text = "Logged in";

            //    }
            //}
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

        private void RecordIt(string msg)
        {
            newDate = DateTime.Now;
            ts = newDate - prevDate;
            prevDate = newDate;
            Logger.__SpecialLogger(msg + " " + ts.TotalSeconds.ToString());
        }
        

    }
}

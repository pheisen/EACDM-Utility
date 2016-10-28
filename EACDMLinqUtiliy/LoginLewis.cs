using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;
using System.Net;
using System.Web;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using mshtml;



namespace EACDMLinqUtiliy
{
    public partial class LewisLogin : Form
    {
        bool loginSuccess = false;
        public WebBrowser wb = null;
        private string loginCookie = null;
        private string waitMsg = "Please wait...";
        int id = 0;
        int testInterval = 1500;
        int loginTries = 3;
        string authUrl = null;
        private static HtmlElement button = null;
        private static HtmlElement username = null;
        private static HtmlElement password = null;
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        private int count = 0;
        private int counters = 0;
        private bool clicked = false;
        private bool ok = false;
        private int counter = 0;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        public LewisLogin()
        {
            InitializeComponent();
            authUrl = BbQuery.getBbLoginUrl();
            Console.WriteLine("");
            Console.WriteLine("authurl = {0}", authUrl);

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            wb.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            wb.Navigate(new Uri(authUrl + "?logout=1"));
            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            Console.WriteLine("after logout=1 " + wb.Url.AbsoluteUri);
            // kill cookies
            object[] jargs = new object[1];
            jargs[0] = (object)clearCookies();
            wb.Document.InvokeScript("eval", jargs);
            Console.WriteLine("after logout=1  with cookies " + wb.Document.Cookie);
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            wb.Navigate(new Uri(authUrl));
            this.loginLabel.Text = waitMsg;
            count = 0;
            counters = 0;
            clicked = false;
            ok = false;
            counter = loginTries;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void testLogin()
        {
            // Utilities.__SpecialLogger("At testlogin with " + authUrl);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
            req.UserAgent = BbQuery.BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            req.KeepAlive = true;
            req.CookieContainer = BbQuery.cc;
            req.Timeout = 10000;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                if (resp.ContentLength > 0)
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
                            //  Utilities.__SpecialLogger("Lastname = " + lastname);

                        }
                        if (!firstname.Equals("") && !lastname.Equals("") /*&& (firstname.ToUpper().IndexOf("BLACKBOARD") < 0) */
                            && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {
                            loginSuccess = true;
                            BbQuery.BbLoginString = name + " logged in.";
                            BbQuery.BbLoggedInName = name;
                            //  BbQuery.cc = cc;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            //   Utilities.__SpecialLogger("Login: " + loginSuccess.ToString());

                        }
                    }
                    else
                    {
                        // Utilities.__SpecialLogger("Response OK but no table");

                    }
                    sr.Close();
                }

            }
            else
            {
                //  Utilities.__SpecialLogger("At test with Response not OK (" + resp.StatusDescription + ")");
            }
            resp.Close();

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
                    //     Utilities.__SpecialLogger("Set cookie: " + c.Name + "=" + c.Value+"; path="+c.Path+"; Expires="+c.Expires.ToShortDateString()+"; HttpOnly="+c.HttpOnly.ToString() );
                    BbQuery.cc = new CookieContainer(1000);
                    BbQuery.cc.Add(theUrl, c);

                    //     Utilities.__SpecialLogger("Added session_id cookie to cc.");
                }
            }
            Console.WriteLine("cookie header: " + BbQuery.cc.GetCookieHeader(theUrl));
            return;
        }




        private void LewisLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnLogin_Click(sender, new EventArgs());
                e.Handled = true;
            }

        }

        private void LewisLogin_Load(object sender, EventArgs e)
        {
            //  Utilities.__SpecialLogger(Environment.NewLine);
            //  Utilities.__SpecialLogger("At loading...");
            wb = new WebBrowser();
            wb.Hide();
            wb.ScriptErrorsSuppressed = true;
            //  wb.Location = new Point(409, 13);
            //  wb.Size = new Size(486, 800);
            //  wb.ScrollBarsEnabled = true;
            wb.Navigated += new WebBrowserNavigatedEventHandler(wb_Navigated);
            this.Controls.Add(wb);
            btnLogin.Enabled = true;
            counter = loginTries;

        }

        void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url != null)
            {
                Console.WriteLine("Navigated to : " + e.Url.AbsoluteUri);
            }
        }



        private void wbBb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            Console.WriteLine("At doc completed.   wb Url = " + wb.Url.AbsoluteUri + " count = " + count.ToString());
            InjectAlertBlocker(wb);
            if (counters == 0)
            {
                bool needsRefresh = true;
                if (wb.Document.Window.Frames != null && wb.Document.Window.Frames.Count > 0)
                {
                    for (int j = 0; j < wb.Document.Window.Frames.Count; j++)
                    {
                        HtmlDocument doc = wb.Document.Window.Frames[j].Document;
                        if (doc.GetElementsByTagName("INPUT") != null && doc.GetElementsByTagName("INPUT").Count > 2)
                        {
                            needsRefresh = false;
                            break;
                        }
                    }
                }
                else
                {
                    HtmlDocument doc = wb.Document;
                    if (doc.GetElementsByTagName("INPUT") != null && doc.GetElementsByTagName("INPUT").Count > 2)
                    {
                        needsRefresh = false;

                    }

                }
                Console.WriteLine("Refreshing " + wb.Url.AbsoluteUri);
                //   InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                if (needsRefresh)
                {

                    wb.Refresh(WebBrowserRefreshOption.Completely);
                    InjectAlertBlocker(wb);

                }
                //object[] jargs = new object[1];
                //jargs[0] = (object)clearCookies();
                //wb.Document.InvokeScript("eval", jargs);
            }


            counters++;

            HtmlDocument thisdoc = wb.Document;
            Console.WriteLine("Document cookie: " + wb.Document.Cookie);
            if (!clicked)
            {
                if (!authUrl.Contains("gvltec"))
                {
                    if (wb.Document.Window.Frames != null && wb.Document.Window.Frames.Count > 0)
                    {
                        for (int j = 0; j < wb.Document.Window.Frames.Count; j++)
                        {
                            HtmlDocument doc = wb.Document.Window.Frames[j].Document;
                            if (doc.GetElementsByTagName("INPUT") != null && doc.GetElementsByTagName("INPUT").Count >= 2)
                            {
                                ok = HandleDoc(doc);
                                button.InvokeMember("Click");
                                clicked = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        ok = HandleDoc(wb.Document);
                        Console.WriteLine("ok done");
                        button.InvokeMember("Click");
                        Console.WriteLine("button clicked");
                        clicked = true;
                    }
                }
                else
                {
                    /*  This is alternative to above */
                    object[] args = new object[1];
                    args[0] = initScript();
                    Console.WriteLine("initScript = " + (string)args[0]);
                    string fields = (string)wb.Document.InvokeScript("eval", args);
                    Console.WriteLine("fields = " + fields);
                    args[0] = JSInjection(fields, tbUsername.Text.Trim(), tbPassword.Text.Trim());
                    Console.WriteLine("function = " + args[0]);
                    wb.Document.InvokeScript("eval", args);
                    clicked = true;
                }
                return;
            }

            Console.WriteLine("ok = " + ok.ToString() + " clicked = " + clicked.ToString());
            loginSuccess = false;
            if (!loginSuccess && clicked)
            {
                loginCookie = wb.Document.Cookie;
                UpdateCookies(loginCookie);
                wb.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
                wb.Stop();
                while (!loginSuccess && counter > 0)
                {
                    counter--;
                    Application.DoEvents();
                    Thread.Sleep(testInterval);
                    Console.WriteLine("Trying login with {0} tries", counter);
                    testLogin();
                }
                if (loginSuccess)
                {

                    wb.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
                    object[] jargs = new object[1];
                    jargs[0] = (object)clearCookies();
                    wb.Document.InvokeScript("eval", jargs);
                    wb.Navigate("about:blank");
                    //   wb.Stop();
                    //   InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                    //   wb.Document.Window.Navigate(new Uri(authUrl));
                    // wb.Hide();
                    this.loginLabel.Text = BbQuery.BbLoggedInName;
                    MessageBox.Show(BbQuery.BbLoggedInName + " logged in.");
                    this.Close();
                }
                else
                {
                    wb.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(wbBb_DocumentCompleted);
                    object[] jargs = new object[1];
                    jargs[0] = (object)clearCookies();
                    wb.Document.InvokeScript("eval", jargs);
                    wb.Navigate("about:blank");
                    //   InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                    //   wb.Document.Window.Navigate(new Uri(authUrl));
                    this.loginLabel.Text = "Failed login";
                    MessageBox.Show("Login failed.  Check username and password and try again.");
                    this.Close();

                }
            }
        }

        private bool HandleDoc(HtmlDocument doc)
        {
            bool ok = false;
            Console.WriteLine("checking frame with {0} inputs", doc.GetElementsByTagName("INPUT").Count);
            int rok = 0;
            //for (int j = 0; j < doc.Forms.Count; j++)
            //{


            //}
            for (int j = 0; j < doc.GetElementsByTagName("INPUT").Count; j++)
            {
                HtmlElement h = doc.GetElementsByTagName("INPUT")[j];



                if (h.GetAttribute("TYPE").ToUpper().Equals("TEXT"))
                {
                    Console.WriteLine("doing " + h.TagName + " type " + h.GetAttribute("TYPE"));
                    username = h;
                    h.SetAttribute("value", tbUsername.Text.Trim());
                    rok = rok | 1;
                }
                if (h.GetAttribute("TYPE").ToUpper().Equals("PASSWORD"))
                {
                    Console.WriteLine("doing " + h.TagName + " type " + h.GetAttribute("TYPE"));
                    password = h;
                    h.SetAttribute("value", tbPassword.Text.Trim());
                    rok = rok | 2;
                }
                if (h.GetAttribute("TYPE").ToUpper().Equals("SUBMIT")
                   || h.GetAttribute("TYPE").ToUpper().Equals("BUTTON")
                    || h.GetAttribute("TYPE").ToUpper().Equals("IMAGE")

                    )
                {
                    Console.WriteLine("doing " + h.TagName + " type " + h.GetAttribute("TYPE"));
                    Console.WriteLine("Found button.");

                    button = h;
                    
                    rok = rok | 4;

                }
                Console.WriteLine("rok= " + rok.ToString());
                if (rok == 7)
                {
                    ok = true;
                    break;
                }

            }
            return ok;
        }

        private void LewisLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
        private string JSInjection(string fields, string user_id, string password)
        {
            // Debug.WriteLine("fields: " + fields);
            string[] fs = fields.Split(':');
            string form = "";
            string parent = "";
            string tmp = "";
            for (int i = 0; i < fs.Length; i++)
            {
                if (fs[i].Split('=')[0].Equals("form"))
                {
                    form = fs[i].Split('=')[1];
                }
                if (fs[i].Split('=')[0].Equals("parent"))
                {
                    tmp = fs[i].Split('=')[1];
                    if (tmp.Equals("document"))
                    {
                        parent = tmp;
                    }
                    else
                    {
                        parent = "document.frames['" + tmp + "'].document";
                    }

                }

            }
            StringBuilder bldr = new StringBuilder();
            foreach (string f in fs)
            {
                if (f.Split('=')[0].Equals("text"))
                {
                    bldr.Append(parent + ".forms[" + form + "]['" + f.Split('=')[1] + "'].value = '" + user_id + "'; ");
                }
                if (f.Split('=')[0].Equals("password"))
                {
                    bldr.Append(parent + ".forms[" + form + "]['" + f.Split('=')[1] + "'].value = '" + password + "'; ");
                }
                if (f.Split('=')[0].Equals("submit"))
                {
                    bldr.Append(parent + ".forms[" + form + "]['" + f.Split('=')[1] + "'].click(); ");
                }
            }
            //  Debug.WriteLine(bldr.ToString());
            return bldr.ToString();
        }
        private static string initScript()
        {
            StringBuilder bldr = new StringBuilder();
            bldr.Append("   var s = ''; ");
            bldr.Append("   var fs = document.getElementsByTagName('FRAME');");
            bldr.Append("  if (fs == null || fs.length == 0) { ");
            bldr.Append("     var ds = document.forms; ");
            bldr.Append("     for (z = 0; z < ds.length; z++) { ");
            bldr.Append("         var d = ds[z]; ");
            bldr.Append(" var ok = false; ");
            bldr.Append(" for (i = 0; i < d.length; i++) { ");
            bldr.Append("     if (d[i].type == 'password') {");
            bldr.Append("         ok = true;");
            bldr.Append("         break;");
            bldr.Append("     }");
            bldr.Append(" }");
            bldr.Append(" if (!ok) {");
            bldr.Append("     continue;");
            bldr.Append(" }");
            bldr.Append("         s += 'parent=document:'; ");
            bldr.Append("         s += 'form=' + z + ':'; ");
            bldr.Append("          for (i = 0; i < d.length; i++) { ");
            bldr.Append("              if (d[i].type == 'password') { ");
            bldr.Append("                 s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("             } ");
            bldr.Append("            if (d[i].type == 'text') { ");
            bldr.Append("                 s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("             } ");
            bldr.Append("             if (d[i].type == 'submit' || d[i].type == 'button' || d[i].type == 'image') { ");
            bldr.Append("                  s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("             } ");
            bldr.Append("          } ");
            bldr.Append("          if (s.split(':').length < 5) { ");
            bldr.Append("              s = ''; ");
            bldr.Append("          } else { ");
            bldr.Append("              break; ");
            bldr.Append("          } ");
            bldr.Append("      } ");
            bldr.Append("  } else { ");
            bldr.Append("      for (i = 0; i < fs.length; i++) { ");
            bldr.Append("          var ds = fs[i].contentWindow.document.forms; ");
            bldr.Append("          if (ds == null) { ");
            bldr.Append("              continue; ");
            bldr.Append("          } else { ");
            bldr.Append("              for (z = 0; z < ds.length; z++) { ");
            bldr.Append("       var d = ds[z]; ");
            bldr.Append(" var ok = false; ");
            bldr.Append(" for (i = 0; i < d.length; i++) { ");
            bldr.Append("     if (d[i].type == 'password') {");
            bldr.Append("         ok = true;");
            bldr.Append("         break;");
            bldr.Append("     }");
            bldr.Append(" }");
            bldr.Append(" if (!ok) {");
            bldr.Append("     continue;");
            bldr.Append(" }");
            bldr.Append("     s += 'parent=' + fs[i].name + ':'; ");
            bldr.Append("     s += 'form=' + z + ':'; ");
            bldr.Append("      for (i = 0; i < d.length; i++) { ");
            bldr.Append("          if (d[i].type == 'password') { ");
            bldr.Append("              s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("          } ");
            bldr.Append("          if (d[i].type == 'text') { ");
            bldr.Append("              s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("          } ");
            bldr.Append("          if (d[i].type == 'submit' || d[i].type == 'button' || d[i].type == 'image') { ");
            bldr.Append("              s += d[i].type + '=' + d[i].name + ':'; ");
            bldr.Append("          } ");
            bldr.Append("      } ");
            bldr.Append("     if (s.split(':').length < 5) { ");
            bldr.Append("         s = ''; ");
            bldr.Append("     } else { ");
            bldr.Append("      break; ");
            bldr.Append("  } ");
            bldr.Append(" } ");
            bldr.Append(" } ");
            bldr.Append(" } ");
            bldr.Append(" } ");
            bldr.Append(" s = s.substr(0,s.length-1); ");
            return bldr.ToString();

        }
        private static string clearCookies()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("void( ");
            sb.Append("(function(){ ");
            sb.Append("var cookie_list,sub_domain,location_pathname,cookie_index,cookie_count;");
            sb.Append("cookie_count=0; ");
            sb.Append("cookie_list=document.cookie.split(\"; \"); ");
            sb.Append("for(cookie_index=0;cookie_index<cookie_list.length&&cookie_list[cookie_index];cookie_index++){ ");
            sb.Append("cookie_count++; ");
            sb.Append("for(sub_domain=\".\"+location.host;sub_domain;sub_domain=sub_domain.replace(/^(?:\\.|[^\\.]+)/,'')){ ");
            sb.Append("for(location_pathname=location.pathname;location_pathname;location_pathname=location_pathname.replace(/.$/,'')){");
            sb.Append("document.cookie=(");
            sb.Append(" cookie_list[cookie_index]+");
            sb.Append("\"; domain=\"+ ");
            sb.Append("sub_domain+ ");
            sb.Append("\"; path=\"+ ");
            sb.Append("location_pathname+");
            sb.Append("\"; expires=\"+ ");
            sb.Append("new Date((new Date()).getTime()-1e11).toGMTString() ");
            sb.Append("); ");
            sb.Append("} ");
            sb.Append("} ");
            sb.Append(" } ");
            //     sb.Append(" alert(\"Expired \"+cookie_count+\" cookies\"); ");
            sb.Append("})() ");
            sb.Append(") ");
            return sb.ToString();
        }



        private void InjectAlertBlocker(WebBrowser webBrowser1)
        {
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
            HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            string alertBlocker = "window.alert = function () { }";
            element.text = alertBlocker;
            head.AppendChild(scriptEl);
        }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Net;
using System.IO;
using QTIUtility;

namespace EACDMLinqUtiliy
{
    public partial class LoginAuth2Test : Form
    {
        public static string scheme = "http";
        public static string host = "eac01";
        //public static string host = "eacbb.from-ca.com";
        public static int port = 81;
        public static string pluginStem = "/webapps/ea-eacdata-bb_bb60/";
        public static string schoolsUrl = "http://edassess.net/getSchoolList.aspx";
        public static string schoolname = null;
        public static string BbUrl = null;
        public static string BbRUrl = null;
        public static string authUrl = null;
        public static string sqlUrl = null;
        public static string checkUrl = null;
        public static string responseUrl = null;
        public static string token = null;
        public static string authUrlPath = "auth_2.jsp";
        public static string responseUrlPath = "responder.jsp";
        public static string sqlUrlPath = "xgetsqlsbzip_2.jsp";
        public static string checkPath = "check.jsp";

        public static string BbUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8 ( .NET CLR 3.5.30729; .NET4.0E)";

        public static CookieContainer cc = null;
        public static string products = "EAC!Products";
        public static string BbLoginString = null;
        public static string BbLoggedInName = null;
        public static bool BbloggedIn = false;
        public static int BbInstructor_id = 0;
        public static string BbServerTime = null;
        public static TimeSpan BbServerTimeSpan = new TimeSpan(0); // BbQuery.BbServerTimeSpan = bbNow - DateTime.Now;  so servertime = DateTime.Now + BbQuery.BbServerTimeSpan
        public static string BbShadowCourseId = "EACD!_";
        public static string BbShadowPrefix = "EACD_";

        public static string BbVersion = null;
        public static string BbPluginVersion = null;
        public static string userSql = null;
        public XElement schools;
        public LoginAuth2Test()
        {
            InitializeComponent();
        }





        public static XElement getSchools()
        {

            WebClient wc = new WebClient();
            wc.BaseAddress = schoolsUrl;
            Console.WriteLine(wc.BaseAddress);
            wc.Encoding = Encoding.UTF8;
            wc.Headers[HttpRequestHeader.UserAgent] = BbUserAgent;
            XElement choices = XElement.Parse(wc.DownloadString(new Uri(wc.BaseAddress)));
            Console.WriteLine(choices.ToString());
            return choices;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void LoginAuth2Test_Load(object sender, EventArgs e)
        {
            schools = getSchools();
            userSql = "select  min(user_id) as user_id,min(passwd) as passwd from users where " +
" pk1 >( (select max(pk1) from users ) - 10) " +
" and system_role = 'N' and available_ind = 'Y' and row_status = 0 group by user_id,passwd";
        }

        public static void initBbUrl()
        {

            BbUrl = null;
            BbRUrl = null;
            authUrl = null;
            sqlUrl = null;
            responseUrl = null;
            checkUrl = null;
            UriBuilder bburl = new UriBuilder();
            bburl.Host = host;
            bburl.Port = port;
            bburl.Scheme = scheme;
            bburl.Path = pluginStem + sqlUrlPath;
            BbUrl = bburl.ToString();
            bburl.Path = pluginStem + authUrlPath;
            authUrl = bburl.ToString();
            bburl.Path = pluginStem + checkPath;
            checkUrl = bburl.ToString();


        }
        public static string getToken(string url)
        {
            string retvalue = null;
           
            string tokenString = getResponse(url);
            if (tokenString.Split('=').Length > 1)
            {
                string[] tokens = tokenString.Split('=');
                retvalue = tokens[1].Trim();
               // token = retvalue;
            }
            else
            {
                retvalue = tokenString;
            }
            return retvalue;

        }
        private static string getResponse(string url)
        {

            string retValue = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("secret", "EAC!Products");
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Method = "POST";
            req.ContentLength = 0;
            req.AllowAutoRedirect = false;
            //req.CookieContainer = cc;
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
                retValue = "EAC";//ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine;
            }
            return retValue;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            XElement[] rows = schools.Elements("schools").ToArray();
            for (int i = 0; i < rows.Length; i++) {
                tbMemo.AppendText(rows[i].Element("schoolname").Value + Environment.NewLine);
                // get urls
                scheme = "http";
                port = 80;
                if (rows[i].Element("port").Value.Equals("81"))
                {
                    port = 81;
                }
                host = rows[i].Element("host").Value;
                pluginStem = rows[i].Element("pluginstem").Value;
                initBbUrl();
                tbMemo.AppendText(BbUrl + Environment.NewLine);
                token = getToken(checkUrl);
                tbMemo.AppendText(token + Environment.NewLine);
                if (!token.Equals("EAC"))
                {

                  RequesterAsync rr = new RequesterAsync(userSql, BbUrl, token, true);
                  DataTable dt = rr.execute();
                  if (dt != null && dt.Rows.Count > 0 && !dt.TableName.Equals("error"))
                  {
                      string user_id = dt.Rows[0]["user_id"].ToString();
                      string passwd = dt.Rows[0]["passwd"].ToString();
                      tbMemo.AppendText( user_id+ " " + passwd + Environment.NewLine);
                      cc = new CookieContainer();
                      for (int j = 0; j < 4; j++)
                      {
                          tbMemo.AppendText(Login(user_id,passwd,j)+Environment.NewLine);
                      }
                  }
                }

            //    get username & password
                // try login & out
            }
        }

        private static string Login(string user_id, string passwd, int j)
        {
            string retValue = "Nothing";
            HttpWebResponse resp = null;
            string qry = "?logout=1";
            if (j > 0 && j < 3)
            {
                qry ="?user_id="+user_id+"&password="+passwd;
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl+qry);
            
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Method = "GET";
            req.ContentLength = 0;
            req.AllowAutoRedirect = false;
            req.CookieContainer = cc;
            req.KeepAlive = true;
            req.Timeout = 60000;
            resp = (HttpWebResponse) req.GetResponse();
            try
            {
                cc = req.CookieContainer;
                if (resp.ContentLength > 0)
                {
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    retValue = sr.ReadToEnd();
                        sr.Close();
                    
                }
            }
            catch (Exception ex)
            {
                retValue=ex.Message;
            }

            return retValue;
        }

      //  dt = new List<string> ();
			/*<schools>
    <domain>http://wbu.blackboard.com</domain>
    <schoolname>Wayland Baptist University</schoolname>
    <pluginstem>/webapps/ea-eacdata-BBLEARN/</pluginstem>
    <scheme>http</scheme>
    <host>wbu.blackboard.com</host>
    <port>80</port>
  </schools>*/			
            //XElement choices = Routines.getSchools ();
            //rows = choices.Elements ("schools").ToArray ();
            //for (int i = 0; i < rows.Length; i++) {
				
            //    dt.Add (rows[i].Element ("schoolname").Value);
            //}
            //this.myList = dt;
            //this.schools = schools;
    }
}

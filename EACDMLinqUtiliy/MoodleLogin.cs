using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;
using System.Net;
using System.IO;
using System.Web;

namespace EACDMLinqUtiliy
{
    public partial class MoodleLogin : Form
    {
        bool loginSuccess = false;
        int id = 0;
        string authUrl = null;
        public MoodleLogin()
        {
            InitializeComponent();
            authUrl = MdlQuery.getMdLoginUrl();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!tbUsername.Text.Trim().Equals("") && !tbPassword.Text.Trim().Equals(""))
            {
                // try login
                testLogin(tbUsername.Text.Trim(), tbPassword.Text.Trim());
            }
        }
        private void testLogin(string user_id,string password)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authUrl);
            req.UserAgent = BbQuery.BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add("secret", BbQuery.products);
            req.Method = "POST";
            req.AllowAutoRedirect = false;
            //req.AllowAutoRedirect = true;
           // req.CookieContainer = BbQuery.cc;
            req.Timeout = 10000;
            string parameter = "";
            parameter += "username=" + HttpUtility.UrlEncode(user_id);
            
            parameter += "&password=" + HttpUtility.UrlEncode(password);
            byte[] byteArray = Encoding.UTF8.GetBytes(parameter);
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byteArray.Length;
            Stream dataStream = req.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
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
                            string Bbstime = user.Rows[0]["servertime"].ToString();
                            BbQuery.BbServerTime = Bbstime.Substring(0, Bbstime.Length - 3);

                            if (user.Columns.Contains("version"))
                            {
                                BbQuery.BbVersion = user.Rows[0]["version"].ToString();
                            }
                            if (user.Columns.Contains("pluginversion"))
                            {
                                BbQuery.BbPluginVersion = user.Rows[0]["pluginversion"].ToString();
                            }
                            if (user.Columns.Contains("tableprefix"))
                            {
                                MdlQuery.MdPrefix = user.Rows[0]["tableprefix"].ToString();
                            }
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
                            //btnCancel.Text = "Return";
                            //  BbQuery.cc = cc;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            BbQuery.BbUsername = username;
                            BbQuery.cc = new CookieContainer();
                            foreach(Cookie c in resp.Cookies)
                            {
                                BbQuery.cc.Add(c);
                            }
                        }
                    }
                    sr.Close();
            }
        }
    }
}

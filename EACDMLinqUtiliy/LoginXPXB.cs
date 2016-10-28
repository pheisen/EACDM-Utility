using QTIUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Web;

namespace EACDMLinqUtiliy
{
    public partial class LoginXPXB : Form
    {
        private string loginUrl = "";
        private string loginDoc = "";
        private HtmlAgilityPack.HtmlDocument doc = null;
        private CookieContainer cc = null;
        public LoginXPXB()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            loginUrl = BbQuery.BbLoginUrlNative;
            loginUrl += "/?action=relogin";
            loginDoc = getLoginDoc(loginUrl);
            doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(loginDoc);
            var forms = doc.DocumentNode.Descendants("FORM");//.Where(node => node.Name == "login");
            foreach (var f in forms)
            {
                HtmlAttributeCollection atts = f.Attributes;
                foreach(HtmlAttribute h in atts) {
                    Debug.WriteLine(h.Name+" = "+h.Value);  
                }
              Debug.WriteLine("form descs inputs "+  f.Descendants("INPUT").Count().ToString());
                
            }
            var inputs = doc.DocumentNode.Descendants("INPUT");
            foreach (var f in inputs)
            {
                HtmlAttributeCollection atts = f.Attributes;
                foreach (HtmlAttribute h in atts)
                {
                    Debug.WriteLine(h.Name + " = " + h.Value);
                }

            }
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPasswd.Text.Trim().Equals("") || tbUser_id.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            // get html elements
            postIt(tbUser_id.Text.Trim(), tbPasswd.Text.Trim());
            Debug.WriteLine("cookies " + cc.Count.ToString());
            Uri url = new Uri(loginUrl);
            string scheme = url.Scheme;
            int port = url.Port;
            string domain = url.Host;
            Uri theUrl = new Uri(scheme+"://"+domain);
            Debug.WriteLine("uri "+theUrl);
            Debug.WriteLine("cookies " +getAllCookies(theUrl,cc));
        }

        private string getLoginDoc(string loginUrl)
        {
            string retValue = "";
            WebClient wc = new WebClient();
            retValue = wc.DownloadString(loginUrl);
            return retValue;
        }
        private void postIt(string user_id,string password)
        {
         
            HttpWebRequest req;
            HttpWebResponse resp;
            MemoryStream ms;
            cc = new CookieContainer();
            byte[] buffer;
            string action = "https://eac01:8443/webapps/login/";
            req = (HttpWebRequest)WebRequest.Create(action);
            req.Proxy = null;
            req.ServicePoint.ConnectionLimit = 20;
            req.Method = "POST";
          //  req.Headers.Add("secret", BbQuery.products);
            req.UserAgent = BbQuery.BbUserAgent;
            req.Timeout = 600000; // 2minutes
            req.KeepAlive = true;
            req.CookieContainer = this.cc;
            string parameter = "";
            parameter += "?action=login";
            parameter += "&remote-user=";
            parameter += "&new_loc=";
            parameter += "&auth_type=";
            parameter += "&one_time_token=";
            parameter += "&encoded_pw=";
            parameter += "&encoded_pw_unicode=";
            parameter += "&user_id=" + user_id;
            parameter += "&password=" + password;
             parameter +="&login=Login";

            
            byte[] byteArray = Encoding.UTF8.GetBytes(parameter);
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byteArray.Length;
            BinaryWriter bw = new BinaryWriter(req.GetRequestStream());
            bw.Write(byteArray, 0, byteArray.Length);
            bw.Close();
            using (resp = (HttpWebResponse)req.GetResponse())
            {

                StreamReader sr = new StreamReader(resp.GetResponseStream(), true);
                string theHtml = sr.ReadToEnd();
               Debug.WriteLine(theHtml);
                
            }
            req.GetResponse().Close();
            resp.Close();
        }

        private string getAllCookies(Uri url,CookieContainer cc)
        {
            string retValue = "(none)";
            if (cc == null)
            {
                return retValue;
            }
            retValue = cc.GetCookieHeader(url);
            return retValue;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Debug.WriteLine("Validate is being called");
            return true;
        }
    }
}

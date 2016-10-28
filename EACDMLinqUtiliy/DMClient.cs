using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Xml.Linq;

using tempuri.org.Clients.xsd;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace EACDMLinqUtiliy
{
    public class DMClient
    {
        
        public int id;
        public string name;
        public string token;
        public DateTime created;
        public DateTime modified;
        public string origUrl;
        public bool isOracle = true;
        public string pluginUrl
        {
            get
            {
                if (origUrl.EndsWith(".jsp"))
                {
                    int l = origUrl.LastIndexOf("/");
                    return origUrl.Substring(0, l + 1);
                }
                else
                {
                    return origUrl;
                }
            }
        }
        public DMClient()
        {

        }
        public DMClient(string name)
        {

        }
        public DMClient(int id, Clients c)
        {
            Clients.DMClientsLocalType mc = c.DMClients.Single(t => t.id == id);
            this.id = id;
            this.name = mc.Name;
            this.origUrl = mc.Url;
            this.created = (DateTime)mc.created;
            this.modified = (DateTime)mc.modified;
            if (mc.token != null)
            {
                this.token = mc.token;

            }
        }
        public string getToken()
        {
            string retvalue = null;
            string url = this.pluginUrl + "check.jsp?ModPagespeed=off";
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
        public string listFiles()
        {
            string retvalue = null;
            string url = this.pluginUrl + "EACFilesText.jsp";
            retvalue = getResponse(url);
            retvalue = strip(retvalue);
            retvalue = stripDoctype(retvalue);
            return retvalue;

        }
        public string listProperties()
        {
            string retvalue = null;
            string url = this.pluginUrl + "EACEnviron.jsp";
            retvalue = getResponse(url);
            retvalue = strip(retvalue);
            retvalue = stripDoctype(retvalue);
            return retvalue;
        }
        public string getUser(string firstname,string lastname)
        {
            string retvalue = null;
            string token = null;
            if (this.token != null)
            {
                token = this.token;
            }
            else
            {
                token = getToken();
            }
            string sql = "select firstname as Firstname,lastname as Lastname,user_id as Username, email "; 
            sql += " from users ";
            sql += " where UPPER(firstname)  = '" + firstname.ToUpper() + "'";
            sql += " and UPPER(lastname) = '" + lastname.ToUpper() + "'";
            QTIUtility.RequesterAsync rr = new QTIUtility.RequesterAsync(sql, this.origUrl, token, true);
            DataTable dt = rr.execute();
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append(dc.ColumnName + " = " + dr[dc].ToString() + Environment.NewLine);
                    }
                }
            }
            retvalue = sb.ToString()+Environment.NewLine;
            return retvalue;
        }

        private string getResponse(string url)
        {
            
            string retValue = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("secret", "EAC!Products");
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Method = "POST";
            req.ContentLength = 0;
            req.AllowAutoRedirect = false;
            var cers = new X509CertificateCollection();
            string Certificate = "china.cer";

            // Load the certificate into an X509Certificate object.
            X509Certificate cert = X509Certificate.CreateFromCertFile(Certificate);
            cers.Add(cert);
            req.ClientCertificates = cers;
            // req.Headers.Add(HttpRequestHeader.)
            //req.CookieContainer = cc;
            req.KeepAlive = true;
            req.Timeout = 60000;
            // ServicePointManager.Expect100Continue = true;
            
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
                retValue = ex.Message+Environment.NewLine+ex.StackTrace+Environment.NewLine;
            }
            return retValue;
        }
        private string strip(string input)
        {
            string retValue = null;
            // ● \\u25cf
            retValue = input.Replace("<LI>", Environment.NewLine + "● ").Replace("<BR>", Environment.NewLine);


            return retValue;

        }
        private string stripDoctype(string input)
        {
            // <!DOCTYPE 
            if (input.IndexOf("<!DOCTYPE") > 0)
            {
                return input.Substring(0, input.IndexOf("<!DOCTYPE"));
            }
            else
            {
                return input;
            }
            
        }
        public bool getOracle()
        {
            
            string sql = "select sysdate from dual";
            QTIUtility.RequesterAsync rr = new QTIUtility.RequesterAsync(sql, this.origUrl, this.token, true);
            DataTable dt = rr.execute();
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            return true;


        }
        public DataTable getSqldt(string sql, DMClient dmc)
        {
            DataTable retValue;
            QTIUtility.RequesterAsync rr = new QTIUtility.RequesterAsync(sql, dmc.origUrl, dmc.token, true);
            retValue= rr.execute();
            return retValue;
        }
        public static string getSql(string sql, DMClient dmc)
        {
            string retValue = null;
            QTIUtility.RequesterAsync rr = new QTIUtility.RequesterAsync(sql, dmc.origUrl, dmc.token, true);
            DataTable dt = rr.execute();
            StringBuilder sb = new StringBuilder();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append(dc.ColumnName+"\t");
                }
                sb.Append(Environment.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append(dr[dc].ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            retValue = sb.ToString() + Environment.NewLine;
            return retValue;
        }

        public static string getSqlHTML(string sql, DMClient dmc)
        {
            string retValue = null;
            bool compressed = ((dmc.origUrl.Contains(".php") && dmc.origUrl.Contains("cc1")) || dmc.origUrl.ToLower().Contains("postgres")) ? false : true;
            QTIUtility.RequesterAsync rr = new QTIUtility.RequesterAsync(sql, dmc.origUrl, dmc.token, compressed);
            DataTable dt = rr.execute();
            if (dt.Columns.Contains("qdecode") || dt.Columns.Contains("QDECODE"))
            {
                foreach(DataRow dr in dt.Rows)
                {
                    string data = QTIUtility.BbQuery.getStringFromData(dr["qdecode"].ToString());
                    System.Xml.Linq.XElement qd = System.Xml.Linq.XElement.Parse(data);
                    var qs = qd.Descendants( XName.Get("mat_formattedtext"));
                    if (qs.Count() > 0)
                    {
                        dr["qdecode"] = QTIUtility.Utilities.StripTags(qs.FirstOrDefault().Value);
                    }
                  
                }
            }
            if (dt.Columns.Contains("qrscore") || dt.Columns.Contains("QRSCORE"))
            {
                foreach (DataRow dr in dt.Rows)
                {

                    string data = QTIUtility.BbQuery.getStringFromData(dr["qrscore"].ToString());
                    System.Xml.Linq.XElement qd = System.Xml.Linq.XElement.Parse(data);
                    string sc = "";
                    var qs = qd.Descendants(XName.Get("score_value"));
                    if (qs.Count() > 0)
                    {
                        foreach(string v in qs)
                        {
                            sc += ":" + v;
                        }
                       
                    }
                    qs = qd.Descendants(XName.Get("score_maximum"));
                    if (qs.Count() > 0)
                    {
                        foreach (string v in qs)
                        {
                            sc += ":" + v;
                        }
                    }
                    dr["qrscore"] = sc.Substring(1);
                }
            }

            StringBuilder sb = new StringBuilder("<table style=\"font-family: Tahoma, Arial, sans-serif;font-size:12px\" >");
            sb.Append("<tr>");
            if (dt != null && dt.Rows.Count > 0)
            {
                
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.Append("<th>"+dc.ColumnName + "</th>");
                }
                sb.Append("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn dc in dt.Columns)
                    {
                        sb.Append("<td>"+dr[dc].ToString() + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
            }
            retValue = sb.ToString() ;
            return retValue;
        }
        public static string testConnection(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            string retValue = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = false;
            //req.CookieContainer = cc;
            req.KeepAlive = true;
            req.Timeout = 60000;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                retValue = resp.StatusDescription;
            }
            catch (Exception ex)
            {
                retValue = ex.Message;
            }
            return retValue;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
           QTIUtility.Logger.__SpecialLogger("Validate is being called");
            return true;
        }

        public static string pluginStem = "/webapps/ea-eacdata-bb_bb60/";
        public static string xgetFile = "xgetsqlsbzip_1.jsp";
    }

}

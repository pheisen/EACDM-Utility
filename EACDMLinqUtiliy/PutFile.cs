using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using System.Web;
using QTIUtility;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace EACDMLinqUtiliy
{
    public class PutFile
    {
        public string jspput = "putUpdates.jsp";
        public static int counter = 0;
        public DataTable dt = null;
        public string jsp;
        public string targetPath;
        public string fileContent;
        private byte[] buffer;

        private HttpWebRequest req;
        private HttpWebResponse resp;
        //putUpdates.jsp?debugging=1&path=<path>&fileContent=Hello_There
        public PutFile( string targetPath, string fileContent)
        {

            this.targetPath = targetPath;
            this.fileContent = fileContent;

        }
        public PutFile()
        {

        }
        public void execute()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            Console.WriteLine("jsp = " + jsp);
            Console.WriteLine("targetPath = " + targetPath);
            Logger.__SpecialLogger("jsp = "+jsp+Environment.NewLine);
            Logger.__SpecialLogger("targetPath = " + targetPath + Environment.NewLine);
            try
            {
                req = (HttpWebRequest)WebRequest.Create(jsp);
                req.Proxy = null;
                req.Method = "POST";
                req.Headers.Add("secret", BbQuery.products);
                req.UserAgent = BbQuery.BbUserAgent;
                req.Timeout = 600000; // 2minutes
                string parameter = "";
                parameter += "debugging=1";
                parameter += "&filename=" + HttpUtility.UrlEncode(targetPath);
                parameter += "&fileContent=" + HttpUtility.UrlEncode(fileContent);
                byte[] byteArray = Encoding.UTF8.GetBytes(parameter);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = byteArray.Length;
                Stream dataStream = req.GetRequestStream();
                dataStream.BeginWrite(byteArray, 0, byteArray.Length, new AsyncCallback(ReadCallback), req);
            }

            catch (Exception ex)
            {
                Console.WriteLine("PutUpdates Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
                Logger.__SpecialLogger(Environment.NewLine);
                Logger.__SpecialLogger("PutUpdates Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);

            }
        }
        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest req = (HttpWebRequest)asynchronousResult.AsyncState;
            resp = (HttpWebResponse)req.GetResponse();
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                MemoryStream ms = new MemoryStream();
                ms.Position = 0;
                buffer = new byte[8192];
                int bytesRead;
                Stream s = resp.GetResponseStream();
                do
                {
                    IAsyncResult result = s.BeginRead(buffer, 0, buffer.Length, null, null);
                    result.AsyncWaitHandle.WaitOne(60000);
                    bytesRead = s.EndRead(result);
                    result = null;
                    if (bytesRead > 0)
                    {
                        ms.Write(buffer, 0, bytesRead);
                    }
                }
                while (bytesRead > 0);
                s.Close();
                //     the following lines used for debugging
                StringBuilder sb = new StringBuilder();
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    // Just read to the end.
                    string respString = reader.ReadToEnd();
                    Console.WriteLine("PutUpdates debug output: " + respString);
                    Logger.__SpecialLogger("PutUpdates debug output: " + respString);
                }
                ms.Close();

            }
            else
            {
                Console.WriteLine("PutUpdates debug response: " + resp.StatusDescription);
                Logger.__SpecialLogger("PutUpdates debug response: " + resp.StatusDescription);
            }

            req = null;
            resp.Close();
            resp = null;
        }

        public string regKeyToJsp(string BbUrl, string jspput)
        {
            string path = BbUrl.Substring(0, BbUrl.LastIndexOf('/'));
            jspput = path + "/" + jspput;
            return jspput;
        }
        public byte[] fileRead(string path)
        {
            byte[] filecontents = null;
            using (FileStream fs = new FileStream(path,FileMode.Open))
            {

                //buffer= new byte[fs.Length];
                    //StreamReader sr = new StreamReader(fs);
                    //filecontents = sr.ReadToEnd();
                using (BinaryReader br = new BinaryReader(fs))
                {


                    filecontents = br.ReadBytes((int)fs.Length);
                    
                }

            }
            return filecontents;
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Logger.__SpecialLogger("Validate is being called");
            return true;
        }
    }
}
/* using (FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                {
                    byte[] chunk;

                    chunk = br.ReadBytes(CHUNK_SIZE);
                    while(chunk.Length > 0)
                    {
                        DumpBytes(chunk, chunk.Length);
                        chunk = br.ReadBytes(CHUNK_SIZE);
                    }
                }
            }
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace BbEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbTo.Text = "pheisen@verizon.net";
            tbBody.Text = "Hi there";
            tbSMPT.Text = "outgoing.verizon.net";
            

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
           

            NetworkCredential nc = new NetworkCredential("pheisen", "areearee1");

            SmtpClient client = new SmtpClient(tbSMPT.Text.Trim());
            client.Credentials = nc;// CredentialCache.DefaultNetworkCredentials;

            // Specify the e-mail sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress(tbFrom.Text.Trim());
           
            MailAddress to = new MailAddress(tbTo.Text.Trim());
            
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            Attachment att = new Attachment(@"C:\utility\EACDataMiner\EACDataMiner\EACDataMiner\bin\Instructor\WorkingData\sl1.pdf", MediaTypeNames.Application.Pdf);
            message.Attachments.Add(att);
            message.Body = "This is a test e-mail message sent by an application. ";
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
           
            string userState = "test message1";
            client.SendAsync(message, userState);
            
            
            

            
        }

        bool mailSent = false;
        private  void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                tbNotes.AppendText(token + " Send canceled."+Environment.NewLine);
            }
            if (e.Error != null)
            {
                tbNotes.AppendText(token+" "+ e.Error.ToString()+Environment.NewLine);
            }
            else
            {
                tbNotes.AppendText("Message sent."+Environment.NewLine);
            }
            mailSent = true;
        }

    }
}

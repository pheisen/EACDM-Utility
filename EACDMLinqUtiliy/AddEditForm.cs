using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EACDMLinqUtiliy
{
    public partial class AddEdit : Form
    {
        
        public AddEdit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // ok button
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string theUrl = "";
            string baseurl = tbUrl.Text;
            if (!baseurl.Contains("moodle"))
            {
                if (baseurl.IndexOf("webapps") > 0)
                {
                    // already is full address
                    if (baseurl.EndsWith(".jsp"))
                    {
                        int l = baseurl.LastIndexOf("/");
                        baseurl = baseurl.Substring(0, l + 1);
                    }
                }
                else
                {
                    if (baseurl.EndsWith(".php"))
                    {
                        int l = baseurl.LastIndexOf("/");
                        baseurl = baseurl.Substring(0, l + 1);
                    }
                    else
                    {
                        baseurl += DMClient.pluginStem;
                    }
                }
                if (tbUrl.Text.EndsWith(".php"))
                {
                    theUrl = baseurl + "auth_1.php";
                }
                else
                {
                    theUrl = baseurl + "auth_1.jsp";
                }
            }
            else
            {
                theUrl = baseurl;
            }
            MessageBox.Show(DMClient.testConnection(theUrl),"Connection Test");
            Cursor.Current = Cursors.Default;
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EACDMLinqUtiliy
{
    public partial class UpdateFile : Form
    {
        public DMClient dmc;
        public UpdateFile()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DialogResult dr = srcDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                PutFile pf = new PutFile();
                pf.jsp = pf.regKeyToJsp(dmc.origUrl, pf.jspput);
                string fname = srcDialog.FileName;
                tbMemo.AppendText(fname + Environment.NewLine);
                string[] delim = {@"\"};
                string[] parts = Path.GetDirectoryName(fname).Split(delim,StringSplitOptions.None);
                string targetPath = Path.GetFileName(fname);
                for(int i = parts.Length - 1; i >= 0;i--)
                {
                    if (parts[i].Equals("web") || parts[i].Equals("EacVD35") || parts[i].Equals("EacVD35_2013"))
                    {
                        break;
                    }
                    if (pf.jsp.Contains(@"\"))
                    {
                        targetPath = parts[i] + @"\" + targetPath;
                    }
                    else
                    {
                    targetPath = parts[i] + "/" + targetPath;
                  //    targetPath = parts[i] + @"\" + targetPath; // for ok christian and Wallace
                    }
                }
                pf.targetPath = targetPath;
                tbMemo.AppendText(targetPath + Environment.NewLine);
                pf.fileContent = Convert.ToBase64String(pf.fileRead(fname));
                tbMemo.AppendText(pf.jsp + Environment.NewLine);
                pf.execute();
                tbMemo.AppendText("Done" + Environment.NewLine);
                
            }
        }
    }
}

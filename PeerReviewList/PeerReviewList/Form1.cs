using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeerReviewList
{
    public partial class Form1 : Form
    {
        private string BbUrl = "http://EAC01:81/webapps/ea-eacdata-bb_bb60/xgetsqlsbzip_2.jsp";
        private string token = "520ea85ddb384e514eaa009196b1e584";
        private int qdpk1 = 34445;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
           PRUtility.Modify(BbUrl, token, qdpk1);
           tbMemo.AppendText("done" + Environment.NewLine);
        }

    }
}

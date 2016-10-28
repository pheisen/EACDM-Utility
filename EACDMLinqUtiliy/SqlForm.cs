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
    public partial class SqlForm : Form
    {
        
        public SqlForm()
        {
            
            InitializeComponent();
            
        }
        private DMClient dmc;
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            dmc = (DMClient)this.Tag;
            string token = dmc.token;
            if (!tbSql.Text.Equals(String.Empty))
            {
                if (token == null || token.Equals(""))
                {
                    dmc.token = dmc.getToken();
                }
            }
            tbResult.AppendText(DMClient.getSql(tbSql.Text,dmc));
           // MessageBox.Show(dmc.name);
        }
    }
}

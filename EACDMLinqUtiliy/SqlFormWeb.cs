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
    public partial class SqlFormWeb : Form
    {
        
        public SqlFormWeb()
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
            QTIUtility.Logger.__SpecialLogger("SQL: " + dmc.name + Environment.NewLine + tbSql.Text + Environment.NewLine + "-- end --"+Environment.NewLine);
            tbResults.DocumentText += DMClient.getSqlHTML(tbSql.Text,dmc);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbResults.DocumentText = "";
        }

        private void btnReadSQL_Click(object sender, EventArgs e)
        {
            ReadSql rs = new ReadSql();
            rs.Show();
        }
    }
}

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
    public partial class ReadSql : Form
    {
        public ReadSql()
        {
            InitializeComponent();
        }

        private void ReadSql_Load(object sender, EventArgs e)
        {
            //tbMemo.AppendText(QTIUtility.Logger.readsql());
            //tbMemo.SelectionStart = tbMemo.TextLength;
            //tbMemo.ScrollToCaret();
            //tbMemo.Refresh();
        }

        private void ReadSql_Activated(object sender, EventArgs e)
        {
            tbMemo.AppendText(QTIUtility.Logger.readsql());
            tbMemo.SelectionStart = tbMemo.TextLength;
            tbMemo.ScrollToCaret();
            tbMemo.Refresh();
        }
    }
}

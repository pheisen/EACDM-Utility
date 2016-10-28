using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;

namespace EACDMLinqUtiliy
{
    public partial class Sauce : Form
    {
        public Sauce()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbData.Text))
            {
                tbMemo.AppendText(BbQuery.getStringFromData(tbData.Text));
            }
            else
            {
                MessageBox.Show("Need some data.");
            }
        }

        private void btnBase64_Click(object sender, EventArgs e)
        {
            // Encoding.UTF8.GetString(Convert.FromBase64String(tbData.Text));
            tbMemo.AppendText(Encoding.UTF8.GetString(Convert.FromBase64String(tbData.Text)));
        }
    }
}

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
    public partial class Urlencode : Form
    {
        public Urlencode()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            tbHash.Text = System.Web.HttpUtility.UrlEncode(textBox1.Text.Trim());
            
        }
    }
}

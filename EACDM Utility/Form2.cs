using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EACDM_Utility
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            EACDM_Utility.DMClients dt = new DMClients();
            dt.WriteXmlSchema(@"c:\utility\dmclients.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EACDM_Utility.DMClients dt = new DMClients();
            dt.WriteXmlSchema(@"c:\utility\dmclients.xml");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using tempuri.org.Clients.xsd;

namespace EACDMLinqUtiliy
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // hard wired for now            
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dataFile = path + @"\clientData.xml";
            Clients c = Clients.Load(dataFile);
            var r = from t in c.DMClients
                    orderby t.Name
                    select t;
                    
            
            foreach (var t in r)
            {
                string d = t.id+" " +t.Name + " " + t.Url + " " + t.token + " " + t.created.ToString() + " " + t.modified.ToString();
                tbMemo.AppendText(d + Environment.NewLine);
                
            }
          //  el.Save(dataFile);
        }
    }
}

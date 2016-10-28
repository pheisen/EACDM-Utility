using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections;

namespace EACDMLinqUtiliy
{
    public partial class SpecialCourse : Form
    {
        public string origXML;
        public SpecialCourse()
        {
            InitializeComponent();
        }

        private void btnLoadQ_Click(object sender, EventArgs e)
        {
            string filename = @"C:\utility\QTIUtility\LINQtoXSDLib\LINQtoXSDLib\eacSpecialQ_13784_mc.xml";
            string[] xmls = File.ReadAllLines(filename);
            tbMemo.Lines = xmls;
            origXML = tbMemo.Text;
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            // setup XPath probes
            string probe = "./presentation/flow/flow/response_lid/render_choice";
            string probe2 = "./resprocessing";
            string probe3 = "./itemfeedback";    
            // get newIdents
            ArrayList idents = new ArrayList();
            int count = 0;
            while (count < lbMembers.Items.Count)
            {
                string id = QTIUtility.Utilities.getIdent().ToUpper();
                if (!idents.Contains(id))
                {
                    idents.Add(id);
                    count++;
                }
            }
            XElement node = XElement.Parse(origXML);
            XElement el1 = node.XPathSelectElement(probe);
            XElement t = el1.Element("flow_label");
            for (int i = 0; i < lbMembers.Items.Count; i++)
            {
                if (i < 2)
                {
                    t.Descendants("mat_formattedtext").Single().SetValue(lbMembers.Items[i].ToString());
                    if (t.NextNode != null)
                    {
                        t = (XElement)t.NextNode;
                    }
                }
                else
                {
                    XElement tt = new XElement(t);
                    tt.Element("response_label").SetAttributeValue("ident", idents[i]);
                    tt.Descendants("mat_formattedtext").Single().SetValue(lbMembers.Items[i].ToString());
                    t.AddAfterSelf(tt);
                    t = tt;
                }

            }
          
            el1 = node.XPathSelectElement(probe2).Elements("respcondition").Where(z => (z.Attribute("title") == null)).First();
            for (int i = 0; i < lbMembers.Items.Count; i++)
            {
                if (i > 1)
                {
                    XElement tt = new XElement(el1);
                    tt.Element("conditionvar").Element("varequal").SetAttributeValue("respident", idents[i]);
                    tt.Element("displayfeedback").SetAttributeValue("linkrefid", idents[i]);
                    el1.AddAfterSelf(tt);
                    el1 = tt;
                }
            }

            el1 = node.XPathSelectElements(probe3).Last();
            for (int i = 0; i < lbMembers.Items.Count; i++)
            {
                if (i > 1)
                {
                    XElement tt = new XElement(el1);
                    tt.SetAttributeValue("ident",idents[i]);
                    
                    el1.AddAfterSelf(tt);
                    el1 = tt;
                }
            }
            
            tbMemoNew.AppendText(node.ToString());
        }
    }
}

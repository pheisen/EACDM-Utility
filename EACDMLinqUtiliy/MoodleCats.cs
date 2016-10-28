using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LINQtoXSDLib;
using System.Text.RegularExpressions;

namespace EACDMLinqUtiliy
{
    public partial class MoodleCats : Form
    {
        public cats c;
        public string xmlString = "<cat pk1=\"1\">BT**Set</cat><cat pk1=\"2\">BT-Analyzing</cat><cat pk1=\"3\">BT-Applying</cat><cat pk1=\"4\">BT-Creating</cat><cat pk1=\"5\">BT-Evaluating</cat><cat pk1=\"6\">BT-Remembering</cat><cat pk1=\"7\">BT-Understanding</cat><cat pk1=\"8\">CN**Set</cat><cat pk1=\"9\">CN-Health Promotion and Maintenance</cat><cat pk1=\"10\">CN-Physiological Integrity</cat><cat pk1=\"11\">CN-Physiological Integrity: Basic Care and Comfort</cat><cat pk1=\"12\">CN-Physiological Integrity: Pharmacological and Parenteral Therapies</cat><cat pk1=\"13\">CN-Physiological Integrity: Physiological Adaptation</cat><cat pk1=\"14\">CN-Physiological Integrity: Reduction of Risk Potential</cat><cat pk1=\"15\">CN-Psychosocial Integrity</cat><cat pk1=\"16\">CN-Safe Effective Care</cat><cat pk1=\"17\">CN-Safe Effective Care: Management of Care</cat><cat pk1=\"18\">CN-Safe Effective Care: Safety and Infection Control</cat><cat pk1=\"19\">IP**Set</cat><cat pk1=\"20\">IP-Caring</cat><cat pk1=\"21\">IP-Communication and Documentation</cat><cat pk1=\"22\">IP-Nursing Process</cat><cat pk1=\"23\">IP-Nursing Process: Analysis</cat><cat pk1=\"24\">IP-Nursing Process: Assessment</cat><cat pk1=\"25\">IP-Nursing Process: Evaluation</cat><cat pk1=\"26\">IP-Nursing Process: Implementation</cat><cat pk1=\"27\">IP-Nursing Process: Planning</cat><cat pk1=\"28\">IP-Teaching and Learning</cat><cat pk1=\"29\">QSEN**Set</cat><cat pk1=\"30\">QSEN-Evidence-based Practice</cat><cat pk1=\"31\">QSEN-Informatics</cat><cat pk1=\"32\">QSEN-Patient-centered Care</cat><cat pk1=\"33\">QSEN-Quality Improvement</cat><cat pk1=\"34\">QSEN-Safety</cat><cat pk1=\"35\">QSEN-Teamwork and Collaboration</cat><ics><ic><qpk1>0</qpk1><pk1>0</pk1><cpk1>0</cpk1></ic><ic><qpk1>1</qpk1><pk1>2</pk1><cpk1>2</cpk1></ic><ic><qpk1>6</qpk1><pk1>2</pk1><cpk1>3</cpk1></ic><ic><qpk1>2</qpk1><pk1>3</pk1><cpk1>2</cpk1></ic><ic><qpk1>8</qpk1><pk1>3</pk1><cpk1>3</cpk1></ic><ic><qpk1>3</qpk1><pk1>4</pk1><cpk1>2</cpk1></ic><ic><qpk1>7</qpk1><pk1>4</pk1><cpk1>3</cpk1></ic><ic><qpk1>4</qpk1><pk1>5</pk1><cpk1>2</cpk1></ic><ic><qpk1>9</qpk1><pk1>5</pk1><cpk1>3</cpk1></ic><ic><qpk1>1</qpk1><pk1>7</pk1><cpk1>2</cpk1></ic></ics><icsr><ic><qpk1>0</qpk1><pk1>0</pk1><cpk1>0</cpk1></ic><ic><qpk1>4</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic><ic><qpk1>1</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic><ic><qpk1>5</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic><ic><qpk1>2</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic><ic><qpk1>6</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic><ic><qpk1>3</qpk1><pk1>2</pk1><cpk1>-1</cpk1></ic></icsr>";
        public MoodleCats()
        {
            InitializeComponent();
            
            tbXml.Text = xmlString;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (tbXml.TextLength == 0)
            {
                return;
            }
            string pattern = Regex.Escape("\\")+Regex.Escape("\\")+"\""+ Regex.Escape("|")+ "\"";
          //  Console.WriteLine("pattern =" + pattern);
            
            string replacement =Regex.Escape("\\")+ "\"";
           // Console.WriteLine("replacement =" + replacement);
            Regex rgx = new Regex(pattern);
            tbXml.Text = rgx.Replace(tbXml.Text, replacement);
            c = MoodleCatsClasses.createFromXMLString(tbXml.Text);
            var cs = c.cat.ToList();
            string catElement = "";
            foreach (var ctor in cs)
            {
                catElement = ctor.pk1.ToString() + " " + ctor.TypedValue + Environment.NewLine;
                tbMemo.Text += catElement;
                    
            }
            tbMemo.Text += Environment.NewLine;
            tbMemo.Text += "TESTS " + Environment.NewLine;
            var ts = c.ics;
            var tqs = ts.ic.AsEnumerable().OrderBy(t =>t.qpk1);
            string qps = "";
            foreach (var q in tqs)
            {
                var qtext = cs.Where(t => t.pk1 == q.pk1 && !String.IsNullOrEmpty(t.TypedValue)).FirstOrDefault();
                if (qtext != null) { 
                qps = "quest " + q.qpk1.ToString() + " catpk1 " + q.pk1.ToString() + " " +  qtext.TypedValue;
                }
                tbMemo.Text += qps + Environment.NewLine;
            }
            tbMemo.Text += Environment.NewLine;
            tbMemo.Text += "RUBRICS " + Environment.NewLine;
            var rs = c.icsr;
            var rss = rs.ic.AsEnumerable().OrderBy(t => t.qpk1); ;
            string rps = "";
            foreach (var r in rss)
            {
                var qtext = cs.Where(t => t.pk1 == r.pk1 && !String.IsNullOrEmpty(t.TypedValue)).FirstOrDefault();
                if (qtext != null)
                {
                    rps = "row " + r.qpk1.ToString() + " catpk1 " + r.pk1.ToString() + " " + qtext.TypedValue;
                }
                tbMemo.Text += rps + Environment.NewLine;
            }
                
        }
    }
}
